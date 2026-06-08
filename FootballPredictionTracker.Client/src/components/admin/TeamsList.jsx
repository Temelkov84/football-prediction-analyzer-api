function TeamsList({ teams }) {
  if (teams.length === 0) {
    return <p>No teams created yet.</p>
  }

  return (
    <div className="admin-list">
      <h3>Existing Teams</h3>

      <table className="admin-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>League</th>
          </tr>
        </thead>

        <tbody>
          {teams.map((team) => (
            <tr key={team.id}>
              <td>{team.name}</td>
              <td>{team.leagueName || team.league?.name || team.leagueId}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default TeamsList