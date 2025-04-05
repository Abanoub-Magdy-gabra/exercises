using FitnessTracker.Exercises.Core.Entities;
using FitnessTracker.Exercises.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessTracker.Exercises.Infrastructure.Data
{
    public class ExercisesDbContext : DbContext
    {
        public ExercisesDbContext(DbContextOptions<ExercisesDbContext> options) : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseMedia> ExerciseMedia { get; set; }
        public DbSet<ExerciseTargetMuscle> ExerciseTargetMuscles { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Exercise entity
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("Exercise", "Workout");
                entity.HasKey(e => e.ExerciseId);
                entity.Property(e => e.ExerciseId).HasColumnName("ExerciseId");
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.Property(e => e.Equipment).HasMaxLength(50);

                // Configure enum conversion to string
                entity.Property(e => e.Difficulty)
                      .HasConversion(
                          v => v.ToString(),
                          v => (ExerciseDifficultyEnum)Enum.Parse(typeof(ExerciseDifficultyEnum), v));

                entity.Property(e => e.Instructions).HasMaxLength(4000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Create indexes for performance
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Equipment);
                entity.HasIndex(e => e.Difficulty);
                entity.HasIndex(e => e.IsVerified);
            });

            // Configure ExerciseMedia entity
            modelBuilder.Entity<ExerciseMedia>(entity =>
            {
                entity.ToTable("ExerciseMedia", "Workout");
                entity.HasKey(e => e.MediaId);
                entity.Property(e => e.MediaId).HasColumnName("MediaId");

                // Configure enum conversion to string
                entity.Property(e => e.MediaType)
                      .HasConversion(
                          v => v.ToString(),
                          v => (MediaTypeEnum)Enum.Parse(typeof(MediaTypeEnum), v));

                entity.Property(e => e.Url).HasMaxLength(255).IsRequired();
                entity.Property(e => e.ThumbnailUrl).HasMaxLength(255);
                entity.Property(e => e.FileName).HasMaxLength(255);
                entity.Property(e => e.ContentType).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Configure relationships
                entity.HasOne(e => e.Exercise)
                      .WithMany(e => e.Media)
                      .HasForeignKey(e => e.ExerciseId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Create indexes for performance
                entity.HasIndex(e => e.ExerciseId);
            });

            // Configure ExerciseTargetMuscle entity (junction table)
            modelBuilder.Entity<ExerciseTargetMuscle>(entity =>
            {
                entity.ToTable("ExerciseTargetMuscle", "Workout");
                entity.HasKey(e => new { e.ExerciseId, e.MuscleGroup });
                entity.Property(e => e.MuscleGroup).HasMaxLength(50);
                entity.Property(e => e.IsPrimary).HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Configure relationships
                entity.HasOne(e => e.Exercise)
                      .WithMany(e => e.TargetMuscles)
                      .HasForeignKey(e => e.ExerciseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Muscle)
                      .WithMany(e => e.Exercises)
                      .HasForeignKey(e => e.MuscleGroup)
                      .OnDelete(DeleteBehavior.Restrict);

                // Create indexes for performance
                entity.HasIndex(e => e.MuscleGroup);
            });

            // Configure MuscleGroup entity
            modelBuilder.Entity<MuscleGroup>(entity =>
            {
                entity.ToTable("MuscleGroup", "Workout");
                entity.HasKey(e => e.Name);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.DisplayName).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.BodyPart).HasMaxLength(50);
                entity.Property(e => e.ImageUrl).HasMaxLength(255);

                // Create indexes for performance
                entity.HasIndex(e => e.BodyPart);
            });

            // Configure ExerciseCategory entity
            modelBuilder.Entity<ExerciseCategory>(entity =>
            {
                entity.ToTable("ExerciseCategory", "Workout");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IconUrl).HasMaxLength(255);
                entity.Property(e => e.SortOrder).HasDefaultValue(0);

                // Configure relationships for hierarchical structure
                entity.HasOne(e => e.ParentCategory)
                      .WithMany(e => e.SubCategories)
                      .HasForeignKey(e => e.ParentCategoryId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired(false);

                // Create indexes for performance
                entity.HasIndex(e => e.ParentCategoryId);
            });

            // Configure query filters, e.g., for soft delete if implemented later
            // modelBuilder.Entity<Exercise>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically update timestamps
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Exercise && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is Exercise exercise)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        exercise.CreatedAt = DateTime.UtcNow;
                    }

                    exercise.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            // Automatically update timestamps
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Exercise && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is Exercise exercise)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        exercise.CreatedAt = DateTime.UtcNow;
                    }

                    exercise.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}