namespace FootballPredictionTracker.Api.DTOs;

public class AdminMatchStatisticsResponse
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string League { get; set; } = string.Empty;

    public DateTime KickoffTime { get; set; }

    public string HomeTeam { get; set; } = string.Empty;

    public string AwayTeam { get; set; } = string.Empty;

    // Recent Form - last 6 matches
    public int HomeRecentWins { get; set; }

    public int HomeRecentDraws { get; set; }

    public int HomeRecentLosses { get; set; }

    public int AwayRecentWins { get; set; }

    public int AwayRecentDraws { get; set; }

    public int AwayRecentLosses { get; set; }

    // Home/Away Form - last 10 home/away matches
    public int HomeLast10HomeWins { get; set; }

    public int HomeLast10HomeDraws { get; set; }

    public int HomeLast10HomeLosses { get; set; }

    public int AwayLast10AwayWins { get; set; }

    public int AwayLast10AwayDraws { get; set; }

    public int AwayLast10AwayLosses { get; set; }

    // xG Strength - averages from last 10 matches
    public decimal HomeXgForAverage { get; set; }

    public decimal HomeXgAgainstAverage { get; set; }

    public decimal AwayXgForAverage { get; set; }

    public decimal AwayXgAgainstAverage { get; set; }

    // Attack Strength - goal averages from last 10 matches
    public decimal HomeGoalsScoredAverage { get; set; }

    public decimal AwayGoalsScoredAverage { get; set; }

    // Defense Strength - goals conceded averages from last 10 matches
    public decimal HomeGoalsConcededAverage { get; set; }

    public decimal AwayGoalsConcededAverage { get; set; }

    // Shots on Target Strength - averages from last 10 matches
    public decimal HomeShotsOnTargetForAverage { get; set; }

    public decimal HomeShotsOnTargetAgainstAverage { get; set; }

    public decimal AwayShotsOnTargetForAverage { get; set; }

    public decimal AwayShotsOnTargetAgainstAverage { get; set; }

    // Head-to-Head - up to last 6 direct matches
    public int HeadToHeadMatchesCount { get; set; }

    public int HeadToHeadHomeWins { get; set; }

    public int HeadToHeadDraws { get; set; }

    public int HeadToHeadAwayWins { get; set; }

    // Impact factors
    // 0 = None, 1 = Low, 2 = Medium, 3 = High
    public int HomeKeyPlayersMissingImpact { get; set; }

    public int AwayKeyPlayersMissingImpact { get; set; }

    public int HomeFatigueImpact { get; set; }

    public int AwayFatigueImpact { get; set; }
}