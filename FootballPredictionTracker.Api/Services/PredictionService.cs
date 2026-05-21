using FootballPredictionTracker.Api.DTOs;

namespace FootballPredictionTracker.Api.Services
{
    public class PredictionService
    {
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
    }
}