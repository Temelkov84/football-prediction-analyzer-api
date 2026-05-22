namespace FootballPredictionTracker.Api.Models;

public class PredictionParameterWeight
{
    public int Id { get; set; }

    public string ParameterName { get; set; } = string.Empty;

    public decimal Weight { get; set; }

    public bool IsActive { get; set; } = true;
}