using DevPractice.Services.Interfaces.Services;
using DevPracice.Domain.Interfaces.DBContexts;
using DevPracice.Domain.Interfaces.Repositories;
using DevPractice.Infrastructure.Business.Services;
using DevPractice.Infrastructure.Data.DBContexts;
using DevPractice.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using DevPractice.Domain.Core.Response;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddDebug();
builder.Logging.AddConsole();
builder.Services.AddLogging();



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddDbContext<TableNameContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionString"));
});

builder.Services.AddScoped<ITableNameContext, TableNameContext>();
builder.Services.AddScoped<ITableNameRepository, TableNameRepository>();
builder.Services.AddScoped<IWeatherService, WeatherService>();


var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status200OK;

        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error != null)
        {
            var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();

            var json = JsonSerializer.Serialize(
                new Response<object>(exceptionHandlerPathFeature?.Error.Message?? "unhandled error occured"), // Switch this with your object
                jsonOptions?.Value.JsonSerializerOptions);

            await context.Response.WriteAsync(json);
        }
        else
        {
            await context.Response.WriteAsync("An exception was thrown.");
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);


