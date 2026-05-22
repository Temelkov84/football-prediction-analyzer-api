namespace FootballPredictionTracker.Api.DTOs;

public class CreateMatchRequest
{
    public int LeagueId { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public DateTime KickoffTime { get; set; }
}