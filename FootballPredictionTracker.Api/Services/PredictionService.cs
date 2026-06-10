using FootballPredictionTracker.Api.Models;

namespace FootballPredictionTracker.Api.Services
{
    public class PredictionService
    {
        private const decimal RecentFormWeight = 16m;
        private const decimal HomeAwayFormWeight = 18m;
        private const decimal XgWeight = 16m;
        private const decimal AttackStrengthWeight = 11m;
        private const decimal DefenseStrengthWeight = 12m;
        private const decimal ShotsOnTargetWeight = 11m;
        private const decimal HeadToHeadWeight = 6m;
        private const decimal KeyPlayersMissingWeight = 5m;
        private const decimal FatigueWeight = 5m;

        public Prediction CalculatePrediction(Match match, MatchStatistics statistics)
        {
            var recentForm = CalculateRecentFormFactor(statistics);
            var homeAwayForm = CalculateHomeAwayFormFactor(statistics);
            var xg = CalculateXgFactor(statistics);
            var attackStrength = CalculateAttackStrengthFactor(statistics);
            var defenseStrength = CalculateDefenseStrengthFactor(statistics);
            var shotsOnTarget = CalculateShotsOnTargetFactor(statistics);
            var headToHead = CalculateHeadToHeadFactor(statistics);
            var keyPlayersMissing = CalculateKeyPlayersMissingFactor(statistics);
            var fatigue = CalculateFatigueFactor(statistics);

            decimal homeScore = 0;
            decimal drawScore = 0;
            decimal awayScore = 0;
            decimal activeWeightTotal = 0;

            AddFactorScore(recentForm, RecentFormWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(homeAwayForm, HomeAwayFormWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(xg, XgWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(attackStrength, AttackStrengthWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(defenseStrength, DefenseStrengthWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(shotsOnTarget, ShotsOnTargetWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(headToHead, HeadToHeadWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(keyPlayersMissing, KeyPlayersMissingWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);
            AddFactorScore(fatigue, FatigueWeight, ref homeScore, ref drawScore, ref awayScore, ref activeWeightTotal);

            var totalScore = homeScore + drawScore + awayScore;

            if (totalScore == 0 || activeWeightTotal == 0)
            {
                return CreateBalancedPrediction(match.Id);
            }

            var homeProbability = (int)Math.Round(homeScore / totalScore * 100);
            var drawProbability = (int)Math.Round(drawScore / totalScore * 100);
            var awayProbability = 100 - homeProbability - drawProbability;

            return new Prediction
            {
                MatchId = match.Id,
                HomeWinProbability = homeProbability,
                DrawProbability = drawProbability,
                AwayWinProbability = awayProbability
            };
        }

        private static void AddFactorScore(
            FactorScore factor,
            decimal weight,
            ref decimal homeScore,
            ref decimal drawScore,
            ref decimal awayScore,
            ref decimal activeWeightTotal)
        {
            if (!factor.IsAvailable)
            {
                return;
            }

            homeScore += factor.HomeScore * weight;
            drawScore += factor.DrawScore * weight;
            awayScore += factor.AwayScore * weight;
            activeWeightTotal += weight;
        }

        private static Prediction CreateBalancedPrediction(int matchId)
        {
            return new Prediction
            {
                MatchId = matchId,
                HomeWinProbability = 33,
                DrawProbability = 34,
                AwayWinProbability = 33
            };
        }

        private static FactorScore CalculateRecentFormFactor(MatchStatistics statistics)
        {
            decimal homeScore =
                statistics.HomeRecentWins * 3 +
                statistics.HomeRecentDraws;

            decimal awayScore =
                statistics.AwayRecentWins * 3 +
                statistics.AwayRecentDraws;

            decimal drawScore =
                statistics.HomeRecentDraws +
                statistics.AwayRecentDraws;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateHomeAwayFormFactor(MatchStatistics statistics)
        {
            decimal homeScore =
                statistics.HomeLast10HomeWins * 3 +
                statistics.HomeLast10HomeDraws;

            decimal awayScore =
                statistics.AwayLast10AwayWins * 3 +
                statistics.AwayLast10AwayDraws;

            decimal drawScore =
                statistics.HomeLast10HomeDraws +
                statistics.AwayLast10AwayDraws;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateXgFactor(MatchStatistics statistics)
        {
            bool hasXgData =
                statistics.HomeXgForAverage > 0 ||
                statistics.HomeXgAgainstAverage > 0 ||
                statistics.AwayXgForAverage > 0 ||
                statistics.AwayXgAgainstAverage > 0;

            if (!hasXgData)
            {
                return FactorScore.Unavailable();
            }

            decimal homeScore =
                statistics.HomeXgForAverage +
                statistics.AwayXgAgainstAverage;

            decimal awayScore =
                statistics.AwayXgForAverage +
                statistics.HomeXgAgainstAverage;

            decimal difference = Math.Abs(homeScore - awayScore);

            decimal drawScore = difference <= 0.30m ? 2m : 0m;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateAttackStrengthFactor(MatchStatistics statistics)
        {
            bool hasAttackData =
                statistics.HomeGoalsScoredAverage > 0 ||
                statistics.AwayGoalsScoredAverage > 0;

            if (!hasAttackData)
            {
                return FactorScore.Unavailable();
            }

            decimal homeScore = statistics.HomeGoalsScoredAverage;
            decimal awayScore = statistics.AwayGoalsScoredAverage;

            decimal difference = Math.Abs(homeScore - awayScore);

            decimal drawScore = difference <= 0.30m ? 1m : 0m;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateDefenseStrengthFactor(MatchStatistics statistics)
        {
            bool hasDefenseData =
                statistics.HomeGoalsConcededAverage > 0 ||
                statistics.AwayGoalsConcededAverage > 0;

            if (!hasDefenseData)
            {
                return FactorScore.Unavailable();
            }

            decimal homeScore = statistics.AwayGoalsConcededAverage;
            decimal awayScore = statistics.HomeGoalsConcededAverage;

            decimal difference = Math.Abs(statistics.HomeGoalsConcededAverage - statistics.AwayGoalsConcededAverage);

            decimal drawScore = difference <= 0.30m ? 1m : 0m;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateShotsOnTargetFactor(MatchStatistics statistics)
        {
            bool hasShotsData =
                statistics.HomeShotsOnTargetForAverage > 0 ||
                statistics.HomeShotsOnTargetAgainstAverage > 0 ||
                statistics.AwayShotsOnTargetForAverage > 0 ||
                statistics.AwayShotsOnTargetAgainstAverage > 0;

            if (!hasShotsData)
            {
                return FactorScore.Unavailable();
            }

            decimal homeScore =
                statistics.HomeShotsOnTargetForAverage +
                statistics.AwayShotsOnTargetAgainstAverage;

            decimal awayScore =
                statistics.AwayShotsOnTargetForAverage +
                statistics.HomeShotsOnTargetAgainstAverage;

            decimal difference = Math.Abs(homeScore - awayScore);

            decimal drawScore = difference <= 1m ? 1m : 0m;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateHeadToHeadFactor(MatchStatistics statistics)
        {
            if (statistics.HeadToHeadMatchesCount == 0)
            {
                return FactorScore.Unavailable();
            }

            decimal homeScore = statistics.HeadToHeadHomeWins * 3;
            decimal drawScore = statistics.HeadToHeadDraws * 2;
            decimal awayScore = statistics.HeadToHeadAwayWins * 3;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateKeyPlayersMissingFactor(MatchStatistics statistics)
        {
            bool hasKeyPlayersData =
                statistics.HomeKeyPlayersMissingImpact > 0 ||
                statistics.AwayKeyPlayersMissingImpact > 0;

            if (!hasKeyPlayersData)
            {
                return FactorScore.Unavailable();
            }

            decimal homeScore = 3 - statistics.HomeKeyPlayersMissingImpact;
            decimal awayScore = 3 - statistics.AwayKeyPlayersMissingImpact;

            decimal difference = Math.Abs(statistics.HomeKeyPlayersMissingImpact - statistics.AwayKeyPlayersMissingImpact);

            decimal drawScore = difference == 0 ? 1m : 0m;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateFatigueFactor(MatchStatistics statistics)
        {
            bool hasFatigueData =
                statistics.HomeFatigueImpact > 0 ||
                statistics.AwayFatigueImpact > 0;

            if (!hasFatigueData)
            {
                return FactorScore.Unavailable();
            }

            decimal homeScore = 3 - statistics.HomeFatigueImpact;
            decimal awayScore = 3 - statistics.AwayFatigueImpact;

            decimal difference = Math.Abs(statistics.HomeFatigueImpact - statistics.AwayFatigueImpact);

            decimal drawScore = difference == 0 ? 1m : 0m;

            return FactorScore.Available(homeScore, drawScore, awayScore);
        }

        private record FactorScore(
            decimal HomeScore,
            decimal DrawScore,
            decimal AwayScore,
            bool IsAvailable)
        {
            public static FactorScore Available(decimal homeScore, decimal drawScore, decimal awayScore)
            {
                return new FactorScore(homeScore, drawScore, awayScore, true);
            }

            public static FactorScore Unavailable()
            {
                return new FactorScore(0, 0, 0, false);
            }
        }
    }
}