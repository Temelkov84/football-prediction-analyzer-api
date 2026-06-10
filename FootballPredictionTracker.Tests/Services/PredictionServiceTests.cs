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

            Match match = CreateMatch(id: 1);

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = match.Id,

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

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            Assert.That(prediction.HomeWinProbability, Is.GreaterThan(prediction.DrawProbability));
            Assert.That(prediction.HomeWinProbability, Is.GreaterThan(prediction.AwayWinProbability));
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));

            AssertProbabilitiesSumTo100(prediction);
        }

        [Test]
        public void CalculatePrediction_WithStrongAwayTeam_ShouldReturnHighestAwayWinProbability()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = CreateMatch(id: 2);

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = match.Id,

                // Recent Form - last 6
                HomeRecentWins = 1,
                HomeRecentDraws = 1,
                HomeRecentLosses = 4,

                AwayRecentWins = 5,
                AwayRecentDraws = 1,
                AwayRecentLosses = 0,

                // Home/Away Form - last 10
                HomeLast10HomeWins = 2,
                HomeLast10HomeDraws = 2,
                HomeLast10HomeLosses = 6,

                AwayLast10AwayWins = 8,
                AwayLast10AwayDraws = 2,
                AwayLast10AwayLosses = 0,

                // xG
                HomeXgForAverage = 0.90m,
                HomeXgAgainstAverage = 1.90m,
                AwayXgForAverage = 2.20m,
                AwayXgAgainstAverage = 0.80m,

                // Attack / Defense
                HomeGoalsScoredAverage = 0.90m,
                AwayGoalsScoredAverage = 2.10m,

                HomeGoalsConcededAverage = 1.80m,
                AwayGoalsConcededAverage = 0.70m,

                // Shots on Target
                HomeShotsOnTargetForAverage = 3.10m,
                HomeShotsOnTargetAgainstAverage = 6.00m,
                AwayShotsOnTargetForAverage = 6.50m,
                AwayShotsOnTargetAgainstAverage = 2.80m,

                // Head-to-Head
                HeadToHeadMatchesCount = 6,
                HeadToHeadHomeWins = 1,
                HeadToHeadDraws = 1,
                HeadToHeadAwayWins = 4,

                // Impact factors
                HomeKeyPlayersMissingImpact = 2,
                AwayKeyPlayersMissingImpact = 0,

                HomeFatigueImpact = 2,
                AwayFatigueImpact = 0
            };

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            Assert.That(prediction.AwayWinProbability, Is.GreaterThan(prediction.HomeWinProbability));
            Assert.That(prediction.AwayWinProbability, Is.GreaterThan(prediction.DrawProbability));
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));

            AssertProbabilitiesSumTo100(prediction);
        }

        [Test]
        public void CalculatePrediction_WithNoStatistics_ShouldReturnBalancedProbabilities()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = CreateMatch(id: 3);

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = match.Id
            };

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            Assert.That(prediction.HomeWinProbability, Is.EqualTo(33));
            Assert.That(prediction.DrawProbability, Is.EqualTo(34));
            Assert.That(prediction.AwayWinProbability, Is.EqualTo(33));
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));

            AssertProbabilitiesSumTo100(prediction);
        }

        [Test]
        public void CalculatePrediction_WithBalancedStatistics_ShouldReturnProbabilitiesThatSumTo100()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = CreateMatch(id: 4);

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = match.Id,

                // Recent Form - last 6
                HomeRecentWins = 3,
                HomeRecentDraws = 1,
                HomeRecentLosses = 2,

                AwayRecentWins = 3,
                AwayRecentDraws = 1,
                AwayRecentLosses = 2,

                // Home/Away Form - last 10
                HomeLast10HomeWins = 5,
                HomeLast10HomeDraws = 3,
                HomeLast10HomeLosses = 2,

                AwayLast10AwayWins = 5,
                AwayLast10AwayDraws = 3,
                AwayLast10AwayLosses = 2,

                // xG
                HomeXgForAverage = 1.50m,
                HomeXgAgainstAverage = 1.20m,
                AwayXgForAverage = 1.50m,
                AwayXgAgainstAverage = 1.20m,

                // Attack / Defense
                HomeGoalsScoredAverage = 1.50m,
                AwayGoalsScoredAverage = 1.50m,

                HomeGoalsConcededAverage = 1.20m,
                AwayGoalsConcededAverage = 1.20m,

                // Shots on Target
                HomeShotsOnTargetForAverage = 4.50m,
                HomeShotsOnTargetAgainstAverage = 4.00m,
                AwayShotsOnTargetForAverage = 4.50m,
                AwayShotsOnTargetAgainstAverage = 4.00m,

                // Head-to-Head
                HeadToHeadMatchesCount = 6,
                HeadToHeadHomeWins = 2,
                HeadToHeadDraws = 2,
                HeadToHeadAwayWins = 2,

                // Impact factors
                HomeKeyPlayersMissingImpact = 1,
                AwayKeyPlayersMissingImpact = 1,

                HomeFatigueImpact = 1,
                AwayFatigueImpact = 1
            };

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            AssertProbabilitiesSumTo100(prediction);
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));
        }

        [Test]
        public void CalculatePrediction_WithNoHeadToHeadMatches_ShouldStillCalculatePrediction()
        {
            // Arrange
            PredictionService predictionService = new PredictionService();

            Match match = CreateMatch(id: 5);

            MatchStatistics statistics = new MatchStatistics
            {
                MatchId = match.Id,

                // Recent Form - last 6
                HomeRecentWins = 4,
                HomeRecentDraws = 1,
                HomeRecentLosses = 1,

                AwayRecentWins = 2,
                AwayRecentDraws = 2,
                AwayRecentLosses = 2,

                // Home/Away Form - last 10
                HomeLast10HomeWins = 6,
                HomeLast10HomeDraws = 2,
                HomeLast10HomeLosses = 2,

                AwayLast10AwayWins = 3,
                AwayLast10AwayDraws = 3,
                AwayLast10AwayLosses = 4,

                // xG
                HomeXgForAverage = 1.80m,
                HomeXgAgainstAverage = 1.00m,
                AwayXgForAverage = 1.20m,
                AwayXgAgainstAverage = 1.50m,

                // Attack / Defense
                HomeGoalsScoredAverage = 1.70m,
                AwayGoalsScoredAverage = 1.20m,

                HomeGoalsConcededAverage = 1.00m,
                AwayGoalsConcededAverage = 1.50m,

                // Shots on Target
                HomeShotsOnTargetForAverage = 5.20m,
                HomeShotsOnTargetAgainstAverage = 3.50m,
                AwayShotsOnTargetForAverage = 4.00m,
                AwayShotsOnTargetAgainstAverage = 4.80m,

                // Head-to-Head unavailable
                HeadToHeadMatchesCount = 0,
                HeadToHeadHomeWins = 0,
                HeadToHeadDraws = 0,
                HeadToHeadAwayWins = 0,

                // Impact factors
                HomeKeyPlayersMissingImpact = 0,
                AwayKeyPlayersMissingImpact = 1,

                HomeFatigueImpact = 0,
                AwayFatigueImpact = 1
            };

            // Act
            Prediction prediction = predictionService.CalculatePrediction(match, statistics);

            // Assert
            AssertProbabilitiesSumTo100(prediction);
            Assert.That(prediction.MatchId, Is.EqualTo(match.Id));
            Assert.That(prediction.HomeWinProbability, Is.GreaterThan(prediction.AwayWinProbability));
        }

        private static Match CreateMatch(int id)
        {
            return new Match
            {
                Id = id,
                LeagueId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                KickoffTime = DateTime.UtcNow
            };
        }

        private static void AssertProbabilitiesSumTo100(Prediction prediction)
        {
            int totalProbability =
                prediction.HomeWinProbability +
                prediction.DrawProbability +
                prediction.AwayWinProbability;

            Assert.That(totalProbability, Is.EqualTo(100));
        }
    }
}