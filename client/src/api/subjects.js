import { api } from './client'

export const getSubjects   = ()       => api.get('/subjects')
export const createSubject = (body)   => api.post('/subjects', body)
export const deleteSubject = (id)     => api.delete(`/subjects/${id}`)
