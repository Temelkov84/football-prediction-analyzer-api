import { useEffect, useState } from 'react'
import './App.css'

function App() {
  const [predictions, setPredictions] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState('')

useEffect(() => {
  async function loadPredictions() {
    try {
      setError('')
      setIsLoading(true)

      const response = await fetch('/api/Predictions/weekly')

      if (!response.ok) {
        throw new Error('Failed to load predictions.')
      }

      const data = await response.json()
      setPredictions(data)
    } catch (error) {
      setError(error.message)
    } finally {
      setIsLoading(false)
    }
  }

  loadPredictions()
}, [])

  return (
    <main className="page">
      <section className="hero">
        <h1>Football Prediction Analyzer</h1>
        <p>Weekly football predictions based on team statistics and weighted factors.</p>
      </section>

      <section className="card">
        <h2>Weekly Predictions</h2>

        {isLoading && <p>Loading predictions...</p>}

        {error && <p className="error-message">{error}</p>}

        {!isLoading && !error && predictions.length === 0 && (
          <p>No predictions available for the next 7 days.</p>
        )}

        {!isLoading && !error && predictions.length > 0 && (
          <table className="predictions-table">
            <thead>
              <tr>
                <th>League</th>
                <th>Date</th>
                <th>Match</th>
                <th>1</th>
                <th>X</th>
                <th>2</th>
              </tr>
            </thead>

            <tbody>
              {predictions.map((prediction, index) => (
                <tr key={index}>
                  <td>{prediction.league}</td>
                  <td>{new Date(prediction.kickoffTime).toLocaleString()}</td>
                  <td>
                    {prediction.homeTeam} vs {prediction.awayTeam}
                  </td>
                  <td>{prediction.homeWinProbability}%</td>
                  <td>{prediction.drawProbability}%</td>
                  <td>{prediction.awayWinProbability}%</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </section>
    </main>
  )
}

export default App