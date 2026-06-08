function formatKickoffTime(kickoffTime) {
  return new Date(kickoffTime).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function AdminPredictionsList({ predictions }) {
  if (predictions.length === 0) {
    return <p>No predictions calculated yet.</p>
  }

  return (
    <div className="admin-list">
      <h3>Existing Predictions</h3>

      <table className="admin-table">
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
          {predictions.map((prediction) => (
            <tr key={prediction.id}>
              <td>{prediction.league}</td>
              <td>{formatKickoffTime(prediction.kickoffTime)}</td>
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
    </div>
  )
}

export default AdminPredictionsList