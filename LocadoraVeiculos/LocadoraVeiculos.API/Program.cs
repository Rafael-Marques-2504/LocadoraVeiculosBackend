using LocadoraVeiculos.API.Data;
using LocadoraVeiculos.API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---- Controllers ----
builder.Services.AddControllers();

// ---- Swagger ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Locadora de Veículos API",
        Version = "v1",
        Description = "API RESTful para gerenciamento de um sistema de aluguel de veículos. " +
                      "Trabalho acadêmico desenvolvido em C# com ASP.NET Core, Entity Framework e SQL Server."
    });
});

// ---- Entity Framework Core + SQL Server ----
builder.Services.AddDbContext<LocadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- CORS (liberado para facilitar os testes via Swagger/Postman) ----
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// ---- Middleware global de tratamento de erros ----
app.UseMiddleware<ErrorHandlingMiddleware>();

// ---- Swagger sempre habilitado (inclusive fora de Development) ----
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Locadora de Veículos API v1");
    c.RoutePrefix = string.Empty; // Swagger passa a abrir direto na raiz da aplicação
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
