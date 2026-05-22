namespace FootballPredictionTracker.Api.DTOs;

public class UpdateTeamRequest
{
    public string Name { get; set; } = string.Empty;

    public int LeagueId { get; set; }
}