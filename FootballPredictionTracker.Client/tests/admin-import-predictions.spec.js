import { test, expect } from '@playwright/test';

const csvHeader = [
    'league_name',
    'home_team_name',
    'away_team_name',
    'kickoff_time',
    'home_recent_wins',
    'home_recent_draws',
    'home_recent_losses',
    'away_recent_wins',
    'away_recent_draws',
    'away_recent_losses',
    'home_last10_home_wins',
    'home_last10_home_draws',
    'home_last10_home_losses',
    'away_last10_away_wins',
    'away_last10_away_draws',
    'away_last10_away_losses',
    'home_xg_for_average',
    'home_xg_against_average',
    'away_xg_for_average',
    'away_xg_against_average',
    'home_goals_scored_average',
    'away_goals_scored_average',
    'home_goals_conceded_average',
    'away_goals_conceded_average',
    'home_shots_on_target_for_average',
    'home_shots_on_target_against_average',
    'away_shots_on_target_for_average',
    'away_shots_on_target_against_average',
    'head_to_head_matches_count',
    'head_to_head_home_wins',
    'head_to_head_draws',
    'head_to_head_away_wins',
    'home_key_players_missing_impact',
    'away_key_players_missing_impact',
    'home_fatigue_impact',
    'away_fatigue_impact'
].join(',');

function createKickoffTime(offsetSeconds = 0) {
    const kickoffTime = new Date();

    kickoffTime.setUTCDate(kickoffTime.getUTCDate() + 2);
    kickoffTime.setUTCHours(19, 0, 0, 0);

    const uniqueSeconds = (Date.now() % 3600) + offsetSeconds;
    kickoffTime.setUTCSeconds(uniqueSeconds);

    return kickoffTime.toISOString().replace('.000Z', 'Z');
}

function createValidPredictionRow(overrides = {}) {
    return {
        leagueName: 'Premier League',
        homeTeamName: 'Liverpool',
        awayTeamName: 'Everton',
        kickoffTime: createKickoffTime(),

        homeRecentWins: 3,
        homeRecentDraws: 2,
        homeRecentLosses: 1,
        awayRecentWins: 2,
        awayRecentDraws: 2,
        awayRecentLosses: 2,

        homeLast10HomeWins: 6,
        homeLast10HomeDraws: 2,
        homeLast10HomeLosses: 2,
        awayLast10AwayWins: 3,
        awayLast10AwayDraws: 3,
        awayLast10AwayLosses: 4,

        homeXgForAverage: 2.1,
        homeXgAgainstAverage: 0.9,
        awayXgForAverage: 1.4,
        awayXgAgainstAverage: 1.2,

        homeGoalsScoredAverage: 2.0,
        awayGoalsScoredAverage: 1.3,
        homeGoalsConcededAverage: 0.8,
        awayGoalsConcededAverage: 1.1,

        homeShotsOnTargetForAverage: 6.2,
        homeShotsOnTargetAgainstAverage: 3.1,
        awayShotsOnTargetForAverage: 4.8,
        awayShotsOnTargetAgainstAverage: 4.2,

        headToHeadMatchesCount: 2,
        headToHeadHomeWins: 1,
        headToHeadDraws: 0,
        headToHeadAwayWins: 1,

        homeKeyPlayersMissingImpact: 0,
        awayKeyPlayersMissingImpact: 0,
        homeFatigueImpact: 0,
        awayFatigueImpact: 0,

        ...overrides
    };
}

function convertRowToCsvLine(row) {
    return [
        row.leagueName,
        row.homeTeamName,
        row.awayTeamName,
        row.kickoffTime,

        row.homeRecentWins,
        row.homeRecentDraws,
        row.homeRecentLosses,
        row.awayRecentWins,
        row.awayRecentDraws,
        row.awayRecentLosses,

        row.homeLast10HomeWins,
        row.homeLast10HomeDraws,
        row.homeLast10HomeLosses,
        row.awayLast10AwayWins,
        row.awayLast10AwayDraws,
        row.awayLast10AwayLosses,

        row.homeXgForAverage,
        row.homeXgAgainstAverage,
        row.awayXgForAverage,
        row.awayXgAgainstAverage,

        row.homeGoalsScoredAverage,
        row.awayGoalsScoredAverage,
        row.homeGoalsConcededAverage,
        row.awayGoalsConcededAverage,

        row.homeShotsOnTargetForAverage,
        row.homeShotsOnTargetAgainstAverage,
        row.awayShotsOnTargetForAverage,
        row.awayShotsOnTargetAgainstAverage,

        row.headToHeadMatchesCount,
        row.headToHeadHomeWins,
        row.headToHeadDraws,
        row.headToHeadAwayWins,

        row.homeKeyPlayersMissingImpact,
        row.awayKeyPlayersMissingImpact,
        row.homeFatigueImpact,
        row.awayFatigueImpact
    ].join(',');
}

