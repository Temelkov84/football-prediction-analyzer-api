using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/matches")]
public class AdminMatchesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminMatchesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetMatches()
    {
        var matches = await _dbContext.Matches
            .Include(m => m.League)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .OrderBy(m => m.League.Name)
            .ThenBy(m => m.KickoffTime)
            .ToListAsync();

        return Ok(matches);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMatch(CreateMatchRequest request)
    {
        if (request.HomeTeamId == request.AwayTeamId)
        {
            return BadRequest("Home team and away team must be different.");
        }

        var leagueExists = await _dbContext.Leagues
            .AnyAsync(l => l.Id == request.LeagueId);

        if (!leagueExists)
        {
            return BadRequest("League does not exist.");
        }

        var homeTeamExists = await _dbContext.Teams
            .AnyAsync(t => t.Id == request.HomeTeamId);

        if (!homeTeamExists)
        {
            return BadRequest("Home team does not exist.");
        }

        var awayTeamExists = await _dbContext.Teams
            .AnyAsync(t => t.Id == request.AwayTeamId);

        if (!awayTeamExists)
        {
            return BadRequest("Away team does not exist.");
        }

        var match = new Match
        {
            LeagueId = request.LeagueId,
            HomeTeamId = request.HomeTeamId,
            AwayTeamId = request.AwayTeamId,
            KickoffTime = request.KickoffTime
        };

        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateMatch), new { id = match.Id }, match);
    }
}