import { useState } from 'react'
import { addTicker } from '../lib/api'
import { useNavigate } from 'react-router-dom'

export default function AddTicker() {
  const [symbol, setSymbol] = useState('')
  const [releaseDate, setReleaseDate] = useState('') // "YYYY-MM-DD"
  const [releaseSession, setReleaseSession] = useState('') // e.g. "Before Open"
  const [tradable, setTradable] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState('')
  const nav = useNavigate()

  async function onSubmit(e) {
    e.preventDefault()
    setSaving(true)
    setError('')
    try {
      await addTicker({
        symbol: symbol.toUpperCase(),
        releaseDate: releaseDate || null,
        releaseSession: releaseSession || null,
        tradable,
      })
      nav('/tickers')
    } catch (e) {
      setError(String(e))
    } finally {
      setSaving(false)
    }
  }

  return (
    <form onSubmit={onSubmit} className="bg-white rounded-2xl shadow border p-6 max-w-xl">
      <h2 className="text-lg font-semibold mb-4">Add Ticker</h2>

      <div className="grid gap-4">
        <input
          className="border rounded-xl px-4 py-2"
          placeholder="Symbol (e.g., ALAR)"
          value={symbol}
          onChange={(e) => setSymbol(e.target.value)}
        />
        <input
          className="border rounded-xl px-4 py-2"
          placeholder="Release date (YYYY-MM-DD)"
          value={releaseDate}
          onChange={(e) => setReleaseDate(e.target.value)}
        />
        <input
          className="border rounded-xl px-4 py-2"
          placeholder="Session (Before Open / After Close)"
          value={releaseSession}
          onChange={(e) => setReleaseSession(e.target.value)}
        />
        <label className="flex items-center gap-2">
          <input
            type="checkbox"
            checked={tradable}
            onChange={(e) => setTradable(e.target.checked)}
          />
          Mark as tradable
        </label>
      </div>

      {error && <p className="text-red-600 mt-4">{error}</p>}

      <div className="mt-6 flex gap-2">
        <button
          disabled={saving}
          className="inline-flex items-center px-4 py-2 rounded-xl font-medium text-white bg-black hover:bg-black/90 disabled:opacity-60"
        >
          {saving ? 'Saving…' : 'Save'}
        </button>
        <button
          type="button"
          onClick={() => nav('/tickers')}
          className="inline-flex items-center px-4 py-2 rounded-xl font-medium border border-black/10 bg-white hover:bg-gray-50"
        >
          Cancel
        </button>
      </div>
    </form>
  )
}
