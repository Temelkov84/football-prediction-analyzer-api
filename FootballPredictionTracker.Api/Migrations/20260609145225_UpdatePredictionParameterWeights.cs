using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballPredictionTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePredictionParameterWeights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "PredictionParameterWeights",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ParameterName",
                table: "PredictionParameterWeights",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "PredictionParameterWeights",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "PredictionParameterWeights");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "PredictionParameterWeights",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PredictionParameterWeights",
                newName: "ParameterName");
        }
    }
}
