using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballPredictionTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionsController : ControllerBase
    {
        private readonly PredictionService predictionService;

        public PredictionsController(PredictionService predictionService)
        {
            this.predictionService = predictionService;
        }
        [HttpPost("calculate")]
        public ActionResult<PredictionResponse> CalculatePrediction(PredictionRequest request)
        {
            if (request.HomeTeam.Trim().Equals(request.AwayTeam.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Home team and away team must be different.");
            }

            PredictionResponse response = predictionService.CalculatePrediction(request);

            return Ok(response);
        }
    }
}