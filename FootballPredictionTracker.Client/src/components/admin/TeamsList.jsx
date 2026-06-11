import { useState } from 'react'
import { deleteTeam, updateTeam } from '../../api/teamsApi'

function TeamsList({ teams, leagues, onTeamChanged }) {
    const [editingTeamId, setEditingTeamId] = useState(null)
    const [editName, setEditName] = useState('')
    const [editLeagueId, setEditLeagueId] = useState('')
    const [error, setError] = useState('')

    function startEditing(team) {
        setEditingTeamId(team.id)
        setEditName(team.name)
        setEditLeagueId(team.leagueId.toString())
        setError('')
    }

    function cancelEditing() {
        setEditingTeamId(null)
        setEditName('')
        setEditLeagueId('')
        setError('')
    }

    async function handleUpdateTeam(teamId) {
        try {
            setError('')

            await updateTeam(teamId, {
                name: editName,
                leagueId: Number(editLeagueId),
            })

            cancelEditing()
            onTeamChanged()
        } catch (error) {
            setError(error.message)
        }
    }

    async function handleDeleteTeam(teamId) {
        const confirmed = window.confirm('Are you sure you want to delete this team?')

        if (!confirmed) {
            return
        }

        try {
            setError('')

            await deleteTeam(teamId)
            onTeamChanged()
        } catch (error) {
            setError(error.message)
        }
    }

    if (!teams || teams.length === 0) {
        return (
            <div className="admin-list">
                <h3>Existing Teams</h3>
                <p>No teams created yet.</p>
            </div>
        )
    }

    function getLeagueName(leagueId) {
        const league = leagues.find((league) => league.id === leagueId)

        return league ? league.name : '-'
    }

    return (
        <div className="admin-list">
            <h3>Existing Teams</h3>

            {error && <p className="error-message">{error}</p>}

            <div className="table-wrapper">
                <table className="admin-table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>League</th>
                            <th>Actions</th>
                        </tr>
                    </thead>

                    <tbody>
                        {teams.map((team) => (
                            <tr key={team.id}>
                                <td>
                                    {editingTeamId === team.id ? (
                                        <input
                                            type="text"
                                            value={editName}
                                            onChange={(event) => setEditName(event.target.value)}
                                        />
                                    ) : (
                                        team.name
                                    )}
                                </td>

                                <td>
                                    {editingTeamId === team.id ? (
                                        <select
                                            value={editLeagueId}
                                            onChange={(event) => setEditLeagueId(event.target.value)}
                                        >
                                            <option value="">Select league</option>

                                            {leagues.map((league) => (
                                                <option key={league.id} value={league.id}>
                                                    {league.country} — {league.name}
                                                </option>
                                            ))}
                                        </select>
                                    ) : (
                                        getLeagueName(team.leagueId)
                                    )}
                                </td>

                                <td>
                                    {editingTeamId === team.id ? (
                                        <>
                                            <button
                                                type="button"
                                                onClick={() => handleUpdateTeam(team.id)}
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
                                                onClick={() => startEditing(team)}
                                            >
                                                Edit
                                            </button>

                                            <button
                                                type="button"
                                                onClick={() => handleDeleteTeam(team.id)}
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

export default TeamsList