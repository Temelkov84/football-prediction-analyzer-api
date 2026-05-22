namespace FootballPredictionTracker.Api.DTOs;

public class CreateTeamRequest
{
    public string Name { get; set; } = string.Empty;

    public int LeagueId { get; set; }
}