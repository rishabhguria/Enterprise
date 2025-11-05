@ECHO off

ECHO USAGE:
ECHO		%0 [option]
ECHO  		r (or R) to have Release mode services uninstalled. It will uninstall services from debug path if nothing is specified.
ECHO.
ECHO.

ECHO Stopping and uninstalling PranaPricingSvc
SET serviceName= PranaPricingSvc
SC STOP %serviceName%
SC DELETE %serviceName%
ECHO PranaPricingSvc uninstalled

ECHO Stopping and uninstalling PranaTradeSvc
SET serviceName= PranaTradeSvc
SC STOP %serviceName%
SC DELETE %serviceName%
ECHO PranaTradeSvc uninstalled

ECHO Stopping and uninstalling PranaExpnlSvc
SET serviceName= PranaExpnlSvc
SC STOP %serviceName%
SC DELETE %serviceName%
ECHO PranaExpnlSvc uninstalled

pause