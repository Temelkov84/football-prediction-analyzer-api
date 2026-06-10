using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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

    [HttpPost("csv")]
    public async Task<IActionResult> ImportPredictionsFromCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "CSV file is empty."
            });
        }

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Only CSV files are supported."
            });
        }

        List<ImportPredictionRequest> requests;

        try
        {
            requests = await ParseCsvFileAsync(file);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message
            });
        }

        if (requests.Count == 0)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "CSV file does not contain any import rows."
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

    private static async Task<List<ImportPredictionRequest>> ParseCsvFileAsync(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());

        string? headerLine = await reader.ReadLineAsync();

        if (string.IsNullOrWhiteSpace(headerLine))
        {
            throw new InvalidOperationException("CSV file is missing header row.");
        }

        string[] headers = SplitCsvLine(headerLine);

        var headerIndexes = headers
            .Select((header, index) => new
            {
                Header = header.Trim(),
                Index = index
            })
            .ToDictionary(
                item => item.Header,
                item => item.Index,
                StringComparer.OrdinalIgnoreCase);

        ValidateRequiredHeaders(headerIndexes);

        var requests = new List<ImportPredictionRequest>();

        int rowNumber = 1;

        while (!reader.EndOfStream)
        {
            rowNumber++;

            string? line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] values = SplitCsvLine(line);

            ImportPredictionRequest request = new ImportPredictionRequest
            {
                LeagueName = GetString(values, headerIndexes, "league_name", rowNumber),
                HomeTeamName = GetString(values, headerIndexes, "home_team_name", rowNumber),
                AwayTeamName = GetString(values, headerIndexes, "away_team_name", rowNumber),
                KickoffTime = GetDateTime(values, headerIndexes, "kickoff_time", rowNumber),

                HomeRecentWins = GetInt(values, headerIndexes, "home_recent_wins", rowNumber),
                HomeRecentDraws = GetInt(values, headerIndexes, "home_recent_draws", rowNumber),
                HomeRecentLosses = GetInt(values, headerIndexes, "home_recent_losses", rowNumber),

                AwayRecentWins = GetInt(values, headerIndexes, "away_recent_wins", rowNumber),
                AwayRecentDraws = GetInt(values, headerIndexes, "away_recent_draws", rowNumber),
                AwayRecentLosses = GetInt(values, headerIndexes, "away_recent_losses", rowNumber),

                HomeLast10HomeWins = GetInt(values, headerIndexes, "home_last10_home_wins", rowNumber),
                HomeLast10HomeDraws = GetInt(values, headerIndexes, "home_last10_home_draws", rowNumber),
                HomeLast10HomeLosses = GetInt(values, headerIndexes, "home_last10_home_losses", rowNumber),

                AwayLast10AwayWins = GetInt(values, headerIndexes, "away_last10_away_wins", rowNumber),
                AwayLast10AwayDraws = GetInt(values, headerIndexes, "away_last10_away_draws", rowNumber),
                AwayLast10AwayLosses = GetInt(values, headerIndexes, "away_last10_away_losses", rowNumber),

                HomeXgForAverage = GetDecimal(values, headerIndexes, "home_xg_for_average", rowNumber),
                HomeXgAgainstAverage = GetDecimal(values, headerIndexes, "home_xg_against_average", rowNumber),
                AwayXgForAverage = GetDecimal(values, headerIndexes, "away_xg_for_average", rowNumber),
                AwayXgAgainstAverage = GetDecimal(values, headerIndexes, "away_xg_against_average", rowNumber),

                HomeGoalsScoredAverage = GetDecimal(values, headerIndexes, "home_goals_scored_average", rowNumber),
                AwayGoalsScoredAverage = GetDecimal(values, headerIndexes, "away_goals_scored_average", rowNumber),
                HomeGoalsConcededAverage = GetDecimal(values, headerIndexes, "home_goals_conceded_average", rowNumber),
                AwayGoalsConcededAverage = GetDecimal(values, headerIndexes, "away_goals_conceded_average", rowNumber),

                HomeShotsOnTargetForAverage = GetDecimal(values, headerIndexes, "home_shots_on_target_for_average", rowNumber),
                HomeShotsOnTargetAgainstAverage = GetDecimal(values, headerIndexes, "home_shots_on_target_against_average", rowNumber),
                AwayShotsOnTargetForAverage = GetDecimal(values, headerIndexes, "away_shots_on_target_for_average", rowNumber),
                AwayShotsOnTargetAgainstAverage = GetDecimal(values, headerIndexes, "away_shots_on_target_against_average", rowNumber),

                HeadToHeadMatchesCount = GetInt(values, headerIndexes, "head_to_head_matches_count", rowNumber),
                HeadToHeadHomeWins = GetInt(values, headerIndexes, "head_to_head_home_wins", rowNumber),
                HeadToHeadDraws = GetInt(values, headerIndexes, "head_to_head_draws", rowNumber),
                HeadToHeadAwayWins = GetInt(values, headerIndexes, "head_to_head_away_wins", rowNumber),

                HomeKeyPlayersMissingImpact = GetInt(values, headerIndexes, "home_key_players_missing_impact", rowNumber),
                AwayKeyPlayersMissingImpact = GetInt(values, headerIndexes, "away_key_players_missing_impact", rowNumber),

                HomeFatigueImpact = GetInt(values, headerIndexes, "home_fatigue_impact", rowNumber),
                AwayFatigueImpact = GetInt(values, headerIndexes, "away_fatigue_impact", rowNumber)
            };

            requests.Add(request);
        }

        return requests;
    }

    private static void ValidateRequiredHeaders(Dictionary<string, int> headerIndexes)
    {
        string[] requiredHeaders =
        {
            "league_name",
            "home_team_name",
            "away_team_name",
            "kickoff_time",

            "home_recent_wins",
            "home_recent_draws",
            "home_recent_losses",
            "away_recent_wins",
            "away_recent_draws",
            "away_recent_losses",

            "home_last10_home_wins",
            "home_last10_home_draws",
            "home_last10_home_losses",
            "away_last10_away_wins",
            "away_last10_away_draws",
            "away_last10_away_losses",

            "home_xg_for_average",
            "home_xg_against_average",
            "away_xg_for_average",
            "away_xg_against_average",

            "home_goals_scored_average",
            "away_goals_scored_average",
            "home_goals_conceded_average",
            "away_goals_conceded_average",

            "home_shots_on_target_for_average",
            "home_shots_on_target_against_average",
            "away_shots_on_target_for_average",
            "away_shots_on_target_against_average",

            "head_to_head_matches_count",
            "head_to_head_home_wins",
            "head_to_head_draws",
            "head_to_head_away_wins",

            "home_key_players_missing_impact",
            "away_key_players_missing_impact",
            "home_fatigue_impact",
            "away_fatigue_impact"
        };

        foreach (string requiredHeader in requiredHeaders)
        {
            if (!headerIndexes.ContainsKey(requiredHeader))
            {
                throw new InvalidOperationException($"CSV file is missing required column '{requiredHeader}'.");
            }
        }
    }

    private static string GetString(
        string[] values,
        Dictionary<string, int> headerIndexes,
        string header,
        int rowNumber)
    {
        int index = headerIndexes[header];

        if (index >= values.Length)
        {
            throw new InvalidOperationException($"Row {rowNumber}: Missing value for '{header}'.");
        }

        string value = values[index].Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Row {rowNumber}: Value for '{header}' is required.");
        }

        return value;
    }

    private static int GetInt(
        string[] values,
        Dictionary<string, int> headerIndexes,
        string header,
        int rowNumber)
    {
        string value = GetString(values, headerIndexes, header, rowNumber);

        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
        {
            throw new InvalidOperationException($"Row {rowNumber}: Value for '{header}' must be a valid integer.");
        }

        return result;
    }

    private static decimal GetDecimal(
        string[] values,
        Dictionary<string, int> headerIndexes,
        string header,
        int rowNumber)
    {
        string value = GetString(values, headerIndexes, header, rowNumber);

        if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
        {
            throw new InvalidOperationException($"Row {rowNumber}: Value for '{header}' must be a valid decimal number.");
        }

        return result;
    }

    private static DateTime GetDateTime(
        string[] values,
        Dictionary<string, int> headerIndexes,
        string header,
        int rowNumber)
    {
        string value = GetString(values, headerIndexes, header, rowNumber);

        if (!DateTime.TryParse(
                value,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out DateTime result))
        {
            throw new InvalidOperationException($"Row {rowNumber}: Value for '{header}' must be a valid date/time.");
        }

        return result;
    }

    private static string[] SplitCsvLine(string line)
    {
        return line.Split(',');
    }
}