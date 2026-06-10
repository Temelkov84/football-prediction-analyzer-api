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
      .Property(parameter => parameter.Value)
      .HasPrecision(5, 2);

        modelBuilder.Entity<PredictionParameterWeight>().HasData(
    new PredictionParameterWeight
    {
        Id = 1,
        Key = "recent_form",
        Name = "Recent Form",
        Value = 16,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 2,
        Key = "home_away_form",
        Name = "Home/Away Form",
        Value = 18,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 3,
        Key = "xg",
        Name = "xG",
        Value = 16,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 4,
        Key = "attack_strength",
        Name = "Attack Strength",
        Value = 11,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 5,
        Key = "defense_strength",
        Name = "Defense Strength",
        Value = 12,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 6,
        Key = "shots_on_target",
        Name = "Shots on Target",
        Value = 11,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 7,
        Key = "head_to_head",
        Name = "Head-to-Head",
        Value = 6,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 8,
        Key = "key_players_missing",
        Name = "Key Players Missing",
        Value = 5,
        IsActive = true
    },
    new PredictionParameterWeight
    {
        Id = 9,
        Key = "fatigue",
        Name = "Fatigue",
        Value = 5,
        IsActive = true
    }
);

        modelBuilder.Entity<MatchStatistics>()
    .Property(statistics => statistics.HomeXgForAverage)
    .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.HomeXgAgainstAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.AwayXgForAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.AwayXgAgainstAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.HomeGoalsScoredAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.AwayGoalsScoredAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.HomeGoalsConcededAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.AwayGoalsConcededAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.HomeShotsOnTargetForAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.HomeShotsOnTargetAgainstAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.AwayShotsOnTargetForAverage)
            .HasPrecision(5, 2);

        modelBuilder.Entity<MatchStatistics>()
            .Property(statistics => statistics.AwayShotsOnTargetAgainstAverage)
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