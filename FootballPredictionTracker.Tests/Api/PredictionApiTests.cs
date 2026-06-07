using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using FootballPredictionTracker.Tests.Helpers;
using FootballPredictionTracker.Api.DTOs;
using Microsoft.EntityFrameworkCore;

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
        public async Task GetWeeklyPredictions_ShouldReturnOk()
        {
            // Act
            HttpResponseMessage response =
                await client!.GetAsync("/api/Predictions/weekly");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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

                HomeTeamRecentWins = 5,
                HomeTeamRecentDraws = 1,
                HomeTeamRecentLosses = 0,

                AwayTeamRecentWins = 1,
                AwayTeamRecentDraws = 1,
                AwayTeamRecentLosses = 4,

                HeadToHeadHomeWins = 4,
                HeadToHeadDraws = 1,
                HeadToHeadAwayWins = 1,

                HomeTeamHomeWins = 5,
                HomeTeamHomeDraws = 1,
                HomeTeamHomeLosses = 0,

                AwayTeamAwayWins = 1,
                AwayTeamAwayDraws = 1,
                AwayTeamAwayLosses = 4,

                HomeTeamGoalsScored = 14,
                AwayTeamGoalsScored = 5,

                HomeTeamGoalsConceded = 3,
                AwayTeamGoalsConceded = 12
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
    }
}