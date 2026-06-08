import { useEffect, useState } from 'react'
import { getLeagues } from '../api/leaguesApi'
import { getTeams } from '../api/teamsApi'
import { getMatches } from '../api/matchesApi'
import CreateLeagueForm from '../components/admin/CreateLeagueForm'
import CreateTeamForm from '../components/admin/CreateTeamForm'
import CreateMatchForm from '../components/admin/CreateMatchForm'
import LeaguesList from '../components/admin/LeaguesList'
import TeamsList from '../components/admin/TeamsList'
import MatchesList from '../components/admin/MatchesList'

function AdminDashboardPage() {
  const [leagues, setLeagues] = useState([])
  const [teams, setTeams] = useState([])
  const [matches, setMatches] = useState([])

  const [isLoadingLeagues, setIsLoadingLeagues] = useState(true)
  const [isLoadingTeams, setIsLoadingTeams] = useState(true)
  const [isLoadingMatches, setIsLoadingMatches] = useState(true)

  const [leaguesError, setLeaguesError] = useState('')
  const [teamsError, setTeamsError] = useState('')
  const [matchesError, setMatchesError] = useState('')

  async function loadLeagues() {
    try {
      setLeaguesError('')
      setIsLoadingLeagues(true)

      const data = await getLeagues()
      setLeagues(data)
    } catch (error) {
      setLeaguesError(error.message)
    } finally {
      setIsLoadingLeagues(false)
    }
  }

  async function loadTeams() {
    try {
      setTeamsError('')
      setIsLoadingTeams(true)

      const data = await getTeams()
      setTeams(data)
    } catch (error) {
      setTeamsError(error.message)
    } finally {
      setIsLoadingTeams(false)
    }
  }

  async function loadMatches() {
    try {
      setMatchesError('')
      setIsLoadingMatches(true)

      const data = await getMatches()
      setMatches(data)
    } catch (error) {
      setMatchesError(error.message)
    } finally {
      setIsLoadingMatches(false)
    }
  }

  useEffect(() => {
    loadLeagues()
    loadTeams()
    loadMatches()
  }, [])

  return (
    <main className="page">
      <section className="hero">
        <h1>Admin Dashboard</h1>
        <p>Manage leagues, teams, matches, statistics, and prediction calculations.</p>
      </section>

      <section className="card">
        <h2>Admin Actions</h2>

        <div className="admin-actions">
          <button type="button">Leagues</button>
          <button type="button">Teams</button>
          <button type="button">Matches</button>
          <button type="button">Match Statistics</button>
          <button type="button">Predictions</button>
        </div>
      </section>

      <section className="card admin-section">
        <CreateLeagueForm onLeagueCreated={loadLeagues} />

        {isLoadingLeagues && <p>Loading leagues...</p>}
        {leaguesError && <p className="error-message">{leaguesError}</p>}

        {!isLoadingLeagues && !leaguesError && (
          <LeaguesList leagues={leagues} />
        )}
      </section>

      <section className="card admin-section">
        <CreateTeamForm leagues={leagues} onTeamCreated={loadTeams} />

        {isLoadingTeams && <p>Loading teams...</p>}
        {teamsError && <p className="error-message">{teamsError}</p>}

        {!isLoadingTeams && !teamsError && (
          <TeamsList teams={teams} />
        )}
      </section>

      <section className="card admin-section">
        <CreateMatchForm
          leagues={leagues}
          teams={teams}
          onMatchCreated={loadMatches}
        />

        {isLoadingMatches && <p>Loading matches...</p>}
        {matchesError && <p className="error-message">{matchesError}</p>}

        {!isLoadingMatches && !matchesError && (
          <MatchesList matches={matches} />
        )}
      </section>
    </main>
  )
}

export default AdminDashboardPage