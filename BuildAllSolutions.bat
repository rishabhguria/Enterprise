@ECHO off
SET msbuildpath=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin
SETLOCAL enabledelayedexpansion

REM Default BuildConfiguration is Debug mode.
SET BuildConfiguration=Debug

REM Default BuildMode is Build. It makes more sense for debug build
SET BuildMode=Build

REM Default BuildConfiguration is Debug Mode and it has 15 files in total
SET TotalJobs=15

REM This variable is to check whether the script is called directly or from another scrip e.g. BuildAndReleaseSetup.bat
SET IsDirectlyCalled=true

IF "%3"=="false" (
SET IsDirectlyCalled=false
)

IF "%IsDirectlyCalled%"=="true" (
ECHO USAGE:
ECHO        %0 -option1 -option2
ECHO        option1: debug/release  ^(debug will be used ^if ^not provided^)
ECHO        option2: build/rebuild  ^(build will be used ^if ^not provided^)
ECHO.
)

IF "%1"=="-release" (
SET BuildConfiguration=Release
SET TotalJobs=17
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

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Pricing Service2
ECHO [%JobCount%/%TotalJobs%] Pricing Service2 %BuildMode% started
ECHO Pricing Service2 %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.PricingService2.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Pricing Service2.
)
ECHO Pricing Service2 %BuildMode% completed
ECHO Pricing Service2 %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Pricing Service2 UI
ECHO [%JobCount%/%TotalJobs%] Pricing Service2 UI %BuildMode% started
ECHO Pricing Service2 UI %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.PricingService2UI.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Pricing Service2 UI.
)
ECHO Pricing Service2 UI %BuildMode% completed
ECHO Pricing Service2 UI %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Trade Service
ECHO [%JobCount%/%TotalJobs%] Trade Service %BuildMode% started
ECHO Trade Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.TradeService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Trade Service.
)
ECHO Trade Service %BuildMode% completed
ECHO Trade Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Trade Service UI
ECHO [%JobCount%/%TotalJobs%] Trade Service UI %BuildMode% started
ECHO Trade Service UI %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.TradeServiceUI.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Trade Service UI.
)
ECHO Trade Service UI %BuildMode% completed
ECHO Trade Service UI %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Expnl Service
ECHO [%JobCount%/%TotalJobs%] Expnl Service %BuildMode% started
ECHO Expnl Service %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.ExpnlService.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Expnl Service.
)
ECHO Expnl Service %BuildMode% completed
ECHO Expnl Service %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Expnl Service UI
ECHO [%JobCount%/%TotalJobs%] Expnl Service UI %BuildMode% started
ECHO Expnl Service UI %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.ExpnlServiceUI.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Expnl Service UI.
)
ECHO Expnl Service UI %BuildMode% completed
ECHO Expnl Service UI %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Client
ECHO [%JobCount%/%TotalJobs%] Client %BuildMode% started
ECHO Client %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.Client.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Client.
)
ECHO Client %BuildMode% completed
ECHO Client %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Admin
ECHO [%JobCount%/%TotalJobs%] Admin %BuildMode% started
ECHO Admin %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.SuperAdmin.sln" /t:restore,%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Admin.
)
ECHO Admin %BuildMode% completed
ECHO Admin %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

