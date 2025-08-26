import { useEffect, useState } from 'react'
import { getTickers } from '../lib/api'

export default function Tickers() {
  const [rows, setRows] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    ;(async () => {
      try {
        const data = await getTickers()
        setRows(data)
      } catch (e) {
        setError(String(e))
      } finally {
        setLoading(false)
      }
    })()
  }, [])

  return (
    <div className="max-w-6xl mx-auto">
      <div className="bg-[#111] rounded-2xl shadow border border-gray-700">
        <div className="px-5 py-4 border-b border-gray-700">
          <h2 className="text-lg font-semibold text-gray-100">Tickers</h2>
        </div>
        <div className="p-5 overflow-x-auto">
          <table className="min-w-full text-sm text-gray-200">
            <thead className="bg-[#1c1c1c] text-left">
              <tr>
                <th className="px-4 py-3 font-semibold">Id</th>
                <th className="px-4 py-3 font-semibold">Symbol</th>
                <th className="px-4 py-3 font-semibold">Release Date</th>
                <th className="px-4 py-3 font-semibold">Session</th>
                <th className="px-4 py-3 font-semibold">Tradable</th>
              </tr>
            </thead>
            <tbody>
              {loading && (
                <tr>
                  <td className="px-4 py-3" colSpan="4">
                    Loading…
                  </td>
                </tr>
              )}
              {error && (
                <tr>
                  <td className="px-4 py-3 text-rose-400" colSpan="4">
                    {error}
                  </td>
                </tr>
              )}
              {!loading && !error && rows.length === 0 && (
                <tr>
                  <td className="px-4 py-3 text-gray-400" colSpan="4">
                    No tickers yet.
                  </td>
                </tr>
              )}
              {rows.map((r) => (
                <tr
                  key={r.id}
                  className="border-t border-gray-800 hover:bg-[#171717] transition-colors"
                >
                  <td className="px-4 py-3 font-mono text-gray-300">{r.id}</td>
                  <td className="px-4 py-3 font-mono text-lg text-gray-100">{r.symbol}</td>
                  <td className="px-4 py-3">{r.releaseDate ?? '—'}</td>
                  <td className="px-4 py-3">{r.releaseSession ?? '—'}</td>
                  <td className="px-4 py-3">
                    {r.tradable ? (
                      <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium border border-emerald-500/40 bg-emerald-500/10 text-emerald-300">
                        Yes
                      </span>
                    ) : (
                      <span className="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium border border-gray-600 bg-gray-700/30 text-gray-300">
                        No
                      </span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
}
