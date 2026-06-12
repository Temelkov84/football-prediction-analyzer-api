import { test, expect } from '@playwright/test';

test.describe('Public Weekly Predictions page', () => {
  test('should load the public weekly predictions page', async ({ page }) => {
    await page.goto('/');

    await expect(
      page.getByRole('heading', { name: 'Football Prediction Analyzer' })
    ).toBeVisible();

    await expect(
      page.getByRole('heading', { name: 'Weekly Predictions' })
    ).toBeVisible();
  });

  test('should show predictions area with either predictions or empty state', async ({ page }) => {
    await page.goto('/');

    const body = page.locator('body');

    await expect(body).toContainText('Weekly Predictions');
    await expect(body).toContainText('1 = Home win, X = Draw, 2 = Away win');

    await expect(body).toContainText(
      /No predictions available|vs|Premier League|Liverpool|Everton/i
    );
  });
});