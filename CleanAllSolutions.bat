@ECHO off
SET msbuildpath=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin

ECHO USAGE:
ECHO     %0 -option
ECHO     option: debug/release  ^(debug will be used ^if ^not provided^)
ECHO.

SET BuildConfiguration=Debug

IF "%1"=="-release" (
SET BuildConfiguration=Release
)

REM Total number of solutions to be built
SET TotalJobs=21

REM JobCount used for serial number of solutions being built
SET JobCount=1

REM %0 is first input arg like in any console program. In this it is the script itself
REM %~d0 returns drive letter
REM %~dp0 returns path to the script
%~d0
CD\
CD %~dp0

TITLE = [%JobCount%/%TotalJobs%] Cleaning Pricing Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.PricingService2.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Pricing Service UI (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.PricingService2UI.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Trade Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.TradeService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Trade Service UI (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.TradeServiceUI.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Expnl Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.ExpnlService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Expnl Service UI (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.ExpnlServiceUI.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Client (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.Client.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Admin (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.SuperAdmin.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Installer (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.Installer.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Database (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.Database.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Service Gateway (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.ServiceGateway.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Allocation Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.AllocationService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Auth Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.AuthService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Blotter Data Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.BlotterDataService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Calculation Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.CalculationService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Common Data Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.CommonDataService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning LiveFeed Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.LiveFeedService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Security Validation Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.SecurityValidationService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Trading Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.TradingService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Watchlist Data Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.WatchlistDataService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Cleaning Layout Service (%BuildConfiguration% Mode)
"%msbuildpath%\msbuild" "Prana.LayoutService.sln" /t:Clean /p:Configuration=%BuildConfiguration% /m
SET /a JobCount+=1

PAUSE