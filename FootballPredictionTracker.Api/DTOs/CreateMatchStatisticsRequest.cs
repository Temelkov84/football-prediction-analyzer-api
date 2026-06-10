using System.ComponentModel.DataAnnotations;

namespace FootballPredictionTracker.Api.DTOs;

public class CreateMatchStatisticsRequest
{
    [Range(1, int.MaxValue)]
    public int MatchId { get; set; }

    // Recent Form - last 6 matches
    [Range(0, 6)]
    public int HomeRecentWins { get; set; }

    [Range(0, 6)]
    public int HomeRecentDraws { get; set; }

    [Range(0, 6)]
    public int HomeRecentLosses { get; set; }

    [Range(0, 6)]
    public int AwayRecentWins { get; set; }

    [Range(0, 6)]
    public int AwayRecentDraws { get; set; }

    [Range(0, 6)]
    public int AwayRecentLosses { get; set; }

    // Home/Away Form - last 10 home/away matches
    [Range(0, 10)]
    public int HomeLast10HomeWins { get; set; }

    [Range(0, 10)]
    public int HomeLast10HomeDraws { get; set; }

    [Range(0, 10)]
    public int HomeLast10HomeLosses { get; set; }

    [Range(0, 10)]
    public int AwayLast10AwayWins { get; set; }

    [Range(0, 10)]
    public int AwayLast10AwayDraws { get; set; }

    [Range(0, 10)]
    public int AwayLast10AwayLosses { get; set; }

    // xG Strength - averages from last 10 matches
    [Range(0, 20)]
    public decimal HomeXgForAverage { get; set; }

    [Range(0, 20)]
    public decimal HomeXgAgainstAverage { get; set; }

    [Range(0, 20)]
    public decimal AwayXgForAverage { get; set; }

    [Range(0, 20)]
    public decimal AwayXgAgainstAverage { get; set; }

    // Attack Strength - goal averages from last 10 matches
    [Range(0, 20)]
    public decimal HomeGoalsScoredAverage { get; set; }

    [Range(0, 20)]
    public decimal AwayGoalsScoredAverage { get; set; }

    // Defense Strength - goals conceded averages from last 10 matches
    [Range(0, 20)]
    public decimal HomeGoalsConcededAverage { get; set; }

    [Range(0, 20)]
    public decimal AwayGoalsConcededAverage { get; set; }

    // Shots on Target Strength - averages from last 10 matches
    [Range(0, 50)]
    public decimal HomeShotsOnTargetForAverage { get; set; }

    [Range(0, 50)]
    public decimal HomeShotsOnTargetAgainstAverage { get; set; }

    [Range(0, 50)]
    public decimal AwayShotsOnTargetForAverage { get; set; }

    [Range(0, 50)]
    public decimal AwayShotsOnTargetAgainstAverage { get; set; }

    // Head-to-Head - up to last 6 direct matches
    [Range(0, 6)]
    public int HeadToHeadMatchesCount { get; set; }

    [Range(0, 6)]
    public int HeadToHeadHomeWins { get; set; }

    [Range(0, 6)]
    public int HeadToHeadDraws { get; set; }

    [Range(0, 6)]
    public int HeadToHeadAwayWins { get; set; }

    // Impact factors
    // 0 = None, 1 = Low, 2 = Medium, 3 = High
    [Range(0, 3)]
    public int HomeKeyPlayersMissingImpact { get; set; }

    [Range(0, 3)]
    public int AwayKeyPlayersMissingImpact { get; set; }

    [Range(0, 3)]
    public int HomeFatigueImpact { get; set; }

    [Range(0, 3)]
    public int AwayFatigueImpact { get; set; }
}