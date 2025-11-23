using Valoron.Activities.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddActivitiesInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
