using InfomedicsPortal.Core.Appointments;
using InfomedicsPortal.Core.Dentists;
using InfomedicsPortal.Core.Patients;
using InfomedicsPortal.Core.Stats;
using InfomedicsPortal.Core.Treatments;
using InfomedicsPortal.Infrastructure.InMemory;

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

builder.Services.AddScoped<IAppointmentsService, AppointmentsService>();
builder.Services.AddScoped<IDentistsService, DentistsService>();
builder.Services.AddScoped<IPatientsService, PatientsService>();
builder.Services.AddScoped<ITreatmentsService, TreatmentsService>();
builder.Services.AddScoped<IStatsService, StatsService>();

builder.Services.AddSingleton<IAppointmentsStorage, AppointmentsRepository>();
builder.Services.AddSingleton<IDentistsStorage, DentistsRepository>();
builder.Services.AddSingleton<IPatientsStorage, PatientsRepository>();
builder.Services.AddSingleton<ITreatmentsStorage, TreatmentsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("LocalCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();