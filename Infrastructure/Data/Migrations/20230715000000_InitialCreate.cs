// FitnessTracker.Exercises.Infrastructure/Data/Migrations/20230715000000_InitialCreate.cs
using System;
using Microsoft.EntityFrameworkCore.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Workout.Infrastructure.Data
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Workout Schema
            migrationBuilder.EnsureSchema(
                name: "Workout");

            // Create MuscleGroup Table
            migrationBuilder.CreateTable(
                name: "MuscleGroup",
                schema: "Workout",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BodyPart = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroup", x => x.Name);
                });

            // Create ExerciseCategory Table
            migrationBuilder.CreateTable(
                name: "ExerciseCategory",
                schema: "Workout",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SortOrder = table.Column<int>(type: "int", defaultValue: 0, nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseCategory", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_ExerciseCategory_ExerciseCategory_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalSchema: "Workout",
                        principalTable: "ExerciseCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create Exercise Table
            migrationBuilder.CreateTable(
                name: "Exercise",
                schema: "Workout",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Difficulty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.ExerciseId);
                });

            // Create ExerciseMedia Table
            migrationBuilder.CreateTable(
                name: "ExerciseMedia",
                schema: "Workout",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMedia", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_ExerciseMedia_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalSchema: "Workout",
                        principalTable: "Exercise",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create ExerciseTargetMuscle Table
            migrationBuilder.CreateTable(
                name: "ExerciseTargetMuscle",
                schema: "Workout",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTargetMuscle", x => new { x.ExerciseId, x.MuscleGroup });
                    table.ForeignKey(
                        name: "FK_ExerciseTargetMuscle_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalSchema: "Workout",
                        principalTable: "Exercise",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseTargetMuscle_MuscleGroup_MuscleGroup",
                        column: x => x.MuscleGroup,
                        principalSchema: "Workout",
                        principalTable: "MuscleGroup",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Exercise_Category",
                schema: "Workout",
                table: "Exercise",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_Difficulty",
                schema: "Workout",
                table: "Exercise",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_Equipment",
                schema: "Workout",
                table: "Exercise",
                column: "Equipment");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_IsVerified",
                schema: "Workout",
                table: "Exercise",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_Name",
                schema: "Workout",
                table: "Exercise",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseCategory_ParentCategoryId",
                schema: "Workout",
                table: "ExerciseCategory",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMedia_ExerciseId",
                schema: "Workout",
                table: "ExerciseMedia",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTargetMuscle_MuscleGroup",
                schema: "Workout",
                table: "ExerciseTargetMuscle",
                column: "MuscleGroup");

            migrationBuilder.CreateIndex(
                name: "IX_MuscleGroup_BodyPart",
                schema: "Workout",
                table: "MuscleGroup",
                column: "BodyPart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseCategory",
                schema: "Workout");

            migrationBuilder.DropTable(
                name: "ExerciseMedia",
                schema: "Workout");

            migrationBuilder.DropTable(
                name: "ExerciseTargetMuscle",
                schema: "Workout");

            migrationBuilder.DropTable(
                name: "Exercise",
                schema: "Workout");

            migrationBuilder.DropTable(
                name: "MuscleGroup",
                schema: "Workout");
        }
    }
}
