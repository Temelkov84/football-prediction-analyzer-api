using System.ComponentModel.DataAnnotations;

namespace FootballPredictionTracker.Api.DTOs
{
    public class UpdatePredictionWeightRequest
    {
        [Range(typeof(decimal), "0", "100", ErrorMessage = "Weight value must be between 0 and 100.")]
        public decimal Value { get; set; }

        public bool IsActive { get; set; }
    }
}