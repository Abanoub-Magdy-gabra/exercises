
// FitnessTracker.Exercises.Infrastructure/Data/Migrations/20230720000000_SeedMuscleGroups.cs
using Microsoft.EntityFrameworkCore.Migrations;

namespace Workout.Infrastructure.Data
{
    public partial class SeedMuscleGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed muscle groups data
            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "pectoralis_major", "Pectoralis Major", "The large chest muscle responsible for movement of the shoulder joint", "Chest", "/images/muscles/pectoralis_major.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "pectoralis_minor", "Pectoralis Minor", "The smaller chest muscle beneath the pectoralis major", "Chest", "/images/muscles/pectoralis_minor.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "anterior_deltoid", "Anterior Deltoid", "The front part of the deltoid muscle on the shoulder", "Shoulders", "/images/muscles/anterior_deltoid.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "lateral_deltoid", "Lateral Deltoid", "The middle part of the deltoid muscle on the shoulder", "Shoulders", "/images/muscles/lateral_deltoid.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "posterior_deltoid", "Posterior Deltoid", "The rear part of the deltoid muscle on the shoulder", "Shoulders", "/images/muscles/posterior_deltoid.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "biceps_brachii", "Biceps Brachii", "The muscle on the front part of the upper arm", "Arms", "/images/muscles/biceps_brachii.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "triceps_brachii", "Triceps Brachii", "The muscle on the back of the upper arm", "Arms", "/images/muscles/triceps_brachii.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "brachialis", "Brachialis", "The muscle located on the outer portion of the upper arm", "Arms", "/images/muscles/brachialis.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "brachioradialis", "Brachioradialis", "The muscle of the forearm that flexes the forearm at the elbow", "Arms", "/images/muscles/brachioradialis.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "forearm_flexors", "Forearm Flexors", "The muscles that flex the wrist and fingers", "Arms", "/images/muscles/forearm_flexors.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "forearm_extensors", "Forearm Extensors", "The muscles that extend the wrist and fingers", "Arms", "/images/muscles/forearm_extensors.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "latissimus_dorsi", "Latissimus Dorsi", "The large, flat muscle on the back that gives the back its width", "Back", "/images/muscles/latissimus_dorsi.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "trapezius", "Trapezius", "The large muscle of the upper back and neck", "Back", "/images/muscles/trapezius.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "rhomboids", "Rhomboids", "The muscles between the shoulder blades", "Back", "/images/muscles/rhomboids.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "erector_spinae", "Erector Spinae", "The group of muscles that extend the spine", "Back", "/images/muscles/erector_spinae.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "rectus_abdominis", "Rectus Abdominis", "The 'six-pack' muscle of the abdomen", "Core", "/images/muscles/rectus_abdominis.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "obliques", "Obliques", "The muscles on the sides of the abdomen", "Core", "/images/muscles/obliques.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "transverse_abdominis", "Transverse Abdominis", "The deepest abdominal muscle that wraps around the waist", "Core", "/images/muscles/transverse_abdominis.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "quadriceps", "Quadriceps", "The large muscles at the front of the thigh", "Legs", "/images/muscles/quadriceps.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "hamstrings", "Hamstrings", "The muscles at the back of the thigh", "Legs", "/images/muscles/hamstrings.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "glutes", "Glutes", "The muscles of the buttocks", "Legs", "/images/muscles/glutes.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "adductors", "Adductors", "The inner thigh muscles", "Legs", "/images/muscles/adductors.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "abductors", "Abductors", "The outer thigh muscles", "Legs", "/images/muscles/abductors.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "calves", "Calves", "The muscles at the back of the lower leg", "Legs", "/images/muscles/calves.png" });

            migrationBuilder.InsertData(
                schema: "Workout",
                table: "MuscleGroup",
                columns: new[] { "Name", "DisplayName", "Description", "BodyPart", "ImageUrl" },
                values: new object[] { "tibialis_anterior", "Tibialis Anterior", "The muscle at the front of the shin", "Legs", "/images/muscles/tibialis_anterior.png" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "pectoralis_major");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "pectoralis_minor");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "anterior_deltoid");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "lateral_deltoid");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "posterior_deltoid");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "biceps_brachii");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "triceps_brachii");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "brachialis");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "brachioradialis");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "forearm_flexors");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "forearm_extensors");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "latissimus_dorsi");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "trapezius");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "rhomboids");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "erector_spinae");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "rectus_abdominis");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "obliques");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "transverse_abdominis");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "quadriceps");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "hamstrings");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "glutes");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "adductors");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "abductors");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "calves");

            migrationBuilder.DeleteData(
                schema: "Workout",
                table: "MuscleGroup",
                keyColumn: "Name",
                keyValue: "tibialis_anterior");
        }
    }
}
