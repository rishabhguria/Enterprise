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
set "Path_RuleEngineMediator=%CURRENT_PATH%\JavaModules\prana-basketComplianceService\Export"
set "Path_ComplianceLoggingTool=%CURRENT_PATH%\JavaModules\prana-loggingTool\Export"
set "Path_TestAutomation=%CURRENT_PATH%\Test Automation\Nirvana.TestAutomation.TestExecutor\bin\Release"
set "Path_TestAutomation_RegressionTestCases=%CURRENT_PATH%\Test Automation\Regression Test Cases"

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

mkdir "EsperCalculator"
xcopy %Path_EsperCalculator% "%RELEASE_PATH%\EsperCalculator" /y /S

mkdir "RuleEngineMediator"
xcopy %Path_RuleEngineMediator% "%RELEASE_PATH%\RuleEngineMediator" /y /S

mkdir "RuleEngineMediator"
xcopy %Path_RuleEngineMediator% "%RELEASE_PATH%\BasketComplianceService" /y /S

mkdir "ComplianceLoggingTool"
xcopy %Path_ComplianceLoggingTool% "%RELEASE_PATH%\ComplianceLoggingTool" /y /S

mkdir "TestAutomation"
xcopy "%Path_TestAutomation%" "%RELEASE_PATH%\TestAutomation" /y /S

mkdir "Regression Test Cases"
xcopy "%Path_TestAutomation_RegressionTestCases%" "%RELEASE_PATH%\Regression Test Cases" /y /S
pause