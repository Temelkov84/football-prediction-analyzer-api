namespace FootballPredictionTracker.Api.DTOs
{
    public class PredictionResponse
    {
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;

        public int HomeWinProbability { get; set; }
        public int DrawProbability { get; set; }
        public int AwayWinProbability { get; set; }

        public string Explanation { get; set; } = string.Empty;
    }
}