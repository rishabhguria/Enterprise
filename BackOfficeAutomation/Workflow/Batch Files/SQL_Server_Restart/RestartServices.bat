@echo off
setlocal enabledelayedexpansion
set EXIT_CODE=0

set "file=..\PromptConfig.txt"

for /f "tokens=*" %%a in ('type "%file%"') do (
    set "response=%%a"    
)


pushd %~dp0
::exit /b %EXIT_CODE%
:: SET esc ENVIRONMENT VARIABLE 
for /F %%a in ('echo prompt $E ^| cmd') do (
  set "ESC=%%a"
)
::echo %EXIT_CODE%
:: defining the color codes
set "red=%ESC%[91m"
set "green=%ESC%[92m"
set "yellow=%ESC%[93m"
set "cyan=%ESC%[96m"
set "magenta=%ESC%[95m"
set "blue=%ESC%[94m"
set "end=%ESC%[0m"

set "startTime=%time%"

:: input file for the input of all the services to restart
set "input_file=WindowServiceList.txt"

:: directory where all the logs will be created
set "log_directory=logs"

:: check whether the input file is available at the location or not if not then it will create it
if not exist "%input_file%" (	
	echo. > "%input_file%"
	echo WindowServiceList.txt file not found and is created at %CD%\%input_file% .Please enter the name of services in the file.
	set EXIT_CODE=15
	pause	
	exit /b %EXIT_CODE%
)

:: Get the current date
for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
set "datestamp=!datetime:~0,4!-!datetime:~4,2!-!datetime:~6,2!"
set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"

:: Define the log file with a datestamp
set "log_file=%log_directory%\Log_!datestamp!.txt"

:: append the Date and time sptemp in the particular log file
>>"!log_file!" (
	echo.
	echo ------------------------------------------------------------------------------------------
	echo.
	echo Current user : %USERNAME%
	
	:: Fetch Ip address and log it into log file 
	for /f "tokens=2 delims=:" %%f in ('ipconfig ^| findstr /c:"IPv4 Address"') do set ip=%%f
	echo IP : !ip!		
) 

:: Removes inbetween empty spaces from the input file and starting and end space in each line
for /f "usebackq delims=" %%a in ("%input_file%") do (
    set "line=%%a"
    if defined line (
        echo !line!>>"%input_file%.tmp"
    )
)
move "%input_file%.tmp" "%input_file%" > nul

:: Print the list of Valid and Invalid Services 
echo %yellow%List of Services to be restarted : (Configured at %CD%\%input_file% file)%end%
echo.

set "validServiceCount=0"
set "invalidServiceCount=0"
set "serviceCount=0"

echo %red%Invalid Services : No action will be taken for Invalid Services.%end%
for /f "tokens=*" %%a in (%input_file%) do (	
	set "serviceName=%%a"
	sc query "!serviceName!" | find "STATE" > nul
	if errorlevel 1 (		
		set /a "invalidServiceCount+=1"
		set /a "serviceCount+=1"
		echo [!serviceCount!] !serviceName!
	)
	
)
echo.

set "serviceCount=0"
echo %green%Valid Services :%end% 
for /f "tokens=*" %%a in (%input_file%) do (
	
	set "serviceName=%%a"
	
	sc query "!serviceName!" | find "STATE" | find "RUNNING" >nul
	if !errorlevel! equ 0 (
		set "serviceState=Running"
	) else (
		set "serviceState=Stopped"
	)
	
	set "connectionCount=0"
	set "runningApps="
	for /f %%a in (ProcessList.txt) do (
		set "processName=%%a"
    
		tasklist /fi "imagename eq !processName!" | find /i "pid" > nul
		if !errorlevel! equ 0 (
			set /a "connectionCount+=1"
			set "runningApps=!runningApps! [!processName!],"
			set EXIT_CODE=16
		)
	)
	
	sc query "!serviceName!" | find "STATE" > nul
	
	if !errorlevel! equ 0 (
		set /a "validServiceCount+=1"
		set /a "serviceCount+=1"		
		echo [!serviceCount!] !serviceName!	- !serviceState! : [ Active Connections : !connectionCount!]
		if not "!runningApps!"=="" echo !runningApps:~0,-1!
		echo.
	)
)
echo.
if !serviceCount! equ 0 (
    set EXIT_CODE=20
)
:: append the list of services to restart in the log file under the time stamp 
>>"!log_file!" (
	echo.
	echo List of Services to be restarted :
    echo.
	set "validServiceCount=0"
	set "invalidServiceCount=0"
	set "serviceCount=0"

	echo Invalid Services : 
	for /f "tokens=*" %%a in (%input_file%) do (	
		set "serviceName=%%a"
		sc query "!serviceName!" | find "STATE" > nul
		if errorlevel 1 (		
			set /a "invalidServiceCount+=1"
			set /a "serviceCount+=1"
			echo [!serviceCount!] !serviceName!
		)
	
	)
	echo.
	
	
	
	set "serviceCount=0"	
	echo Valid Services : 
	for /f "tokens=*" %%a in (%input_file%) do (	
		
		set "serviceName=%%a"
		
		sc query "!serviceName!" | find "STATE" | find "RUNNING" >nul
		if !errorlevel! equ 0 (
			set "serviceState=Running"
		) else (
			set "serviceState=Stopped"
		)
		
		set "connectionCount=0"
		set "runningApps="
		for /f %%a in (ProcessList.txt) do (
			set "processName=%%a"
    
			tasklist /fi "imagename eq !processName!" | find /i "pid" > nul
			if !errorlevel! equ 0 (
				set /a "connectionCount+=1"
				set "runningApps=!runningApps! [!processName!],"
				
			)
		)
		
		
		sc query "!serviceName!" | find "STATE" > nul
	
		if !errorlevel! equ 0 (
			set /a "validServiceCount+=1"
			set /a "serviceCount+=1"			
			echo [!serviceCount!] !serviceName!	- !serviceState! : [ Active Connections : !connectionCount!]
			if not "!runningApps!"=="" echo !runningApps:~0,-1!
			echo.
		)
	)
	echo.

)

