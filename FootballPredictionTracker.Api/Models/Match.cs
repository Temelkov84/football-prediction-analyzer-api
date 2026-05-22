namespace FootballPredictionTracker.Api.Models;

public class Match
{
    public int Id { get; set; }

    public int LeagueId { get; set; }

    public League League { get; set; } = null!;

    public int HomeTeamId { get; set; }

    public Team HomeTeam { get; set; } = null!;

    public int AwayTeamId { get; set; }

    public Team AwayTeam { get; set; } = null!;

    public DateTime KickoffTime { get; set; }
}