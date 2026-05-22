namespace FootballPredictionTracker.Api.DTOs;

public class AdminMatchResponse
{
    public int Id { get; set; }

    public string League { get; set; } = string.Empty;

    public string HomeTeam { get; set; } = string.Empty;

    public string AwayTeam { get; set; } = string.Empty;

    public DateTime KickoffTime { get; set; }
}