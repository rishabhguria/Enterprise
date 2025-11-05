@ECHO OFF

REM USAGE:
REM		%0 [VersionNumber]
REM  		If nothing is specified, previously saved version number will be used.
REM
REM

REM This script should be called after the successful build of all solutions.
REM The script builds the MSI and copies all the required files (MSI, DACPAC and installer) to a single folder 'Setup'

SET "CURRENT_PATH=%cd%"
SET "Path_Installer=%CURRENT_PATH%\WIX"

SET "Path_PricingService2=%CURRENT_PATH%\..\Application\Prana.PricingAnalysisModels\Prana.PricingService2Host\bin"
SET "Path_PricingService2UI=%CURRENT_PATH%\..\Application\Prana.PricingAnalysisModels\Prana.PricingService2UI\bin"
SET "Path_TradeService=%CURRENT_PATH%\..\Application\Prana.Server\Prana.TradeServiceHost\bin"
SET "Path_TradeServiceUI=%CURRENT_PATH%\..\Application\Prana.Server\Prana.TradeServiceUI\bin"
SET "Path_ExpnlService=%CURRENT_PATH%\..\Application\Prana.ExpnlServiceHost\bin"
SET "Path_ExpnlServiceUI=%CURRENT_PATH%\..\Application\Prana.ExpnlServiceUI\bin"
SET "Path_Client=%CURRENT_PATH%\..\Application\Prana.Client\Prana\bin"
SET "Path_Admin=%CURRENT_PATH%\..\ApplicationAdmin\PranaSuperAdmin\Prana.Admin\bin"
SET "Path_ServiceGateway=%CURRENT_PATH%\..\Prana.ServiceGateway\Prana.ServiceGateway\bin\Release"
SET "Path_GreenFieldServices=%CURRENT_PATH%\..\Prana.GreenFieldServices"
SET "Path_EsperCalculator=%CURRENT_PATH%\..\JavaModules\prana-esperCalculator"
SET "Path_RuleEngineMediator=%CURRENT_PATH%\..\JavaModules\prana-ruleEngineMediator"
SET "Path_ComplianceLoggingTool=%CURRENT_PATH%\..\JavaModules\prana-loggingTool"
SET "Path_BasketComplianceService=%CURRENT_PATH%\..\JavaModules\prana-basketComplianceService"
SET "Path_Infragistics=%INFRAGISTICS_HOME_20_2%"

REM Copy all files of Releases into Installer Folder

REM Pricing Service
IF EXIST "%Path_PricingService2%\Installer" ( 
	@RD /S /Q "%Path_PricingService2%\Installer"
)
MKDIR "%Path_PricingService2%\Installer\Pricing"
ROBOCOPY "%Path_PricingService2%\Release" "%Path_PricingService2%\Installer\Pricing" /E
ROBOCOPY "%Path_PricingService2UI%\Release" "%Path_PricingService2%\Installer\Pricing" /E
XCOPY /S /E /Y "%Path_Infragistics%Windows Forms\CLR4.0\Bin\*.dll" "%Path_PricingService2%\Installer\Pricing"
XCOPY /S /E /Y "%Path_Infragistics%WPF\CLR4.0\Bin\*.dll" "%Path_PricingService2%\Installer\Pricing"

REM Trade Service
IF EXIST "%Path_TradeService%\Installer" ( 
	@RD /S /Q "%Path_TradeService%\Installer"
)
MKDIR "%Path_TradeService%\Installer\Server"
ROBOCOPY "%Path_TradeService%\Release" "%Path_TradeService%\Installer\Server" /E
ROBOCOPY "%Path_TradeServiceUI%\Release" "%Path_TradeService%\Installer\Server" /E
XCOPY /S /E /Y "%Path_Infragistics%Windows Forms\CLR4.0\Bin\*.dll" "%Path_TradeService%\Installer\Server"
XCOPY /S /E /Y "%Path_Infragistics%WPF\CLR4.0\Bin\*.dll" "%Path_TradeService%\Installer\Server"

