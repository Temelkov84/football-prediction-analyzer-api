import { useState } from 'react'
import './App.css'
import WeeklyPredictionsPage from './pages/WeeklyPredictionsPage'
import AdminDashboardPage from './pages/AdminDashboardPage'

function App() {
  const [activePage, setActivePage] = useState('public')

  return (
    <>
      <header className="app-header">
        <div className="app-header-content">
          <span className="app-logo">Football Prediction Analyzer</span>

          <nav className="app-nav">
            <button
              type="button"
              className={activePage === 'public' ? 'nav-button active' : 'nav-button'}
              onClick={() => setActivePage('public')}
            >
              Public Predictions
            </button>

            <button
              type="button"
              className={activePage === 'admin' ? 'nav-button active' : 'nav-button'}
              onClick={() => setActivePage('admin')}
            >
              Admin
            </button>
          </nav>
        </div>
      </header>

      {activePage === 'public' && <WeeklyPredictionsPage />}
      {activePage === 'admin' && <AdminDashboardPage />}
    </>
  )
}

export default App