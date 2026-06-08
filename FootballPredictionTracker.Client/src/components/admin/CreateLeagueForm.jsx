import { useState } from 'react'
import { createLeague } from '../../api/leaguesApi'

function CreateLeagueForm({ onLeagueCreated }) {
  const [name, setName] = useState('')
  const [country, setCountry] = useState('')
  const [successMessage, setSuccessMessage] = useState('')
  const [errorMessage, setErrorMessage] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(event) {
    event.preventDefault()

    try {
      setSuccessMessage('')
      setErrorMessage('')
      setIsSubmitting(true)

      const createdLeague = await createLeague({
        name,
        country,
      })

      setSuccessMessage(`League created: ${createdLeague.name}`)
      onLeagueCreated()
      setName('')
      setCountry('')
    } catch (error) {
      setErrorMessage(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form className="admin-form" onSubmit={handleSubmit}>
      <h3>Create League</h3>

      <div className="form-group">
        <label htmlFor="league-name">League Name</label>
        <input
          id="league-name"
          type="text"
          value={name}
          onChange={(event) => setName(event.target.value)}
          placeholder="Premier League"
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="league-country">Country</label>
        <input
          id="league-country"
          type="text"
          value={country}
          onChange={(event) => setCountry(event.target.value)}
          placeholder="England"
          required
        />
      </div>

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Creating...' : 'Create League'}
      </button>

      {successMessage && <p className="success-message">{successMessage}</p>}
      {errorMessage && <p className="error-message">{errorMessage}</p>}
    </form>
  )
}

export default CreateLeagueForm