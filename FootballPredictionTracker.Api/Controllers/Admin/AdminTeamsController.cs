using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballPredictionTracker.Api.DTOs;

namespace FootballPredictionTracker.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/teams")]
public class AdminTeamsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminTeamsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetTeams()
    {
        var teams = await _dbContext.Teams
            .Include(t => t.League)
            .ToListAsync();

        return Ok(teams);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeam(CreateTeamRequest request)
    {
        var leagueExists = await _dbContext.Leagues
            .AnyAsync(l => l.Id == request.LeagueId);

        if (!leagueExists)
        {
            return BadRequest("League does not exist.");
        }

        var teamAlreadyExists = await _dbContext.Teams
            .AnyAsync(t => t.Name == request.Name && t.LeagueId == request.LeagueId);

        if (teamAlreadyExists)
        {
            return BadRequest("Team already exists in this league.");
        }

        var team = new Team
        {
            Name = request.Name,
            LeagueId = request.LeagueId
        };

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateTeam), new { id = team.Id }, team);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(int id, UpdateTeamRequest request)
    {
        var team = await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
        {
            return NotFound("Team does not exist.");
        }

        var leagueExists = await _dbContext.Leagues
            .AnyAsync(l => l.Id == request.LeagueId);

        if (!leagueExists)
        {
            return BadRequest("League does not exist.");
        }

        var teamAlreadyExists = await _dbContext.Teams
            .AnyAsync(t =>
                t.Id != id &&
                t.Name == request.Name &&
                t.LeagueId == request.LeagueId);

        if (teamAlreadyExists)
        {
            return BadRequest("Team already exists in this league.");
        }

        team.Name = request.Name;
        team.LeagueId = request.LeagueId;

        await _dbContext.SaveChangesAsync();

        var league = await _dbContext.Leagues.FindAsync(team.LeagueId);

        var response = new
        {
            team.Id,
            team.Name,
            team.LeagueId,
            League = league!.Name
        };

        return Ok(response);
    }
}