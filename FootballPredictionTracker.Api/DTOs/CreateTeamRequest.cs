using System.ComponentModel.DataAnnotations;

namespace FootballPredictionTracker.Api.DTOs;

public class CreateTeamRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int LeagueId { get; set; }
}