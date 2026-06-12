import { test, expect } from '@playwright/test';

test.describe('Admin Import Predictions page', () => {
    test('should open the admin import predictions tab', async ({ page }) => {
        await page.goto('/');

        await page.getByText('Admin').click();

        await page.getByRole('button', { name: 'Import Predictions' }).click();

        await expect(
            page.getByRole('heading', { name: 'Import Predictions' })
        ).toBeVisible();

        await expect(page.locator('input[type="file"]')).toBeVisible();

        await expect(
            page.getByRole('button', { name: 'Upload CSV' })
        ).toBeVisible();
    });

    test('should keep upload form visible after opening Import Predictions tab', async ({ page }) => {
        await page.goto('/');

        await page.getByText('Admin').click();

        await page.getByRole('button', { name: 'Import Predictions' }).click();

        const fileInput = page.locator('input[type="file"]');

        const uploadButton = page.getByRole('button', { name: 'Upload CSV' });

        await expect(fileInput).toBeVisible();
        await expect(uploadButton).toBeVisible();
        await expect(uploadButton).toBeEnabled();

    });

    test('should show validation message when upload is clicked without selecting a file', async ({ page }) => {
        await page.goto('/');

        await page.getByText('Admin').click();

        await page.getByRole('button', { name: 'Import Predictions' }).click();

        await page.getByRole('button', { name: 'Upload CSV' }).click();

        const body = page.locator('body');

        await expect(body).toContainText('Import failed');
        await expect(body).toContainText('No data was imported.');
        await expect(body).toContainText('Please choose a CSV file first.');
    });

    test('should show backed validation error when CSV contains invalid league', async ({ page }) => {
        await page.goto('/');

        await page.getByText('Admin').click();

        await page.getByRole('button', { name: 'Import Predictions' }).click();

        const fileInput = page.locator('input[type="file"]');

        const uploadButton = page.getByRole('button', { name: 'Upload CSV' });

        const csvContent = `league_name,home_team_name,away_team_name,kickoff_time,home_recent_wins,home_recent_draws,home_recent_losses,away_recent_wins,away_recent_draws,away_recent_losses,home_last10_home_wins,home_last10_home_draws,home_last10_home_losses,away_last10_away_wins,away_last10_away_draws,away_last10_away_losses,home_xg_for_average,home_xg_against_average,away_xg_for_average,away_xg_against_average,home_goals_scored_average,away_goals_scored_average,home_goals_conceded_average,away_goals_conceded_average,home_shots_on_target_for_average,home_shots_on_target_against_average,away_shots_on_target_for_average,away_shots_on_target_against_average,head_to_head_matches_count,head_to_head_home_wins,head_to_head_draws,head_to_head_away_wins,home_key_players_missing_impact,away_key_players_missing_impact,home_fatigue_impact,away_fatigue_impact
        Wrong League,Liverpool,Everton,2026-06-15T19:00:00Z,3,2,1,2,2,2,6,2,2,3,3,4,2.1,0.9,1.4,1.2,2.0,1.3,0.8,1.1,6.2,3.1,4.8,4.2,2,1,0,1,0,0,0,0`;

        await fileInput.setInputFiles({
            name: 'invalid-league.csv',
            mimeType: 'text/csv',
            buffer: Buffer.from(csvContent)
        });

        await uploadButton.click();

        await page.getByRole('button', { name: 'Upload CSV' }).click();

        const body = page.locator('body');

        await expect(body).toContainText('Import failed');
        await expect(body).toContainText('No data was imported.');
        await expect(body).toContainText("League 'Wrong League' does not exist.");
    });

   test('should show backend validation error when home recent form does not contain exactly 6 matches', async ({ page }) => {
        await page.goto('/');

        await page.getByText('Admin').click();

        await page.getByRole('button', { name: 'Import Predictions' }).click();

        const fileInput = page.locator('input[type="file"]');

        const uploadButton = page.getByRole('button', { name: 'Upload CSV' });

        const csvContent = `league_name,home_team_name,away_team_name,kickoff_time,home_recent_wins,home_recent_draws,home_recent_losses,away_recent_wins,away_recent_draws,away_recent_losses,home_last10_home_wins,home_last10_home_draws,home_last10_home_losses,away_last10_away_wins,away_last10_away_draws,away_last10_away_losses,home_xg_for_average,home_xg_against_average,away_xg_for_average,away_xg_against_average,home_goals_scored_average,away_goals_scored_average,home_goals_conceded_average,away_goals_conceded_average,home_shots_on_target_for_average,home_shots_on_target_against_average,away_shots_on_target_for_average,away_shots_on_target_against_average,head_to_head_matches_count,head_to_head_home_wins,head_to_head_draws,head_to_head_away_wins,home_key_players_missing_impact,away_key_players_missing_impact,home_fatigue_impact,away_fatigue_impact
        Premier League,Liverpool,Everton,2026-06-15T19:00:00Z,4,2,1,2,2,2,6,2,2,3,3,4,2.1,0.9,1.4,1.2,2.0,1.3,0.8,1.1,6.2,3.1,4.8,4.2,2,1,0,1,0,0,0,0`;

        await fileInput.setInputFiles({
            name: 'invalid-league.csv',
            mimeType: 'text/csv',
            buffer: Buffer.from(csvContent)
        });

        await uploadButton.click();

        await page.getByRole('button', { name: 'Upload CSV' }).click();

        const body = page.locator('body');

        await expect(body).toContainText('Import failed');
        await expect(body).toContainText('No data was imported.');
        await expect(body).toContainText('Row 1: Home recent form must contain exactly 6 matches.');
    });

     test('should show backend validation error when head-to-head results do not match matches count', async ({ page }) => {
        await page.goto('/');

        await page.getByText('Admin').click();

        await page.getByRole('button', { name: 'Import Predictions' }).click();

        const fileInput = page.locator('input[type="file"]');

        const uploadButton = page.getByRole('button', { name: 'Upload CSV' });

        const csvContent = `league_name,home_team_name,away_team_name,kickoff_time,home_recent_wins,home_recent_draws,home_recent_losses,away_recent_wins,away_recent_draws,away_recent_losses,home_last10_home_wins,home_last10_home_draws,home_last10_home_losses,away_last10_away_wins,away_last10_away_draws,away_last10_away_losses,home_xg_for_average,home_xg_against_average,away_xg_for_average,away_xg_against_average,home_goals_scored_average,away_goals_scored_average,home_goals_conceded_average,away_goals_conceded_average,home_shots_on_target_for_average,home_shots_on_target_against_average,away_shots_on_target_for_average,away_shots_on_target_against_average,head_to_head_matches_count,head_to_head_home_wins,head_to_head_draws,head_to_head_away_wins,home_key_players_missing_impact,away_key_players_missing_impact,home_fatigue_impact,away_fatigue_impact
        Premier League,Liverpool,Everton,2026-06-15T19:00:00Z,3,2,1,2,2,2,6,2,2,3,3,4,2.1,0.9,1.4,1.2,2.0,1.3,0.8,1.1,6.2,3.1,4.8,4.2,3,1,1,0,0,0,0,0`;

        await fileInput.setInputFiles({
            name: 'invalid-league.csv',
            mimeType: 'text/csv',
            buffer: Buffer.from(csvContent)
        });

        await uploadButton.click();

        await page.getByRole('button', { name: 'Upload CSV' }).click();

        const body = page.locator('body');

        await expect(body).toContainText('Import failed');
        await expect(body).toContainText('No data was imported.');
        await expect(body).toContainText('Row 1: Head-to-head results must equal head-to-head matches count.');
    });
});