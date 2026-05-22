namespace FootballPredictionTracker.Api.Models;

public class MatchStatistics
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public Match Match { get; set; } = null!;

    public int HomeTeamRecentWins { get; set; }

    public int HomeTeamRecentDraws { get; set; }

    public int HomeTeamRecentLosses { get; set; }

    public int AwayTeamRecentWins { get; set; }

    public int AwayTeamRecentDraws { get; set; }

    public int AwayTeamRecentLosses { get; set; }

    public int HeadToHeadHomeWins { get; set; }

    public int HeadToHeadDraws { get; set; }

    public int HeadToHeadAwayWins { get; set; }

    public int HomeTeamHomeWins { get; set; }

    public int HomeTeamHomeDraws { get; set; }

    public int HomeTeamHomeLosses { get; set; }

    public int AwayTeamAwayWins { get; set; }

    public int AwayTeamAwayDraws { get; set; }

    public int AwayTeamAwayLosses { get; set; }

    public int HomeTeamGoalsScored { get; set; }

    public int HomeTeamGoalsConceded { get; set; }

    public int AwayTeamGoalsScored { get; set; }

    public int AwayTeamGoalsConceded { get; set; }
}