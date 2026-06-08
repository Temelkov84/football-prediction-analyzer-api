function formatKickoffTime(kickoffTime) {
  if (!kickoffTime) {
    return '-'
  }

  return new Date(kickoffTime).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function getLeagueName(prediction) {
  return prediction.leagueName || prediction.league || 'Unknown League'
}

function getCountryName(prediction) {
  return prediction.country || prediction.countryName || prediction.leagueCountry || '-'
}

function groupPredictionsByLeague(predictions) {
  return predictions.reduce((groups, prediction) => {
    const leagueName = getLeagueName(prediction)
    const countryName = getCountryName(prediction)
    const groupKey = `${countryName}-${leagueName}`

    if (!groups[groupKey]) {
      groups[groupKey] = {
        countryName,
        leagueName,
        predictions: [],
      }
    }

    groups[groupKey].predictions.push(prediction)

    return groups
  }, {})
}

function PredictionsTable({ predictions }) {
  if (!predictions || predictions.length === 0) {
    return <p>No predictions available for this week.</p>
  }

  const groupedPredictions = groupPredictionsByLeague(predictions)

  const predictionGroups = Object.values(groupedPredictions).sort((firstGroup, secondGroup) =>
    firstGroup.leagueName.localeCompare(secondGroup.leagueName)
  )

  return (
    <div className="predictions-groups">
      {predictionGroups.map((group) => (
        <section
          key={`${group.countryName}-${group.leagueName}`}
          className="prediction-league-group"
        >
          <h2>{group.leagueName}</h2>

          <div className="table-wrapper">
            <table className="predictions-table">
              <thead>
                <tr>
                  <th>Date / Time</th>
                  <th>Country</th>
                  <th>League</th>
                  <th>Match</th>
                  <th>1</th>
                  <th>X</th>
                  <th>2</th>
                </tr>
              </thead>

              <tbody>
                {group.predictions
                  .sort((firstPrediction, secondPrediction) =>
                    new Date(firstPrediction.kickoffTime) - new Date(secondPrediction.kickoffTime)
                  )
                  .map((prediction) => (
                    <tr key={prediction.id}>
                      <td>{formatKickoffTime(prediction.kickoffTime)}</td>
                      <td>{getCountryName(prediction)}</td>
                      <td>{getLeagueName(prediction)}</td>
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
        </section>
      ))}
    </div>
  )
}

export default PredictionsTable