set "totalServiceCount=0"
set "serviceCount=0"

:: take user input whether to restart all or restart individually
set "restartAll=Y"

if /i "!response!"=="No" (
	set /p "restartAll=Do you want to restart all services without asking for each one? [Y/y/N/n] : "
)

for /f "tokens=*" %%a in (%input_file%) do (
	
	set "serviceName=%%a"
	
	set "connectionCount=0"
	for /f %%a in (ProcessList.txt) do (
		set "processName=%%a"
    
		tasklist /fi "imagename eq !processName!" | find /i "pid" > nul
		if !errorlevel! equ 0 (
			set /a "connectionCount+=1"
		)
	)
	
	:: chek the state of the services either running or not
	sc query "!serviceName!" | find "STATE" | find "RUNNING" >nul
    if !errorlevel! equ 0 (
        set "serviceState=Running"
    ) else (
        set "serviceState=Stopped"
    )
	
	:: Check if the service is valid or not 	
	sc query "!serviceName!" | find "STATE" > nul
	
	if !errorlevel! equ 0 (
		
		set /a "serviceCount+=1"	
		
		
		if "!serviceState!"=="Running" (
			echo [!serviceCount!] Details for "!serviceName!" service >>"!log_file!"
			echo. >>"!log_file!"
		) else if "!serviceState!"=="Stopped" (
			echo [!serviceCount!] Details for "!serviceName!" service >>"!log_file!"
			echo. >>"!log_file!"
		)
		
		
		if !connectionCount! equ 0 (
		
			if /i "!restartAll!"=="Y" (
				echo.
				echo %blue%[!serviceCount!] !serviceName!%end%
				echo.
			
				:: if service is in running state the will stop it and then start it
				if "!serviceState!"=="Running" (
				
					for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
					set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
				
					:: Stop the service
					echo [!timestamp!] [!serviceName!] service is stopping.
					echo [!timestamp!] [!serviceName!] service is stopping. >>"!log_file!"
					net stop "!serviceName!" /y > nul
					set stopError=!errorlevel!

					if !stopError! equ 0 (
						for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
						set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
						echo %green%[!timestamp!] [!serviceName!] service was stopped successfully.%end%
						echo [!timestamp!] [!serviceName!] service was stopped successfully. >>"!log_file!"
						set "serviceState=Stopped"
					) else (
						for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
						set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
						echo %red%[!timestamp!] [!serviceName!] service failed to stop.%end%
						echo [!timestamp!] [!serviceName!] service failed to stop. >>"!log_file!"
					)			
				
				)
		
				if "!serviceState!"=="Stopped" (
				
					for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
					set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
				
					:: Start the service
					echo [!timestamp!] [!serviceName!] service is starting. 
					echo [!timestamp!] [!serviceName!] service is starting. >>"!log_file!"
					net start "!serviceName!" > nul
					set startError=!errorlevel!
		
		
					:: success message if successfully started the service else print the error message
					if !startError! equ 0 (	
						for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
						set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
						set /a "totalServiceCount+=1"					
						echo %green%[!timestamp!] [!serviceName!] service was started successfully.%end%
						echo [!timestamp!] [!serviceName!] service was started successfully. >>"!log_file!"
						echo. >>"!log_file!"
			
					) else (
						for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
						set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"									
						echo %red%[!timestamp!] [!serviceName!] service failed to start.%end%
						echo [!timestamp!] [!serviceName!] service failed to start. >>"!log_file!"
						echo. >>"!log_file!"
					) 
				)
		 
			) else (
		
				:: if want to individually restart the services ask for yes/no prompt for each services
				echo.
				set /p "yesNo=Do you want to restart the "!serviceName!" service (Currently "!serviceState!")? [Y/y/N/n] : "
				echo.
		
				if /I "!yesNo!"=="y" (
		
					if "!serviceState!"=="Running" (		
			
						for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
						set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
				
						:: Stop the service
						echo [!timestamp!] [!serviceName!] service is stopping.
						echo [!timestamp!] [!serviceName!] service is stopping. >>"!log_file!"
						net stop "!serviceName!" /y > nul
						set stopError=!errorlevel!
					
						if !stopError! equ 0 (
							for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
							set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
							echo %green%[!timestamp!] [!serviceName!] service was stopped successfully.%end%
							echo [!timestamp!] [!serviceName!] service was stopped successfully. >>"!log_file!"
							set "serviceState=Stopped"
						) else (
							for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
							set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
							echo %red%[!timestamp!] [!serviceName!] service failed to stop.%end%
							echo [!timestamp!] [!serviceName!] service failed to stop. >>"!log_file!"
						)
					)
		
					if "!serviceState!"=="Stopped" (
							
						for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
						set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
				
						:: Start the service
						echo [!timestamp!] [!serviceName!] service is starting. 
						echo [!timestamp!] [!serviceName!] service is starting. >>"!log_file!"
						net start "!serviceName!" > nul
						set startError=!errorlevel!
		
		
						:: success message if successfully started the service else print the error message
						if !startError! equ 0 (	
							for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
							set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"
							set /a "totalServiceCount+=1"					
							echo %green%[!timestamp!] [!serviceName!] service was started successfully.%end%
							echo [!timestamp!] [!serviceName!] service was started successfully. >>"!log_file!"
							echo. >>"!log_file!"
			
						) else (
							for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
							set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"									
							echo %red%[!timestamp!] [!serviceName!] service failed to start.%end%
							echo [!timestamp!] [!serviceName!] service failed to start. >>"!log_file!"
							echo. >>"!log_file!"
						) 
					)		
            
				) else (
					for /f "delims=" %%a in ('wmic OS Get LocalDateTime ^| find "."') do set datetime=%%a
					set "timestamp=!datetime:~6,2!/!datetime:~4,2!/!datetime:~0,4! !datetime:~8,2!:!datetime:~10,2!:!datetime:~12,2!:!datetime:~15,3!"	
					echo %yellow%[!timestamp!] Service Restart canceled for service "!serviceName!".%end%
					echo [!timestamp!] Service Restart canceled for service "!serviceName!". >>"!log_file!"
					echo. >>"!log_file!"
				)        
			)	
		
		) else (
			echo.
			set EXIT_CODE=16
			echo %red%!serviceName! contains [!connectionCount! Active Connections] Please close them before restart the service.%end%
			echo !serviceName! contains [!connectionCount! Active Connections] Please close them before restart the service. >>"!log_file!"
			echo. >>"!log_file!"
			
		)
		
	)
)
echo.

