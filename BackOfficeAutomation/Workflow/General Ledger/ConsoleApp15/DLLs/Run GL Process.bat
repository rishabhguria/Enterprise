set "app_name=GLProcessor.exe"
set "app_args=g"
if exist "%app_name%" (
	"%app_name%" "%app_args%" 
	
) else (
    echo Application not found.
)

pause 