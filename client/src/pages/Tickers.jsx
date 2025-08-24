import { useEffect, useState } from 'react'
import { getTickers } from '../lib/api'


export default function Tickers(){
const [rows, setRows] = useState([])
const [loading, setLoading] = useState(true)
const [error, setError] = useState('')


useEffect(() => {
(async () => {
try{
const data = await getTickers()
setRows(data)
}catch(e){ setError(String(e)) }
finally{ setLoading(false) }
})()
}, [])


return (
<div className="bg-white rounded-2xl shadow border">
<div className="p-4 border-b"><h2 className="text-lg font-semibold">Tickers</h2></div>
<div className="overflow-x-auto">
<table className="min-w-full text-sm">
<thead className="bg-gray-50 text-left">
<tr>
<th className="px-4 py-3 font-semibold">Id</th>
<th className="px-4 py-3 font-semibold">Symbol</th>
<th className="px-4 py-3 font-semibold">Earnings Release</th>
<th className="px-4 py-3 font-semibold">Tradable</th>
</tr>
</thead>
<tbody>
{loading && (
<tr><td className="px-4 py-3" colSpan="4">Loading…</td></tr>
)}
{error && (
<tr><td className="px-4 py-3 text-red-600" colSpan="4">{error}</td></tr>
)}
{!loading && !error && rows.length === 0 && (
<tr><td className="px-4 py-3" colSpan="4">No tickers yet.</td></tr>
)}
{rows.map(r => (
<tr key={r.id} className="border-t">
<td className="px-4 py-3 font-mono">{r.id}</td>
<td className="px-4 py-3 font-mono text-lg">{r.symbol}</td>
<td className="px-4 py-3">{r.earningsRelease ?? '—'}</td>
<td className="px-4 py-3">{r.tradable ? 'Yes' : 'No'}</td>
</tr>
))}
</tbody>
</table>
</div>
</div>
)
}