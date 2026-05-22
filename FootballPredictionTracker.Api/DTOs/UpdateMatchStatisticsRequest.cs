using System.ComponentModel.DataAnnotations;

namespace FootballPredictionTracker.Api.DTOs;

public class UpdateMatchStatisticsRequest
{
    [Range(0, 100)]
    public int HomeTeamRecentWins { get; set; }

    [Range(0, 100)]
    public int HomeTeamRecentDraws { get; set; }

    [Range(0, 100)]
    public int HomeTeamRecentLosses { get; set; }

    [Range(0, 100)]
    public int AwayTeamRecentWins { get; set; }

    [Range(0, 100)]
    public int AwayTeamRecentDraws { get; set; }

    [Range(0, 100)]
    public int AwayTeamRecentLosses { get; set; }

    [Range(0, 100)]
    public int HeadToHeadHomeWins { get; set; }

    [Range(0, 100)]
    public int HeadToHeadDraws { get; set; }

    [Range(0, 100)]
    public int HeadToHeadAwayWins { get; set; }
}