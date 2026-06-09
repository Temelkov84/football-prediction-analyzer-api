import { useMemo, useState } from 'react'
import { createMatch } from '../../api/matchesApi'

function CreateMatchForm({ leagues, teams, onMatchCreated }) {
  const [leagueId, setLeagueId] = useState('')
  const [homeTeamId, setHomeTeamId] = useState('')
  const [awayTeamId, setAwayTeamId] = useState('')
  const [kickoffDate, setKickoffDate] = useState('')
  const [kickoffHour, setKickoffHour] = useState('')
  const [kickoffMinute, setKickoffMinute] = useState('')

  const [successMessage, setSuccessMessage] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  const hourOptions = Array.from({ length: 24 }, (_, index) =>
  index.toString().padStart(2, '0')
)

  const minuteOptions = ['00', '15', '30', '45']

  const filteredTeams = useMemo(() => {
    if (!leagueId) {
      return []
    }

    return teams.filter((team) => team.leagueId === Number(leagueId))
  }, [teams, leagueId])

  const availableAwayTeams = useMemo(() => {
  return filteredTeams.filter((team) => team.id !== Number(homeTeamId))
}, [filteredTeams, homeTeamId])

  function handleLeagueChange(event) {
    setLeagueId(event.target.value)
    setHomeTeamId('')
    setAwayTeamId('')
    setSuccessMessage('')
    setErrorMessage('')
  }

 async function handleSubmit(event) {
  event.preventDefault()

  if (homeTeamId === awayTeamId) {
    setSuccessMessage('')
    setErrorMessage('Home team and away team must be different.')
    return
  }

  try {
    setSuccessMessage('')
    setErrorMessage('')
    setIsSubmitting(true)

    const kickoffTime = `${kickoffDate}T${kickoffHour}:${kickoffMinute}:00`

    const createdMatch = await createMatch({
      leagueId: Number(leagueId),
      homeTeamId: Number(homeTeamId),
      awayTeamId: Number(awayTeamId),
      kickoffTime,
    })

    setSuccessMessage(`Match created with ID: ${createdMatch.id}`)
    onMatchCreated()

    setLeagueId('')
    setHomeTeamId('')
    setAwayTeamId('')
    setKickoffDate('')
    setKickoffHour('')
    setKickoffMinute('')
  } catch (error) {
    setErrorMessage(error.message)
  } finally {
    setIsSubmitting(false)
  }
}

const canSubmit =
  leagueId &&
  homeTeamId &&
  awayTeamId &&
  kickoffDate &&
  kickoffHour &&
  kickoffMinute &&
  filteredTeams.length >= 2 &&
  !isSubmitting

  return (
    <form className="admin-form" onSubmit={handleSubmit}>
      <h3>Create Match</h3>

      <div className="form-group">
        <label htmlFor="match-league">League</label>
        <select
          id="match-league"
          value={leagueId}
          onChange={handleLeagueChange}
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

      {leagueId && filteredTeams.length < 2 && (
        <p className="error-message">
          This league needs at least two teams before you can create a match.
        </p>
      )}

      <div className="form-group">
        <label htmlFor="home-team">Home Team</label>
        <select
          id="home-team"
          value={homeTeamId}
          onChange={(event) => setHomeTeamId(event.target.value)}
          required
          disabled={!leagueId || filteredTeams.length < 2}
        >
          <option value="">Select home team</option>

          {filteredTeams.map((team) => (
            <option key={team.id} value={team.id}>
              {team.name}
            </option>
          ))}
        </select>
      </div>

      <div className="form-group">
        <label htmlFor="away-team">Away Team</label>
        <select
          id="away-team"
          value={awayTeamId}
          onChange={(event) => setAwayTeamId(event.target.value)}
          required
          disabled={!leagueId || filteredTeams.length < 2}
        >
          <option value="">Select away team</option>

          {availableAwayTeams.map((team) => (
            <option key={team.id} value={team.id}>
              {team.name}
            </option>
          ))}
        </select>
      </div>

     <div className="form-group">
  <label htmlFor="kickoff-date">Kickoff Date</label>
  <input
    id="kickoff-date"
    type="date"
    value={kickoffDate}
    onChange={(event) => setKickoffDate(event.target.value)}
    required
  />
</div>

<div className="form-group">
  <label>Kickoff Time</label>

  <div className="time-select-row">
    <select
      value={kickoffHour}
      onChange={(event) => setKickoffHour(event.target.value)}
      required
    >
      <option value="">Hour</option>

      {hourOptions.map((hour) => (
        <option key={hour} value={hour}>
          {hour}
        </option>
      ))}
    </select>

    <select
      value={kickoffMinute}
      onChange={(event) => setKickoffMinute(event.target.value)}
      required
    >
      <option value="">Minute</option>

      {minuteOptions.map((minute) => (
        <option key={minute} value={minute}>
          {minute}
        </option>
      ))}
    </select>
  </div>
</div>

      <button type="submit" disabled={!canSubmit}>
        {isSubmitting ? 'Creating...' : 'Create Match'}
      </button>

      {successMessage && <p className="success-message">{successMessage}</p>}
      {errorMessage && <p className="error-message">{errorMessage}</p>}
    </form>
  )
}

export default CreateMatchForm