using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Services.API.Extensions;
using System;
using System.Security.Claims;

namespace Services.API
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration prop
        /// </summary>
        public readonly IConfiguration myConfiguration;

        /// <summary>
        /// Startup ctor
        /// </summary>
        /// <param name="configuration">The configuration to be used</param>
        /// <exception cref="ArgumentNullException">Exception thrown in case of no configuration being provided</exception>
        public Startup(IConfiguration configuration)
        {
            myConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Method to configure services (Called automatically by the runtime)
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            // Auth0 Variables
            var domain = myConfiguration["Auth0:AUTH0_DOMAIN"];
            var audience = myConfiguration["Auth0:AUTH0_AUDIENCE"];


            // Add swagger generation
            services.AddSwaggerGenerator();
            // Add dependency injectoins
            services.AddLocalServices(myConfiguration);
            services.AddHttpClient();
            services.AddControllers();

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            // Add authentication services
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = audience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };

                });

            // Add the authentication service
            services.AddAuthorization(options =>
            {
                options.AddPolicy("manage:users",
                    policy => policy.Requirements.Add(
                        new HasScopeRequirement("manage:users", domain)));

                options.AddPolicy("manage:admin",
                    policy => policy.Requirements.Add(
                        new HasScopeRequirement("manage:admin", domain)));

                options.AddPolicy("user:requests",
                    policy => policy.Requirements.Add(
                        new HasScopeRequirement("user:requests", domain)));

                options.AddPolicy("read:users", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            (c.Type == "permissions" && c.Value == "manage:users") ||
                            (c.Type == "permissions" && c.Value == "manage:tasks")
                        )
                    );
                });

            });

            // Register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


        }

        /// <summary>
        /// Method to configure the application (Called automatically by the runtime)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Running Configure");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Services.API v1"));
            }

            // Configuração do CORS
            app.UseCors("AllowAnyOrigin");

            // Adicione o middleware personalizado para OPTIONS
            app.UseMiddleware<CorsMiddleware>();

            app.UseHttpsRedirection();
            app.UseExceptionHandler("/Error");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
