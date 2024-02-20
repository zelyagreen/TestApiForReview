using GreenPipes;
using Identity.API.Consumers;
using Identity.DAL.ContextModels;
using Identity.DAL.Data;
using Identity.Domain;
using Identity.Domain.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using TestApiForReview.Infrastructure.Filters;
using TestApiForReview.Infrastructure.Middlewares;
using System;
using System.Collections.Generic;

namespace Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(ValidateModelAttribute));
            });
            services.Configure<AppSettings>(Configuration);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["DefaultConnection"]), ServiceLifetime.Transient);
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IUserService, UserService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title   = "Identity Service",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name   = "Authorization",
                    In     = ParameterLocation.Header,
                    Type   = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name   = "Bearer",
                            In     = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            services.AddMassTransit(x =>
            {
                // Identity
                x.AddConsumer<Authenticate>();
                x.AddConsumer<CreateUser>();
                x.AddConsumer<GetUserByToken>();
                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host(new Uri("rabbitmq://host.docker.internal/"));
                    cfg.ReceiveEndpoint("identityQueue", e =>
                    {
                        e.PrefetchCount = 20;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        //// Identity
                        e.Consumer<Authenticate>(context);
                        e.Consumer<CreateUser>(context);
                        e.Consumer<GetUserByToken>(context);

                    });
                    cfg.ConfigureJsonSerializer(settings =>
                    {
                        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

                        return settings;
                    });
                    cfg.ConfigureJsonDeserializer(configure =>
                    {
                        configure.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                        return configure;
                    });
                });

            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API V1");
                });
            app.UseRouting();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}