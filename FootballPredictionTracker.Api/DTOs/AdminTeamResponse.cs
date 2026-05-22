namespace FootballPredictionTracker.Api.DTOs;

public class AdminTeamResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int LeagueId { get; set; }

    public string League { get; set; } = string.Empty;
}