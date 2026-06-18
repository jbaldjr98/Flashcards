import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Navbar from './components/Navbar'
import Index from './pages/Index'
import CreateSubject from './pages/CreateSubject'
import CreateChapter from './pages/CreateChapter'
import CreateFlashcards from './pages/CreateFlashcards'
import UpdateDeck from './pages/UpdateDeck'
import Study from './pages/Study'

export default function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/" element={<Index />} />
        <Route path="/create-subject" element={<CreateSubject />} />
        <Route path="/create-chapter" element={<CreateChapter />} />
        <Route path="/create-flashcards" element={<CreateFlashcards />} />
        <Route path="/update-deck/:subjectId" element={<UpdateDeck />} />
        <Route path="/study" element={<Study />} />
      </Routes>
    </BrowserRouter>
  )
}
