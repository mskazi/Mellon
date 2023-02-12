namespace Mellon.Services.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Mellon.Services.Infrastracture.Context;
    using System.Security.Claims;
    using Mellon.Services.Infrastracture.Repositotiries;
    using MediatR;
    using Mellon.Services.Application;
    using Mellon.Services.Application.Services;
    using Serilog;
    using Mellon.Services.Apib.StartupExtensions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Identity.Web;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.IdentityModel.Logging;

    namespace XO.CRM.Services.Web
    {
#pragma warning disable CS1591
        public class Startup
        {
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .CreateLogger();

                Log.Logger.Information("Started");
                IdentityModelEventSource.ShowPII = true;
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(Configuration);
                // Creating policies that wraps the authorization requirements
                services.AddAuthorization();
                services.AddMediatR(typeof(App).Assembly);
                //ef core contexts
                services.AddDbContext<MellonContext>(
                    options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("mellon"));
                        options.EnableSensitiveDataLogging();
                    });

                services.AddScoped(typeof(IApprovalsRepository), typeof(ApprovalsRepository));

                // services.AddTransient(s => s.GetService<IHttpContextAccessor>().HttpContext?.User ?? new ClaimsPrincipal());
                 services.AddSingleton(Configuration);
                ////cors
                services.AddCors(opt =>
                {
                    opt.AddDefaultPolicy(builder =>
                    {
                        var origins = Configuration["CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        builder.WithOrigins(origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                });

                services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

                services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
                services.AddTransient<IEmailService, EmailService>();
                services.AddScoped<IApprovalProcessorHost, ApprovalProcessorHost>();

                services.AddControllers();
                services.AddSwaggerGen();
                services.AddHostedService<ApporvalBackgroundServiceHost>();
                services.AddMellonSwaggerGen(Configuration);
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    // Since IdentityModel version 5.2.1 (or since Microsoft.AspNetCore.Authentication.JwtBearer version 2.2.0),
                    // Personal Identifiable Information is not written to the logs by default, to be compliant with GDPR.
                    // For debugging/development purposes, one can enable additional detail in exceptions by setting IdentityModelEventSource.ShowPII to true.
                    // Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                else
                {
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseMiddleware<ExceptionMiddleware>();

                app.UseCors();
                app.UseHttpsRedirection();
                app.UseAuthentication();

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}
