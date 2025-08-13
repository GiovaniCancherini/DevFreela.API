using DevFreela.API.ExceptionHandler;
using DevFreela.Application.Models;
using DevFreela.API.Services;
using DevFreela.Application;
using DevFreela.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços customizados
builder.Services.Configure<FreelanceTotalCostConfig>(
    builder.Configuration.GetSection("FreelanceTotalCostConfig")
    );

builder.Services.AddScoped<IConfigService, ConfigService>();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

// Add Application & Infrastructure layers
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// ===== Pipeline =====
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

// Ordem correta: Authentication antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
