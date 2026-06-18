import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getSubjects, deleteSubject } from '../api/subjects'
import Modal from '../components/Modal'

export default function Index() {
  const [subjects, setSubjects] = useState([])
  const [deleteTarget, setDeleteTarget] = useState(null)
  const navigate = useNavigate()

  useEffect(() => { getSubjects().then(setSubjects) }, [])

  async function handleDelete() {
    await deleteSubject(deleteTarget.id)
    setSubjects(s => s.filter(x => x.id !== deleteTarget.id))
    setDeleteTarget(null)
  }

  return (
    <div className="page">
      <div className="page-header" style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between' }}>
        <div>
          <h1 className="page-title">My Subjects</h1>
          <p className="page-subtitle">Select a subject to study, update, or delete it.</p>
        </div>
        <button className="btn-primary" onClick={() => navigate('/create-flashcards')}>+ New Deck</button>
      </div>

      {subjects.length === 0 ? (
        <div className="empty-state">
          <p>No subjects yet.</p>
          <a onClick={() => navigate('/create-subject')} style={{ cursor: 'pointer' }}>Create your first subject →</a>
        </div>
      ) : (
        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th style={{ width: '22%' }}>Subject</th>
                <th>Description</th>
                <th style={{ width: '220px', textAlign: 'right' }}>Actions</th>
              </tr>
            </thead>
            <tbody>
              {subjects.map(s => (
                <tr key={s.id}>
                  <td style={{ fontWeight: 600, color: '#f3f4f6' }}>{s.name}</td>
                  <td style={{ color: '#9ca3af', fontSize: '.9rem' }}>{s.description || '—'}</td>
                  <td style={{ textAlign: 'right', whiteSpace: 'nowrap' }}>
                    <button className="action-btn study"   onClick={() => navigate(`/study?subjectId=${s.id}`)}>Study</button>
                    <button className="action-btn update"  onClick={() => navigate(`/update-deck/${s.id}`)}>Update</button>
                    <button className="action-btn delete"  onClick={() => setDeleteTarget(s)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {deleteTarget && (
        <Modal onClose={() => setDeleteTarget(null)}>
          <h2 className="modal-title">Delete Subject</h2>
          <p className="modal-body">
            Are you sure you want to delete <strong>{deleteTarget.name}</strong>?
            All chapters and flashcards within this subject will also be permanently deleted. This cannot be undone.
          </p>
          <div className="modal-actions">
            <button className="btn-modal-cancel" onClick={() => setDeleteTarget(null)}>Cancel</button>
            <button className="btn-modal-confirm danger" onClick={handleDelete}>Yes, Delete</button>
          </div>
        </Modal>
      )}

    </div>
  )
}
