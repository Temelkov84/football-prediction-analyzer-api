export async function clearWeeklyData() {
  const response = await fetch('/api/admin/weekly-data', {
    method: 'DELETE',
  })

  if (!response.ok) {
    const errorData = await response.json()
    throw new Error(errorData.message || 'Failed to clear weekly data.')
  }

  return await response.json()
}