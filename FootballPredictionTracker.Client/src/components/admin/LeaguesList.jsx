function LeaguesList({ leagues }) {
  if (leagues.length === 0) {
    return <p>No leagues created yet.</p>
  }

  return (
    <div className="admin-list">
      <h3>Existing Leagues</h3>

      <table className="admin-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Country</th>
          </tr>
        </thead>

        <tbody>
          {leagues.map((league) => (
            <tr key={league.id}>
              <td>{league.name}</td>
              <td>{league.country}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default LeaguesList