using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FootballPredictionTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedPredictionParameterWeights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PredictionParameterWeights",
                columns: new[] { "Id", "IsActive", "Key", "Name", "Value" },
                values: new object[,]
                {
                    { 1, true, "recent_form", "Recent Form", 16m },
                    { 2, true, "home_away_form", "Home/Away Form", 18m },
                    { 3, true, "xg", "xG", 16m },
                    { 4, true, "attack_strength", "Attack Strength", 11m },
                    { 5, true, "defense_strength", "Defense Strength", 12m },
                    { 6, true, "shots_on_target", "Shots on Target", 11m },
                    { 7, true, "head_to_head", "Head-to-Head", 6m },
                    { 8, true, "key_players_missing", "Key Players Missing", 5m },
                    { 9, true, "fatigue", "Fatigue", 5m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PredictionParameterWeights",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
