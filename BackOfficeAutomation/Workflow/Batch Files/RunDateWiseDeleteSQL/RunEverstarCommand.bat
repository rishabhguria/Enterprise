@echo off

REM Set the current date and time for log file
for /f "delims=" %%a in ('wmic OS Get localdatetime  ^| find "."') do set datetime=%%a
set "datetime=%datetime:~0,14%"
set "logfilename=Logs\log_%datetime%.txt"

REM Redirect both stdout and stderr to the log file
sqlcmd -S localhost -d Everstar -E -i DatewiseDeleteQuery_EverStar.sql >> "%logfilename%" 2>&1

REM Check the error level after sqlcmd execution
if %errorlevel% neq 0 (
    echo Error: SQL Server command failed. Check the log file for details: %logfilename%
) else (
    echo Success: SQL Server command executed successfully. Log file: %logfilename%
)

pause
