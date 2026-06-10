using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/prediction-weights")]
public class AdminPredictionWeightsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminPredictionWeightsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetPredictionWeights()
    {
        var weights = await _dbContext.PredictionParameterWeights
            .OrderBy(weight => weight.Id)
            .Select(weight => new AdminPredictionWeightResponse
            {
                Id = weight.Id,
                Key = weight.Key,
                Name = weight.Name,
                Value = weight.Value,
                IsActive = weight.IsActive
            })
            .ToListAsync();

        return Ok(weights);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePredictionWeight(
        int id,
        UpdatePredictionWeightRequest request)
    {
        var weight = await _dbContext.PredictionParameterWeights
            .FirstOrDefaultAsync(weight => weight.Id == id);

        if (weight == null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Prediction weight does not exist."
            });
        }

        weight.Value = request.Value;
        weight.IsActive = request.IsActive;

        await _dbContext.SaveChangesAsync();

        var response = new AdminPredictionWeightResponse
        {
            Id = weight.Id,
            Key = weight.Key,
            Name = weight.Name,
            Value = weight.Value,
            IsActive = weight.IsActive
        };

        return Ok(response);
    }
}