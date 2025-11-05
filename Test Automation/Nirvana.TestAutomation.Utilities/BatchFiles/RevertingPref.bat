@echo off
echo Deleting PranaPreferences Folder 

@RD /S /Q %2

xcopy /s /i %1 %2