IF "%1"=="-release" (
TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Installer
ECHO [%JobCount%/%TotalJobs%] Installer %BuildMode% started
ECHO Installer %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.Installer.sln" /t:restore,%BuildMode% /p:Configuration=Release /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Installer.
)
ECHO Installer %BuildMode% completed
ECHO Installer %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [!JobCount!/%TotalJobs%] %BuildMode%ing Database
ECHO [!JobCount!/%TotalJobs%] Database %BuildMode% started
ECHO Database %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.Database.sln" /t:restore,%BuildMode% /p:Configuration=Release /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Database.
)
ECHO Database %BuildMode% completed
ECHO Database %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1
)

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance Utility
ECHO [%JobCount%/%TotalJobs%] Compliance Utility %BuildMode% started
ECHO Compliance Utility %BuildMode% started >> "%LogFileName%"
CD "JavaModules\prana-utility"
CALL mvn install > NUL
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Compliance Utility.
	ECHO Error occurred while %BuildMode%ing the Compliance Utility. >> "%~dp0%LogFileName%"
) 
ECHO Compliance Utility %BuildMode% completed
ECHO Compliance Utility %BuildMode% completed >> "%~dp0%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance Business Objects
ECHO [%JobCount%/%TotalJobs%] Compliance Business Objects %BuildMode% started
ECHO Compliance Business Objects %BuildMode% started >> "%~dp0%LogFileName%"
CD "..\prana-businessObjects"
CALL mvn install > NUL
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Compliance Business Objects.
	ECHO Error occurred while %BuildMode%ing the Compliance Business Objects. >> "%~dp0%LogFileName%"
)
ECHO Compliance Business Objects %BuildMode% completed
ECHO Compliance Business Objects %BuildMode% completed >> "%~dp0%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance AMQP Adapter
ECHO [%JobCount%/%TotalJobs%] Compliance AMQP Adapter %BuildMode% started
ECHO Compliance AMQP Adapter %BuildMode% started >> "%~dp0%LogFileName%"
CD "..\prana-amqpAdapter"
CALL mvn install > NUL
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Compliance AMQP Adapter.
	ECHO Error occurred while %BuildMode%ing the Compliance AMQP Adapter. >> "%~dp0%LogFileName%"
)
ECHO Compliance AMQP Adapter %BuildMode% completed
ECHO Compliance AMQP Adapter %BuildMode% completed >> "%~dp0%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance Rule Engine Mediator
ECHO [%JobCount%/%TotalJobs%] Compliance Rule Engine Mediator %BuildMode% started
ECHO Compliance Rule Engine Mediator %BuildMode% started >> "%~dp0%LogFileName%"
CD "..\prana-ruleEngineMediator"
CALL mvn install > NUL
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Compliance Rule Engine Mediator.
	ECHO Error occurred while %BuildMode%ing the Compliance Rule Engine Mediator. >> "%~dp0%LogFileName%"
)
ECHO Compliance Rule Engine Mediator %BuildMode% completed
ECHO Compliance Rule Engine Mediator %BuildMode% completed >> "%~dp0%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance Esper Calculator
ECHO [%JobCount%/%TotalJobs%] Compliance Esper Calculator %BuildMode% started
ECHO Compliance Esper Calculator %BuildMode% started >> "%~dp0%LogFileName%"
CD "..\prana-esperCalculator"
CALL mvn install > NUL
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Compliance Esper Calculator.
	ECHO Error occurred while %BuildMode%ing the Compliance Esper Calculator. >> "%~dp0%LogFileName%"
)
ECHO Compliance Esper Calculator %BuildMode% completed
ECHO Compliance Esper Calculator %BuildMode% completed >> "%~dp0%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Compliance Logging Tool
ECHO [%JobCount%/%TotalJobs%] Compliance Logging Tool %BuildMode% started
ECHO Compliance Logging Tool %BuildMode% started >> "%~dp0%LogFileName%"
CD "..\prana-loggingTool"
CALL mvn install > NUL
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Compliance Logging Tool.
	ECHO Error occurred while %BuildMode%ing the Compliance Logging Tool. >> "%~dp0%LogFileName%"
)
ECHO Compliance Logging Tool %BuildMode% completed
ECHO Compliance Logging Tool %BuildMode% completed >> "%~dp0%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Basket Compliance Service
ECHO [%JobCount%/%TotalJobs%] Basket Compliance Service %BuildMode% started
ECHO Basket Compliance Service %BuildMode% started >> "%~dp0%LogFileName%"
CD "..\prana-basketComplianceService"
CALL mvn install > NUL
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Basket Compliance Service.
	ECHO Error occurred while %BuildMode%ing the Basket Compliance Service. >> "%~dp0%LogFileName%"
)
ECHO Basket Compliance Service %BuildMode% completed
ECHO Basket Compliance Service %BuildMode% completed >> "%~dp0%LogFileName%"
SET /a JobCount+=1

CD ../..

ECHO.
ECHO %BuildMode% completed in %BuildConfiguration% Mode - %date% %time%
ECHO %BuildMode% completed in %BuildConfiguration% Mode - %date% %time%>> "%LogFileName%"

IF "%IsDirectlyCalled%"=="false" (
EXIT /b 0
)

REM SET the target directory for creating shortcuts
SET targetDirectory="Shortcuts_%BuildConfiguration%"

REM create a directory
IF NOT EXIST %targetDirectory% MKDIR %targetDirectory%

REM Get current working directory
REM %0 is first input arg like in any console program. In this it is the script itself
REM %~dp0 returns path to the script
SET cwd=%~dp0