function createCsv(rows) {
    return [
        csvHeader,
        ...rows.map(convertRowToCsvLine)
    ].join('\n');
}

async function openImportPredictionsPage(page) {
    await page.goto('/');

    await page.getByText('Admin').click();

    await page.getByRole('button', { name: 'Import Predictions' }).click();
}

async function uploadCsv(page, fileName, csvContent) {
    await page.locator('input[type="file"]').setInputFiles({
        name: fileName,
        mimeType: 'text/csv',
        buffer: Buffer.from(csvContent)
    });

    await page.getByRole('button', { name: 'Upload CSV' }).click();
}

async function expectImportFailed(page, expectedErrorMessage) {
    const body = page.locator('body');

    await expect(body).toContainText('Import failed');
    await expect(body).toContainText('No data was imported.');
    await expect(body).toContainText(expectedErrorMessage);
}

test.describe('Admin Import Predictions page', () => {
    test('should open the admin import predictions tab and show upload form', async ({ page }) => {
        await openImportPredictionsPage(page);

        await expect(
            page.getByRole('heading', { name: 'Import Predictions' })
        ).toBeVisible();

        await expect(page.locator('input[type="file"]')).toBeVisible();

        await expect(
            page.getByRole('button', { name: 'Upload CSV' })
        ).toBeVisible();

        await expect(
            page.getByRole('button', { name: 'Upload CSV' })
        ).toBeEnabled();
    });

    test('should show validation message when upload is clicked without selecting a file', async ({ page }) => {
        await openImportPredictionsPage(page);

        await page.getByRole('button', { name: 'Upload CSV' }).click();

        await expectImportFailed(page, 'Please choose a CSV file first.');
    });

    test('should import valid CSV through UI and show prediction on public weekly page', async ({ page }) => {
        await openImportPredictionsPage(page);

        const csvContent = createCsv([
            createValidPredictionRow()
        ]);

        await uploadCsv(page, 'valid-prediction-import.csv', csvContent);

        const body = page.locator('body');

        await expect(body).toContainText('Import successful');
        await expect(body).toContainText('Created matches: 1');
        await expect(body).toContainText('Created statistics: 1');
        await expect(body).toContainText('Created predictions: 1');

        await page.goto('/');

        await expect(page.locator('body')).toContainText('Premier League');
        await expect(page.locator('body')).toContainText('Liverpool');
        await expect(page.locator('body')).toContainText('Everton');
    });

    test('should show backend validation error when CSV contains invalid league', async ({ page }) => {
        await openImportPredictionsPage(page);

        const csvContent = createCsv([
            createValidPredictionRow({
                leagueName: 'Wrong League'
            })
        ]);

        await uploadCsv(page, 'invalid-league.csv', csvContent);

        await expectImportFailed(
            page,
            "League 'Wrong League' does not exist."
        );
    });

    test('should show backend validation error when home recent form does not contain exactly 6 matches', async ({ page }) => {
        await openImportPredictionsPage(page);

        const csvContent = createCsv([
            createValidPredictionRow({
                homeRecentWins: 4,
                homeRecentDraws: 2,
                homeRecentLosses: 1
            })
        ]);

        await uploadCsv(page, 'invalid-recent-form.csv', csvContent);

        await expectImportFailed(
            page,
            'Row 1: Home recent form must contain exactly 6 matches.'
        );
    });

    test('should show backend validation error when head-to-head results do not match matches count', async ({ page }) => {
        await openImportPredictionsPage(page);

        const csvContent = createCsv([
            createValidPredictionRow({
                headToHeadMatchesCount: 3,
                headToHeadHomeWins: 1,
                headToHeadDraws: 1,
                headToHeadAwayWins: 0
            })
        ]);

        await uploadCsv(page, 'invalid-head-to-head.csv', csvContent);

        await expectImportFailed(
            page,
            'Row 1: Head-to-head results must equal head-to-head matches count.'
        );
    });

    test('should reject entire CSV import when one of multiple rows is invalid', async ({ page }) => {
        await openImportPredictionsPage(page);

        const csvContent = createCsv([
            createValidPredictionRow({
                kickoffTime: createKickoffTime(60)
            }),
            createValidPredictionRow({
                leagueName: 'Wrong League',
                kickoffTime: createKickoffTime(120)
            })
        ]);

        await uploadCsv(page, 'multi-row-invalid-league.csv', csvContent);

        await expectImportFailed(
            page,
            "Row 2: League 'Wrong League' does not exist."
        );
    });
});