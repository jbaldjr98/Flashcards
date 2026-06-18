import { api } from './client'

export const getChaptersBySubject = (subjectId) => api.get(`/chapters?subjectId=${subjectId}`)
export const createChapter        = (body)       => api.post('/chapters', body)
