import { Routes, Route, NavLink } from 'react-router-dom'
import Home from './pages/Home.jsx'
import Tickers from './pages/Tickers.jsx'
import AddTicker from './pages/AddTicker.jsx'


export default function App(){
const linkBase = "px-3 py-2 rounded-xl hover:bg-gray-100";
const linkActive = ({isActive}) => isActive ? linkBase + " bg-black text-white hover:bg-black" : linkBase;


return (
<div className="min-h-screen bg-gray-50">
<header className="bg-white shadow">
<div className="max-w-5xl mx-auto px-4 py-4 flex items-center gap-3">
<h1 className="text-2xl font-bold mr-4">Earnings Research</h1>
<nav className="flex gap-2">
<NavLink className={linkActive} to="/">Home</NavLink>
<NavLink className={linkActive} to="/tickers">Tickers</NavLink>
<NavLink className={linkActive} to="/add">Add Ticker</NavLink>
</nav>
</div>
</header>


<main className="max-w-5xl mx-auto px-4 py-6">
<Routes>
<Route path="/" element={<Home/>} />
<Route path="/tickers" element={<Tickers/>} />
<Route path="/add" element={<AddTicker/>} />
</Routes>
</main>
</div>
)
}