REM Expnl Service
IF EXIST "%Path_ExpnlService%\Installer" ( 
	@RD /S /Q "%Path_ExpnlService%\Installer"
)
MKDIR "%Path_ExpnlService%\Installer\Expnl"
ROBOCOPY "%Path_ExpnlService%\Release" "%Path_ExpnlService%\Installer\Expnl" /E
ROBOCOPY "%Path_ExpnlServiceUI%\Release" "%Path_ExpnlService%\Installer\Expnl" /E
XCOPY /S /E /Y "%Path_Infragistics%Windows Forms\CLR4.0\Bin\*.dll" "%Path_ExpnlService%\Installer\Expnl"
XCOPY /S /E /Y "%Path_Infragistics%WPF\CLR4.0\Bin\*.dll" "%Path_ExpnlService%\Installer\Expnl"

REM Client
IF EXIST "%Path_Client%\Installer" ( 
	@RD /S /Q "%Path_Client%\Installer"
)
MKDIR "%Path_Client%\Installer\Client Release"
ROBOCOPY "%Path_Client%\Release" "%Path_Client%\Installer\Client Release" /E
XCOPY /S /E /Y "%Path_Infragistics%Windows Forms\CLR4.0\Bin\*.dll" "%Path_Client%\Installer\Client Release"
XCOPY /S /E /Y "%Path_Infragistics%WPF\CLR4.0\Bin\*.dll" "%Path_Client%\Installer\Client Release"

REM Admin
IF EXIST "%Path_Admin%\Installer" ( 
	@RD /S /Q "%Path_Admin%\Installer"
)
MKDIR "%Path_Admin%\Installer\Admin Release"
ROBOCOPY "%Path_Admin%\Release" "%Path_Admin%\Installer\Admin Release" /E
XCOPY /S /E /Y "%Path_Infragistics%Windows Forms\CLR4.0\Bin\*.dll" "%Path_Admin%\Installer\Admin Release"
XCOPY /S /E /Y "%Path_Infragistics%WPF\CLR4.0\Bin\*.dll" "%Path_Admin%\Installer\Admin Release"

REM ServiceGateway
IF EXIST "%Path_ServiceGateway%\Installer" ( 
	@RD /S /Q "%Path_ServiceGateway%\Installer"
)
MKDIR "%Path_ServiceGateway%\Installer\ServiceGateway"
ROBOCOPY "%Path_ServiceGateway%\net6.0" "%Path_ServiceGateway%\Installer\ServiceGateway" /E

REM GreenFieldServices
IF EXIST "%Path_GreenFieldServices%\Installer" ( 
	@RD /S /Q "%Path_GreenFieldServices%\Installer"
)
MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services"

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Allocation Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.AllocationServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Allocation Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Auth Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.AuthServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Auth Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Blotter Data Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.BlotterDataServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Blotter Data Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Calculation Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.CalculationServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Calculation Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Common Data Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.CommonDataServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Common Data Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\LiveFeed Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.LiveFeedServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\LiveFeed Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Security Validation Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.SecurityValidationServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Security Validation Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Trading Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.TradingServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Trading Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Watchlist Data Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.WatchlistDataServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Watchlist Data Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Compliance Alerts Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.ComplianceAlertsServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Compliance Alerts Service" /E

MKDIR "%Path_GreenFieldServices%\Installer\GreenField Services\Layout Service"
ROBOCOPY "%Path_GreenFieldServices%\Prana.LayoutServiceHost\bin\Release" "%Path_GreenFieldServices%\Installer\GreenField Services\Layout Service" /E

REM Esper Calculator
IF EXIST "%Path_EsperCalculator%\Installer" ( 
	@RD /S /Q "%Path_EsperCalculator%\Installer"
)
MKDIR "%Path_EsperCalculator%\Installer\EsperCalculator"
ROBOCOPY "%Path_EsperCalculator%\Export" "%Path_EsperCalculator%\Installer\EsperCalculator" /E

