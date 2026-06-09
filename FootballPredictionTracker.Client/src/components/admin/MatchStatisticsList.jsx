function formatKickoffTime(kickoffTime) {
  return new Date(kickoffTime).toLocaleString('en-GB', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function MatchStatisticsList({ statistics }) {
  if (statistics.length === 0) {
    return <p>No match statistics created yet.</p>
  }

  const sortedStatistics = [...statistics].sort(
    (firstItem, secondItem) =>
      new Date(firstItem.kickoffTime) - new Date(secondItem.kickoffTime)
  )

  return (
    <div className="admin-list">
      <h3>Existing Match Statistics</h3>

      <table className="admin-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Match</th>
            <th>Recent Form</th>
            <th>Goals</th>
          </tr>
        </thead>

        <tbody>
          {sortedStatistics.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>
                {item.league} | {item.homeTeam} vs {item.awayTeam} |{' '}
                {formatKickoffTime(item.kickoffTime)}
              </td>
              <td>
                H: {item.homeTeamRecentWins}-{item.homeTeamRecentDraws}-{item.homeTeamRecentLosses}
                {' / '}
                A: {item.awayTeamRecentWins}-{item.awayTeamRecentDraws}-{item.awayTeamRecentLosses}
              </td>
              <td>
                H: {item.homeTeamGoalsScored}/{item.homeTeamGoalsConceded}
                {' / '}
                A: {item.awayTeamGoalsScored}/{item.awayTeamGoalsConceded}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default MatchStatisticsList