export async function getAdminPredictions() {
  const response = await fetch('/api/admin/predictions')

  if (!response.ok) {
    throw new Error('Failed to load predictions.')
  }

  return await response.json()
}

export async function calculatePrediction(matchId) {
  const response = await fetch(`/api/admin/predictions/calculate/${matchId}`, {
    method: 'POST',
  })

  if (!response.ok) {
    let errorMessage = 'Failed to calculate prediction.'

    try {
      const errorData = await response.json()
      errorMessage = errorData.message || errorMessage
    } catch {
      // Response did not contain JSON.
    }

    throw new Error(errorMessage)
  }

  return await response.json()
}