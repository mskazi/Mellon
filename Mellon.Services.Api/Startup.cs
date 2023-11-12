namespace Mellon.Services.Api
{
    using Mellon.Services.Apib.StartupExtensions;
    using Mellon.Services.Application;
    using Mellon.Services.Application.Services;
    using Mellon.Services.Common.interfaces;
    using Mellon.Services.External.CourierProviders;
    using Mellon.Services.Infrastracture.Models;
    using Mellon.Services.Infrastracture.Repositotiries;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Web;
    using Microsoft.IdentityModel.Logging;
    using Newtonsoft.Json.Converters;
    using Serilog;
    using System;
    using System.Reflection;
    using System.Security.Claims;

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
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(App))));
                //ef core contexts
                services.AddDbContext<MellonContext>(
                    options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("mellon"));
                        options.EnableSensitiveDataLogging();
                    });

                services.AddScoped(typeof(IApprovalsRepository), typeof(ApprovalsRepository));
                services.AddScoped(typeof(IMembersRepository), typeof(MembersRepository));
                services.AddScoped(typeof(IVouchersRepository), typeof(VouchersRepository));
                services.AddScoped(typeof(ILookupRepository), typeof(LookupRepository));
                services.AddScoped(typeof(IContactsRepository), typeof(ContactsRepository));
                services.AddScoped(typeof(ICarriersRepository), typeof(CarriersRepository));

                services.AddTransient(s => s.GetService<IHttpContextAccessor>().HttpContext?.User ?? new ClaimsPrincipal());
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

                services.AddTransient<ICourierService, GenikiTaxydromikiCourierService>();
                services.AddTransient<CourierServiceFactory>();

                services.AddScoped<ICurrentUserService, CurrentUserService>();
                services.AddScoped<IApprovalProcessorHost, ApprovalProcessorHost>();

                services.Configure<IISServerOptions>(options =>
                {
                    options.MaxRequestBodySize = 64 * 1024 * 1024;
                });

                services
                    .AddControllers()
                //.AddJsonOptions(options =>
                //{
                //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //});
                .AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));
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
