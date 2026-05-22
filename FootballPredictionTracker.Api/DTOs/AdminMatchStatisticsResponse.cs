namespace FootballPredictionTracker.Api.DTOs;

public class AdminMatchStatisticsResponse
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string League { get; set; } = string.Empty;

    public DateTime KickoffTime { get; set; }

    public string HomeTeam { get; set; } = string.Empty;

    public string AwayTeam { get; set; } = string.Empty;

    public int HomeTeamRecentWins { get; set; }

    public int HomeTeamRecentDraws { get; set; }

    public int HomeTeamRecentLosses { get; set; }

    public int AwayTeamRecentWins { get; set; }

    public int AwayTeamRecentDraws { get; set; }

    public int AwayTeamRecentLosses { get; set; }

    public int HeadToHeadHomeWins { get; set; }

    public int HeadToHeadDraws { get; set; }

    public int HeadToHeadAwayWins { get; set; }
}