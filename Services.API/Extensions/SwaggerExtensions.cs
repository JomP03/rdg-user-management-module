using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Services.API.Extensions
{
    public static class SwaggerExtensions
    {

        /// <summary>
        /// Adds the swagger documentation generator
        /// </summary>
        /// <param name="services">Service collection.</param>
        public static void AddSwaggerGenerator(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "User Management",
                    Version = "v1",
                    Description = "User Management is used to create users, manage register requests and even delete users information."
                });

                // using xml from Services.API comments
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                options.IncludeXmlComments(xmlPath);

                // using xml from Domain comments
                xmlPath = xmlPath.Replace("Services.API", "App.Services");
                options.IncludeXmlComments(xmlPath);

                options.UseInlineDefinitionsForEnums();

                options.EnableAnnotations();
            });
        }

        /// <summary>
        /// Uses the swagger documentation generator
        /// </summary>
        /// <param name="app">.</param>
        public static void UseSwaggerWithUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Services.API v1"); ;
            });
        }

    }
}
