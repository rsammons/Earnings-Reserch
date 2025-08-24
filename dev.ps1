# Start Postgres
Write-Host "Starting Docker..." -ForegroundColor Cyan
docker compose up -d


# Start API (.NET watch)
Write-Host "Starting API..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit","-Command","cd api; dotnet watch run"


# Start client (Vite)
Write-Host "Starting Client..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit","-Command","cd client; npm run dev"