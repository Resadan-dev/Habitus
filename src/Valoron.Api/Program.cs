using Valoron.Activities.Application;
using Valoron.Activities.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddActivitiesInfrastructure(builder.Configuration);
builder.Services.AddActivitiesApplication();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/activities/{activityId}/session", async (
    Guid activityId,
    LogReadingSessionRequest request,
    LogReadingSessionHandler handler,
    CancellationToken cancellationToken) =>
{
    try
    {
        var command = new LogReadingSessionCommand(activityId, request.PagesRead);
        await handler.Handle(command, cancellationToken);
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
