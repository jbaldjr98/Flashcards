import { api } from './client'

export const getFlashcardsBySubject = (subjectId) => api.get(`/flashcards?subjectId=${subjectId}`)
export const createFlashcards       = (cards)     => api.post('/flashcards/batch', cards)
export const updateFlashcard        = (id, body)  => api.put(`/flashcards/${id}`, body)
export const deleteFlashcard        = (id)        => api.delete(`/flashcards/${id}`)
export const markFlashcard          = (id, body)  => api.patch(`/flashcards/${id}/mark`, body)
