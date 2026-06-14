using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using FootballPredictionTracker.Tests.Helpers;
using FootballPredictionTracker.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Identity.Client;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace FootballPredictionTracker.Tests.Api
{
    [TestFixture]
    public class PredictionApiTests
    {
        private CustomWebApplicationFactory? factory;
        private HttpClient? client;

        [SetUp]
        public void Setup()
        {
            factory = new CustomWebApplicationFactory();
            client = factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
            factory?.Dispose();
        }

        [Test]
        public async Task GetWeeklyPredictions_WhenNoPredictionsExist_ShouldReturnOkAndEmptyList()
        {

            //Arrange
            using IServiceScope scope = factory!.Services.CreateScope();

            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            // Act
            HttpResponseMessage response =
                await client!.GetAsync("/api/Predictions/weekly");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            List<WeeklyPredictionResponse>? predictions = await response.Content.ReadFromJsonAsync<List<WeeklyPredictionResponse>>();

            Assert.That(predictions, Is.Not.Null);
            Assert.That(predictions, Is.Empty);
        }

        [Test]
        public async Task GetWeeklyPredictions_WhenPredictionExists_ShouldReturnPrediction()
        {
            //Arrange
            using IServiceScope scope = factory!.Services.CreateScope();

            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            League league = new League
            {
                Name = "Premier League",
                Country = "England"
            };

            Team homeTeam = new Team
            {
                Name = "Liverpool",
                League = league
            };

            Team awayTeam = new Team
            {
                Name = "Everton",
                League = league
            };

            Match match = new Match
            {
                League = league,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                KickoffTime = DateTime.UtcNow.AddDays(1)
            };

            Prediction prediction = new Prediction
            {
                Match = match,
                HomeWinProbability = 50,
                DrawProbability = 20,
                AwayWinProbability = 30
            };

            dbContext.Leagues.Add(league);
            dbContext.Teams.AddRange(homeTeam, awayTeam);
            dbContext.Matches.Add(match);
            dbContext.Predictions.Add(prediction);

            await dbContext.SaveChangesAsync();

            //Act
            HttpResponseMessage response = await client!.GetAsync("/api/Predictions/weekly");


            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            List<WeeklyPredictionResponse>? predictions =
              await response.Content.ReadFromJsonAsync<List<WeeklyPredictionResponse>>();

            Assert.That(predictions, Is.Not.Null);
            Assert.That(predictions, Has.Count.EqualTo(1));

            WeeklyPredictionResponse predictionResponse = predictions!.Single();

            Assert.That(predictionResponse.HomeTeam, Is.EqualTo("Liverpool"));
            Assert.That(predictionResponse.AwayTeam, Is.EqualTo("Everton"));
            Assert.That(predictionResponse.League, Is.EqualTo("Premier League"));

            Assert.That(predictionResponse.HomeWinProbability, Is.EqualTo(50));
            Assert.That(predictionResponse.DrawProbability, Is.EqualTo(20));
            Assert.That(predictionResponse.AwayWinProbability, Is.EqualTo(30));

            int totalProbability = 
                predictionResponse.HomeWinProbability +
                predictionResponse.DrawProbability +
                predictionResponse.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
           
        }


        [Test]
        public async Task CalculatePrediction_WithExistingMatchAndStatistics_ShouldReturnOkAndPrediction()
        {
            // Arrange
            using IServiceScope scope = factory!.Services.CreateScope();

            ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            League league = new League
            {
                Name = "Premier League"
            };

            Team homeTeam = new Team
            {
                Name = "Liverpool",
                League = league
            };

            Team awayTeam = new Team
            {
                Name = "Everton",
                League = league
            };

            Match match = new Match
            {
                League = league,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                KickoffTime = DateTime.UtcNow.AddDays(1)
            };

            MatchStatistics statistics = new MatchStatistics
            {
                Match = match,

                // Recent Form - last 6
                HomeRecentWins = 5,
                HomeRecentDraws = 1,
                HomeRecentLosses = 0,

                AwayRecentWins = 1,
                AwayRecentDraws = 1,
                AwayRecentLosses = 4,

                // Home/Away Form - last 10
                HomeLast10HomeWins = 8,
                HomeLast10HomeDraws = 2,
                HomeLast10HomeLosses = 0,

                AwayLast10AwayWins = 2,
                AwayLast10AwayDraws = 2,
                AwayLast10AwayLosses = 6,

                // xG
                HomeXgForAverage = 2.20m,
                HomeXgAgainstAverage = 0.80m,
                AwayXgForAverage = 0.90m,
                AwayXgAgainstAverage = 1.90m,

                // Attack / Defense
                HomeGoalsScoredAverage = 2.10m,
                AwayGoalsScoredAverage = 0.90m,

                HomeGoalsConcededAverage = 0.70m,
                AwayGoalsConcededAverage = 1.80m,

                // Shots on Target
                HomeShotsOnTargetForAverage = 6.50m,
                HomeShotsOnTargetAgainstAverage = 2.80m,
                AwayShotsOnTargetForAverage = 3.10m,
                AwayShotsOnTargetAgainstAverage = 6.00m,

                // Head-to-Head
                HeadToHeadMatchesCount = 6,
                HeadToHeadHomeWins = 4,
                HeadToHeadDraws = 1,
                HeadToHeadAwayWins = 1,

                // Impact factors
                HomeKeyPlayersMissingImpact = 0,
                AwayKeyPlayersMissingImpact = 2,

                HomeFatigueImpact = 0,
                AwayFatigueImpact = 2
            };

            dbContext.Leagues.Add(league);
            dbContext.Teams.AddRange(homeTeam, awayTeam);
            dbContext.Matches.Add(match);
            dbContext.MatchStatistics.Add(statistics);

            await dbContext.SaveChangesAsync();

            // Act
            HttpResponseMessage response =
                await client!.PostAsync($"/api/admin/predictions/calculate/{match.Id}", null);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            AdminPredictionResponse? predictionResponse =
                await response.Content.ReadFromJsonAsync<AdminPredictionResponse>();

            Assert.That(predictionResponse, Is.Not.Null);

            int totalProbability =
                predictionResponse!.HomeWinProbability +
                predictionResponse.DrawProbability +
                predictionResponse.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));

            Assert.That(predictionResponse.HomeWinProbability, Is.GreaterThan(predictionResponse.DrawProbability));
            Assert.That(predictionResponse.HomeWinProbability, Is.GreaterThan(predictionResponse.AwayWinProbability));

            Prediction savedPrediction = await dbContext.Predictions
                .SingleAsync(p => p.Id == predictionResponse.Id);

            Assert.That(savedPrediction.MatchId, Is.EqualTo(match.Id));
        }

        [Test]
        public async Task CalculatePrediction_WithMissingMatch_ShouldReturnNotFound()
        {
            // Arrange
            using IServiceScope scope = factory!.Services.CreateScope();

            ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            int missingMatchId = 999;

            // Act
            HttpResponseMessage response =
                await client!.PostAsync($"/api/admin/predictions/calculate/{missingMatchId}", null);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            ErrorResponse? errorResponse =
                await response.Content.ReadFromJsonAsync<ErrorResponse>();

            Assert.That(errorResponse, Is.Not.Null);
            Assert.That(errorResponse!.Message, Is.EqualTo("Match does not exist."));
        }

        [Test]
        public async Task ImportPredictions_WithValidRow_ShouldCreateMatchStatisticsAndPrediction()
        {
            // Arrange
            using IServiceScope scope = factory!.Services.CreateScope();

            ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            League league = new League
            {
                Name = "Premier League"
            };

            Team homeTeam = new Team
            {
                Name = "Liverpool",
                League = league
            };

            Team awayTeam = new Team
            {
                Name = "Everton",
                League = league
            };

            dbContext.Leagues.Add(league);
            dbContext.Teams.AddRange(homeTeam, awayTeam);

            await dbContext.SaveChangesAsync();

            List<ImportPredictionRequest> request = new()
    {
        new ImportPredictionRequest
        {
            LeagueName = "Premier League",
            HomeTeamName = "Liverpool",
            AwayTeamName = "Everton",
            KickoffTime = DateTime.UtcNow.AddDays(1),

            HomeRecentWins = 5,
            HomeRecentDraws = 1,
            HomeRecentLosses = 0,

            AwayRecentWins = 1,
            AwayRecentDraws = 1,
            AwayRecentLosses = 4,

            HomeLast10HomeWins = 8,
            HomeLast10HomeDraws = 2,
            HomeLast10HomeLosses = 0,

            AwayLast10AwayWins = 2,
            AwayLast10AwayDraws = 2,
            AwayLast10AwayLosses = 6,

            HomeXgForAverage = 2.20m,
            HomeXgAgainstAverage = 0.80m,
            AwayXgForAverage = 0.90m,
            AwayXgAgainstAverage = 1.90m,

            HomeGoalsScoredAverage = 2.10m,
            AwayGoalsScoredAverage = 0.90m,

            HomeGoalsConcededAverage = 0.70m,
            AwayGoalsConcededAverage = 1.80m,

            HomeShotsOnTargetForAverage = 6.50m,
            HomeShotsOnTargetAgainstAverage = 2.80m,
            AwayShotsOnTargetForAverage = 3.10m,
            AwayShotsOnTargetAgainstAverage = 6.00m,

            HeadToHeadMatchesCount = 6,
            HeadToHeadHomeWins = 4,
            HeadToHeadDraws = 1,
            HeadToHeadAwayWins = 1,

            HomeKeyPlayersMissingImpact = 0,
            AwayKeyPlayersMissingImpact = 2,

            HomeFatigueImpact = 0,
            AwayFatigueImpact = 2
        }
    };

            // Act
            HttpResponseMessage response =
                await client!.PostAsJsonAsync("/api/admin/prediction-imports", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ImportPredictionResponse? importResponse =
                await response.Content.ReadFromJsonAsync<ImportPredictionResponse>();

            Assert.That(importResponse, Is.Not.Null);
            Assert.That(importResponse!.CreatedMatches, Is.EqualTo(1));
            Assert.That(importResponse.CreatedStatistics, Is.EqualTo(1));
            Assert.That(importResponse.CreatedPredictions, Is.EqualTo(1));
            Assert.That(importResponse.Errors, Is.Empty);
            Assert.That(importResponse.ImportedPredictions, Has.Count.EqualTo(1));

            ImportedPredictionResponse importedPrediction =
                importResponse.ImportedPredictions.Single();

            int totalProbability =
                importedPrediction.HomeWinProbability +
                importedPrediction.DrawProbability +
                importedPrediction.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
            Assert.That(importedPrediction.HomeTeam, Is.EqualTo("Liverpool"));
            Assert.That(importedPrediction.AwayTeam, Is.EqualTo("Everton"));

            int matchesCount = await dbContext.Matches.CountAsync();
            int statisticsCount = await dbContext.MatchStatistics.CountAsync();
            int predictionsCount = await dbContext.Predictions.CountAsync();

            Assert.That(matchesCount, Is.EqualTo(1));
            Assert.That(statisticsCount, Is.EqualTo(1));
            Assert.That(predictionsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task ImportPredictionsFromCsv_WithValidFile_ShouldCreateMatchStatisticsAndPrediction()
        {
            // Arrange
            using IServiceScope scope = factory!.Services.CreateScope();

            ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            League league = new League
            {
                Name = "Premier League"
            };

            Team homeTeam = new Team
            {
                Name = "Liverpool",
                League = league
            };

            Team awayTeam = new Team
            {
                Name = "Everton",
                League = league
            };

            dbContext.Leagues.Add(league);
            dbContext.Teams.AddRange(homeTeam, awayTeam);

            await dbContext.SaveChangesAsync();

            string csvContent =
                "league_name,home_team_name,away_team_name,kickoff_time,home_recent_wins,home_recent_draws,home_recent_losses,away_recent_wins,away_recent_draws,away_recent_losses,home_last10_home_wins,home_last10_home_draws,home_last10_home_losses,away_last10_away_wins,away_last10_away_draws,away_last10_away_losses,home_xg_for_average,home_xg_against_average,away_xg_for_average,away_xg_against_average,home_goals_scored_average,away_goals_scored_average,home_goals_conceded_average,away_goals_conceded_average,home_shots_on_target_for_average,home_shots_on_target_against_average,away_shots_on_target_for_average,away_shots_on_target_against_average,head_to_head_matches_count,head_to_head_home_wins,head_to_head_draws,head_to_head_away_wins,home_key_players_missing_impact,away_key_players_missing_impact,home_fatigue_impact,away_fatigue_impact\n" +
                "Premier League,Liverpool,Everton,2026-07-06T19:00:00Z,5,1,0,1,1,4,8,2,0,2,2,6,2.20,0.80,0.90,1.90,2.10,0.90,0.70,1.80,6.50,2.80,3.10,6.00,6,4,1,1,0,2,0,2";

            using var form = new MultipartFormDataContent();

            using var fileContent = new StringContent(csvContent, Encoding.UTF8);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

            form.Add(fileContent, "file", "prediction-import-test.csv");

            // Act
            HttpResponseMessage response =
                await client!.PostAsync("/api/admin/prediction-imports/csv", form);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ImportPredictionResponse? importResponse =
                await response.Content.ReadFromJsonAsync<ImportPredictionResponse>();

            Assert.That(importResponse, Is.Not.Null);
            Assert.That(importResponse!.CreatedMatches, Is.EqualTo(1));
            Assert.That(importResponse.CreatedStatistics, Is.EqualTo(1));
            Assert.That(importResponse.CreatedPredictions, Is.EqualTo(1));
            Assert.That(importResponse.Errors, Is.Empty);
            Assert.That(importResponse.ImportedPredictions, Has.Count.EqualTo(1));

            ImportedPredictionResponse importedPrediction =
                importResponse.ImportedPredictions.Single();

            int totalProbability =
                importedPrediction.HomeWinProbability +
                importedPrediction.DrawProbability +
                importedPrediction.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
            Assert.That(importedPrediction.HomeTeam, Is.EqualTo("Liverpool"));
            Assert.That(importedPrediction.AwayTeam, Is.EqualTo("Everton"));

            int matchesCount = await dbContext.Matches.CountAsync();
            int statisticsCount = await dbContext.MatchStatistics.CountAsync();
            int predictionsCount = await dbContext.Predictions.CountAsync();

            Assert.That(matchesCount, Is.EqualTo(1));
            Assert.That(statisticsCount, Is.EqualTo(1));
            Assert.That(predictionsCount, Is.EqualTo(1));
        }
    }
}