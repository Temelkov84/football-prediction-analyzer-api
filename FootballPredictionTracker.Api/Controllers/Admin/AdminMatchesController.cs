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
            .Select(m => new AdminMatchResponse
            {
                Id = m.Id,
                League = m.League.Name,
                HomeTeam = m.HomeTeam.Name,
                AwayTeam = m.AwayTeam.Name,
                KickoffTime = m.KickoffTime
            })
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

        var matchAlreadyExists = await _dbContext.Matches
    .AnyAsync(m =>
        m.LeagueId == request.LeagueId &&
        m.HomeTeamId == request.HomeTeamId &&
        m.AwayTeamId == request.AwayTeamId &&
        m.KickoffTime == request.KickoffTime);

        if (matchAlreadyExists)
        {
            return BadRequest("Match already exists.");
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatch(int id, UpdateMatchRequest request)
    {
        if (request.HomeTeamId == request.AwayTeamId)
        {
            return BadRequest("Home team and away team must be different.");
        }

        var match = await _dbContext.Matches
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match == null)
        {
            return NotFound("Match does not exist.");
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

        var matchAlreadyExists = await _dbContext.Matches
    .AnyAsync(m =>
        m.Id != id &&
        m.LeagueId == request.LeagueId &&
        m.HomeTeamId == request.HomeTeamId &&
        m.AwayTeamId == request.AwayTeamId &&
        m.KickoffTime == request.KickoffTime);

        if (matchAlreadyExists)
        {
            return BadRequest("Match already exists.");
        }

        match.LeagueId = request.LeagueId;
        match.HomeTeamId = request.HomeTeamId;
        match.AwayTeamId = request.AwayTeamId;
        match.KickoffTime = request.KickoffTime;

        await _dbContext.SaveChangesAsync();

        var response = new AdminMatchResponse
        {
            Id = match.Id,
            League = (await _dbContext.Leagues.FindAsync(match.LeagueId))!.Name,
            HomeTeam = (await _dbContext.Teams.FindAsync(match.HomeTeamId))!.Name,
            AwayTeam = (await _dbContext.Teams.FindAsync(match.AwayTeamId))!.Name,
            KickoffTime = match.KickoffTime
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        var match = await _dbContext.Matches
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match == null)
        {
            return NotFound("Match does not exist.");
        }

        var statisticsExist = await _dbContext.MatchStatistics
            .AnyAsync(s => s.MatchId == id);

        if (statisticsExist)
        {
            return BadRequest("Cannot delete match while statistics exist for this match.");
        }

        var predictionExists = await _dbContext.Predictions
            .AnyAsync(p => p.MatchId == id);

        if (predictionExists)
        {
            return BadRequest("Cannot delete match while prediction exists for this match.");
        }

        _dbContext.Matches.Remove(match);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}