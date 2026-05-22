using FootballPredictionTracker.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Net;

namespace FootballPredictionTracker.Tests.Api
{
    [TestFixture]
    public class PredictionApiTests
    {
        private WebApplicationFactory<Program>? factory;
        private HttpClient? client;

        [SetUp]
        public void Setup()
        {
            factory = new WebApplicationFactory<Program>();
            client = factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            client?.Dispose();
            factory?.Dispose();
        }

        [Test]
        public async Task CalculatePrediction_WithValidRequest_ShouldReturnOkAndPrediction()
        {
            // Arrange
            PredictionRequest request = new PredictionRequest
            {
                HomeTeam = "Arsenal",
                AwayTeam = "Chelsea",
                HeadToHeadHomeWins = 4,
                HeadToHeadDraws = 2,
                HeadToHeadAwayWins = 1,
                HomeTeamRecentWins = 4,
                HomeTeamRecentDraws = 1,
                HomeTeamRecentLosses = 0,
                AwayTeamRecentWins = 2,
                AwayTeamRecentDraws = 1,
                AwayTeamRecentLosses = 2
            };

            // Act
            HttpResponseMessage response =
                await client!.PostAsJsonAsync("/api/Predictions/calculate", request);

            PredictionResponse? prediction =
                await response.Content.ReadFromJsonAsync<PredictionResponse>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(prediction, Is.Not.Null);

            int totalProbability =
                prediction.HomeWinProbability +
                prediction.DrawProbability +
                prediction.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
            Assert.That(prediction.HomeTeam, Is.EqualTo("Arsenal"));
            Assert.That(prediction.AwayTeam, Is.EqualTo("Chelsea"));
            Assert.That(prediction.Explanation, Is.Not.Empty);
        }

        [Test]
        public async Task CalculatePrediction_WithSameTeams_ShouldReturnBadRequest()
        {
            // Arrange
            PredictionRequest request = new PredictionRequest
            {
                HomeTeam = "Arsenal",
                AwayTeam = "Arsenal",
                HeadToHeadHomeWins = 1,
                HeadToHeadDraws = 1,
                HeadToHeadAwayWins = 1,
                HomeTeamRecentWins = 2,
                HomeTeamRecentDraws = 1,
                HomeTeamRecentLosses = 2,
                AwayTeamRecentWins = 2,
                AwayTeamRecentDraws = 1,
                AwayTeamRecentLosses = 2
            };

            // Act
            HttpResponseMessage response =
                await client!.PostAsJsonAsync("/api/Predictions/calculate", request);

            string responseBody =
                await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(responseBody, Is.EqualTo("Home team and away team must be different."));
        }

        [Test]
        public async Task CalculatePrediction_WithInvalidModelData_ShouldReturnBadRequest()
        {
            // Arrange
            PredictionRequest request = new PredictionRequest
            {
                HomeTeam = "",
                AwayTeam = "C",
                HeadToHeadHomeWins = -1,
                HeadToHeadDraws = 0,
                HeadToHeadAwayWins = 0,
                HomeTeamRecentWins = 0,
                HomeTeamRecentDraws = 0,
                HomeTeamRecentLosses = 0,
                AwayTeamRecentWins = 0,
                AwayTeamRecentDraws = 0,
                AwayTeamRecentLosses = 0
            };

            // Act
            HttpResponseMessage response =
                await client!.PostAsJsonAsync("/api/Predictions/calculate", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetWeeklyPredictions_ShouldReturnOk()
        {
            // Arrange

            // Act
            HttpResponseMessage response =
            await client!.GetAsync("/api/Predictions/weekly");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}