import { useState } from 'react'
import { createTeam } from '../../api/teamsApi'

function CreateTeamForm({ leagues, onTeamCreated }) {
  const [name, setName] = useState('')
  const [leagueId, setLeagueId] = useState('')
  const [successMessage, setSuccessMessage] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(event) {
    event.preventDefault()

    try {
      setSuccessMessage('')
      setErrorMessage('')
      setIsSubmitting(true)

      const createdTeam = await createTeam({
        name,
        leagueId: Number(leagueId),
      })

      setSuccessMessage(`Team created: ${createdTeam.name}`)
      onTeamCreated()
      setName('')
      setLeagueId('')
    } catch (error) {
      setErrorMessage(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form className="admin-form" onSubmit={handleSubmit}>
      <h3>Create Team</h3>

      <div className="form-group">
        <label htmlFor="team-name">Team Name</label>
        <input
          id="team-name"
          type="text"
          value={name}
          onChange={(event) => setName(event.target.value)}
          placeholder="Leeds United"
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="team-league">League</label>
        <select
          id="team-league"
          value={leagueId}
          onChange={(event) => setLeagueId(event.target.value)}
          required
        >
          <option value="">Select league</option>

          {leagues.map((league) => (
            <option key={league.id} value={league.id}>
              {league.name} ({league.country})
            </option>
          ))}
        </select>
      </div>

      <button type="submit" disabled={isSubmitting || leagues.length === 0}>
        {isSubmitting ? 'Creating...' : 'Create Team'}
      </button>

      {successMessage && <p className="success-message">{successMessage}</p>}
      {errorMessage && <p className="error-message">{errorMessage}</p>}
    </form>
  )
}

export default CreateTeamForm