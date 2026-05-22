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
                HomeTeamRecentWins = s.HomeTeamRecentWins,
                HomeTeamRecentDraws = s.HomeTeamRecentDraws,
                HomeTeamRecentLosses = s.HomeTeamRecentLosses,
                AwayTeamRecentWins = s.AwayTeamRecentWins,
                AwayTeamRecentDraws = s.AwayTeamRecentDraws,
                AwayTeamRecentLosses = s.AwayTeamRecentLosses,
                HeadToHeadHomeWins = s.HeadToHeadHomeWins,
                HeadToHeadDraws = s.HeadToHeadDraws,
                HeadToHeadAwayWins = s.HeadToHeadAwayWins
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
            return BadRequest("Match does not exist.");
        }

        var statisticsAlreadyExist = await _dbContext.MatchStatistics
            .AnyAsync(s => s.MatchId == request.MatchId);

        if (statisticsAlreadyExist)
        {
            return BadRequest("Statistics already exist for this match.");
        }

        var statistics = new MatchStatistics
        {
            MatchId = request.MatchId,
            HomeTeamRecentWins = request.HomeTeamRecentWins,
            HomeTeamRecentDraws = request.HomeTeamRecentDraws,
            HomeTeamRecentLosses = request.HomeTeamRecentLosses,
            AwayTeamRecentWins = request.AwayTeamRecentWins,
            AwayTeamRecentDraws = request.AwayTeamRecentDraws,
            AwayTeamRecentLosses = request.AwayTeamRecentLosses,
            HeadToHeadHomeWins = request.HeadToHeadHomeWins,
            HeadToHeadDraws = request.HeadToHeadDraws,
            HeadToHeadAwayWins = request.HeadToHeadAwayWins
        };

        _dbContext.MatchStatistics.Add(statistics);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateMatchStatistics), new { id = statistics.Id }, statistics);
    }
}