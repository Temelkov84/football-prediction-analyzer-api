export async function getLeagues() {
  const response = await fetch('/api/admin/leagues')

  if (!response.ok) {
    throw new Error('Failed to load leagues.')
  }

  return await response.json()
}

export async function createLeague(league) {
  const response = await fetch('/api/admin/leagues', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(league),
  })

  const data = await response.json()

  if (!response.ok) {
    throw new Error(data.message || 'Failed to create league.')
  }

  return data
}

export async function updateLeague(id, league) {
  const response = await fetch(`/api/admin/leagues/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(league),
  })

  if (!response.ok) {
    const errorData = await response.json()
    throw new Error(errorData.message || 'Failed to update league.')
  }
}

export async function deleteLeague(id) {
  const response = await fetch(`/api/admin/leagues/${id}`, {
    method: 'DELETE',
  })

  if (!response.ok) {
    const errorData = await response.json()
    throw new Error(errorData.message || 'Failed to delete league.')
  }
}