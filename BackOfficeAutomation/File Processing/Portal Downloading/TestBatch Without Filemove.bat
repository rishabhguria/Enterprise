

cd /d "D:\Nirvana\Enterprise\BackOfficeAutomation\File Processing\Portal Downloading"

@echo off
REM Run all tests in Debug Mode
mvn -Dmaven.surefire.debug test

REM Pause to keep the window open after execution
pause


