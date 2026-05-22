namespace FootballPredictionTracker.Api.DTOs;

public class AdminPredictionResponse
{
    public int Id { get; set; }

    public string League { get; set; } = string.Empty;

    public DateTime KickoffTime { get; set; }

    public string HomeTeam { get; set; } = string.Empty;

    public string AwayTeam { get; set; } = string.Empty;

    public int HomeWinProbability { get; set; }

    public int DrawProbability { get; set; }

    public int AwayWinProbability { get; set; }

    public DateTime CreatedAt { get; set; }
}