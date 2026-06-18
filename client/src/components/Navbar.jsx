import { Link, useLocation } from 'react-router-dom'
import './Navbar.css'

export default function Navbar() {
  const { pathname } = useLocation()
  return (
    <nav className="navbar">
      <Link to="/" className="navbar-brand">Flashcards</Link>
      <div className="navbar-links">
        <Link to="/"       className={pathname === '/'       ? 'nav-link active' : 'nav-link'}>Home</Link>
        <Link to="/study"  className={pathname === '/study'  ? 'nav-link active' : 'nav-link'}>Study</Link>
      </div>
    </nav>
  )
}
