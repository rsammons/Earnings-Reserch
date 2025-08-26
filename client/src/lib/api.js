export const API = import.meta.env.VITE_API_URL ?? 'http://localhost:5086'

export async function getTickers(date) {
  const url = new URL(`${API}/api/tickers`)
  if (date) url.searchParams.set('date', date) // YYYY-MM-DD
  const r = await fetch(url)
  if (!r.ok) throw new Error(`GET /tickers ${r.status}`)
  return r.json()
}

export async function addTicker({ symbol, releaseDate, releaseSession, tradable }) {
  const r = await fetch(`${API}/api/tickers`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ symbol, releaseDate, releaseSession, tradable }),
  })
  if (!r.ok) throw new Error(`POST /tickers ${r.status}`)
  return r.json()
}
