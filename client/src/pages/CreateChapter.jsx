import { useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { createChapter } from '../api/chapters'

export default function CreateChapter() {
  const [params] = useSearchParams()
  const subjectId = Number(params.get('subjectId') || 0)
  const [name, setName] = useState('')
  const [description, setDescription] = useState('')
  const [error, setError] = useState('')
  const navigate = useNavigate()

  async function handleSubmit(e) {
    e.preventDefault()
    setError('')
    try {
      await createChapter({ name, description, subjectId })
      navigate(-1)
    } catch {
      setError('A chapter with that name already exists.')
    }
  }

  return (
    <div className="page" style={{ maxWidth: 560 }}>
      <div className="page-header">
        <h1 className="page-title">Create Chapter</h1>
        <p className="page-subtitle">Chapters organise your flashcards within a subject.</p>
      </div>

      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
        <div className="field">
          <label className="label">Chapter Name</label>
          <input className="input" placeholder="e.g., Multiplication, Conjunctions, The Renaissance"
            value={name} onChange={e => setName(e.target.value)} required />
          {error && <span className="error-text">{error}</span>}
        </div>

        <div className="field">
          <label className="label">Description <span className="optional">optional</span></label>
          <textarea className="input" placeholder="A brief description of this chapter…"
            value={description} onChange={e => setDescription(e.target.value)} />
        </div>

        <div className="form-actions">
          <button type="button" className="btn-ghost" onClick={() => navigate(-1)}>Cancel</button>
          <button type="submit" className="btn-primary">Create Chapter</button>
        </div>
      </form>
    </div>
  )
}
