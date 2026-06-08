import { useEffect, useState } from 'react'
import { getLeagues } from '../api/leaguesApi'
import { getTeams } from '../api/teamsApi'
import { getMatches } from '../api/matchesApi'
import { getMatchStatistics } from '../api/matchStatisticsApi'
import { getAdminPredictions } from '../api/adminPredictionsApi'
import CreateLeagueForm from '../components/admin/CreateLeagueForm'
import CreateTeamForm from '../components/admin/CreateTeamForm'
import CreateMatchForm from '../components/admin/CreateMatchForm'
import CreateMatchStatisticsForm from '../components/admin/CreateMatchStatisticsForm'
import CalculatePredictionForm from '../components/admin/CalculatePredictionForm'
import LeaguesList from '../components/admin/LeaguesList'
import TeamsList from '../components/admin/TeamsList'
import MatchesList from '../components/admin/MatchesList'
import MatchStatisticsList from '../components/admin/MatchStatisticsList'
import AdminPredictionsList from '../components/admin/AdminPredictionsList'


function AdminDashboardPage() {
  const [leagues, setLeagues] = useState([])
  const [teams, setTeams] = useState([])
  const [matches, setMatches] = useState([])
  const [matchStatistics, setMatchStatistics] = useState([])
  const [predictions, setPredictions] = useState([])

  const [isLoadingLeagues, setIsLoadingLeagues] = useState(true)
  const [isLoadingTeams, setIsLoadingTeams] = useState(true)
  const [isLoadingMatches, setIsLoadingMatches] = useState(true)
  const [isLoadingStatistics, setIsLoadingStatistics] = useState(true)
  const [isLoadingPredictions, setIsLoadingPredictions] = useState(true)

  const [leaguesError, setLeaguesError] = useState('')
  const [teamsError, setTeamsError] = useState('')
  const [matchesError, setMatchesError] = useState('')
  const [statisticsError, setStatisticsError] = useState('')
  const [predictionsError, setPredictionsError] = useState('')

  const [activeAdminSection, setActiveAdminSection] = useState('leagues')

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

  async function loadMatchStatistics() {
    try {
      setStatisticsError('')
      setIsLoadingStatistics(true)

      const data = await getMatchStatistics()
      setMatchStatistics(data)
    } catch (error) {
      setStatisticsError(error.message)
    } finally {
      setIsLoadingStatistics(false)
    }
  }

  async function loadPredictions() {
    try {
      setPredictionsError('')
      setIsLoadingPredictions(true)

      const data = await getAdminPredictions()
      setPredictions(data)
    } catch (error) {
      setPredictionsError(error.message)
    } finally {
      setIsLoadingPredictions(false)
    }
  }

  useEffect(() => {
    loadLeagues()
    loadTeams()
    loadMatches()
    loadMatchStatistics()
    loadPredictions()
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
  <button
    type="button"
    className={activeAdminSection === 'leagues' ? 'active' : ''}
    onClick={() => setActiveAdminSection('leagues')}
  >
    Leagues
  </button>

  <button
    type="button"
    className={activeAdminSection === 'teams' ? 'active' : ''}
    onClick={() => setActiveAdminSection('teams')}
  >
    Teams
  </button>

  <button
    type="button"
    className={activeAdminSection === 'matches' ? 'active' : ''}
    onClick={() => setActiveAdminSection('matches')}
  >
    Matches
  </button>

  <button
    type="button"
    className={activeAdminSection === 'statistics' ? 'active' : ''}
    onClick={() => setActiveAdminSection('statistics')}
  >
    Match Statistics
  </button>

  <button
    type="button"
    className={activeAdminSection === 'predictions' ? 'active' : ''}
    onClick={() => setActiveAdminSection('predictions')}
  >
    Predictions
  </button>
</div>
      </section>

        {activeAdminSection === 'leagues' && (
        <section className="card admin-section">
          <CreateLeagueForm onLeagueCreated={loadLeagues} />

          {isLoadingLeagues && <p>Loading leagues...</p>}
          {leaguesError && <p className="error-message">{leaguesError}</p>}

          {!isLoadingLeagues && !leaguesError && (
            <LeaguesList leagues={leagues} />
          )}
        </section>
      )}

      {activeAdminSection === 'teams' && (
        <section className="card admin-section">
          <CreateTeamForm leagues={leagues} onTeamCreated={loadTeams} />

          {isLoadingTeams && <p>Loading teams...</p>}
          {teamsError && <p className="error-message">{teamsError}</p>}

          {!isLoadingTeams && !teamsError && (
            <TeamsList teams={teams} />
          )}
        </section>
      )}

      {activeAdminSection === 'matches' && (
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
      )}

      {activeAdminSection === 'statistics' && (
        <section className="card admin-section">
          <CreateMatchStatisticsForm
            matches={matches}
            onStatisticsCreated={loadMatchStatistics}
          />

          {isLoadingStatistics && <p>Loading match statistics...</p>}
          {statisticsError && <p className="error-message">{statisticsError}</p>}

          {!isLoadingStatistics && !statisticsError && (
            <MatchStatisticsList statistics={matchStatistics} />
          )}
        </section>
      )}

      {activeAdminSection === 'predictions' && (
        <section className="card admin-section">
          <CalculatePredictionForm
            matches={matches}
            onPredictionCalculated={loadPredictions}
          />

          {isLoadingPredictions && <p>Loading predictions...</p>}
          {predictionsError && <p className="error-message">{predictionsError}</p>}

          {!isLoadingPredictions && !predictionsError && (
            <AdminPredictionsList predictions={predictions} />
          )}
        </section>
      )}
    </main>
  )
}

export default AdminDashboardPage