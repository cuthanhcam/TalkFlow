using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace TalkFlow.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static void ConfigureSwaggerOptions(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TalkFlow API",
                Version = "v1.0",
                Description = "API documentation for TalkFlow - Real-time video calling and chat application",
                Contact = new OpenApiContact
                {
                    Name = "TalkFlow Team",
                    Email = "support@talkflow.com",
                    Url = new Uri("https://github.com/cuthanhcam/TalkFlow")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // Include XML comments
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            // Add JWT Authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            // Custom schema IDs to avoid conflicts
            options.CustomSchemaIds(type => type.FullName);

            // Group endpoints by controller
            options.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return new[] { api.GroupName };
                }

                var controllerName = api.ActionDescriptor.RouteValues["controller"];
                return new[] { controllerName ?? "Unknown" };
            });

            options.DocInclusionPredicate((name, api) => true);
        }
    }
}