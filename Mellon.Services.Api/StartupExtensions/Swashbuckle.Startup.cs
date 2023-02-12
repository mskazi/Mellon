using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mellon.Services.Apib.StartupExtensions
{
    /// <summary>
    /// Extension methods to enable swagger specific for XO.CRM API
    /// </summary>
    public static class SwashbuckleStartupExtensions
    {
        /// <summary>
        /// Adds Swagger Gen support and configures Authorization field for the screen
        /// </summary>
        /// <param name="services">Dotnet Service Collection</param>
        /// <param name="configuration">Dotnet IConfiguration Collection</param>
        /// <returns></returns>
        public static IServiceCollection AddMellonSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                //Support for swagger doc
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"Mellon.Services.Web - {configuration["ASPNETCORE_ENVIRONMENT"]}",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                    },
                });
                string instance = $"{configuration.GetValue<string>("AzureAd:Instance")}";
                //Flow configuration
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {

                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{instance}/{configuration.GetValue<string>("AzureAd:TenantId")}/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri($"{instance}/{configuration.GetValue<string>("AzureAd:TenantId")}/oauth2/v2.0/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { $"{configuration.GetValue<string>("AzureAd:scopes")}", "Mellon.Services" }
                            }
                        }
                    }
                });
                // Security Scheme
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            },
                            Scheme = "oauth2",
                            Name = "oauth2",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        /// <summary>
        /// Add swagger UI and configures oauth2 client-secret
        /// </summary>
        /// <param name="app">Dotnet App Builder</param>
        /// <param name="configuration">Dotnet IConfiguration Collection</param>
        /// <returns></returns>
        public static IApplicationBuilder UseMellonSwaggerUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mellon.Services.Web v1");
                c.RoutePrefix = string.Empty;
                c.OAuthClientId(configuration.GetValue<string>("AzureAd:ClientId"));
                c.OAuthClientSecret(configuration.GetValue<string>("AzureAd:ClientSecret"));
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
            return app;
        }
    }
}
