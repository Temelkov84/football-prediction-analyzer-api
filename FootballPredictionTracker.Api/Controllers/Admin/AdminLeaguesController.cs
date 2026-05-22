using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.Models;
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
    public async Task<IActionResult> CreateLeague(League league)
    {
        _dbContext.Leagues.Add(league);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateLeague), new { id = league.Id }, league);
    }
}