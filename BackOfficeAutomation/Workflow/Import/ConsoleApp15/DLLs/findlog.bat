@echo off
setlocal enabledelayedexpansion
set "latestFile="
for /f "delims=" %%I in ('dir "" /b /od /a-d') do (
    set "filename=%%~nI"
    if /I "!filename:~0,3!"=="Log" (
	set "latestFile=%%~fI"
        
    ) 
)
rem echo !latestFile!
pause