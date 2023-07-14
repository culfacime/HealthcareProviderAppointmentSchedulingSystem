using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Healthcare.API.Configurations.ApiVersion;

public class SwaggerConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public SwaggerConfiguration(
        IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
            var filePath = Path.Combine(AppContext.BaseDirectory, "Healthcare.API.xml");
            options.IncludeXmlComments(filePath);
        }
    }

    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private OpenApiInfo CreateVersionInfo(
            ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "HealthCare",
            Version = description.ApiVersion.ToString(),
            Description = "Healthcare Provider Appointment Management System",
            Contact = new OpenApiContact { Name = "A Information Technologies", Email = "info@google.com", Url = new Uri("https://www.google.com") }

        };

        if (description.IsDeprecated)
        {
            info.Description += "This version has been disabled.";
        }

        return info;
    }
}
