export async function getWeeklyPredictions() {
  const response = await fetch('/api/Predictions/weekly')

  if (!response.ok) {
    throw new Error('Failed to load weekly predictions.')
  }

  return await response.json()
}