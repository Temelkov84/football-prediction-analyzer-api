import { useState } from 'react'
import { importPredictionsFromCsv } from '../../api/adminPredictionImportsApi'

export default function ImportPredictionsForm() {
  const [selectedFile, setSelectedFile] = useState(null)
  const [isUploading, setIsUploading] = useState(false)
  const [result, setResult] = useState(null)
  const [error, setError] = useState(null)

  function handleFileChange(event) {
    setSelectedFile(event.target.files[0])
    setResult(null)
    setError(null)
  }

  async function handleSubmit(event) {
    event.preventDefault()

    if (!selectedFile) {
      setError('Please choose a CSV file first.')
      return
    }

    setIsUploading(true)
    setResult(null)
    setError(null)

    try {
      const importResult = await importPredictionsFromCsv(selectedFile)
      setResult(importResult)
      setSelectedFile(null)
      event.target.reset()
    } catch (err) {
      setError(formatImportError(err))
    } finally {
      setIsUploading(false)
    }
  }

  return (
    <section>
      <h2>Import Predictions from CSV</h2>

      <p>
        Upload one CSV file containing one or many prediction rows. The import is
        all-or-nothing: if any row has an error, no data should be imported.
      </p>

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="prediction-csv">CSV file</label>
          <br />
          <input
            id="prediction-csv"
            type="file"
            accept=".csv,text/csv"
            onChange={handleFileChange}
          />
        </div>

        <button type="submit" disabled={isUploading}>
          {isUploading ? 'Uploading...' : 'Upload CSV'}
        </button>
      </form>

      {error && (
        <div style={{ marginTop: '1rem', color: 'crimson' }}>
          <h3>Import failed</h3>
          <p>No data was imported.</p>
          <pre>{error}</pre>
        </div>
      )}

      {result && (
        <div style={{ marginTop: '1rem' }}>
          <h3>Import successful</h3>

          <ul>
            <li>Created matches: {result.createdMatches}</li>
            <li>Created statistics: {result.createdStatistics}</li>
            <li>Created predictions: {result.createdPredictions}</li>
          </ul>

          {result.importedPredictions?.length > 0 && (
            <>
              <h4>Imported predictions</h4>

              <table>
                <thead>
                  <tr>
                    <th>Date / Time</th>
                    <th>League</th>
                    <th>Match</th>
                    <th>1</th>
                    <th>X</th>
                    <th>2</th>
                  </tr>
                </thead>
                <tbody>
                  {result.importedPredictions.map((prediction) => (
                    <tr key={prediction.predictionId}>
                      <td>{formatDate(prediction.kickoffTime)}</td>
                      <td>{prediction.league}</td>
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
            </>
          )}
        </div>
      )}
    </section>
  )
}

function formatDate(value) {
  return new Date(value).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function formatImportError(error) {
  if (!error) {
    return 'Unknown import error.'
  }

  if (typeof error === 'string') {
    return error
  }

  if (error.message) {
    return error.message
  }

  return JSON.stringify(error, null, 2)
}