REM SET the path of each folder
SET sourcefolder1="%cwd%\Application\Prana.PricingAnalysisModels\Prana.PricingService2Host\bin\%BuildConfiguration%"
SET sourcefolder2="%cwd%\Application\Prana.PricingAnalysisModels\Prana.PricingService2UI\bin\%BuildConfiguration%"
SET sourcefolder3="%cwd%\Application\Prana.Server\Prana.TradeServiceHost\bin\%BuildConfiguration%"
SET sourcefolder4="%cwd%\Application\Prana.Server\Prana.TradeServiceUI\bin\%BuildConfiguration%"
SET sourcefolder5="%cwd%\Application\Prana.ExpnlServiceHost\bin\%BuildConfiguration%"
SET sourcefolder6="%cwd%\Application\Prana.ExpnlServiceUI\bin\%BuildConfiguration%"
SET sourcefolder7="%cwd%\Application\Prana.Client\Prana\bin\%BuildConfiguration%"
SET sourcefolder8="%cwd%\ApplicationAdmin\PranaSuperAdmin\Prana.Admin\bin\%BuildConfiguration%"
SET sourcefolder9="%cwd%\JavaModules\prana-ruleEngineMediator\Export"
SET sourcefolder10="%cwd%\JavaModules\prana-esperCalculator\Export"
SET sourcefolder11="%cwd%\JavaModules\prana-loggingTool\Export"
SET sourcefolder12="%cwd%\JavaModules\prana-basketComplianceService\Export"

REM SET the path of each file
SET sourcefile1="%cwd%\Application\Prana.PricingAnalysisModels\Prana.PricingService2Host\bin\%BuildConfiguration%\Prana.PricingService2Host.exe"
SET sourcefile2="%cwd%\Application\Prana.PricingAnalysisModels\Prana.PricingService2UI\bin\%BuildConfiguration%\Prana.PricingService2UI.exe"
SET sourcefile3="%cwd%\Application\Prana.Server\Prana.TradeServiceHost\bin\%BuildConfiguration%\Prana.TradeServiceHost.exe"
SET sourcefile4="%cwd%\Application\Prana.Server\Prana.TradeServiceUI\bin\%BuildConfiguration%\Prana.TradeServiceUI.exe"
SET sourcefile5="%cwd%\Application\Prana.ExpnlServiceHost\bin\%BuildConfiguration%\Prana.ExpnlServiceHost.exe"
SET sourcefile6="%cwd%\Application\Prana.ExpnlServiceUI\bin\%BuildConfiguration%\Prana.ExpnlServiceUI.exe"
SET sourcefile7="%cwd%\Application\Prana.Client\Prana\bin\%BuildConfiguration%\Prana.exe"
SET sourcefile8="%cwd%\ApplicationAdmin\PranaSuperAdmin\Prana.Admin\bin\%BuildConfiguration%\Prana.Admin.exe"
SET sourcefile9="%cwd%\JavaModules\prana-ruleEngineMediator\Export\StartRuleEngineMediator.bat"
SET sourcefile10="%cwd%\JavaModules\prana-esperCalculator\Export\StartEsperCalculator.bat"
SET sourcefile11="%cwd%\JavaModules\prana-loggingTool\Export\StartLoggingTool.bat"
SET sourcefile12="%cwd%\JavaModules\prana-basketComplianceService\Export\StartBasketComplianceService.bat"

REM SET the path of each file shortcut
SET targetgfile1=%targetDirectory%\1_PricingService2Host
SET targetgfile2=%targetDirectory%\2_PricingService2UI
SET targetgfile3=%targetDirectory%\3_TradeServiceHost
SET targetgfile4=%targetDirectory%\4_TradeServiceUI
SET targetgfile5=%targetDirectory%\5_ExpnlServiceHost
SET targetgfile6=%targetDirectory%\6_ExpnlServiceUI
SET targetgfile7=%targetDirectory%\7_Prana
SET targetgfile8=%targetDirectory%\8_Admin
SET targetgfile9=%targetDirectory%\9_RuleMediator
SET targetgfile10=%targetDirectory%\10_EsperEngine
SET targetgfile11=%targetDirectory%\11_LoggingTool
SET targetgfile12=%targetDirectory%\12_BasketComplianceService

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
ShortcutGenerator /target:%sourcefile11% /shortcut:%targetgfile11% /startin:%sourcefolder11%
ShortcutGenerator /target:%sourcefile12% /shortcut:%targetgfile12% /startin:%sourcefolder12%

PAUSE