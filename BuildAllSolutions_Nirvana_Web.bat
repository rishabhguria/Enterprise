@ECHO off
SET msbuildpath=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin
SETLOCAL enabledelayedexpansion

REM Default BuildConfiguration is Debug mode.
SET BuildConfiguration=Debug

REM Default BuildMode is Build. It makes more sense for debug build
SET BuildMode=Build

REM Total number of solutions to be built
SET TotalJobs=10

ECHO USAGE:
ECHO        %0 -option1 -option2
ECHO        option1: debug/release  ^(debug will be used ^if ^not provided^)
ECHO        option2: build/rebuild  ^(build will be used ^if ^not provided^)
ECHO.

IF "%1"=="-release" (
SET BuildConfiguration=Release
)

IF "%2"=="-rebuild" (
SET BuildMode=ReBuild
)

REM %0 is first input arg like in any console program. In this it is the script itself
REM %~d0 returns drive letter
REM %~dp0 returns path to the script
%~d0
cd\
CD %~dp0

SET LogFileName=%~n0.txt

REM JobCount used for serial number of solutions being built
SET JobCount=1

ECHO %BuildMode% started in %BuildConfiguration% Mode - %date% %time%
ECHO %BuildMode% started in %BuildConfiguration% Mode - %date% %time%> "%LogFileName%"
ECHO.

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Service Gateway
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Service Gateway
ECHO Service Gateway %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.ServiceGateway.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Service Gateway.
)
ECHO Service Gateway %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1


TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Auth Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Auth Service
ECHO Auth Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.AuthService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Auth Service.
)
ECHO Auth Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Blotter Data Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Blotter Data Service
ECHO Blotter Data Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.BlotterDataService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Blotter Data Service.
)
ECHO Blotter Data Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Calculation Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Calculation Service
ECHO Calculation Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.CalculationService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Calculation Service.
)
ECHO Calculation Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Common Data Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Common Data Service
ECHO Common Data Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.CommonDataService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Common Data Service.
)
ECHO Common Data Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing LiveFeed Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing LiveFeed Service
ECHO LiveFeed Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.LiveFeedService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the LiveFeed Service.
)
ECHO LiveFeed Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Security Validation Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Security Validation Service
ECHO Security Validation Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.SecurityValidationService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Security Validation Service.
)
ECHO Security Validation Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Trading Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Trading Service
ECHO Trading Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.TradingService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Trading Service.
)
ECHO Trading Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance Alerts Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance Alerts Service
ECHO Compliance Alerts Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.ComplianceAlertsService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Compliance Alerts Service.
)
ECHO Compliance Alerts Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Layout Service
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Layout Service
ECHO Layout Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.LayoutService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Layout Service.
)
ECHO Layout Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

ECHO.
ECHO %BuildMode% completed in %BuildConfiguration% Mode - %date% %time%
ECHO %BuildMode% completed in %BuildConfiguration% Mode - %date% %time%>> "%LogFileName%"

REM SET the target directory for creating shortcuts
SET targetDirectory="Shortcuts_%BuildConfiguration%"

REM create a directory
IF NOT EXIST %targetDirectory% MKDIR %targetDirectory%

REM Get current working directory
REM %0 is first input arg like in any console program. In this it is the script itself
REM %~dp0 returns path to the script
SET cwd=%~dp0

REM SET the path of each folder
SET sourcefolder1="%cwd%\Prana.ServiceGateway\Prana.ServiceGateway\bin\%BuildConfiguration%\net6.0"
SET sourcefolder2="%cwd%\Prana.GreenFieldServices\Prana.AuthServiceHost\bin\%BuildConfiguration%"
SET sourcefolder3="%cwd%\Prana.GreenFieldServices\Prana.BlotterDataServiceHost\bin\%BuildConfiguration%"
SET sourcefolder4="%cwd%\Prana.GreenFieldServices\Prana.CalculationServiceHost\bin\%BuildConfiguration%"
SET sourcefolder5="%cwd%\Prana.GreenFieldServices\Prana.CommonDataServiceHost\bin\%BuildConfiguration%"
SET sourcefolder6="%cwd%\Prana.GreenFieldServices\Prana.LiveFeedServiceHost\bin\%BuildConfiguration%"
SET sourcefolder7="%cwd%\Prana.GreenFieldServices\Prana.SecurityValidationServiceHost\bin\%BuildConfiguration%"
SET sourcefolder8="%cwd%\Prana.GreenFieldServices\Prana.TradingServiceHost\bin\%BuildConfiguration%"
SET sourcefolder9="%cwd%\Prana.GreenFieldServices\Prana.ComplianceAlertsServiceHost\bin\%BuildConfiguration%"
SET sourcefolder10="%cwd%\Prana.GreenFieldServices\Prana.LayoutServiceHost\bin\%BuildConfiguration%"

