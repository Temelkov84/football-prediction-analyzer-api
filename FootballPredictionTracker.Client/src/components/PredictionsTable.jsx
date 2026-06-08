function formatKickoffTime(kickoffTime) {
  return new Date(kickoffTime).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function PredictionsTable({ predictions }) {
  return (
    <div className="table-wrapper">
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
          {predictions.map((prediction) => (
            <tr
              key={`${prediction.league}-${prediction.homeTeam}-${prediction.awayTeam}-${prediction.kickoffTime}`}
            >
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

export default PredictionsTable