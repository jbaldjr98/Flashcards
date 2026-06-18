import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getSubjects } from '../api/subjects'
import { getChaptersBySubject } from '../api/chapters'
import { createFlashcards } from '../api/flashcards'

const TrashIcon = () => (
  <svg xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24"
    fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <polyline points="3 6 5 6 21 6"/><path d="M19 6l-1 14H6L5 6"/>
    <path d="M10 11v6"/><path d="M14 11v6"/><path d="M9 6V4h6v2"/>
  </svg>
)

export default function CreateFlashcards() {
  const [subjects, setSubjects]   = useState([])
  const [chapters, setChapters]   = useState([])
  const [subjectId, setSubjectId] = useState('')
  const [chapterId, setChapterId] = useState('')
  const [rows, setRows]           = useState([{ front: '', back: '' }])
  const navigate = useNavigate()

  useEffect(() => { getSubjects().then(setSubjects) }, [])

  async function onSubjectChange(id) {
    setSubjectId(id)
    setChapterId('')
    setChapters([])
    if (id) {
      const data = await getChaptersBySubject(id)
      setChapters(data)
      if (data.length) setChapterId(String(data[0].id))
    }
  }

  function updateRow(i, field, val) {
    setRows(r => r.map((row, idx) => idx === i ? { ...row, [field]: val } : row))
  }

  function removeRow(i) {
    if (rows.length === 1) return
    setRows(r => r.filter((_, idx) => idx !== i))
  }

  async function handleSubmit(e) {
    e.preventDefault()
    const cards = rows
      .filter(r => r.front.trim() || r.back.trim())
      .map(r => ({ front: r.front, back: r.back, subjectId: Number(subjectId), chapterId: Number(chapterId) }))
    await createFlashcards(cards)
    navigate('/')
  }

  return (
    <div className="page">
      <div className="page-header">
        <h1 className="page-title">Create Flashcards</h1>
        <p className="page-subtitle">Choose a subject and chapter, then add your cards below.</p>
      </div>

      <form onSubmit={handleSubmit}>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1.25rem', marginBottom: '2.5rem' }}>
          <div className="field">
            <label className="label">Subject</label>
            <div style={{ display: 'flex', gap: '.5rem' }}>
              <select className="select" value={subjectId} onChange={e => onSubjectChange(e.target.value)} required>
                <option value="">Select a subject…</option>
                {subjects.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
              </select>
              <a className="btn-outline" onClick={() => navigate('/create-subject')} style={{ cursor: 'pointer' }}>+ New</a>
            </div>
          </div>

          <div className="field">
            <label className="label">Chapter</label>
            <div style={{ display: 'flex', gap: '.5rem' }}>
              <select className="select" value={chapterId} onChange={e => setChapterId(e.target.value)}
                disabled={!subjectId || chapters.length === 0} required>
                <option value="">Select a chapter…</option>
                {chapters.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
              <a className={`btn-outline${!subjectId ? ' disabled' : ''}`}
                onClick={() => subjectId && navigate(`/create-chapter?subjectId=${subjectId}`)}
                style={{ cursor: subjectId ? 'pointer' : 'not-allowed', opacity: subjectId ? 1 : .4 }}>+ New</a>
            </div>
          </div>
        </div>

        <div style={{ marginBottom: '2rem' }}>
          <div style={{ display: 'flex', alignItems: 'baseline', gap: '.75rem', marginBottom: '.75rem' }}>
            <span className="label">Cards</span>
            <span style={{ fontSize: '.8rem', color: '#6b7280' }}>{rows.length} card{rows.length !== 1 ? 's' : ''}</span>
          </div>

          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th style={{ width: '2.5rem', textAlign: 'center' }}>#</th>
                  <th>Front</th>
                  <th>Back</th>
                  <th style={{ width: '3rem' }}></th>
                </tr>
              </thead>
              <tbody>
                {rows.map((row, i) => (
                  <tr key={i}>
                    <td style={{ textAlign: 'center', color: '#6b7280', fontWeight: 600, fontSize: '.8rem' }}>{i + 1}</td>
                    <td><input className="td-input" placeholder="Term, question, or concept…"
                      value={row.front} onChange={e => updateRow(i, 'front', e.target.value)} /></td>
                    <td><input className="td-input" placeholder="Definition or answer…"
                      value={row.back} onChange={e => updateRow(i, 'back', e.target.value)} /></td>
                    <td style={{ textAlign: 'center' }}>
                      <button type="button" className="trash-btn" onClick={() => removeRow(i)}><TrashIcon /></button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <button type="button" className="add-row-btn"
            onClick={() => setRows(r => [...r, { front: '', back: '' }])}>
            + Add Card
          </button>
        </div>

        <div className="form-actions">
          <button type="button" className="btn-ghost" onClick={() => navigate('/')}>Cancel</button>
          <button type="submit" className="btn-primary" disabled={!subjectId || !chapterId}>Save Flashcards</button>
        </div>
      </form>
    </div>
  )
}
