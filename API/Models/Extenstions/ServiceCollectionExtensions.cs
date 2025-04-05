using AutoMapper;
using Workout.Infrastructure.Data;
using Workout.Infrastructure.Stoarge;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Workout.API.Models.Extenstions;
using Workout.Application.Services;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;
using Workout.Infrastructure.Caching;
using FitnessTracker.Exercises.Infrastructure.Data;

namespace Workout.API.Models.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all services required for the Exercises API
        /// </summary>
        public static IServiceCollection AddExercisesServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddExercisesDatabase(configuration)
                .AddExercisesRepositories()
                .AddExercisesApplicationServices()
                .AddAutoMapper()
                .AddValidation();
        }

        /// <summary>
        /// Configures the exercises database connection
        /// </summary>
        public static IServiceCollection AddExercisesDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Get the connection string from configuration
            var connectionString = configuration.GetConnectionString("ExercisesDatabase");

            // Register the DbContext
            services.AddDbContext<ExercisesDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);

                    sqlOptions.MigrationsAssembly(typeof(ExercisesDbContext).Assembly.FullName);
                });
            });

            return services;
        }

        /// <summary>
        /// Registers all repositories
        /// </summary>
        public static IServiceCollection AddExercisesRepositories(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IExerciseRepository, IExerciseRepository>();
            services.AddScoped<IExerciseMediaRepository, IExerciseMediaRepository>();
            services.AddScoped<IExerciseTargetMuscleRepository, IExerciseTargetMuscleRepository>();
            services.AddScoped<IMuscleGroupRepository, IMuscleGroupRepository>();
            services.AddScoped<IExerciseCategoryRepository, IExerciseCategoryRepository>();

            return services;
        }

        /// <summary>
        /// Registers all application services
        /// </summary>
        public static IServiceCollection AddExercisesApplicationServices(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IExerciseMediaService, ExerciseMediaService>();
            services.AddScoped<IExerciseSearchService, ExerciseSearchService>();
            services.AddScoped<IMuscleGroupService, MuscleGroupService>();

            // Register caching service
            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            return services;
        }

        /// <summary>
        /// Registers AutoMapper
        /// </summary>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }

        /// <summary>
        /// Registers FluentValidation
        /// </summary>
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}