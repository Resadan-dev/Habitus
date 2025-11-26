using Valoron.Activities.Application;
using Valoron.Activities.Infrastructure;

using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

builder.Services.AddActivitiesInfrastructure(builder.Configuration);
builder.Services.AddActivitiesApplication();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

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
