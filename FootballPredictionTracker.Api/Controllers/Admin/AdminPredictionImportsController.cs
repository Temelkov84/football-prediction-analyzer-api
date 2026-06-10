using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootballPredictionTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/prediction-imports")]
public class AdminPredictionImportsController : ControllerBase
{
    private readonly PredictionImportService _predictionImportService;

    public AdminPredictionImportsController(PredictionImportService predictionImportService)
    {
        _predictionImportService = predictionImportService;
    }

    [HttpPost]
    public async Task<IActionResult> ImportPredictions(List<ImportPredictionRequest> requests)
    {
        if (requests.Count == 0)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Import file is empty."
            });
        }

        ImportPredictionResponse response =
            await _predictionImportService.ImportPredictionsAsync(requests);

        if (response.Errors.Count > 0)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}