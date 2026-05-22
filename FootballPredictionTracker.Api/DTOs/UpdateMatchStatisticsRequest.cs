namespace FootballPredictionTracker.Api.DTOs;

public class UpdateMatchStatisticsRequest
{
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