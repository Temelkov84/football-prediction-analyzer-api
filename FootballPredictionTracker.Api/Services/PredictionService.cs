using FootballPredictionTracker.Api.Models;

namespace FootballPredictionTracker.Api.Services
{
    public class PredictionService
    {
        private const decimal RecentFormWeight = 30m;
        private const decimal HomeAwayStrengthWeight = 25m;
        private const decimal HeadToHeadWeight = 15m;
        private const decimal AttackStrengthWeight = 20m;
        private const decimal DefenseStrengthWeight = 10m;

      

        public Prediction CalculatePrediction(Match match, MatchStatistics statistics)
        {
            var recentForm = CalculateRecentFormFactor(statistics);
            var homeAwayStrength = CalculateHomeAwayStrengthFactor(statistics);
            var headToHead = CalculateHeadToHeadFactor(statistics);
            var attackStrength = CalculateAttackStrengthFactor(statistics);
            var defenseStrength = CalculateDefenseStrengthFactor(statistics);

            decimal homeScore =
                recentForm.HomeScore * RecentFormWeight +
                homeAwayStrength.HomeScore * HomeAwayStrengthWeight +
                headToHead.HomeScore * HeadToHeadWeight +
                attackStrength.HomeScore * AttackStrengthWeight +
                defenseStrength.HomeScore * DefenseStrengthWeight;

            decimal drawScore =
                recentForm.DrawScore * RecentFormWeight +
                homeAwayStrength.DrawScore * HomeAwayStrengthWeight +
                headToHead.DrawScore * HeadToHeadWeight +
                attackStrength.DrawScore * AttackStrengthWeight +
                defenseStrength.DrawScore * DefenseStrengthWeight;

            decimal awayScore =
                recentForm.AwayScore * RecentFormWeight +
                homeAwayStrength.AwayScore * HomeAwayStrengthWeight +
                headToHead.AwayScore * HeadToHeadWeight +
                attackStrength.AwayScore * AttackStrengthWeight +
                defenseStrength.AwayScore * DefenseStrengthWeight;

            var totalScore = homeScore + drawScore + awayScore;

            if (totalScore == 0)
            {
                return new Prediction
                {
                    MatchId = match.Id,
                    HomeWinProbability = 33,
                    DrawProbability = 34,
                    AwayWinProbability = 33
                };
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

        private static FactorScore CalculateRecentFormFactor(MatchStatistics statistics)
        {
            decimal homeScore =
                statistics.HomeTeamRecentWins * 3 +
                statistics.HomeTeamRecentDraws;

            decimal awayScore =
                statistics.AwayTeamRecentWins * 3 +
                statistics.AwayTeamRecentDraws;

            decimal drawScore =
                statistics.HomeTeamRecentDraws +
                statistics.AwayTeamRecentDraws;

            return new FactorScore(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateHomeAwayStrengthFactor(MatchStatistics statistics)
        {
            decimal homeScore =
                statistics.HomeTeamHomeWins * 3 +
                statistics.HomeTeamHomeDraws;

            decimal awayScore =
                statistics.AwayTeamAwayWins * 3 +
                statistics.AwayTeamAwayDraws;

            decimal drawScore =
                statistics.HomeTeamHomeDraws +
                statistics.AwayTeamAwayDraws;

            return new FactorScore(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateHeadToHeadFactor(MatchStatistics statistics)
        {
            decimal homeScore = statistics.HeadToHeadHomeWins * 3;
            decimal drawScore = statistics.HeadToHeadDraws * 2;
            decimal awayScore = statistics.HeadToHeadAwayWins * 3;

            return new FactorScore(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateAttackStrengthFactor(MatchStatistics statistics)
        {
            decimal homeScore = statistics.HomeTeamGoalsScored;
            decimal awayScore = statistics.AwayTeamGoalsScored;

            bool hasAttackData =
                statistics.HomeTeamGoalsScored > 0 ||
                statistics.AwayTeamGoalsScored > 0;

            if (!hasAttackData)
            {
                return new FactorScore(0, 0, 0);
            }

            decimal goalDifference = Math.Abs(statistics.HomeTeamGoalsScored - statistics.AwayTeamGoalsScored);

            decimal drawScore = goalDifference <= 2 ? 2 : 0;

            return new FactorScore(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateDefenseStrengthFactor(MatchStatistics statistics)
        {
            bool hasDefenseData =
                statistics.HomeTeamGoalsConceded > 0 ||
                statistics.AwayTeamGoalsConceded > 0;

            if (!hasDefenseData)
            {
                return new FactorScore(0, 0, 0);
            }

            decimal homeScore = statistics.AwayTeamGoalsConceded;
            decimal awayScore = statistics.HomeTeamGoalsConceded;

            decimal defensiveDifference = Math.Abs(statistics.HomeTeamGoalsConceded - statistics.AwayTeamGoalsConceded);

            decimal drawScore = defensiveDifference <= 2 ? 2 : 0;

            return new FactorScore(homeScore, drawScore, awayScore);
        }

        private record FactorScore(decimal HomeScore, decimal DrawScore, decimal AwayScore);
    }
}