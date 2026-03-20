@echo off
echo ========================================
echo   FundAI - Starting Full Stack App
echo ========================================
echo.

REM Start Backend API
echo [1/2] Starting Backend API...
cd /d "d:\AI_Fund\Ai_Fund\Ai_Fund"
start "FundAI Backend" cmd /k "dotnet run"

REM Wait for API to start
echo Waiting for API to initialize...
timeout /t 8 /nobreak > nul

REM Start Frontend React App
echo [2/2] Starting Frontend React App...
cd /d "d:\AI_Fund\Ai_Fund\frontend"
start "FundAI Frontend" cmd /k "npm run dev"

echo.
echo ========================================
echo   ✅ FundAI Started Successfully!
echo ========================================
echo.
echo Backend API: https://localhost:44328
echo Frontend: http://localhost:3000
echo.
echo Opening browser in 5 seconds...
timeout /t 5 /nobreak > nul
start http://localhost:3000
echo.
echo Press any key to stop all services...
pause > nul

REM Stop all services
echo.
echo Stopping services...
taskkill /FI "WINDOWTITLE eq FundAI Backend*" /T /F > nul 2>&1
taskkill /FI "WINDOWTITLE eq FundAI Frontend*" /T /F > nul 2>&1
echo.
echo All services stopped.
pause
