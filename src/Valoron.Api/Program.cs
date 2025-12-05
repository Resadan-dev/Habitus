using Valoron.Activities.Application;
using Valoron.Activities.Application.Dtos;
using Valoron.Activities.Application.Queries;
using Valoron.Activities.Infrastructure;
using Valoron.RpgCore.Infrastructure;
using Valoron.RpgCore.Domain;
using Valoron.Api.Services;
using Valoron.BuildingBlocks;

using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Postgresql;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Host.UseWolverine(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");

    // Configure PostgreSQL persistence pour le transactional outbox
    opts.PersistMessagesWithPostgresql(connectionString);

    // Active l'intégration EF Core
    // ActivitiesDbContext uses AddDbContextWithWolverineIntegration (automatic transactions)
    // RpgDbContext uses standard AddDbContext (manual SaveChangesAsync in handlers)
    opts.UseEntityFrameworkCoreTransactions();

    // Découverte des handlers dans les deux modules
    opts.Discovery.IncludeAssembly(typeof(CreateBookHandler).Assembly);
    opts.Discovery.IncludeAssembly(typeof(Valoron.RpgCore.Application.Handlers.BookFinishedHandler).Assembly);
});

builder.Services.AddActivitiesInfrastructure(builder.Configuration);
builder.Services.AddRpgCoreInfrastructure(builder.Configuration);
builder.Services.AddActivitiesApplication();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175") // Port par défaut de Vite
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowReactApp");

app.Use(async (context, next) =>
{
    if (!context.Request.Headers.ContainsKey("x-user-id"))
    {
        context.Request.Headers.Append("x-user-id", "00000000-0000-0000-0000-000000000001");
    }
    await next();
});

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.MapGet("/api/activities", (IMessageBus bus, CancellationToken ct) =>
    bus.InvokeAsync<IEnumerable<ActivityDto>>(new GetActivitiesQuery(), ct));

app.MapGet("/api/activities/{id}", async (Guid id, IMessageBus bus, CancellationToken ct) =>
{
    var activity = await bus.InvokeAsync<ActivityDto?>(new GetActivityByIdQuery(id), ct);
    return activity is not null ? Results.Ok(activity) : Results.NotFound();
});

app.MapPost("/api/activities", async (CreateActivityCommand command, IMessageBus bus, CancellationToken ct) =>
{
    var (id, _) = await bus.InvokeAsync<(Guid, IEnumerable<object>)>(command, ct);
    return Results.Created($"/api/activities/{id}", id);
});

app.MapGet("/api/books", (IMessageBus bus, CancellationToken ct) =>
    bus.InvokeAsync<IEnumerable<BookDto>>(new GetBooksQuery(), ct));

app.MapPost("/api/books", async (CreateBookCommand command, IMessageBus bus, CancellationToken ct) =>
{
    var id = await bus.InvokeAsync<Guid>(command, ct);
    return Results.Created($"/api/books/{id}", id);
});

app.MapPost("/api/books/{id}/abandon", async (Guid id, IMessageBus bus, CancellationToken ct) =>
{
    try
    {
        await bus.InvokeAsync(new AbandonBookCommand(id), ct);
        return Results.Ok();
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (UnauthorizedAccessException)
    {
        return Results.Forbid();
    }
});

app.MapPost("/api/activities/{activityId}/session", async (
    Guid activityId,
    LogReadingSessionRequest request,
    IMessageBus bus,
    CancellationToken cancellationToken) =>
{
    try
    {
        var command = new LogReadingSessionCommand(activityId, request.PagesRead);
        await bus.InvokeAsync(command, cancellationToken);
        return Results.Ok();
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
    {
        return Results.NotFound(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/api/players/{userId:guid}", async (
    Guid userId,
    IPlayerRepository playerRepo,
    CancellationToken ct) =>
{
    var player = await playerRepo.GetByIdAsync(userId, ct);
    if (player == null)
        return Results.NotFound();

    return Results.Ok(new
    {
        player.Id,
        player.Xp,
        player.Level,
        Stats = new
        {
            player.Stats.Strength,
            player.Stats.Intellect,
            player.Stats.Stamina
        }
    });
});

app.Run();

record LogReadingSessionRequest(int PagesRead);
