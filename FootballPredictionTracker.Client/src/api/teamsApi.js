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

export async function updateTeam(id, team) {
  const response = await fetch(`/api/admin/teams/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(team),
  })

  if (!response.ok) {
    const errorData = await response.json()
    throw new Error(errorData.message || 'Failed to update team.')
  }
}

export async function deleteTeam(id) {
  const response = await fetch(`/api/admin/teams/${id}`, {
    method: 'DELETE',
  })

  if (!response.ok) {
    const errorData = await response.json()
    throw new Error(errorData.message || 'Failed to delete team.')
  }
}