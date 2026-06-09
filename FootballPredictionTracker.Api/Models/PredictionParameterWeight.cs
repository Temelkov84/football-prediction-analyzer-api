namespace FootballPredictionTracker.Api.Models;

public class PredictionParameterWeight
{
    public int Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Value { get; set; }

    public bool IsActive { get; set; } = true;
}