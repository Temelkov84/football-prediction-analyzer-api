using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Models;
using FootballPredictionTracker.Api.Services;
using NUnit.Framework;

namespace FootballPredictionTracker.Tests.Services
{
    [TestFixture]
    public class PredictionServiceTests
    {
        [Test]
        public void CalculatePrediction_WithStrongHomeTeam_ShouldReturnHighestHomeWinProbability()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = new Match
            {
                Id = 1,
                LeagueId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                KickoffTime = DateTime.UtcNow
            };

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = 1,

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

                HomeTeamGoalsScored = 15,
                HomeTeamGoalsConceded = 4,

                AwayTeamGoalsScored = 5,
                AwayTeamGoalsConceded = 14
            };

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            Assert.That(prediction.HomeWinProbability, Is.GreaterThan(prediction.DrawProbability));
            Assert.That(prediction.HomeWinProbability, Is.GreaterThan(prediction.AwayWinProbability));

            int totalProbability =
                prediction.HomeWinProbability +
                prediction.DrawProbability +
                prediction.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));
        }
     
        [Test]
        public void CalculatePrediction_WithNoStatistics_ShouldReturnBalancedProbabilities()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = new Match
            {
                Id = 2,
                LeagueId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                KickoffTime = DateTime.UtcNow
            };

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = 2,

                HomeTeamRecentWins = 0,
                HomeTeamRecentDraws = 0,
                HomeTeamRecentLosses = 0,

                AwayTeamRecentWins = 0,
                AwayTeamRecentDraws = 0,
                AwayTeamRecentLosses = 0,

                HeadToHeadHomeWins = 0,
                HeadToHeadDraws = 0,
                HeadToHeadAwayWins = 0,

                HomeTeamHomeWins = 0,
                HomeTeamHomeDraws = 0,
                HomeTeamHomeLosses = 0,

                AwayTeamAwayWins = 0,
                AwayTeamAwayDraws = 0,
                AwayTeamAwayLosses = 0,

                HomeTeamGoalsScored = 0,
                HomeTeamGoalsConceded = 0,

                AwayTeamGoalsScored = 0,
                AwayTeamGoalsConceded = 0
            };

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            Assert.That(prediction.HomeWinProbability, Is.EqualTo(33));
            Assert.That(prediction.DrawProbability, Is.EqualTo(34));
            Assert.That(prediction.AwayWinProbability, Is.EqualTo(33));
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));
        }

        [Test]
        public void CalculatePrediction_WhenHomeTeamHasStrongerStatistics_ShouldReturnHomeTeamAsHighestProbability()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = new Match
            {
                Id = 1,
                LeagueId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                KickoffTime = DateTime.UtcNow
            };

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = 1,
                HomeTeamRecentWins = 5,
                HomeTeamRecentDraws = 0,
                HomeTeamRecentLosses = 0,
                AwayTeamRecentWins = 0,
                AwayTeamRecentDraws = 1,
                AwayTeamRecentLosses = 4,
                HeadToHeadHomeWins = 5,
                HeadToHeadDraws = 0,
                HeadToHeadAwayWins = 0,
                HomeTeamHomeWins = 5,
                HomeTeamHomeDraws = 0,
                HomeTeamHomeLosses = 0,
                AwayTeamAwayWins = 0,
                AwayTeamAwayDraws = 1,
                AwayTeamAwayLosses = 4,
                HomeTeamGoalsScored = 20,
                HomeTeamGoalsConceded = 2,
                AwayTeamGoalsScored = 2,
                AwayTeamGoalsConceded = 20
            };

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            Assert.That(prediction.HomeWinProbability, Is.GreaterThan(prediction.DrawProbability));
            Assert.That(prediction.HomeWinProbability, Is.GreaterThan(prediction.AwayWinProbability));
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));
        }

            [Test]
        public void CalculatePrediction_WhenAwayTeamHasStrongerStatistics_ShouldReturnAwayTeamAsHighestProbability()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

                Match match = new Match
                {
                    Id = 1,
                    LeagueId = 1,
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    KickoffTime = DateTime.UtcNow
                };

                MatchStatistics statistics = new MatchStatistics
                {
                    MatchId = 1,
                    HomeTeamRecentWins = 0,
                    HomeTeamRecentDraws = 1,
                    HomeTeamRecentLosses = 4,

                    AwayTeamRecentWins = 5,
                    AwayTeamRecentDraws = 0,
                    AwayTeamRecentLosses = 0,

                    HeadToHeadHomeWins = 0,
                    HeadToHeadDraws = 1,
                    HeadToHeadAwayWins = 5,

                    HomeTeamHomeWins = 0,
                    HomeTeamHomeDraws = 1,
                    HomeTeamHomeLosses = 3,
              
                    AwayTeamAwayWins = 3,
                    AwayTeamAwayDraws = 1,
                    AwayTeamAwayLosses = 0,
                   
                    HomeTeamGoalsScored = 2,
                    HomeTeamGoalsConceded = 2,
                    
                    AwayTeamGoalsScored = 12,
                    AwayTeamGoalsConceded = 0
                };
                
                //Act
                Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            
            // Assert
            Assert.That(prediction.AwayWinProbability, Is.GreaterThan(prediction.HomeWinProbability));
            Assert.That(prediction.AwayWinProbability, Is.GreaterThan(prediction.DrawProbability));
        }

        [Test]
        public void CalculatePrediction_WhenStatisticsAreBalanced_ShouldReturnBalancedExplanation()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = new Match
            {
                Id = 1,
                LeagueId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                KickoffTime = DateTime.UtcNow
            };

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = 1,

                HomeTeamRecentWins = 3,
                HomeTeamRecentDraws = 1,
                HomeTeamRecentLosses = 2,

                AwayTeamRecentWins = 3,
                AwayTeamRecentDraws = 1,
                AwayTeamRecentLosses = 2,

                HeadToHeadHomeWins = 2,
                HeadToHeadDraws = 2,
                HeadToHeadAwayWins = 2,

                HomeTeamHomeWins = 3,
                HomeTeamHomeDraws = 2,
                HomeTeamHomeLosses = 1,

                AwayTeamAwayWins = 3,
                AwayTeamAwayDraws = 2,
                AwayTeamAwayLosses = 1,

                HomeTeamGoalsScored = 10,
                HomeTeamGoalsConceded = 7,

                AwayTeamGoalsScored = 10,
                AwayTeamGoalsConceded = 7
            };

            // Act
           Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            int totalProbability =
                prediction.HomeWinProbability +
                prediction.DrawProbability +
                prediction.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
        }

    }
}