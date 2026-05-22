namespace FootballPredictionTracker.Api.Models;

public class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int LeagueId { get; set; }

    public League League { get; set; } = null!;
}