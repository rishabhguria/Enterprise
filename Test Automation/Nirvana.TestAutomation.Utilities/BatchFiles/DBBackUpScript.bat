@echo off
set BackUpPath=%1
set MachineIP=%2
set ClientDB=%3
set SMDB=%4

REM creating the database folder in the release path 
if not exist %BackUpPath% md %BackUpPath% 
echo '%BackUpPath%' Folder created

echo Processing BackUp Of Client DB %ClientDB%
osql -E -S %MachineIP% -Q "BACKUP DATABASE [%ClientDB%] to disk='%BackUpPath%\%ClientDB%.bak'"

echo Processing BackUp Of SMDB %SMDB%
osql -E -S %MachineIP% -Q "BACKUP DATABASE [%SMDB%] to disk='%BackUpPath%\%SMDB%.bak'"
