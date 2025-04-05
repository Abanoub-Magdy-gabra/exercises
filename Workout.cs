using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using System.Reflection;
using System.IO;

// Exercise-specific namespaces
using FitnessTracker.Exercises.Infrastructure.Data;

using Workout.Application.Services;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Validators;
using Workout.Core.Interfaces.Services;
using FitnessTracker.Exercises.Application.Validators;

namespace FitnessTracker.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure Database Context
            services.AddDbContext<ExercisesDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("FitnessTracker.Data")
                )
                .EnableSensitiveDataLogging(Environment.IsDevelopment())
            );

            // Configure Controllers
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = Environment.IsDevelopment();
                })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    fv.AutomaticValidationEnabled = true;
                });

            // Dependency Injection for Exercise Services
            ConfigureExerciseDependencies(services);

            // Swagger Configuration
            ConfigureSwagger(services);

            // CORS Configuration
            ConfigureCors(services);

            // Caching
            services.AddResponseCaching();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exercise API v1");
                    c.RoutePrefix = "docs";
                });
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // CORS
            app.UseCors("ExerciseApiPolicy");

            // Caching
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureExerciseDependencies(IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IExerciseRepository, IExerciseRepository>();
            services.AddScoped<IExerciseMediaRepository, IExerciseMediaRepository>();
            services.AddScoped<IExerciseTargetMuscleRepository, IExerciseTargetMuscleRepository>();

            // Services
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IExerciseSearchService, ExerciseSearchService>();

            // Validators
            services.AddScoped<IExerciseValidator, ExerciseValidator>();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Exercise API",
                    Version = "v1",
                    Description = "API for Managing Fitness Exercises"
                });

                // XML Documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ExerciseApiPolicy", builder =>
                {
                    builder
                        .WithOrigins(
                            "https://localhost:5001",
                            "http://localhost:5000"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}