REM Rule Engine Mediator
IF EXIST "%Path_RuleEngineMediator%\Installer" ( 
	@RD /S /Q "%Path_RuleEngineMediator%\Installer"
)
MKDIR "%Path_RuleEngineMediator%\Installer\RuleEngineMediator"
ROBOCOPY "%Path_RuleEngineMediator%\Export" "%Path_RuleEngineMediator%\Installer\RuleEngineMediator" /E

REM Compliance Logging Tool
IF EXIST "%Path_ComplianceLoggingTool%\Installer" ( 
	@RD /S /Q "%Path_ComplianceLoggingTool%\Installer"
)
MKDIR "%Path_ComplianceLoggingTool%\Installer\ComplianceLoggingTool"
ROBOCOPY "%Path_ComplianceLoggingTool%\Export" "%Path_ComplianceLoggingTool%\Installer\ComplianceLoggingTool" /E

REM Basket Compliance Service 
IF EXIST "%Path_BasketComplianceService%\Installer" ( 
	@RD /S /Q "%Path_BasketComplianceService%\Installer"
)
MKDIR "%Path_BasketComplianceService%\Installer\BasketComplianceService"
ROBOCOPY "%Path_BasketComplianceService%\Export" "%Path_BasketComplianceService%\Installer\BasketComplianceService" /E

REM Cleanup of release folders
@RD /S /Q "%Path_PricingService2%\Release"
@RD /S /Q "%Path_PricingService2UI%\Release"
@RD /S /Q "%Path_TradeService%\Release"
@RD /S /Q "%Path_TradeServiceUI%\Release"
@RD /S /Q "%Path_ExpnlService%\Release"
@RD /S /Q "%Path_ExpnlServiceUI%\Release"
@RD /S /Q "%Path_Client%\Release"
@RD /S /Q "%Path_Admin%\Release"
@RD /S /Q "%Path_ServiceGateway%\net6.0"
@RD /S /Q "%Path_GreenFieldServices%\Prana.AllocationServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.AuthServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.BlotterDataServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.CalculationServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.CommonDataServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.LiveFeedServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.SecurityValidationServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.TradingServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.WatchlistDataServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.ComplianceAlertsServiceHost\bin\Release"
@RD /S /Q "%Path_GreenFieldServices%\Prana.LayoutServiceHost\bin\Release"
@RD /S /Q "%Path_EsperCalculator%\Export"
@RD /S /Q "%Path_RuleEngineMediator%\Export"
@RD /S /Q "%Path_ComplianceLoggingTool%\Export"
@RD /S /Q "%Path_BasketComplianceService%\Export"

cd "%Path_PricingService2%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_Prana.Path_PricingService2 -dr INSTALL_SUBFOLDER -t "%CURRENT_PATH%\Configmodify.xslt" -out directory.wxs
candle.exe directory.wxs

cd "%Path_TradeService%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_Prana.Path_TradeService -dr INSTALL_SUBFOLDER -t "%CURRENT_PATH%\Configmodify.xslt" -out directory.wxs
candle.exe directory.wxs

cd "%Path_ExpnlService%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_Prana.Path_ExpnlService -dr INSTALL_SUBFOLDER -t "%CURRENT_PATH%\Configmodify.xslt" -out directory.wxs
candle.exe directory.wxs

cd "%Path_Client%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_Client -dr INSTALL_SUBFOLDER -t "%CURRENT_PATH%\Configmodify.xslt" -out directory.wxs
candle.exe directory.wxs

cd "%Path_Admin%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_Admin -dr INSTALL_SUBFOLDER -t "%CURRENT_PATH%\Configmodify.xslt" -out directory.wxs
candle.exe directory.wxs

