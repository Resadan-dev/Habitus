using Valoron.Activities.Application;
using Valoron.Activities.Application.Dtos;
using Valoron.Activities.Application.Queries;
using Valoron.Activities.Infrastructure;

using Wolverine;
using Wolverine.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opts =>
{
    opts.UseEntityFrameworkCoreTransactions();
    opts.Policies.AutoApplyTransactions();
});

builder.Services.AddActivitiesInfrastructure(builder.Configuration);
builder.Services.AddActivitiesApplication();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => "Hello World!");

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

app.Run();

record LogReadingSessionRequest(int PagesRead);
