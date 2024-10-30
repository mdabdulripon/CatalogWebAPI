using System.Text.Json.Serialization;
using Amazon.S3;
using Catalog.Core.RequestHelpers;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure Service Configuration
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAWSService<IAmazonS3>(builder.Configuration.GetAWSOptions());
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(CatalogContext).Assembly);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddTransient<IValidator<ProductParams>, ProductParamsValidator>();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "http://localhost:4100"));

app.UseHealthChecks("/health");

app.UseAuthorization();

app.MapControllers();

// TODO: seeding data only on development env 
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await CatalogDbInitializer.SeedData(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "Problem With Migrating Product Database");
}
await app.RunAsync();