REM SET the path of each file
SET sourcefile1="%cwd%\Prana.ServiceGateway\Prana.ServiceGateway\bin\%BuildConfiguration%\net6.0\Prana.ServiceGateway.exe"
SET sourcefile2="%cwd%\Prana.GreenFieldServices\Prana.AuthServiceHost\bin\%BuildConfiguration%\Prana.AuthServiceHost.exe"
SET sourcefile3="%cwd%\Prana.GreenFieldServices\Prana.BlotterDataServiceHost\bin\%BuildConfiguration%\Prana.BlotterDataServiceHost.exe"
SET sourcefile4="%cwd%\Prana.GreenFieldServices\Prana.CalculationServiceHost\bin\%BuildConfiguration%\Prana.CalculationServiceHost.exe"
SET sourcefile5="%cwd%\Prana.GreenFieldServices\Prana.CommonDataServiceHost\bin\%BuildConfiguration%\Prana.CommonDataServiceHost.exe"
SET sourcefile6="%cwd%\Prana.GreenFieldServices\Prana.LiveFeedServiceHost\bin\%BuildConfiguration%\Prana.LiveFeedServiceHost.exe"
SET sourcefile7="%cwd%\Prana.GreenFieldServices\Prana.SecurityValidationServiceHost\bin\%BuildConfiguration%\Prana.SecurityValidationServiceHost.exe"
SET sourcefile8="%cwd%\Prana.GreenFieldServices\Prana.TradingServiceHost\bin\%BuildConfiguration%\Prana.TradingServiceHost.exe"
SET sourcefile9="%cwd%\Prana.GreenFieldServices\Prana.ComplianceAlertsServiceHost\bin\%BuildConfiguration%\Prana.ComplianceAlertsServiceHost.exe"
SET sourcefile10="%cwd%\Prana.GreenFieldServices\Prana.LayoutServiceHost\bin\%BuildConfiguration%\Prana.LayoutServiceHost.exe"

REM SET the path of each file shortcut
SET targetgfile1=%targetDirectory%\13_ServiceGateway
SET targetgfile2=%targetDirectory%\14_AuthServiceHost
SET targetgfile3=%targetDirectory%\15_BlotterDataServiceHost
SET targetgfile4=%targetDirectory%\16_CalculationServiceHost
SET targetgfile5=%targetDirectory%\17_CommonDataServiceHost
SET targetgfile6=%targetDirectory%\18_LiveFeedServiceHost
SET targetgfile7=%targetDirectory%\19_SecurityValidationServiceHost
SET targetgfile8=%targetDirectory%\20_TradingServiceHost
SET targetgfile9=%targetDirectory%\21_ComplianceAlertsServiceHost
SET targetgfile10=%targetDirectory%\22_LayoutServiceHost

REM generate shortcut
ShortcutGenerator /target:%sourcefile1% /shortcut:%targetgfile1% /startin:%sourcefolder1%
ShortcutGenerator /target:%sourcefile2% /shortcut:%targetgfile2% /startin:%sourcefolder2%
ShortcutGenerator /target:%sourcefile3% /shortcut:%targetgfile3% /startin:%sourcefolder3%
ShortcutGenerator /target:%sourcefile4% /shortcut:%targetgfile4% /startin:%sourcefolder4%
ShortcutGenerator /target:%sourcefile5% /shortcut:%targetgfile5% /startin:%sourcefolder5%
ShortcutGenerator /target:%sourcefile6% /shortcut:%targetgfile6% /startin:%sourcefolder6%
ShortcutGenerator /target:%sourcefile7% /shortcut:%targetgfile7% /startin:%sourcefolder7%
ShortcutGenerator /target:%sourcefile8% /shortcut:%targetgfile8% /startin:%sourcefolder8%
ShortcutGenerator /target:%sourcefile9% /shortcut:%targetgfile9% /startin:%sourcefolder9%
ShortcutGenerator /target:%sourcefile10% /shortcut:%targetgfile10% /startin:%sourcefolder10%

PAUSE