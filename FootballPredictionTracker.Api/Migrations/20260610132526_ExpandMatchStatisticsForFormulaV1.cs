using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballPredictionTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandMatchStatisticsForFormulaV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttackStrength",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayFatigueImpact",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AwayGoalsConcededAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AwayGoalsScoredAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AwayKeyPlayersMissingImpact",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayLast10AwayDraws",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayLast10AwayLosses",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayLast10AwayWins",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayRecentDraws",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayRecentLosses",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AwayRecentWins",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AwayShotsOnTargetAgainstAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AwayShotsOnTargetForAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AwayXgAgainstAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AwayXgForAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DefenseStrength",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HeadToHeadMatchesCount",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeAwayStrength",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeFatigueImpact",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeGoalsConcededAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeGoalsScoredAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "HomeKeyPlayersMissingImpact",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeLast10HomeDraws",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeLast10HomeLosses",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeLast10HomeWins",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeRecentDraws",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeRecentLosses",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeRecentWins",
                table: "MatchStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeShotsOnTargetAgainstAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeShotsOnTargetForAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeXgAgainstAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HomeXgForAverage",
                table: "MatchStatistics",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttackStrength",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayFatigueImpact",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayGoalsConcededAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayGoalsScoredAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayKeyPlayersMissingImpact",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayLast10AwayDraws",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayLast10AwayLosses",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayLast10AwayWins",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayRecentDraws",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayRecentLosses",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayRecentWins",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayShotsOnTargetAgainstAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayShotsOnTargetForAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayXgAgainstAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "AwayXgForAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "DefenseStrength",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HeadToHeadMatchesCount",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeAwayStrength",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeFatigueImpact",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeGoalsConcededAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeGoalsScoredAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeKeyPlayersMissingImpact",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeLast10HomeDraws",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeLast10HomeLosses",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeLast10HomeWins",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeRecentDraws",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeRecentLosses",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeRecentWins",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeShotsOnTargetAgainstAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeShotsOnTargetForAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeXgAgainstAverage",
                table: "MatchStatistics");

            migrationBuilder.DropColumn(
                name: "HomeXgForAverage",
                table: "MatchStatistics");
        }
    }
}
