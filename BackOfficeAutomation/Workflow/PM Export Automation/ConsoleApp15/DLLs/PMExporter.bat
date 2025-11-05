@echo off
set "app_name=PMExporter.exe"
set "app_args=p"

set "file=..\PromptConfig.txt"

for /f "tokens=*" %%a in ('type "%file%"') do (
    set "response=%%a"    
)

if exist "%app_name%" (
	"%app_name%" "%app_args%" 
	
	set EXIT_CODE=%ERRORLEVEL%
	if /i "%response%"=="No" (
		pause
	)
	exit /B %EXIT_CODE%
	
) else (
    echo Application not found.
)
