namespace FootballPredictionTracker.Api.DTOs
{
    public class ImportPredictionResponse
    {
        public int CreatedMatches { get; set; }

        public int CreatedStatistics { get; set; }

        public int CreatedPredictions { get; set; }

        public List<ImportedPredictionResponse> ImportedPredictions { get; set; } = new();

        public List<string> Errors { get; set; } = new();
    }
}