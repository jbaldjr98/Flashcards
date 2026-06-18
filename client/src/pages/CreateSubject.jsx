import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { createSubject } from '../api/subjects'

export default function CreateSubject() {
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [error, setError] = useState('')
  const navigate = useNavigate()

  async function handleSubmit(e) {
    e.preventDefault()
    setError('')
    try {
      await createSubject({ name, description })
      navigate('/')
    } catch {
      setError('A subject with that name already exists.')
    }
  }

  return (
    <div className="page" style={{ maxWidth: 560 }}>
      <div className="page-header">
        <h1 className="page-title">Create Subject</h1>
        <p className="page-subtitle">Subjects group your chapters and flashcards together.</p>
      </div>

      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
        <div className="field">
          <label className="label">Subject Name</label>
          <input className="input" placeholder="e.g., Biology, Spanish, History"
            value={name} onChange={e => setName(e.target.value)} required />
          {error && <span className="error-text">{error}</span>}
        </div>

        <div className="field">
          <label className="label">Description <span className="optional">optional</span></label>
          <textarea className="input" placeholder="A brief description of this subject…"
            value={description} onChange={e => setDescription(e.target.value)} />
        </div>

        <div className="form-actions">
          <button type="button" className="btn-ghost" onClick={() => navigate('/')}>Cancel</button>
          <button type="submit" className="btn-primary">Create Subject</button>
        </div>
      </form>
    </div>
  )
}
