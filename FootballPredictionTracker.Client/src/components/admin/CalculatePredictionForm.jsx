import { useState } from 'react'
import { calculatePrediction } from '../../api/adminPredictionsApi'

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

function createMatchKey(item) {
  return `${item.league}|${item.homeTeam}|${item.awayTeam}|${item.kickoffTime}`
}

function CalculatePredictionForm({ matches, predictions, onPredictionCalculated }) {
  const [matchId, setMatchId] = useState('')
  const [calculatedPrediction, setCalculatedPrediction] = useState(null)
  const [successMessage, setSuccessMessage] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  const predictedMatchKeys = predictions.map((prediction) =>
    createMatchKey(prediction)
  )

  const availableMatches = matches
    .filter((match) => !predictedMatchKeys.includes(createMatchKey(match)))
    .sort(
      (firstMatch, secondMatch) =>
        new Date(firstMatch.kickoffTime) - new Date(secondMatch.kickoffTime)
    )

  async function handleSubmit(event) {
    event.preventDefault()

    try {
      setSuccessMessage('')
      setErrorMessage('')
      setCalculatedPrediction(null)
      setIsSubmitting(true)

      const prediction = await calculatePrediction(matchId)

      setCalculatedPrediction(prediction)
      setSuccessMessage('Prediction calculated successfully.')

      await onPredictionCalculated()

      setMatchId('')
    } catch (error) {
      setErrorMessage(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form className="admin-form" onSubmit={handleSubmit}>
      <h3>Calculate Prediction</h3>

      {availableMatches.length === 0 && (
        <p className="success-message">
          All available matches already have calculated predictions.
        </p>
      )}

      {availableMatches.length > 0 && (
        <div className="form-group">
          <label htmlFor="prediction-match">Match</label>
          <select
            id="prediction-match"
            value={matchId}
            onChange={(event) => setMatchId(event.target.value)}
            required
          >
            <option value="">Select match</option>

            {availableMatches.map((match) => (
              <option key={match.id} value={match.id}>
                {formatMatchLabel(match)}
              </option>
            ))}
          </select>
        </div>
      )}

      <button
        type="submit"
        disabled={isSubmitting || availableMatches.length === 0 || !matchId}
      >
        {isSubmitting ? 'Calculating...' : 'Calculate Prediction'}
      </button>

      {successMessage && <p className="success-message">{successMessage}</p>}
      {errorMessage && <p className="error-message">{errorMessage}</p>}

      {calculatedPrediction && (
        <div className="prediction-result">
          <h4>Calculated Result</h4>
          <p>
            {calculatedPrediction.homeTeam} vs {calculatedPrediction.awayTeam}
          </p>
          <p>
            1: {calculatedPrediction.homeWinProbability}% | X:{' '}
            {calculatedPrediction.drawProbability}% | 2:{' '}
            {calculatedPrediction.awayWinProbability}%
          </p>
        </div>
      )}
    </form>
  )
}

export default CalculatePredictionForm