cd "%Path_ServiceGateway%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_ServiceGateway -dr INSTALL_SUBFOLDER -t "%CURRENT_PATH%\Configmodify.xslt" -out directory.wxs
candle.exe directory.wxs

cd "%Path_GreenFieldServices%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_GreenFieldServices -dr INSTALL_SUBFOLDER -t "%CURRENT_PATH%\Configmodify.xslt" -out directory.wxs
candle.exe directory.wxs

cd "%Path_EsperCalculator%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_EsperCalculator -dr INSTALL_SUBFOLDER -out directory.wxs
candle.exe directory.wxs -out directory.wixobj

cd "%Path_RuleEngineMediator%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_RuleEngineMediator -dr INSTALL_SUBFOLDER -out directory.wxs
candle.exe directory.wxs -out directory.wixobj

cd "%Path_ComplianceLoggingTool%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_ComplianceLoggingTool -dr INSTALL_SUBFOLDER -out directory.wxs
candle.exe directory.wxs -out directory.wixobj

cd "%Path_BasketComplianceService%\Installer"
del directory.wxs directory.wixobj
heat.exe dir "." -sreg -gg -sfrag -g1 -srd -ke -cg componentGroupId_BasketComplianceService -dr INSTALL_SUBFOLDER -out directory.wxs
candle.exe directory.wxs -out directory.wixobj

REM Changing release version number
IF NOT "%~1"=="" (
	Powershell.exe -executionpolicy remotesigned -File "%CURRENT_PATH%\ChangeProductVersion.ps1" %1
)

REM Creating product.wixobj
candle.exe -ext WixUtilExtension "%CURRENT_PATH%\product.wxs" -out "%CURRENT_PATH%\product.wixobj"

light.exe -ext WixUIExtension -ext WixUtilExtension "%Path_PricingService2%\Installer\directory.wixobj" "%Path_TradeService%\Installer\directory.wixobj" "%Path_ExpnlService%\Installer\directory.wixobj" "%Path_Client%\Installer\directory.wixobj" "%Path_Admin%\Installer\directory.wixobj" "%Path_GreenFieldServices%\Installer\directory.wixobj" "%Path_ServiceGateway%\Installer\directory.wixobj" "%Path_EsperCalculator%\Installer\directory.wixobj" "%Path_RuleEngineMediator%\Installer\directory.wixobj" "%Path_ComplianceLoggingTool%\Installer\directory.wixobj" "%Path_BasketComplianceService%\Installer\directory.wixobj" "%CURRENT_PATH%\product.wixobj" -out "%Path_Installer%\installer.msi"

REM Copy ALL the required files into one folder
cd "%CURRENT_PATH%"

IF EXIST SetUp ( 
	@RD /S /Q SetUp
)

MKDIR "SetUp\ClientSpecificScripts"
XCOPY /S /E /Y "Prana.Installer\Prana.Installer\bin\Release" SetUp
XCOPY /S /E /Y Wix SetUp
XCOPY "..\Database\Prana.NirvanaClient\bin\Release\Prana.NirvanaClient.dacpac" SetUp
XCOPY "..\Database\Prana.NirvanaClient\bin\Release\Prana.SecurityMaster.dacpac" SetUp
XCOPY "..\Database\Prana.NirvanaClient\bin\Release\msdb.dacpac" SetUp
XCOPY "..\Database\Prana.NirvanaClient\bin\Release\master.dacpac" SetUp

FOR /f "delims=" %%a IN ('dir "..\Database\ClientDB" /b /a:d') DO (
	IF EXIST "..\Database\ClientDB\%%a\Prana.NirvanaClient\bin\Release\Prana.NirvanaClient.dacpac" ( 
		MKDIR "SetUp\ClientSpecificScripts\%%a"
		XCOPY "..\Database\ClientDB\%%a\Prana.NirvanaClient\bin\Release\Prana.NirvanaClient.dacpac" "SetUp\ClientSpecificScripts\%%a"
	)
)