using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.Models;
using FootballPredictionTracker.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/leagues")]
public class AdminLeaguesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminLeaguesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeagues()
    {
        var leagues = await _dbContext.Leagues.ToListAsync();

        return Ok(leagues);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLeague(CreateLeagueRequest request)
    {
        var leagueAlreadyExists = await _dbContext.Leagues
            .AnyAsync(l => l.Name == request.Name && l.Country == request.Country);

        if (leagueAlreadyExists)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "League already exists."
            });
        }

        var league = new League
        {
            Name = request.Name,
            Country = request.Country
        };

        _dbContext.Leagues.Add(league);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateLeague), new { id = league.Id }, league);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeague(int id, UpdateLeagueRequest request)
    {
        var league = await _dbContext.Leagues
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league == null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "League does not exist."
            });
        }

        var leagueAlreadyExists = await _dbContext.Leagues
            .AnyAsync(l =>
                l.Id != id &&
                l.Name == request.Name &&
                l.Country == request.Country);

        if (leagueAlreadyExists)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "League already exists."
            });
        }

        league.Name = request.Name;
        league.Country = request.Country;

        await _dbContext.SaveChangesAsync();

        return Ok(league);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeague(int id)
    {
        var league = await _dbContext.Leagues
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league == null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "League does not exist."
            });
        }

        var leagueHasTeams = await _dbContext.Teams
            .AnyAsync(t => t.LeagueId == id);

        if (leagueHasTeams)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Cannot delete league while it has teams."
            });
        }

        _dbContext.Leagues.Remove(league);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}