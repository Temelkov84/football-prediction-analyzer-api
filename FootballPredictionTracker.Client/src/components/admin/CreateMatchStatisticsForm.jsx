import { useState } from 'react'
import { createMatchStatistics } from '../../api/matchStatisticsApi'

function formatMatchLabel(match) {
  const kickoffTime = new Date(match.kickoffTime).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })

  return `${match.league} | ${match.homeTeam} vs ${match.awayTeam} | ${kickoffTime}`
}

function CreateMatchStatisticsForm({ matches, onStatisticsCreated }) {
  const [matchId, setMatchId] = useState('')

  const [homeTeamRecentWins, setHomeTeamRecentWins] = useState('')
  const [homeTeamRecentDraws, setHomeTeamRecentDraws] = useState('')
  const [homeTeamRecentLosses, setHomeTeamRecentLosses] = useState('')

  const [awayTeamRecentWins, setAwayTeamRecentWins] = useState('')
  const [awayTeamRecentDraws, setAwayTeamRecentDraws] = useState('')
  const [awayTeamRecentLosses, setAwayTeamRecentLosses] = useState('')

  const [headToHeadHomeWins, setHeadToHeadHomeWins] = useState('')
  const [headToHeadDraws, setHeadToHeadDraws] = useState('')
  const [headToHeadAwayWins, setHeadToHeadAwayWins] = useState('')

  const [homeTeamHomeWins, setHomeTeamHomeWins] = useState('')
  const [homeTeamHomeDraws, setHomeTeamHomeDraws] = useState('')
  const [homeTeamHomeLosses, setHomeTeamHomeLosses] = useState('')

  const [awayTeamAwayWins, setAwayTeamAwayWins] = useState('')
  const [awayTeamAwayDraws, setAwayTeamAwayDraws] = useState('')
  const [awayTeamAwayLosses, setAwayTeamAwayLosses] = useState('')

  const [homeTeamGoalsScored, setHomeTeamGoalsScored] = useState('')
  const [homeTeamGoalsConceded, setHomeTeamGoalsConceded] = useState('')
  const [awayTeamGoalsScored, setAwayTeamGoalsScored] = useState('')
  const [awayTeamGoalsConceded, setAwayTeamGoalsConceded] = useState('')

  const [successMessage, setSuccessMessage] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  function toNumber(value) {
    return Number(value)
  }

  function resetForm() {
    setMatchId('')

    setHomeTeamRecentWins('')
    setHomeTeamRecentDraws('')
    setHomeTeamRecentLosses('')

    setAwayTeamRecentWins('')
    setAwayTeamRecentDraws('')
    setAwayTeamRecentLosses('')

    setHeadToHeadHomeWins('')
    setHeadToHeadDraws('')
    setHeadToHeadAwayWins('')

    setHomeTeamHomeWins('')
    setHomeTeamHomeDraws('')
    setHomeTeamHomeLosses('')

    setAwayTeamAwayWins('')
    setAwayTeamAwayDraws('')
    setAwayTeamAwayLosses('')

    setHomeTeamGoalsScored('')
    setHomeTeamGoalsConceded('')
    setAwayTeamGoalsScored('')
    setAwayTeamGoalsConceded('')
  }

  async function handleSubmit(event) {
    event.preventDefault()

    try {
      setSuccessMessage('')
      setErrorMessage('')
      setIsSubmitting(true)

      const createdStatistics = await createMatchStatistics({
        matchId: toNumber(matchId),

        homeTeamRecentWins: toNumber(homeTeamRecentWins),
        homeTeamRecentDraws: toNumber(homeTeamRecentDraws),
        homeTeamRecentLosses: toNumber(homeTeamRecentLosses),

        awayTeamRecentWins: toNumber(awayTeamRecentWins),
        awayTeamRecentDraws: toNumber(awayTeamRecentDraws),
        awayTeamRecentLosses: toNumber(awayTeamRecentLosses),

        headToHeadHomeWins: toNumber(headToHeadHomeWins),
        headToHeadDraws: toNumber(headToHeadDraws),
        headToHeadAwayWins: toNumber(headToHeadAwayWins),

        homeTeamHomeWins: toNumber(homeTeamHomeWins),
        homeTeamHomeDraws: toNumber(homeTeamHomeDraws),
        homeTeamHomeLosses: toNumber(homeTeamHomeLosses),

        awayTeamAwayWins: toNumber(awayTeamAwayWins),
        awayTeamAwayDraws: toNumber(awayTeamAwayDraws),
        awayTeamAwayLosses: toNumber(awayTeamAwayLosses),

        homeTeamGoalsScored: toNumber(homeTeamGoalsScored),
        homeTeamGoalsConceded: toNumber(homeTeamGoalsConceded),
        awayTeamGoalsScored: toNumber(awayTeamGoalsScored),
        awayTeamGoalsConceded: toNumber(awayTeamGoalsConceded),
      })

      setSuccessMessage(`Match statistics created with ID: ${createdStatistics.id}`)
      onStatisticsCreated()
      resetForm()
    } catch (error) {
      setErrorMessage(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form className="admin-form admin-form-wide" onSubmit={handleSubmit}>
      <h3>Create Match Statistics</h3>

      <div className="form-group">
        <label htmlFor="statistics-match">Match</label>
        <select
          id="statistics-match"
          value={matchId}
          onChange={(event) => setMatchId(event.target.value)}
          required
        >
          <option value="">Select match</option>

          {matches.map((match) => (
            <option key={match.id} value={match.id}>
              {formatMatchLabel(match)}
            </option>
          ))}
        </select>
      </div>

      <div className="form-grid">
        <div className="form-group">
          <label>Home Recent Wins</label>
          <input type="number" min="0" value={homeTeamRecentWins} onChange={(event) => setHomeTeamRecentWins(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Home Recent Draws</label>
          <input type="number" min="0" value={homeTeamRecentDraws} onChange={(event) => setHomeTeamRecentDraws(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Home Recent Losses</label>
          <input type="number" min="0" value={homeTeamRecentLosses} onChange={(event) => setHomeTeamRecentLosses(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Recent Wins</label>
          <input type="number" min="0" value={awayTeamRecentWins} onChange={(event) => setAwayTeamRecentWins(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Recent Draws</label>
          <input type="number" min="0" value={awayTeamRecentDraws} onChange={(event) => setAwayTeamRecentDraws(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Recent Losses</label>
          <input type="number" min="0" value={awayTeamRecentLosses} onChange={(event) => setAwayTeamRecentLosses(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>H2H Home Wins</label>
          <input type="number" min="0" value={headToHeadHomeWins} onChange={(event) => setHeadToHeadHomeWins(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>H2H Draws</label>
          <input type="number" min="0" value={headToHeadDraws} onChange={(event) => setHeadToHeadDraws(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>H2H Away Wins</label>
          <input type="number" min="0" value={headToHeadAwayWins} onChange={(event) => setHeadToHeadAwayWins(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Home Home Wins</label>
          <input type="number" min="0" value={homeTeamHomeWins} onChange={(event) => setHomeTeamHomeWins(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Home Home Draws</label>
          <input type="number" min="0" value={homeTeamHomeDraws} onChange={(event) => setHomeTeamHomeDraws(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Home Home Losses</label>
          <input type="number" min="0" value={homeTeamHomeLosses} onChange={(event) => setHomeTeamHomeLosses(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Away Wins</label>
          <input type="number" min="0" value={awayTeamAwayWins} onChange={(event) => setAwayTeamAwayWins(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Away Draws</label>
          <input type="number" min="0" value={awayTeamAwayDraws} onChange={(event) => setAwayTeamAwayDraws(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Away Losses</label>
          <input type="number" min="0" value={awayTeamAwayLosses} onChange={(event) => setAwayTeamAwayLosses(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Home Goals Scored</label>
          <input type="number" min="0" value={homeTeamGoalsScored} onChange={(event) => setHomeTeamGoalsScored(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Home Goals Conceded</label>
          <input type="number" min="0" value={homeTeamGoalsConceded} onChange={(event) => setHomeTeamGoalsConceded(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Goals Scored</label>
          <input type="number" min="0" value={awayTeamGoalsScored} onChange={(event) => setAwayTeamGoalsScored(event.target.value)} required />
        </div>

        <div className="form-group">
          <label>Away Goals Conceded</label>
          <input type="number" min="0" value={awayTeamGoalsConceded} onChange={(event) => setAwayTeamGoalsConceded(event.target.value)} required />
        </div>
      </div>

      <button type="submit" disabled={isSubmitting || matches.length === 0}>
        {isSubmitting ? 'Creating...' : 'Create Match Statistics'}
      </button>

      {successMessage && <p className="success-message">{successMessage}</p>}
      {errorMessage && <p className="error-message">{errorMessage}</p>}
    </form>
  )
}

export default CreateMatchStatisticsForm