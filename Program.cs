using AutoMapper;
using CatalogoAPI.Context;
using CatalogoAPI.DTOs.Mappings;
using CatalogoAPI.Logging;
using CatalogoAPI.Repositories;
using CatalogoAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");

});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "habilitarCORS",
                      policy =>
                      {
                          policy.WithOrigins("https://apirequest.io")
                          .WithMethods("GET");
                      });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";    
    config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ApiCatalogo",
        Description = "Catalogo de produtos e categorias",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Leonardo Nunes",
            Email = "leonardoleeko88@gmail.com",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Usar sobre LICX",
            Url = new Uri("https://example.com/license")
        }
    });

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Header de autorização JWT usando o equema Bearer. " +
        "\r\n\r\nInforme 'Bearer'[espaço] e o seu token.\r\n\r\nExemplo: \'Bearer 12345abcdef\'"
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
            }
    });
});

//Recuperar string de conexão
var connectionStringMysql = builder.Configuration.GetConnectionString("DefaultConnection");

//Registrar o serviço do contexto EF Core no conteiner DI
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionStringMysql,
    ServerVersion.AutoDetect(connectionStringMysql)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
        ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Debug
}));

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiCatalogo");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("habilitarCORS");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
