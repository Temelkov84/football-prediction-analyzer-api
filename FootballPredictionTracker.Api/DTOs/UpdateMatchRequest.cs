using System.ComponentModel.DataAnnotations;

namespace FootballPredictionTracker.Api.DTOs;

public class UpdateMatchRequest
{
    [Range(1, int.MaxValue)]
    public int LeagueId { get; set; }

    [Range(1, int.MaxValue)]
    public int HomeTeamId { get; set; }

    [Range(1, int.MaxValue)]
    public int AwayTeamId { get; set; }

    [Required]
    public DateTime KickoffTime { get; set; }
}