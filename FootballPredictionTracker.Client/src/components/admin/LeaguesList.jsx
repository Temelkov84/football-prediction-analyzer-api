import { useState } from 'react'
import { deleteLeague, updateLeague } from '../../api/leaguesApi'

function LeaguesList({ leagues, onLeagueChanged }) {
  const [editingLeagueId, setEditingLeagueId] = useState(null)
  const [editName, setEditName] = useState('')
  const [editCountry, setEditCountry] = useState('')
  const [error, setError] = useState('')

  function startEditing(league) {
    setEditingLeagueId(league.id)
    setEditName(league.name)
    setEditCountry(league.country)
    setError('')
  }

  function cancelEditing() {
    setEditingLeagueId(null)
    setEditName('')
    setEditCountry('')
    setError('')
  }

  async function handleUpdateLeague(leagueId) {
    try {
      setError('')

      await updateLeague(leagueId, {
        name: editName,
        country: editCountry,
      })

      cancelEditing()
      onLeagueChanged()
    } catch (error) {
      setError(error.message)
    }
  }

  async function handleDeleteLeague(leagueId) {
    const confirmed = window.confirm('Are you sure you want to delete this league?')

    if (!confirmed) {
      return
    }

    try {
      setError('')

      await deleteLeague(leagueId)
      onLeagueChanged()
    } catch (error) {
      setError(error.message)
    }
  }

  if (!leagues || leagues.length === 0) {
    return (
      <div className="admin-list">
        <h3>Existing Leagues</h3>
        <p>No leagues created yet.</p>
      </div>
    )
  }

  return (
    <div className="admin-list">
      <h3>Existing Leagues</h3>

      {error && <p className="error-message">{error}</p>}

      <div className="table-wrapper">
        <table className="admin-table">
          <thead>
            <tr>
              <th>Name</th>
              <th>Country</th>
              <th>Actions</th>
            </tr>
          </thead>

          <tbody>
            {leagues.map((league) => (
              <tr key={league.id}>
                <td>
                  {editingLeagueId === league.id ? (
                    <input
                      type="text"
                      value={editName}
                      onChange={(event) => setEditName(event.target.value)}
                    />
                  ) : (
                    league.name
                  )}
                </td>

                <td>
                  {editingLeagueId === league.id ? (
                    <input
                      type="text"
                      value={editCountry}
                      onChange={(event) => setEditCountry(event.target.value)}
                    />
                  ) : (
                    league.country
                  )}
                </td>

                <td>
                  {editingLeagueId === league.id ? (
                    <>
                      <button
                        type="button"
                        onClick={() => handleUpdateLeague(league.id)}
                      >
                        Save
                      </button>

                      <button
                        type="button"
                        onClick={cancelEditing}
                      >
                        Cancel
                      </button>
                    </>
                  ) : (
                    <>
                      <button
                        type="button"
                        onClick={() => startEditing(league)}
                      >
                        Edit
                      </button>

                      <button
                        type="button"
                        onClick={() => handleDeleteLeague(league.id)}
                      >
                        Delete
                      </button>
                    </>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

export default LeaguesList