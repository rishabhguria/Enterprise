@echo off
REM New Line 2 and 3 in place of MovePath Direct
set MovePathTemp=%1
set MovePath=%MovePathTemp:~1,-1%
echo %MovePath%
set MachineIP=%2
set ClientDB=%3
set SMDB=%4

if not "%ClientDB%"=="" (
sqlcmd -E -S %MachineIP% -Q "ALTER DATABASE [%ClientDB%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE"

sqlcmd -E -S %MachineIP% -Q "DROP DATABASE [%ClientDB%]"
)

if not "%SMDB%"=="" (
    sqlcmd -E -S %MachineIP% -Q "ALTER DATABASE [%SMDB%] SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
    sqlcmd -E -S %MachineIP% -Q "DROP DATABASE [%SMDB%]"
)
if not "%ClientDB%"=="" (
echo Restoration of Client DB %ClientDB%
sqlcmd -E -S %MachineIP% -Q "RESTORE DATABASE [%ClientDB%] FROM DISK='%MovePath%\%ClientDB%.bak' with replace, move '%ClientDB%_DATA' to '%MovePath%\%ClientDB%.mdf', move '%ClientDB%_LOG' to '%MovePath%\%ClientDB%.LDF'"
)
if not "%SMDB%"=="" (
    echo Restoration of SMDB %SMDB%
    sqlcmd -E -S %MachineIP% -Q "RESTORE DATABASE [%SMDB%] FROM DISK='%MovePath%\%SMDB%.bak' with replace, move '%SMDB%_DATA' to '%MovePath%\%SMDB%.mdf', move '%SMDB%_LOG' to '%MovePath%\%SMDB%.LDF'"
)
