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

    [Range(0, 100)]
    public int HomeTeamHomeWins { get; set; }

    [Range(0, 100)]
    public int HomeTeamHomeDraws { get; set; }

    [Range(0, 100)]
    public int HomeTeamHomeLosses { get; set; }

    [Range(0, 100)]
    public int AwayTeamAwayWins { get; set; }

    [Range(0, 100)]
    public int AwayTeamAwayDraws { get; set; }

    [Range(0, 100)]
    public int AwayTeamAwayLosses { get; set; }

    [Range(0, 100)]
    public int HomeTeamGoalsScored { get; set; }

    [Range(0, 100)]
    public int HomeTeamGoalsConceded { get; set; }

    [Range(0, 100)]
    public int AwayTeamGoalsScored { get; set; }

    [Range(0, 100)]
    public int AwayTeamGoalsConceded { get; set; }
}