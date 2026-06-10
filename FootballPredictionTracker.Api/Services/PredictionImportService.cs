using FootballPredictionTracker.Api.Data;
using FootballPredictionTracker.Api.DTOs;
using FootballPredictionTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballPredictionTracker.Api.Services
{
    public class PredictionImportService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PredictionService _predictionService;

        public PredictionImportService(
            ApplicationDbContext dbContext,
            PredictionService predictionService)
        {
            _dbContext = dbContext;
            _predictionService = predictionService;
        }

        public async Task<ImportPredictionResponse> ImportPredictionsAsync(
            List<ImportPredictionRequest> requests)
        {
            var response = new ImportPredictionResponse();

            var activeWeights = await _dbContext.PredictionParameterWeights
                .Where(weight => weight.IsActive)
                .ToDictionaryAsync(
                    weight => weight.Key,
                    weight => weight.Value);

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            for (int i = 0; i < requests.Count; i++)
            {
                ImportPredictionRequest request = requests[i];
                int rowNumber = i + 1;

                string? validationError = await ValidateImportRowAsync(request, rowNumber);

                if (validationError != null)
                {
                    response.Errors.Add(validationError);
                    continue;
                }

                League league = await _dbContext.Leagues
                    .FirstAsync(league => league.Name == request.LeagueName);

                Team homeTeam = await _dbContext.Teams
                    .FirstAsync(team =>
                        team.Name == request.HomeTeamName &&
                        team.LeagueId == league.Id);

                Team awayTeam = await _dbContext.Teams
                    .FirstAsync(team =>
                        team.Name == request.AwayTeamName &&
                        team.LeagueId == league.Id);

                Match match = new Match
                {
                    LeagueId = league.Id,
                    HomeTeamId = homeTeam.Id,
                    AwayTeamId = awayTeam.Id,
                    KickoffTime = request.KickoffTime
                };

                _dbContext.Matches.Add(match);
                await _dbContext.SaveChangesAsync();

                MatchStatistics statistics = CreateMatchStatistics(match.Id, request);

                _dbContext.MatchStatistics.Add(statistics);
                await _dbContext.SaveChangesAsync();

                Prediction prediction = _predictionService.CalculatePrediction(
                    match,
                    statistics,
                    activeWeights);

                _dbContext.Predictions.Add(prediction);
                await _dbContext.SaveChangesAsync();

                response.CreatedMatches++;
                response.CreatedStatistics++;
                response.CreatedPredictions++;

                response.ImportedPredictions.Add(new ImportedPredictionResponse
                {
                    MatchId = match.Id,
                    PredictionId = prediction.Id,
                    League = league.Name,
                    KickoffTime = match.KickoffTime,
                    HomeTeam = homeTeam.Name,
                    AwayTeam = awayTeam.Name,
                    HomeWinProbability = prediction.HomeWinProbability,
                    DrawProbability = prediction.DrawProbability,
                    AwayWinProbability = prediction.AwayWinProbability
                });
            }

            if (response.Errors.Count > 0)
            {
                await transaction.RollbackAsync();
                return response;
            }

            await transaction.CommitAsync();

            return response;
        }

        private async Task<string?> ValidateImportRowAsync(
            ImportPredictionRequest request,
            int rowNumber)
        {
            if (request.HomeTeamName == request.AwayTeamName)
            {
                return $"Row {rowNumber}: Home team and away team must be different.";
            }

            if (request.HomeRecentWins + request.HomeRecentDraws + request.HomeRecentLosses != 6)
            {
                return $"Row {rowNumber}: Home recent form must contain exactly 6 matches.";
            }

            if (request.AwayRecentWins + request.AwayRecentDraws + request.AwayRecentLosses != 6)
            {
                return $"Row {rowNumber}: Away recent form must contain exactly 6 matches.";
            }

            if (request.HomeLast10HomeWins + request.HomeLast10HomeDraws + request.HomeLast10HomeLosses != 10)
            {
                return $"Row {rowNumber}: Home home-form must contain exactly 10 matches.";
            }

            if (request.AwayLast10AwayWins + request.AwayLast10AwayDraws + request.AwayLast10AwayLosses != 10)
            {
                return $"Row {rowNumber}: Away away-form must contain exactly 10 matches.";
            }

            if (request.HeadToHeadHomeWins + request.HeadToHeadDraws + request.HeadToHeadAwayWins != request.HeadToHeadMatchesCount)
            {
                return $"Row {rowNumber}: Head-to-head results must equal head-to-head matches count.";
            }

            League? league = await _dbContext.Leagues
                .FirstOrDefaultAsync(league => league.Name == request.LeagueName);

            if (league == null)
            {
                return $"Row {rowNumber}: League '{request.LeagueName}' does not exist.";
            }

            bool homeTeamExists = await _dbContext.Teams
                .AnyAsync(team =>
                    team.Name == request.HomeTeamName &&
                    team.LeagueId == league.Id);

            if (!homeTeamExists)
            {
                return $"Row {rowNumber}: Home team '{request.HomeTeamName}' does not exist in league '{request.LeagueName}'.";
            }

            bool awayTeamExists = await _dbContext.Teams
                .AnyAsync(team =>
                    team.Name == request.AwayTeamName &&
                    team.LeagueId == league.Id);

            if (!awayTeamExists)
            {
                return $"Row {rowNumber}: Away team '{request.AwayTeamName}' does not exist in league '{request.LeagueName}'.";
            }

            bool matchAlreadyExists = await _dbContext.Matches
                .AnyAsync(match =>
                    match.LeagueId == league.Id &&
                    match.HomeTeam.Name == request.HomeTeamName &&
                    match.AwayTeam.Name == request.AwayTeamName &&
                    match.KickoffTime == request.KickoffTime);

            if (matchAlreadyExists)
            {
                return $"Row {rowNumber}: Match already exists.";
            }

            return null;
        }

        private static MatchStatistics CreateMatchStatistics(
            int matchId,
            ImportPredictionRequest request)
        {
            return new MatchStatistics
            {
                MatchId = matchId,

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
        }
    }
}