using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionsController : ControllerBase
    {
        private readonly PredictionService predictionService;
        private readonly ApplicationDbContext dbContext;

        public PredictionsController(
            PredictionService predictionService,
            ApplicationDbContext dbContext)
        {
            this.predictionService = predictionService;
            this.dbContext = dbContext;
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

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyPredictions()
        {
            var today = DateTime.Today;
            var nextWeek = today.AddDays(7);

            var predictions = await dbContext.Predictions
                .Include(p => p.Match)
                    .ThenInclude(m => m.League)
                .Include(p => p.Match)
                    .ThenInclude(m => m.HomeTeam)
                .Include(p => p.Match)
                    .ThenInclude(m => m.AwayTeam)
                .Where(p => p.Match.KickoffTime >= today && p.Match.KickoffTime <= nextWeek)
                .OrderBy(p => p.Match.League.Name)
                .ThenBy(p => p.Match.KickoffTime)
                .Select(p => new WeeklyPredictionResponse
                {
                    League = p.Match.League.Name,
                    KickoffTime = p.Match.KickoffTime,
                    HomeTeam = p.Match.HomeTeam.Name,
                    AwayTeam = p.Match.AwayTeam.Name,
                    HomeWinProbability = p.HomeWinProbability,
                    DrawProbability = p.DrawProbability,
                    AwayWinProbability = p.AwayWinProbability
                })
                .ToListAsync();

            return Ok(predictions);
        }
    }
}