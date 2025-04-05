
// FitnessTracker.Exercises.Infrastructure/Data/Migrations/20230725000000_SeedExerciseCategories.cs
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workout.Infrastructure.Data.Migrations
{
    public partial class SeedExerciseCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Main categories
            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Strength", "Exercises focusing on building strength and muscle mass", "/images/categories/strength.png", 1, null });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Cardio", "Exercises focusing on cardiovascular fitness and endurance", "/images/categories/cardio.png", 2, null });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Flexibility", "Exercises focusing on improving range of motion and flexibility", "/images/categories/flexibility.png", 3, null });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Balance", "Exercises focusing on improving balance and stability", "/images/categories/balance.png", 4, null });

            // Get IDs for the main categories
            var strengthId = 1;
            var cardioId = 2;
            var flexibilityId = 3;
            var balanceId = 4;

            // Strength subcategories
            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Bodyweight", "Strength exercises using only bodyweight for resistance", "/images/categories/bodyweight.png", 1, strengthId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Free Weights", "Strength exercises using free weights like dumbbells and barbells", "/images/categories/free_weights.png", 2, strengthId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Machines", "Strength exercises using weight machines", "/images/categories/machines.png", 3, strengthId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Cables", "Strength exercises using cable machines", "/images/categories/cables.png", 4, strengthId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Resistance Bands", "Strength exercises using resistance bands", "/images/categories/resistance_bands.png", 5, strengthId });

            // Cardio subcategories
            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Running", "Cardio exercises involving running", "/images/categories/running.png", 1, cardioId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Cycling", "Cardio exercises involving cycling", "/images/categories/cycling.png", 2, cardioId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Swimming", "Cardio exercises involving swimming", "/images/categories/swimming.png", 3, cardioId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Rowing", "Cardio exercises involving rowing", "/images/categories/rowing.png", 4, cardioId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "HIIT", "High-Intensity Interval Training exercises", "/images/categories/hiit.png", 5, cardioId });

            // Flexibility subcategories
            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Static Stretching", "Holding a stretch for a period of time", "/images/categories/static_stretching.png", 1, flexibilityId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Dynamic Stretching", "Active movements that stretch muscles to their full range of motion", "/images/categories/dynamic_stretching.png", 2, flexibilityId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Yoga", "Yoga poses and sequences", "/images/categories/yoga.png", 3, flexibilityId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Pilates", "Pilates exercises focusing on core strength and flexibility", "/images/categories/pilates.png", 4, flexibilityId });

            // Balance subcategories
            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Stability Ball", "Balance exercises using a stability ball", "/images/categories/stability_ball.png", 1, balanceId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "BOSU Ball", "Balance exercises using a BOSU ball", "/images/categories/bosu_ball.png", 2, balanceId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Unilateral", "Single-limb exercises to improve balance", "/images/categories/unilateral.png", 3, balanceId });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "ExerciseCategory",
                columns: new[] { "Name", "Description", "IconUrl", "SortOrder", "ParentCategoryId" },
                values: new object[] { "Balance Board", "Balance exercises using a balance board", "/images/categories/balance_board.png", 4, balanceId });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete all ExerciseCategory records
            migrationBuilder.Sql("DELETE FROM [Workout].[ExerciseCategory]");
        }
    }
}