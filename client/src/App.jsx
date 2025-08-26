import { Routes, Route, NavLink } from 'react-router-dom'
import Home from './pages/Home.jsx'
import Tickers from './pages/Tickers.jsx'
import AddTicker from './pages/AddTicker.jsx'
import NavMenu from './components/NavMenu'

export default function App() {
  const linkBase = 'px-3 py-2 rounded-xl hover:bg-gray-100'
  const linkActive = ({ isActive }) =>
    isActive ? linkBase + ' bg-black text-white hover:bg-black' : linkBase

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="block w-full bg-[#111] text-gray-200 shadow-sm">
        <div className="w-full h-1 bg-gradient-to-r from-green-500 via-gray-500 to-red-500" />
        <div className="w-full py-4 flex justify-center">
          <h1 className="text-2xl font-bold tracking-tight text-red-500">Earnings Research</h1>
        </div>
        <NavMenu
          className="bg-[#1c1c1c] border-t border-b border-gray-700"
          items={[
            { to: '/', label: 'Home' },
            { to: '/tickers', label: 'Tickers' },
            { to: '/add', label: 'Add Ticker' },
          ]}
        />
      </header>

      <main className="w-full py-6">
        {/* site-wide gutter (DEBUG BORDER so you can see it) */}
        <div className="px-4 sm:px-6">
          {/* width limiter inside the gutter */}
          <div className="max-w-6xl mx-auto">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/tickers" element={<Tickers />} />
              <Route path="/add" element={<AddTicker />} />
            </Routes>
          </div>
        </div>
      </main>
    </div>
  )
}
