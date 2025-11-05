set "app_name=File Validation.exe"

if exist "%app_name%" (
	"%app_name%" 
	
) else (
    echo Application not found.
)

pause 