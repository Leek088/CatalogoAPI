using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerOptions
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Configura toda API, percorrendo cada controlador do projeto, para documentação das versões no Swagger
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }
    }

    /// <summary>
    /// Configura as opções do Swagger. Herdado da interface
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    /// <summary>
    /// Cria as informações sobre a versão das APIs
    /// </summary>
    /// <param name="desc"></param>
    /// <returns>Information about the API</returns>
    private OpenApiInfo CreateVersionInfo(
            ApiVersionDescription desc)
    {
        var info = new OpenApiInfo()
        {
            Title = "ApiCatalogo - .NET Core (.NET 6) Web API",
            Version = desc.ApiVersion.ToString(),
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
        };

        if (desc.IsDeprecated)
        {
            info.Description += " Esta versão da API está descontinuada. Por favor, use uma das novas APIs definidas nas opções.";
        }

        return info;
    }
}