set "endTime=%time%"

rem for /f "tokens=1-4 delims=:.," %%a in ("%startTime%") do set /a "start_h=10%%a %% 100", "start_m=10%%b %% 100", "start_s=10%%c %% 100", "start_ms=1000%%d %% 1000"
rem for /f "tokens=1-4 delims=:.," %%a in ("%endTime%") do set /a "end_h=10%%a %% 100", "end_m=10%%b %% 100", "end_s=10%%c %% 100", "end_ms=1000%%d %% 1000"

rem Calculate the time difference in milliseconds
rem set /a "diff_ms=((end_h*3600 + end_m*60 + end_s)*1000 + end_ms) - ((start_h*3600 + start_m*60 + start_s)*1000 + start_ms)"

rem Calculate minutes, seconds, and milliseconds
rem set /a "_min=diff_ms / 60000, "_sec=(diff_ms / 1000) %% 60", "_milliseconds=diff_ms %% 1000"

:: Print the total number of services restarted
echo %cyan%Total Services Restarted : [!totalServiceCount!]%end%
echo Total Services Restarted : [!totalServiceCount!] >>"!log_file!"

rem echo Time taken: !_min! min !_sec! sec !_milliseconds! milliseconds
rem echo Time taken: !_min! min !_sec! sec !_milliseconds! milliseconds >>"!log_file!"
::echo %EXIT_CODE%

if /i "!response!"=="No" (
	pause
)

exit /b %EXIT_CODE%
endlocal
popd %~dp0
