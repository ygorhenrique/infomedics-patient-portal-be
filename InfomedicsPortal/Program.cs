using InfomedicsPortal.Core;
using InfomedicsPortal.Infrastructure;
using InfomedicsPortal.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "LocalCorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ProductionCorsPolicy",
        builder => builder
            .WithOrigins(
                "https://*.randes.me")
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("LocalCorsPolicy");
}
else
{
    app.UseCors("ProductionCorsPolicy");
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Infomedics Api!");

app.Run();

