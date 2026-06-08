export async function getMatches() {
  const response = await fetch('/api/admin/matches')

  if (!response.ok) {
    throw new Error('Failed to load matches.')
  }

  return await response.json()
}

export async function createMatch(match) {
  const response = await fetch('/api/admin/matches', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(match),
  })

  const data = await response.json()

  if (!response.ok) {
    throw new Error(data.message || 'Failed to create match.')
  }

  return data
}