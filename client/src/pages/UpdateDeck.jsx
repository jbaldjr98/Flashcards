import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getChaptersBySubject, createChapter } from '../api/chapters'
import { getFlashcardsBySubject, updateFlashcard, deleteFlashcard, createFlashcards } from '../api/flashcards'
import { getSubjects } from '../api/subjects'
import Modal from '../components/Modal'

const TrashIcon = () => (
  <svg xmlns="http://www.w3.org/2000/svg" width="15" height="15" viewBox="0 0 24 24"
    fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <polyline points="3 6 5 6 21 6"/><path d="M19 6l-1 14H6L5 6"/>
    <path d="M10 11v6"/><path d="M14 11v6"/><path d="M9 6V4h6v2"/>
  </svg>
)

export default function UpdateDeck() {
  const { subjectId } = useParams()
  const navigate = useNavigate()
  const [subject, setSubject]       = useState(null)
  const [chapters, setChapters]     = useState([])
  const [cards, setCards]           = useState([])
  const [newRows, setNewRows]       = useState([])
  const [showChapterModal, setShowChapterModal] = useState(false)
  const [chapterName, setChapterName]           = useState('')
  const [chapterDesc, setChapterDesc]           = useState('')

  useEffect(() => {
    getSubjects().then(all => setSubject(all.find(s => s.id === Number(subjectId))))
    getChaptersBySubject(subjectId).then(setChapters)
    getFlashcardsBySubject(subjectId).then(setCards)
  }, [subjectId])

  function updateCard(id, field, val) {
    setCards(c => c.map(card => card.id === id ? { ...card, [field]: val } : card))
  }

  function updateNewRow(i, field, val) {
    setNewRows(r => r.map((row, idx) => idx === i ? { ...row, [field]: val } : row))
  }

  async function handleDelete(id) {
    await deleteFlashcard(id)
    setCards(c => c.filter(card => card.id !== id))
  }

  async function handleSave() {
    await Promise.all(cards.map(c => updateFlashcard(c.id, { front: c.front, back: c.back, chapterId: c.chapterId })))
    const toAdd = newRows.filter(r => r.front?.trim() || r.back?.trim())
      .map(r => ({ ...r, subjectId: Number(subjectId) }))
    if (toAdd.length) {
      const created = await createFlashcards(toAdd)
      setCards(c => [...c, ...created])
    }
    setNewRows([])
  }

  async function handleAddChapter(e) {
    e.preventDefault()
    const created = await createChapter({ name: chapterName, description: chapterDesc, subjectId: Number(subjectId) })
    setChapters(c => [...c, created])
    setChapterName('')
    setChapterDesc('')
    setShowChapterModal(false)
  }

  const allRows = cards.length + newRows.length

  return (
    <div className="page" style={{ maxWidth: 960 }}>
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '2rem' }}>
        <div>
          <h1 className="page-title">{subject?.name ?? '…'}</h1>
          <p className="page-subtitle">Edit your flashcards or add new ones below.</p>
        </div>
        <button className="btn-ghost" onClick={() => navigate('/')}>← Back</button>
      </div>

      <div style={{ marginBottom: '2rem' }}>
        <div style={{ display: 'flex', alignItems: 'baseline', gap: '.75rem', marginBottom: '.75rem' }}>
          <span className="label">Cards</span>
          <span style={{ fontSize: '.8rem', color: '#6b7280' }}>{allRows} card{allRows !== 1 ? 's' : ''}</span>
          <button type="button" className="btn-outline" style={{ marginLeft: 'auto', height: 30, fontSize: '.78rem' }}
            onClick={() => setShowChapterModal(true)}>+ Add Chapter</button>
        </div>

        <div className="table-wrap">
          <div className="table-scroll">
            <table>
              <thead>
                <tr>
                  <th style={{ width: '3rem', textAlign: 'center' }}>#</th>
                  <th>Front</th>
                  <th>Back</th>
                  <th style={{ width: '160px' }}>Chapter</th>
                  <th style={{ width: '3rem' }}></th>
                </tr>
              </thead>
              <tbody>
                {cards.map((card, i) => (
                  <tr key={card.id}>
                    <td style={{ textAlign: 'center', color: '#6b7280', fontSize: '.8rem', fontWeight: 600 }}>{i + 1}</td>
                    <td><input className="td-input" value={card.front}
                      onChange={e => updateCard(card.id, 'front', e.target.value)} /></td>
                    <td><input className="td-input" value={card.back}
                      onChange={e => updateCard(card.id, 'back', e.target.value)} /></td>
                    <td>
                      <select className="td-select" value={card.chapterId}
                        onChange={e => updateCard(card.id, 'chapterId', Number(e.target.value))}>
                        {chapters.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                      </select>
                    </td>
                    <td style={{ textAlign: 'center' }}>
                      <button type="button" className="trash-btn" onClick={() => handleDelete(card.id)}><TrashIcon /></button>
                    </td>
                  </tr>
                ))}
                {newRows.map((row, i) => (
                  <tr key={`new-${i}`}>
                    <td style={{ textAlign: 'center', color: '#4b5563', fontSize: '.8rem' }}>·</td>
                    <td><input className="td-input" placeholder="Front"
                      value={row.front} onChange={e => updateNewRow(i, 'front', e.target.value)} /></td>
                    <td><input className="td-input" placeholder="Back"
                      value={row.back} onChange={e => updateNewRow(i, 'back', e.target.value)} /></td>
                    <td>
                      <select className="td-select" value={row.chapterId ?? chapters[0]?.id ?? ''}
                        onChange={e => updateNewRow(i, 'chapterId', Number(e.target.value))}>
                        {chapters.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                      </select>
                    </td>
                    <td style={{ textAlign: 'center' }}>
                      <button type="button" className="trash-btn"
                        onClick={() => setNewRows(r => r.filter((_, idx) => idx !== i))}><TrashIcon /></button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>

        <button className="add-row-btn"
          onClick={() => setNewRows(r => [...r, { front: '', back: '', chapterId: chapters[0]?.id ?? 0 }])}>
          + Add Card
        </button>
      </div>

      <div className="form-actions">
        <button className="btn-ghost" onClick={() => navigate('/')}>Cancel</button>
        <button className="btn-primary" onClick={handleSave}>Save Changes</button>
      </div>

      {showChapterModal && (
        <Modal onClose={() => setShowChapterModal(false)}>
          <h2 className="modal-title">Add Chapter</h2>
          <form onSubmit={handleAddChapter} style={{ display: 'contents' }}>
            <div className="field">
              <label className="label">Chapter Name</label>
              <input className="input" placeholder="e.g., Multiplication, Conjunctions…"
                value={chapterName} onChange={e => setChapterName(e.target.value)} required />
            </div>
            <div className="field">
              <label className="label">Description <span className="optional">optional</span></label>
              <textarea className="input" placeholder="A brief description…"
                value={chapterDesc} onChange={e => setChapterDesc(e.target.value)} />
            </div>
            <div className="modal-actions">
              <button type="button" className="btn-modal-cancel" onClick={() => setShowChapterModal(false)}>Cancel</button>
              <button type="submit" className="btn-modal-confirm primary">Add Chapter</button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  )
}
