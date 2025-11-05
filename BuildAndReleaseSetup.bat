@ECHO off
TITLE = Release Setup Builder

ECHO USAGE:
ECHO		%0 [version]
ECHO  		If nothing is specified, previously saved version number will be used.
ECHO.

RD /s /q "Release Setup"
ECHO.

call BuildAllSolutions.bat -release -rebuild false

TITLE = Release Setup Builder
CD Installer
CALL BuildInstaller.bat %1

MD "..\Release Setup"
XCOPY /s /e setup "..\Release Setup\"

PAUSE