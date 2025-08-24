export const API = import.meta.env.VITE_API_URL ?? 'http://localhost:5086';


export async function getTickers(){
const r = await fetch(`${API}/api/tickers`);
if(!r.ok) throw new Error(`GET /tickers ${r.status}`);
return r.json();
}


export async function addTicker(payload){
const r = await fetch(`${API}/api/tickers`, {
method: 'POST', headers: {'Content-Type':'application/json'},
body: JSON.stringify(payload)
});
if(!r.ok) throw new Error(`POST /tickers ${r.status}`);
return r.json();
}