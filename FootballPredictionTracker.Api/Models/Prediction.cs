namespace FootballPredictionTracker.Api.Models;

public class Prediction
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public Match Match { get; set; } = null!;

    public int HomeWinProbability { get; set; }

    public int DrawProbability { get; set; }

    public int AwayWinProbability { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}