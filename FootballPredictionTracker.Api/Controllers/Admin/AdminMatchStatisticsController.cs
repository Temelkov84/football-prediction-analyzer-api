using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/match-statistics")]
public class AdminMatchStatisticsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminMatchStatisticsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetMatchStatistics()
    {
        var statistics = await _dbContext.MatchStatistics
            .Include(s => s.Match)
                .ThenInclude(m => m.HomeTeam)
            .Include(s => s.Match)
                .ThenInclude(m => m.AwayTeam)
            .Include(s => s.Match)
                .ThenInclude(m => m.League)
            .OrderBy(s => s.Match.League.Name)
            .ThenBy(s => s.Match.KickoffTime)
            .Select(s => new AdminMatchStatisticsResponse
            {
                Id = s.Id,
                MatchId = s.MatchId,
                League = s.Match.League.Name,
                KickoffTime = s.Match.KickoffTime,
                HomeTeam = s.Match.HomeTeam.Name,
                AwayTeam = s.Match.AwayTeam.Name,

                HomeRecentWins = s.HomeRecentWins,
                HomeRecentDraws = s.HomeRecentDraws,
                HomeRecentLosses = s.HomeRecentLosses,

                AwayRecentWins = s.AwayRecentWins,
                AwayRecentDraws = s.AwayRecentDraws,
                AwayRecentLosses = s.AwayRecentLosses,

                HomeLast10HomeWins = s.HomeLast10HomeWins,
                HomeLast10HomeDraws = s.HomeLast10HomeDraws,
                HomeLast10HomeLosses = s.HomeLast10HomeLosses,

                AwayLast10AwayWins = s.AwayLast10AwayWins,
                AwayLast10AwayDraws = s.AwayLast10AwayDraws,
                AwayLast10AwayLosses = s.AwayLast10AwayLosses,

                HomeXgForAverage = s.HomeXgForAverage,
                HomeXgAgainstAverage = s.HomeXgAgainstAverage,
                AwayXgForAverage = s.AwayXgForAverage,
                AwayXgAgainstAverage = s.AwayXgAgainstAverage,

                HomeGoalsScoredAverage = s.HomeGoalsScoredAverage,
                AwayGoalsScoredAverage = s.AwayGoalsScoredAverage,

                HomeGoalsConcededAverage = s.HomeGoalsConcededAverage,
                AwayGoalsConcededAverage = s.AwayGoalsConcededAverage,

                HomeShotsOnTargetForAverage = s.HomeShotsOnTargetForAverage,
                HomeShotsOnTargetAgainstAverage = s.HomeShotsOnTargetAgainstAverage,
                AwayShotsOnTargetForAverage = s.AwayShotsOnTargetForAverage,
                AwayShotsOnTargetAgainstAverage = s.AwayShotsOnTargetAgainstAverage,

                HeadToHeadMatchesCount = s.HeadToHeadMatchesCount,
                HeadToHeadHomeWins = s.HeadToHeadHomeWins,
                HeadToHeadDraws = s.HeadToHeadDraws,
                HeadToHeadAwayWins = s.HeadToHeadAwayWins,

                HomeKeyPlayersMissingImpact = s.HomeKeyPlayersMissingImpact,
                AwayKeyPlayersMissingImpact = s.AwayKeyPlayersMissingImpact,

                HomeFatigueImpact = s.HomeFatigueImpact,
                AwayFatigueImpact = s.AwayFatigueImpact
            })
            .ToListAsync();

        return Ok(statistics);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMatchStatistics(CreateMatchStatisticsRequest request)
    {
        var matchExists = await _dbContext.Matches
            .AnyAsync(m => m.Id == request.MatchId);

        if (!matchExists)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Match does not exist."
            });
        }

        var statisticsAlreadyExist = await _dbContext.MatchStatistics
            .AnyAsync(s => s.MatchId == request.MatchId);

        if (statisticsAlreadyExist)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Statistics already exist for this match."
            });
        }

        var formValidationError = ValidateMatchStatistics(
            request.HomeRecentWins,
            request.HomeRecentDraws,
            request.HomeRecentLosses,
            request.AwayRecentWins,
            request.AwayRecentDraws,
            request.AwayRecentLosses,
            request.HomeLast10HomeWins,
            request.HomeLast10HomeDraws,
            request.HomeLast10HomeLosses,
            request.AwayLast10AwayWins,
            request.AwayLast10AwayDraws,
            request.AwayLast10AwayLosses,
            request.HeadToHeadMatchesCount,
            request.HeadToHeadHomeWins,
            request.HeadToHeadDraws,
            request.HeadToHeadAwayWins);

        if (formValidationError != null)
        {
            return BadRequest(new ErrorResponse
            {
                Message = formValidationError
            });
        }

        var statistics = new MatchStatistics
        {
            MatchId = request.MatchId,

            HomeRecentWins = request.HomeRecentWins,
            HomeRecentDraws = request.HomeRecentDraws,
            HomeRecentLosses = request.HomeRecentLosses,

            AwayRecentWins = request.AwayRecentWins,
            AwayRecentDraws = request.AwayRecentDraws,
            AwayRecentLosses = request.AwayRecentLosses,

            HomeLast10HomeWins = request.HomeLast10HomeWins,
            HomeLast10HomeDraws = request.HomeLast10HomeDraws,
            HomeLast10HomeLosses = request.HomeLast10HomeLosses,

            AwayLast10AwayWins = request.AwayLast10AwayWins,
            AwayLast10AwayDraws = request.AwayLast10AwayDraws,
            AwayLast10AwayLosses = request.AwayLast10AwayLosses,

            HomeXgForAverage = request.HomeXgForAverage,
            HomeXgAgainstAverage = request.HomeXgAgainstAverage,
            AwayXgForAverage = request.AwayXgForAverage,
            AwayXgAgainstAverage = request.AwayXgAgainstAverage,

            HomeGoalsScoredAverage = request.HomeGoalsScoredAverage,
            AwayGoalsScoredAverage = request.AwayGoalsScoredAverage,

            HomeGoalsConcededAverage = request.HomeGoalsConcededAverage,
            AwayGoalsConcededAverage = request.AwayGoalsConcededAverage,

            HomeShotsOnTargetForAverage = request.HomeShotsOnTargetForAverage,
            HomeShotsOnTargetAgainstAverage = request.HomeShotsOnTargetAgainstAverage,
            AwayShotsOnTargetForAverage = request.AwayShotsOnTargetForAverage,
            AwayShotsOnTargetAgainstAverage = request.AwayShotsOnTargetAgainstAverage,

            HeadToHeadMatchesCount = request.HeadToHeadMatchesCount,
            HeadToHeadHomeWins = request.HeadToHeadHomeWins,
            HeadToHeadDraws = request.HeadToHeadDraws,
            HeadToHeadAwayWins = request.HeadToHeadAwayWins,

            HomeKeyPlayersMissingImpact = request.HomeKeyPlayersMissingImpact,
            AwayKeyPlayersMissingImpact = request.AwayKeyPlayersMissingImpact,

            HomeFatigueImpact = request.HomeFatigueImpact,
            AwayFatigueImpact = request.AwayFatigueImpact
        };

        _dbContext.MatchStatistics.Add(statistics);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateMatchStatistics), new { id = statistics.Id }, statistics);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatchStatistics(int id, UpdateMatchStatisticsRequest request)
    {
        var statistics = await _dbContext.MatchStatistics
            .Include(s => s.Match)
                .ThenInclude(m => m.League)
            .Include(s => s.Match)
                .ThenInclude(m => m.HomeTeam)
            .Include(s => s.Match)
                .ThenInclude(m => m.AwayTeam)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (statistics == null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Statistics do not exist."
            });
        }

        var formValidationError = ValidateMatchStatistics(
            request.HomeRecentWins,
            request.HomeRecentDraws,
            request.HomeRecentLosses,
            request.AwayRecentWins,
            request.AwayRecentDraws,
            request.AwayRecentLosses,
            request.HomeLast10HomeWins,
            request.HomeLast10HomeDraws,
            request.HomeLast10HomeLosses,
            request.AwayLast10AwayWins,
            request.AwayLast10AwayDraws,
            request.AwayLast10AwayLosses,
            request.HeadToHeadMatchesCount,
            request.HeadToHeadHomeWins,
            request.HeadToHeadDraws,
            request.HeadToHeadAwayWins);

        if (formValidationError != null)
        {
            return BadRequest(new ErrorResponse
            {
                Message = formValidationError
            });
        }

        statistics.HomeRecentWins = request.HomeRecentWins;
        statistics.HomeRecentDraws = request.HomeRecentDraws;
        statistics.HomeRecentLosses = request.HomeRecentLosses;

        statistics.AwayRecentWins = request.AwayRecentWins;
        statistics.AwayRecentDraws = request.AwayRecentDraws;
        statistics.AwayRecentLosses = request.AwayRecentLosses;

        statistics.HomeLast10HomeWins = request.HomeLast10HomeWins;
        statistics.HomeLast10HomeDraws = request.HomeLast10HomeDraws;
        statistics.HomeLast10HomeLosses = request.HomeLast10HomeLosses;

        statistics.AwayLast10AwayWins = request.AwayLast10AwayWins;
        statistics.AwayLast10AwayDraws = request.AwayLast10AwayDraws;
        statistics.AwayLast10AwayLosses = request.AwayLast10AwayLosses;

        statistics.HomeXgForAverage = request.HomeXgForAverage;
        statistics.HomeXgAgainstAverage = request.HomeXgAgainstAverage;
        statistics.AwayXgForAverage = request.AwayXgForAverage;
        statistics.AwayXgAgainstAverage = request.AwayXgAgainstAverage;

        statistics.HomeGoalsScoredAverage = request.HomeGoalsScoredAverage;
        statistics.AwayGoalsScoredAverage = request.AwayGoalsScoredAverage;

        statistics.HomeGoalsConcededAverage = request.HomeGoalsConcededAverage;
        statistics.AwayGoalsConcededAverage = request.AwayGoalsConcededAverage;

        statistics.HomeShotsOnTargetForAverage = request.HomeShotsOnTargetForAverage;
        statistics.HomeShotsOnTargetAgainstAverage = request.HomeShotsOnTargetAgainstAverage;
        statistics.AwayShotsOnTargetForAverage = request.AwayShotsOnTargetForAverage;
        statistics.AwayShotsOnTargetAgainstAverage = request.AwayShotsOnTargetAgainstAverage;

        statistics.HeadToHeadMatchesCount = request.HeadToHeadMatchesCount;
        statistics.HeadToHeadHomeWins = request.HeadToHeadHomeWins;
        statistics.HeadToHeadDraws = request.HeadToHeadDraws;
        statistics.HeadToHeadAwayWins = request.HeadToHeadAwayWins;

        statistics.HomeKeyPlayersMissingImpact = request.HomeKeyPlayersMissingImpact;
        statistics.AwayKeyPlayersMissingImpact = request.AwayKeyPlayersMissingImpact;

        statistics.HomeFatigueImpact = request.HomeFatigueImpact;
        statistics.AwayFatigueImpact = request.AwayFatigueImpact;

        await _dbContext.SaveChangesAsync();

        var response = new AdminMatchStatisticsResponse
        {
            Id = statistics.Id,
            MatchId = statistics.MatchId,
            League = statistics.Match.League.Name,
            KickoffTime = statistics.Match.KickoffTime,
            HomeTeam = statistics.Match.HomeTeam.Name,
            AwayTeam = statistics.Match.AwayTeam.Name,

            HomeRecentWins = statistics.HomeRecentWins,
            HomeRecentDraws = statistics.HomeRecentDraws,
            HomeRecentLosses = statistics.HomeRecentLosses,

            AwayRecentWins = statistics.AwayRecentWins,
            AwayRecentDraws = statistics.AwayRecentDraws,
            AwayRecentLosses = statistics.AwayRecentLosses,

            HomeLast10HomeWins = statistics.HomeLast10HomeWins,
            HomeLast10HomeDraws = statistics.HomeLast10HomeDraws,
            HomeLast10HomeLosses = statistics.HomeLast10HomeLosses,

            AwayLast10AwayWins = statistics.AwayLast10AwayWins,
            AwayLast10AwayDraws = statistics.AwayLast10AwayDraws,
            AwayLast10AwayLosses = statistics.AwayLast10AwayLosses,

            HomeXgForAverage = statistics.HomeXgForAverage,
            HomeXgAgainstAverage = statistics.HomeXgAgainstAverage,
            AwayXgForAverage = statistics.AwayXgForAverage,
            AwayXgAgainstAverage = statistics.AwayXgAgainstAverage,

            HomeGoalsScoredAverage = statistics.HomeGoalsScoredAverage,
            AwayGoalsScoredAverage = statistics.AwayGoalsScoredAverage,

            HomeGoalsConcededAverage = statistics.HomeGoalsConcededAverage,
            AwayGoalsConcededAverage = statistics.AwayGoalsConcededAverage,

            HomeShotsOnTargetForAverage = statistics.HomeShotsOnTargetForAverage,
            HomeShotsOnTargetAgainstAverage = statistics.HomeShotsOnTargetAgainstAverage,
            AwayShotsOnTargetForAverage = statistics.AwayShotsOnTargetForAverage,
            AwayShotsOnTargetAgainstAverage = statistics.AwayShotsOnTargetAgainstAverage,

            HeadToHeadMatchesCount = statistics.HeadToHeadMatchesCount,
            HeadToHeadHomeWins = statistics.HeadToHeadHomeWins,
            HeadToHeadDraws = statistics.HeadToHeadDraws,
            HeadToHeadAwayWins = statistics.HeadToHeadAwayWins,

            HomeKeyPlayersMissingImpact = statistics.HomeKeyPlayersMissingImpact,
            AwayKeyPlayersMissingImpact = statistics.AwayKeyPlayersMissingImpact,

            HomeFatigueImpact = statistics.HomeFatigueImpact,
            AwayFatigueImpact = statistics.AwayFatigueImpact
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatchStatistics(int id)
    {
        var statistics = await _dbContext.MatchStatistics
            .FirstOrDefaultAsync(s => s.Id == id);

        if (statistics == null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "Statistics do not exist."
            });
        }

        var predictionExists = await _dbContext.Predictions
            .AnyAsync(p => p.MatchId == statistics.MatchId);

        if (predictionExists)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Cannot delete statistics while prediction exists for this match."
            });
        }

        _dbContext.MatchStatistics.Remove(statistics);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private static string? ValidateMatchStatistics(
        int homeRecentWins,
        int homeRecentDraws,
        int homeRecentLosses,
        int awayRecentWins,
        int awayRecentDraws,
        int awayRecentLosses,
        int homeLast10HomeWins,
        int homeLast10HomeDraws,
        int homeLast10HomeLosses,
        int awayLast10AwayWins,
        int awayLast10AwayDraws,
        int awayLast10AwayLosses,
        int headToHeadMatchesCount,
        int headToHeadHomeWins,
        int headToHeadDraws,
        int headToHeadAwayWins)
    {
        if (homeRecentWins + homeRecentDraws + homeRecentLosses != 6)
        {
            return "Home recent form must contain exactly 6 matches.";
        }

        if (awayRecentWins + awayRecentDraws + awayRecentLosses != 6)
        {
            return "Away recent form must contain exactly 6 matches.";
        }

        if (homeLast10HomeWins + homeLast10HomeDraws + homeLast10HomeLosses != 10)
        {
            return "Home home/away form must contain exactly 10 matches.";
        }

        if (awayLast10AwayWins + awayLast10AwayDraws + awayLast10AwayLosses != 10)
        {
            return "Away home/away form must contain exactly 10 matches.";
        }

        if (headToHeadHomeWins + headToHeadDraws + headToHeadAwayWins != headToHeadMatchesCount)
        {
            return "Head-to-head results must match the head-to-head matches count.";
        }

        return null;
    }
}