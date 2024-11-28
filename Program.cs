
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// appsettings.json'u yükle
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// AWS hizmetlerini ekle
builder.Services.AddAWSService<IAmazonDynamoDB>(new AWSOptions
{
    Credentials = new Amazon.Runtime.BasicAWSCredentials(
        builder.Configuration["AWS:AccessKey"],
        builder.Configuration["AWS:SecretKey"]
    ),
    Region = Amazon.RegionEndpoint.GetBySystemName(
        builder.Configuration["AWS:Region"]
    )
});

// Controller'ları ekleyin
builder.Services.AddControllers();

// Swagger'ı ekleyin
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Picus API",
        Version = "v1",
        Description = "API for CRUD operations with AWS DynamoDB"
    });
});

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Picus API v1");
        options.RoutePrefix = string.Empty; // Swagger'ın kök dizinde çalışması için
    });

app.UseAuthorization();
app.MapControllers();

app.Run();
