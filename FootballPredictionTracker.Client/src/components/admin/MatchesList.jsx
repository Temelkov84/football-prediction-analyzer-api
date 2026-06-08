function formatKickoffTime(kickoffTime) {
  return new Date(kickoffTime).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function MatchesList({ matches }) {
  if (matches.length === 0) {
    return <p>No matches created yet.</p>
  }

  return (
    <div className="admin-list">
      <h3>Existing Matches</h3>

      <table className="admin-table">
        <thead>
          <tr>
            <th>League</th>
            <th>Date</th>
            <th>Match</th>
          </tr>
        </thead>

        <tbody>
          {matches.map((match) => (
            <tr key={match.id}>
              <td>{match.league}</td>
              <td>{formatKickoffTime(match.kickoffTime)}</td>
              <td>
                {match.homeTeam} vs {match.awayTeam}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default MatchesList