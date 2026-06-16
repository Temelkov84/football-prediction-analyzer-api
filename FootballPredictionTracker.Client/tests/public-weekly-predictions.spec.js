import { test, expect } from '@playwright/test';

test.describe('Public Weekly Predictions page', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should display the public page title and weekly predictions section', async ({ page }) => {
        await expect(
            page.getByRole('heading', { name: 'Football Prediction Analyzer' })
        ).toBeVisible();

        await expect(
            page.getByText('Weekly football predictions based on team statistics and weighted factors.')
        ).toBeVisible();

        await expect(
            page.getByRole('heading', { name: 'Weekly Predictions' })
        ).toBeVisible();

        await expect(
            page.getByText('1 = Home win, X = Draw, 2 = Away win')
        ).toBeVisible();
    });

    test('should show either weekly predictions or empty state after loading finishes', async ({ page }) => {
        const body = page.locator('body');

        await expect(body).not.toContainText('Loading predictions...');

        await expect(body).toContainText(
            /No predictions available for the next 7 days\.|vs/i
        );
    });
});