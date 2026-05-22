using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballPredictionTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandMatchStatisticsForPredictionFactors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayTeamAwayDraws",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamAwayLosses",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamAwayWins",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamGoalsConceded",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamGoalsScored",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamGoalsConceded",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamGoalsScored",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamHomeDraws",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamHomeLosses",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamHomeWins",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamAwayDraws",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayTeamAwayLosses",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayTeamAwayWins",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayTeamGoalsConceded",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayTeamGoalsScored",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeTeamGoalsConceded",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeTeamGoalsScored",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeTeamHomeDraws",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeTeamHomeLosses",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeTeamHomeWins",
                table: "MatchStatistics");
        }
    }
}
