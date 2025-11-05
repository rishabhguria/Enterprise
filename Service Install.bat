
@ECHO OFF
ECHO USAGE:
ECHO		%0 [option]
ECHO  		r (or R) to have services installed from "Release" path. Release will also be added to service names. It will install services from debug path if nothing is specified.
ECHO.
ECHO.

SET Mode=Debug
IF "%1"=="R" (SET Mode=Release) 
IF "%1"=="r" (SET Mode=Release)

SET serviceUserName=<SPECIFY USERNAME>
SET password=<SPECIFY PASSWORD>

ECHO Installing Pricing Service
SET servicePath="%CD%\Application\Prana.PricingAnalysisModels\Prana.PricingService2Host\bin\%Mode%\Prana.PricingService2Host.exe"
SET serviceName=PranaPricingSvc
SC create %serviceName% binPath= %servicePath% DisplayName= "Prana Pricing Service-"%Mode%
SC description %serviceName% "Prana Pricing Service provides services to manage and work with market data feed, options and Risk analytics."
SC CONFIG %serviceName% obj= %serviceUserName% password= %password%
ECHO Pricing Service Installed

ECHO Installing Trade Service
SET servicePath="%CD%\Application\Prana.Server\Prana.TradeServiceHost\bin\%Mode%\Prana.TradeServiceHost.exe"
SET serviceName=PranaTradeSvc
SC create %serviceName% binPath= %servicePath% DisplayName= "Prana Trade Service-"%Mode%
SC description %serviceName% "Prana Trade Service provides services to manage and work with Security Master, OMS features (e.g. Trade Order management, allocation, trade reporting) Portfolio Accounting, interaction with thirdparties and general ledger."
SC CONFIG %serviceName% obj= %serviceUserName% password= %password%
ECHO Trade Service Installed

ECHO Installing Expnl Service
SET servicePath="%CD%\Application\Prana.ExpnlServiceHost\bin\%Mode%\Prana.ExpnlServiceHost.exe"
SET serviceName=PranaExpnlSvc
SC create %serviceName% binPath= %servicePath% DisplayName= "Prana Exposure Profit and Loss Service-"%Mode%
SC description %serviceName% "Prana Exposure, Profit & Loss Service provides real time Profit, loss and risk monitoring."
SC CONFIG %serviceName% obj= %serviceUserName% password= %password%
ECHO Expnl Service Installed

pause