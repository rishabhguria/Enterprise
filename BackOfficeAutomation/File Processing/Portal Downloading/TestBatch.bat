@echo off
setlocal enabledelayedexpansion

REM Set the source directory path
set "sourcedir=C:\Backoffice Automation\AutomationTesting\Data"

REM Set the target directory path
set "target=C:\Backoffice Automation\AutomationTesting\Data"

REM Ask user for the folder name inside the source directory
set /p folder="Enter the folder name (case-insensitive): "
set "folder_lowercase=!folder:l=L!"
set "folder_lowercase=!folder_lowercase:~0,100!"

REM Check if the specified source folder exists
set "found=false"
for /d %%i in ("%sourcedir%\*") do (
    set "folder_name=%%~nxi"
    set "folder_name_lowercase=!folder_name:l=L!"
    echo !folder_name_lowercase! | findstr /i /c:"!folder_lowercase!" >nul
    if !errorlevel! equ 0 (
        set "source=%%i"
        set "found=true"
        goto Found
    )
)
:Found

if not "%found%"=="true" (
    echo Folder does not exist inside the source directory.
    exit /b
)

REM Check if the target directory exists, if not create it
if not exist "%target%" (
    mkdir "%target%"
)

REM Iterate through the files in the source directory and copy them to the target directory
for %%I in ("%source%\*.*") do (
    copy "%%I" "%target%"
)

echo Files copied successfully.

REM Run Maven test
cd /d "C:\Backoffice Automation\AutomationTesting"

mvn clean install

pause

endlocal
