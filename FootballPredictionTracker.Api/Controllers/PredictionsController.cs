using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public PredictionsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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