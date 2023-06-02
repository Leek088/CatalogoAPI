using CatalogoAPI.Context;
using CatalogoAPI.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Recuperar string de conexão
var connectionStringMysql = builder.Configuration.GetConnectionString("DefaultConnection");

//Registrar o serviço do contexto EF Core no conteiner DI
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionStringMysql, ServerVersion.AutoDetect(connectionStringMysql)));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Warning
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
