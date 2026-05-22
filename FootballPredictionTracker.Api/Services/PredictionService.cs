using FootballPredictionTracker.Api.DTOs;
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

        public PredictionResponse CalculatePrediction(PredictionRequest request)
        {
            int homeScore = 0;
            int drawScore = 0;
            int awayScore = 0;

            homeScore += request.HeadToHeadHomeWins * 2;
            drawScore += request.HeadToHeadDraws * 2;
            awayScore += request.HeadToHeadAwayWins * 2;

            homeScore += request.HomeTeamRecentWins * 3;
            drawScore += request.HomeTeamRecentDraws;
            awayScore += request.HomeTeamRecentLosses * 2;

            awayScore += request.AwayTeamRecentWins * 3;
            drawScore += request.AwayTeamRecentDraws;
            homeScore += request.AwayTeamRecentLosses * 2;

            int totalScore = homeScore + drawScore + awayScore;

            if (totalScore == 0)
            {
                return new PredictionResponse
                {
                    HomeTeam = request.HomeTeam,
                    AwayTeam = request.AwayTeam,
                    HomeWinProbability = 33,
                    DrawProbability = 34,
                    AwayWinProbability = 33,
                    Explanation = "Not enough statistical data. Probabilities are balanced by default."
                };
            }

            int homeProbability = (int)Math.Round((double)homeScore / totalScore * 100);
            int drawProbability = (int)Math.Round((double)drawScore / totalScore * 100);
            int awayProbability = 100 - homeProbability - drawProbability;

            return new PredictionResponse
            {
                HomeTeam = request.HomeTeam,
                AwayTeam = request.AwayTeam,
                HomeWinProbability = homeProbability,
                DrawProbability = drawProbability,
                AwayWinProbability = awayProbability,
                Explanation = GenerateExplanation(homeProbability, drawProbability, awayProbability)
            };
        }

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

            decimal goalDifference = Math.Abs(statistics.HomeTeamGoalsScored - statistics.AwayTeamGoalsScored);

            decimal drawScore = goalDifference <= 2 ? 2 : 0;

            return new FactorScore(homeScore, drawScore, awayScore);
        }

        private static FactorScore CalculateDefenseStrengthFactor(MatchStatistics statistics)
        {
            decimal homeScore = statistics.AwayTeamGoalsConceded;
            decimal awayScore = statistics.HomeTeamGoalsConceded;

            decimal defensiveDifference = Math.Abs(statistics.HomeTeamGoalsConceded - statistics.AwayTeamGoalsConceded);

            decimal drawScore = defensiveDifference <= 2 ? 2 : 0;

            return new FactorScore(homeScore, drawScore, awayScore);
        }

        private string GenerateExplanation(int homeProbability, int drawProbability, int awayProbability)
        {
            if (homeProbability > drawProbability && homeProbability > awayProbability)
            {
                return "The home team has the strongest statistical advantage based on the provided data.";
            }

            if (awayProbability > homeProbability && awayProbability > drawProbability)
            {
                return "The away team has the strongest statistical advantage based on the provided data.";
            }

            return "The provided statistics suggest a balanced match with no clear dominant outcome.";
        }

        private record FactorScore(decimal HomeScore, decimal DrawScore, decimal AwayScore);
    }
}