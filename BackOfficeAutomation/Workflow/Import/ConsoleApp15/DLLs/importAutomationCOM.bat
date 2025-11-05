@echo off

set "app_name=ConsoleApp15.exe"
set "app_args=i"
if exist "%app_name%" (
	"%app_name%" "%app_args%" 
	
) else (
    echo Application not found.
)

pause 