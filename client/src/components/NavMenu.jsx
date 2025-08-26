import { NavLink } from 'react-router-dom'

export default function NavMenu({ items = [], className = '' }) {
  const base = 'px-3 py-2 font-medium transition-colors tracking-wide' // tracking-wide = more letter spacing
  const active = 'bg-black text-white rounded-md' // active pill
  const idle = 'text-green-400 hover:text-red-400' // normal green, hover red

  return (
    <nav className={`w-full ${className}`}>
      <div className="flex justify-center items-center py-2 text-lg">
        {items.map(({ to, label }, i) => (
          <span key={to} className="flex items-center">
            <NavLink to={to} className={({ isActive }) => `${base} ${isActive ? active : idle}`}>
              {label}
            </NavLink>
            {/* separator after every item except the last */}
            {i < items.length - 1 && <span className="mx-2 text-gray-500">…</span>}
          </span>
        ))}
      </div>
    </nav>
  )
}
