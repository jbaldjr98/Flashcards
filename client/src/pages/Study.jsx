import { useEffect, useState, useCallback } from 'react'
import { useSearchParams, useNavigate } from 'react-router-dom'
import { getSubjects } from '../api/subjects'
import { getChaptersBySubject } from '../api/chapters'
import { getFlashcardsBySubject, markFlashcard } from '../api/flashcards'
import './Study.css'

export default function Study() {
  const [params] = useSearchParams()
  const navigate = useNavigate()

  const [subjects, setSubjects]   = useState([])
  const [chapters, setChapters]   = useState([])
  const [subjectId, setSubjectId] = useState(params.get('subjectId') || '')
  const [chapterId, setChapterId] = useState(params.get('chapterId') || '0')
  const [deck, setDeck]           = useState(null)

  const [idx, setIdx]           = useState(0)
  const [flipped, setFlipped]   = useState(false)
  const [revealed, setRevealed] = useState(false)
  const [isRevisit, setIsRevisit] = useState(false)
  const [correct, setCorrect]   = useState(0)
  const [incorrect, setIncorrect] = useState(0)
  const [done, setDone]         = useState(false)

  useEffect(() => { getSubjects().then(setSubjects) }, [])

  useEffect(() => {
    if (!subjectId) return
    getChaptersBySubject(subjectId).then(setChapters)
  }, [subjectId])

  async function startStudying() {
    let cards = await getFlashcardsBySubject(subjectId)
    if (chapterId && chapterId !== '0')
      cards = cards.filter(c => c.chapterId === Number(chapterId))
    setDeck(cards)
    setIdx(0)
    setFlipped(false)
    setRevealed(false)
    setCorrect(0)
    setIncorrect(0)
    setDone(false)
  }

  function flipCard() {
    if (!revealed) { setFlipped(true); setRevealed(true) }
  }

  async function handleVerdict(result) {
    await markFlashcard(deck[idx].id, { result, isRevisit })
    if (result === 'success') setCorrect(c => c + 1)
    else setIncorrect(c => c + 1)
    setFlipped(false)
    setRevealed(false)
    setIsRevisit(false)
    setTimeout(() => {
      if (idx + 1 >= deck.length) setDone(true)
      else setIdx(i => i + 1)
    }, 300)
  }

  async function handleRevisitChange(checked) {
    setIsRevisit(checked)
    if (deck) await markFlashcard(deck[idx].id, { result: null, isRevisit: checked })
  }

  const card = deck?.[idx]
  const progress = deck ? (idx / deck.length) * 100 : 0

  return (
    <div className="page" style={{ maxWidth: 700 }}>
      <div className="page-header">
        <h1 className="page-title">Study</h1>
        <p className="page-subtitle">Pick a subject and chapter, then work through your cards.</p>
      </div>

      {/* Selector */}
      <div className="st-selector">
        <div className="field" style={{ flex: 1, minWidth: 160 }}>
          <label className="label">Subject</label>
          <select className="select" value={subjectId} onChange={e => { setSubjectId(e.target.value); setChapterId('0') }}>
            <option value="">Select a subject…</option>
            {subjects.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
          </select>
        </div>

        <div className="field" style={{ flex: 1, minWidth: 160 }}>
          <label className="label">Chapter</label>
          <select className="select" value={chapterId} onChange={e => setChapterId(e.target.value)} disabled={!subjectId}>
            <option value="0">All Chapters</option>
            {chapters.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
          </select>
        </div>

        <button className="btn-primary" style={{ alignSelf: 'flex-end', height: 42 }}
          disabled={!subjectId} onClick={startStudying}>
          Start Studying
        </button>
      </div>

      {/* Study area */}
      {deck && !done && card && (
        <>
          <div className="st-progress-row">
            <span style={{ fontSize: '.8rem', color: '#9ca3af' }}>Card {idx + 1} of {deck.length}</span>
            <div style={{ height: 4, background: '#1f2937', borderRadius: 9999, overflow: 'hidden', marginTop: '.35rem' }}>
              <div style={{ height: '100%', background: '#6366f1', width: `${progress}%`, transition: 'width .3s ease', borderRadius: 9999 }} />
            </div>
          </div>

          <div className="st-scene" onClick={flipCard}>
            <div className={`st-card${flipped ? ' flipped' : ''}`}>
              <div className="st-face st-front">
                <span className="st-face-label">Front</span>
                <p className="st-card-text">{card.front}</p>
                {!revealed && <span style={{ fontSize: '.78rem', color: '#4b5563', marginTop: '.5rem' }}>Click to reveal answer</span>}
              </div>
              <div className="st-face st-back">
                <span className="st-face-label" style={{ color: '#818cf8' }}>Back</span>
                <p className="st-card-text">{card.back}</p>
              </div>
            </div>
          </div>

          <div className={`st-actions${revealed ? ' visible' : ''}`}>
            <label style={{ display: 'flex', alignItems: 'center', gap: '.5rem', fontSize: '.9rem', color: '#9ca3af', cursor: 'pointer' }}>
              <input type="checkbox" checked={isRevisit} onChange={e => handleRevisitChange(e.target.checked)}
                style={{ width: 16, height: 16, accentColor: '#f59e0b' }} />
              Mark for revisit
            </label>
            <div style={{ display: 'flex', gap: '.75rem' }}>
              <button className="st-verdict failure" onClick={() => handleVerdict('failure')}>✗ Incorrect</button>
              <button className="st-verdict success" onClick={() => handleVerdict('success')}>✓ Correct</button>
            </div>
          </div>
        </>
      )}

      {deck && !done && deck.length === 0 && (
        <div className="empty-state"><p>No flashcards found for the selected subject/chapter.</p></div>
      )}

      {/* End screen */}
      {done && (
        <div style={{ textAlign: 'center', padding: '3rem 1rem', display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '1.25rem' }}>
          <div style={{ fontSize: '3rem' }}>🎉</div>
          <h2 style={{ fontSize: '1.5rem', fontWeight: 700, color: '#f3f4f6' }}>Deck Complete!</h2>
          <div style={{ display: 'flex', gap: '2rem' }}>
            <div className="st-stat success-stat">
              <span style={{ fontSize: '2rem', fontWeight: 700, color: '#f3f4f6' }}>{correct}</span>
              <span style={{ fontSize: '.8rem', color: '#9ca3af', textTransform: 'uppercase', letterSpacing: '.05em' }}>Correct</span>
            </div>
            <div className="st-stat failure-stat">
              <span style={{ fontSize: '2rem', fontWeight: 700, color: '#f3f4f6' }}>{incorrect}</span>
              <span style={{ fontSize: '.8rem', color: '#9ca3af', textTransform: 'uppercase', letterSpacing: '.05em' }}>Incorrect</span>
            </div>
          </div>
          <div style={{ display: 'flex', gap: '1rem' }}>
            <button className="btn-ghost" style={{ border: '1.5px solid #374151' }} onClick={startStudying}>Study Again</button>
            <button className="btn-primary" onClick={() => navigate('/')}>Back to Home</button>
          </div>
        </div>
      )}
    </div>
  )
}
