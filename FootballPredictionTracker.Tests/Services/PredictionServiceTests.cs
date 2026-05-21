using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Services;
using NUnit.Framework;

namespace FootballPredictionTracker.Tests.Services
{
    [TestFixture]
    public class PredictionServiceTests
    {
        [Test]
        public void CalculatePrediction_WithValidStatistics_ShouldReturnProbabilitiesThatSumTo100()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

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
            PredictionResponse response = predictionService.CalculatePrediction(request);

            // Assert
            int totalProbability =
                response.HomeWinProbability +
                response.DrawProbability +
                response.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
            Assert.That(response.HomeTeam, Is.EqualTo("Arsenal"));
            Assert.That(response.AwayTeam, Is.EqualTo("Chelsea"));
            Assert.That(response.Explanation, Is.Not.Empty);
        }

        [Test]
        public void CalculatePrediction_WithNoStatistics_ShouldReturnBalancedProbabilities()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            PredictionRequest request = new PredictionRequest
            {
                HomeTeam = "Arsenal",
                AwayTeam = "Chelsea",
                HeadToHeadHomeWins = 0,
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
            PredictionResponse response = predictionService.CalculatePrediction(request);

            // Assert
            Assert.That(response.HomeWinProbability, Is.EqualTo(33));
            Assert.That(response.DrawProbability, Is.EqualTo(34));
            Assert.That(response.AwayWinProbability, Is.EqualTo(33));
            Assert.That(response.Explanation, Is.EqualTo("Not enough statistical data. Probabilities are balanced by default."));
        }

        [Test]
        public void CalculatePrediction_WhenHomeTeamHasStrongerStatistics_ShouldReturnHomeTeamAsHighestProbability()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            PredictionRequest request = new PredictionRequest
            {
                HomeTeam = "Arsenal",
                AwayTeam = "Chelsea",
                HeadToHeadHomeWins = 5,
                HeadToHeadDraws = 1,
                HeadToHeadAwayWins = 0,
                HomeTeamRecentWins = 5,
                HomeTeamRecentDraws = 0,
                HomeTeamRecentLosses = 0,
                AwayTeamRecentWins = 1,
                AwayTeamRecentDraws = 1,
                AwayTeamRecentLosses = 3
            };

            // Act
            PredictionResponse response = predictionService.CalculatePrediction(request);

            // Assert
            Assert.That(response.HomeWinProbability, Is.GreaterThan(response.DrawProbability));
            Assert.That(response.HomeWinProbability, Is.GreaterThan(response.AwayWinProbability));
            Assert.That(response.Explanation, Is.EqualTo("The home team has the strongest statistical advantage based on the provided data."));
        }

        [Test]
        public void CalculatePrediction_WhenAwayTeamHasStrongerStatistics_ShouldReturnAwayTeamAsHighestProbability()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            PredictionRequest request = new PredictionRequest
            {
                HomeTeam = "Arsenal",
                AwayTeam = "Chelsea",
                HeadToHeadHomeWins = 0,
                HeadToHeadDraws = 1,
                HeadToHeadAwayWins = 5,
                HomeTeamRecentWins = 1,
                HomeTeamRecentDraws = 1,
                HomeTeamRecentLosses = 3,
                AwayTeamRecentWins = 5,
                AwayTeamRecentDraws = 0,
                AwayTeamRecentLosses = 0
            };

            // Act
            PredictionResponse response = predictionService.CalculatePrediction(request);

            // Assert
            Assert.That(response.AwayWinProbability, Is.GreaterThan(response.HomeWinProbability));
            Assert.That(response.AwayWinProbability, Is.GreaterThan(response.DrawProbability));
            Assert.That(response.Explanation, Is.EqualTo("The away team has the strongest statistical advantage based on the provided data."));
        }

        [Test]
        public void CalculatePrediction_WhenStatisticsAreBalanced_ShouldReturnBalancedExplanation()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            PredictionRequest request = new PredictionRequest
            {
                HomeTeam = "Arsenal",
                AwayTeam = "Chelsea",
                HeadToHeadHomeWins = 2,
                HeadToHeadDraws = 2,
                HeadToHeadAwayWins = 2,
                HomeTeamRecentWins = 2,
                HomeTeamRecentDraws = 1,
                HomeTeamRecentLosses = 2,
                AwayTeamRecentWins = 2,
                AwayTeamRecentDraws = 1,
                AwayTeamRecentLosses = 2
            };

            // Act
            PredictionResponse response = predictionService.CalculatePrediction(request);

            // Assert
            Assert.That(response.Explanation, Is.EqualTo("The provided statistics suggest a balanced match with no clear dominant outcome."));

            int totalProbability =
                response.HomeWinProbability +
                response.DrawProbability +
                response.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
        }
    }
}