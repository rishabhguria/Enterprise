@echo off
title = Create Prana Release Folder
set "RELEASE_PATH=%1"

REM %0 is first input arg like in any console program. In this it is the script itself
REM %~d0 returns drive letter
REM %!dp0 returns path to the script
set "CURRENT_PATH=%~dp0"
set "Path_PranaAdmin=%CURRENT_PATH%\ApplicationAdmin\PranaSuperAdmin\Prana.Admin\bin\Release"
set "Path_ExposureAndPNLCalculator=%CURRENT_PATH%\Application\Prana.ExpnlServiceHost\bin\Release"
set "Path_ExposureAndPNLCalculatorUI=%CURRENT_PATH%\Application\Prana.ExpnlServiceUI\bin\Release"
set "Path_TradeServer=%CURRENT_PATH%\Application\Prana.Server\Prana.TradeServiceHost\bin\Release"
set "Path_TradeServerUI=%CURRENT_PATH%\Application\Prana.Server\Prana.TradeServiceUI\bin\Release"
set "Path_PricingServer=%CURRENT_PATH%\Application\Prana.PricingAnalysisModels\Prana.PricingService2Host\bin\Release"
set "Path_PricingServerUI=%CURRENT_PATH%\Application\Prana.PricingAnalysisModels\Prana.PricingService2UI\bin\Release"
set "Path_PranaClient=%CURRENT_PATH%\Application\Prana.Client\Prana\bin\Release"
set "Path_EsperCalculator=%CURRENT_PATH%\JavaModules\prana-esperCalculator\Export"
set "Path_RuleEngineMediator=%CURRENT_PATH%\JavaModules\prana-ruleEngineMediator\Export"
set "Path_BasketComplianceService=%CURRENT_PATH%\JavaModules\prana-basketComplianceService\Export"
set "Path_ComplianceLoggingTool=%CURRENT_PATH%\JavaModules\prana-loggingTool\Export"
set "Path_TestAutomation=%CURRENT_PATH%\Test Automation\Nirvana.TestAutomation.TestExecutor\bin\Release"
set "Path_TestAutomation_RegressionTestCases=%CURRENT_PATH%\Test Automation\Regression Test Cases"

set "Path_Auth_Service=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.AuthServiceHost\bin\Release"
set "Path_BlotterDataService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.BlotterDataServiceHost\bin\Release"
set "Path_CommonDataService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.CommonDataServiceHost\bin\Release"
set "Path_ComplianceAlertService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.ComplianceAlertsServiceHost\bin\Release"
set "Path_SecurityValidationService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.SecurityValidationServiceHost\bin\Release"
set "Path_serviceGateway=%CURRENT_PATH%\Prana.ServiceGateway\Prana.ServiceGateway\bin\Release\net6.0"

set "Path_TradingService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.TradingServiceHost\bin\Release"
set "Path_AllocationService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.AllocationServiceHost\bin\Release"
set "Path_CalculationService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.CalculationServiceHost\bin\Release"
set "Path_LiveFeedService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.LiveFeedServiceHost\bin\Release"
set "Path_WatchlistDataService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.WatchlistDataServiceHost\bin\Release"
set "Path_LayoutService=%CURRENT_PATH%\Prana.GreenFieldServices\Prana.LayoutServiceHost\bin\Release"

D:
cd D:\
cd %RELEASE_PATH%


mkdir "Admin Release"
xcopy %Path_PranaAdmin% "%RELEASE_PATH%\Admin Release" /y /S

mkdir "Pricing"
xcopy %Path_PricingServer% "%RELEASE_PATH%\Pricing" /y /S

mkdir "Pricing UI"
xcopy %Path_PricingServerUI% "%RELEASE_PATH%\Pricing UI" /y /S

mkdir "Server"
xcopy %Path_TradeServer% "%RELEASE_PATH%\Server" /y /S

mkdir "Server UI"
xcopy %Path_TradeServerUI% "%RELEASE_PATH%\Server UI" /y /S

mkdir "Expnl"
xcopy %Path_ExposureAndPNLCalculator% "%RELEASE_PATH%\Expnl" /y /S

mkdir "Expnl UI"
xcopy %Path_ExposureAndPNLCalculatorUI% "%RELEASE_PATH%\Expnl UI" /y /S

mkdir "Client Release"
xcopy %Path_PranaClient% "%RELEASE_PATH%\Client Release" /y /S

rmdir /s /q "%RELEASE_PATH%\EsperCalculator\target\resources\eplModules"

mkdir "EsperCalculator"
xcopy %Path_EsperCalculator% "%RELEASE_PATH%\EsperCalculator" /y /S

mkdir "RuleEngineMediator"
xcopy %Path_RuleEngineMediator% "%RELEASE_PATH%\RuleEngineMediator" /y /S

rmdir /s /q "%RELEASE_PATH%\BasketComplianceService\target\resources\eplModules"

mkdir "BasketComplianceService"
xcopy %Path_BasketComplianceService% "%RELEASE_PATH%\BasketComplianceService" /y /S

mkdir "ComplianceLoggingTool"
xcopy %Path_ComplianceLoggingTool% "%RELEASE_PATH%\ComplianceLoggingTool" /y /S

mkdir "TestAutomation"
xcopy "%Path_TestAutomation%" "%RELEASE_PATH%\TestAutomation" /y /S

mkdir "Regression Test Cases"
xcopy "%Path_TestAutomation_RegressionTestCases%" "%RELEASE_PATH%\Regression Test Cases" /y /S

mkdir "Auth"
xcopy %Path_Auth_Service% "%RELEASE_PATH%\Auth" /S /E /Y /I

mkdir "BlotterDataService"
xcopy %Path_BlotterDataService% "%RELEASE_PATH%\BlotterDataService" /S /E /Y /I

mkdir "CommonDataService"
xcopy %Path_CommonDataService% "%RELEASE_PATH%\CommonDataService" /S /E /Y /I

mkdir "ComplianceAlertService"
xcopy %Path_ComplianceAlertService% "%RELEASE_PATH%\ComplianceAlertService" /S /E /Y /I

mkdir "SecurityValidationService"
xcopy %Path_SecurityValidationService% "%RELEASE_PATH%\SecurityValidationService" /S /E /Y /I

mkdir "serviceGateway"
xcopy %Path_serviceGateway% "%RELEASE_PATH%\serviceGateway" /S /E /Y /I


mkdir "TradingService"
xcopy %Path_TradingService% "%RELEASE_PATH%\TradingService" /S /E /Y /I

mkdir "AllocationService"
xcopy %Path_AllocationService% "%RELEASE_PATH%\AllocationService" /S /E /Y /I

mkdir "CalculationService"
xcopy %Path_CalculationService% "%RELEASE_PATH%\CalculationService" /S /E /Y /I

mkdir "LiveFeedService"
xcopy %Path_LiveFeedService% "%RELEASE_PATH%\LiveFeedService" /S /E /Y /I

mkdir "WatchlistDataService"
xcopy %Path_WatchlistDataService% "%RELEASE_PATH%\WatchlistDataService" /S /E /Y /I

mkdir "LayoutService"
xcopy %Path_LayoutService% "%RELEASE_PATH%\LayoutService" /S /E /Y /I

pause