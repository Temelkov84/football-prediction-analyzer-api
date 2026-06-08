export async function getTeams() {
  const response = await fetch('/api/admin/teams')

  if (!response.ok) {
    throw new Error('Failed to load teams.')
  }

  return await response.json()
}

export async function createTeam(team) {
  const response = await fetch('/api/admin/teams', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(team),
  })

  const data = await response.json()

  if (!response.ok) {
    throw new Error(data.message || 'Failed to create team.')
  }

  return data
}