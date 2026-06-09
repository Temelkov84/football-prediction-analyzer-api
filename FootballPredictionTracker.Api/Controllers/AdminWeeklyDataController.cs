using FootballPredictionTracker.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Controllers;

[ApiController]
[Route("api/admin/weekly-data")]
public class AdminWeeklyDataController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminWeeklyDataController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpDelete]
    public async Task<IActionResult> ClearWeeklyData()
    {
        var predictions = await _dbContext.Predictions.ToListAsync();
        var matchStatistics = await _dbContext.MatchStatistics.ToListAsync();
        var matches = await _dbContext.Matches.ToListAsync();

        _dbContext.Predictions.RemoveRange(predictions);
        _dbContext.MatchStatistics.RemoveRange(matchStatistics);
        _dbContext.Matches.RemoveRange(matches);

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            Message = "Weekly data cleared successfully.",
            DeletedPredictions = predictions.Count,
            DeletedMatchStatistics = matchStatistics.Count,
            DeletedMatches = matches.Count
        });
    }
}