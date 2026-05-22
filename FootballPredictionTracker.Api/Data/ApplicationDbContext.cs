using FootballPredictionTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<League> Leagues { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Match> Matches { get; set; }

    public DbSet<MatchStatistics> MatchStatistics { get; set; }

    public DbSet<Prediction> Predictions { get; set; }

    public DbSet<PredictionParameterWeight> PredictionParameterWeights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PredictionParameterWeight>()
            .Property(p => p.Weight)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.League)
            .WithMany()
            .HasForeignKey(m => m.LeagueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}