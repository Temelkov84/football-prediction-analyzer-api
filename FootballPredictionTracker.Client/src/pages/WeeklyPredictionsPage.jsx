import { useEffect, useState } from 'react'
import { getWeeklyPredictions } from '../api/predictionsApi'
import PredictionsTable from '../components/PredictionsTable'

function WeeklyPredictionsPage() {
  const [predictions, setPredictions] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    async function loadPredictions() {
      try {
        setError('')
        setIsLoading(true)

        const data = await getWeeklyPredictions()
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
    <main className="weekly-page">
      <section className="weekly-hero">
        <h1>Football Prediction Analyzer</h1>
        <p>Weekly football predictions based on team statistics and weighted factors.</p>
      </section>

      <section className="weekly-content">
        <h2>Weekly Predictions</h2>

        {isLoading && <p>Loading predictions...</p>}

        {error && <p className="error-message">{error}</p>}

        {!isLoading && !error && predictions.length === 0 && (
          <p>No predictions available for the next 7 days.</p>
        )}

        {!isLoading && !error && predictions.length > 0 && (
          <PredictionsTable predictions={predictions} />
        )}
      </section>
    </main>
  )
}

export default WeeklyPredictionsPage