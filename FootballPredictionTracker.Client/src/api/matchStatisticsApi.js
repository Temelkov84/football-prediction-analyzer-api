export async function getMatchStatistics() {
  const response = await fetch('/api/admin/match-statistics')

  if (!response.ok) {
    throw new Error('Failed to load match statistics.')
  }

  return await response.json()
}

export async function createMatchStatistics(statistics) {
  const response = await fetch('/api/admin/match-statistics', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(statistics),
  })

  if (!response.ok) {
    let errorMessage = 'Failed to create match statistics.'

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