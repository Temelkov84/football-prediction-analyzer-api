using System.ComponentModel.DataAnnotations;

namespace FootballPredictionTracker.Api.DTOs
{
    public class ImportPredictionRequest
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string LeagueName { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string HomeTeamName { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string AwayTeamName { get; set; } = string.Empty;

        [Required]
        public DateTime KickoffTime { get; set; }

        // Recent Form - last 6
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

        // Home/Away Form - last 10
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

        // xG
        [Range(typeof(decimal), "0", "10")]
        public decimal HomeXgForAverage { get; set; }

        [Range(typeof(decimal), "0", "10")]
        public decimal HomeXgAgainstAverage { get; set; }

        [Range(typeof(decimal), "0", "10")]
        public decimal AwayXgForAverage { get; set; }

        [Range(typeof(decimal), "0", "10")]
        public decimal AwayXgAgainstAverage { get; set; }

        // Attack / Defense
        [Range(typeof(decimal), "0", "10")]
        public decimal HomeGoalsScoredAverage { get; set; }

        [Range(typeof(decimal), "0", "10")]
        public decimal AwayGoalsScoredAverage { get; set; }

        [Range(typeof(decimal), "0", "10")]
        public decimal HomeGoalsConcededAverage { get; set; }

        [Range(typeof(decimal), "0", "10")]
        public decimal AwayGoalsConcededAverage { get; set; }

        // Shots on Target
        [Range(typeof(decimal), "0", "30")]
        public decimal HomeShotsOnTargetForAverage { get; set; }

        [Range(typeof(decimal), "0", "30")]
        public decimal HomeShotsOnTargetAgainstAverage { get; set; }

        [Range(typeof(decimal), "0", "30")]
        public decimal AwayShotsOnTargetForAverage { get; set; }

        [Range(typeof(decimal), "0", "30")]
        public decimal AwayShotsOnTargetAgainstAverage { get; set; }

        // Head-to-Head
        [Range(0, 6)]
        public int HeadToHeadMatchesCount { get; set; }

        [Range(0, 6)]
        public int HeadToHeadHomeWins { get; set; }

        [Range(0, 6)]
        public int HeadToHeadDraws { get; set; }

        [Range(0, 6)]
        public int HeadToHeadAwayWins { get; set; }

        // Impact factors: 0 none, 1 low, 2 medium, 3 high
        [Range(0, 3)]
        public int HomeKeyPlayersMissingImpact { get; set; }

        [Range(0, 3)]
        public int AwayKeyPlayersMissingImpact { get; set; }

        [Range(0, 3)]
        public int HomeFatigueImpact { get; set; }

        [Range(0, 3)]
        public int AwayFatigueImpact { get; set; }
    }
}