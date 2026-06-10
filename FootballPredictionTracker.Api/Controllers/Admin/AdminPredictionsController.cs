using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballPredictionTracker.Api.DTOs;

namespace FootballPredictionTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/predictions")]
public class AdminPredictionsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly PredictionService _predictionService;

    public AdminPredictionsController(
        ApplicationDbContext dbContext,
        PredictionService predictionService)
    {
        _dbContext = dbContext;
        _predictionService = predictionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPredictions()
    {
        var predictions = await _dbContext.Predictions
            .Include(p => p.Match)
                .ThenInclude(m => m.League)
            .Include(p => p.Match)
                .ThenInclude(m => m.HomeTeam)
            .Include(p => p.Match)
                .ThenInclude(m => m.AwayTeam)
            .OrderBy(p => p.Match.League.Name)
            .ThenBy(p => p.Match.KickoffTime)
            .Select(p => new AdminPredictionResponse
            {
                Id = p.Id,
                League = p.Match.League.Name,
                KickoffTime = p.Match.KickoffTime,
                HomeTeam = p.Match.HomeTeam.Name,
                AwayTeam = p.Match.AwayTeam.Name,
                HomeWinProbability = p.HomeWinProbability,
                DrawProbability = p.DrawProbability,
                AwayWinProbability = p.AwayWinProbability,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(predictions);
    }

    [HttpPost("calculate/{matchId}")]
    public async Task<IActionResult> CalculatePrediction(int matchId)
    {
        var match = await _dbContext.Matches
            .Include(m => m.League)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .FirstOrDefaultAsync(m => m.Id == matchId);

        if (match == null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Match does not exist."
            });
        }

        var statistics = await _dbContext.MatchStatistics
            .FirstOrDefaultAsync(s => s.MatchId == matchId);

        if (statistics == null)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Statistics do not exist for this match."
            });
        }

        var existingPrediction = await _dbContext.Predictions
            .FirstOrDefaultAsync(p => p.MatchId == matchId);

        if (existingPrediction != null)
        {
            _dbContext.Predictions.Remove(existingPrediction);
        }

        var activeWeights = await _dbContext.PredictionParameterWeights
    .Where(weight => weight.IsActive)
    .ToDictionaryAsync(
        weight => weight.Key,
        weight => weight.Value);

        var prediction = _predictionService.CalculatePrediction(match, statistics, activeWeights);

        _dbContext.Predictions.Add(prediction);
        await _dbContext.SaveChangesAsync();

        var response = new AdminPredictionResponse
        {
            Id = prediction.Id,
            League = match.League.Name,
            KickoffTime = match.KickoffTime,
            HomeTeam = match.HomeTeam.Name,
            AwayTeam = match.AwayTeam.Name,
            HomeWinProbability = prediction.HomeWinProbability,
            DrawProbability = prediction.DrawProbability,
            AwayWinProbability = prediction.AwayWinProbability,
            CreatedAt = prediction.CreatedAt
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrediction(int id)
    {
        var prediction = await _dbContext.Predictions
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prediction == null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Prediction does not exist."
            });
        }

        _dbContext.Predictions.Remove(prediction);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}