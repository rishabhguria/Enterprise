@ECHO OFF
SET msbuildpath=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin
SET sqlpackagepath=C:\Program Files\Microsoft SQL Server\160\DAC\bin
SETLOCAL enabledelayedexpansion

REM Default BuildConfiguration is Debug mode.
SET BuildConfiguration=Debug

REM Default BuildMode is Build. It makes more sense for debug build
SET BuildMode=Build

REM Total number of solutions to be built
SET TotalJobs=3

ECHO USAGE:
ECHO        %0 -option1 -option2
ECHO        option1: debug/release  ^(debug will be used ^if ^not provided^)
ECHO        option2: build/rebuild  ^(build will be used ^if ^not provided^)
ECHO.

IF "%1"=="-release" (
SET BuildConfiguration=Release
)

IF "%2"=="-rebuild" (
SET BuildMode=ReBuild
)

REM %0 is first input arg like in any console program. In this it is the script itself
REM %~dp0 returns path to the script
SET currentpath=%~dp0
SET LogFileName=%~n0.txt

REM JobCount used for serial number of solutions being built
SET JobCount=1

ECHO %BuildMode% and Publish started in %BuildConfiguration% Mode - %date% %time%
ECHO %BuildMode% and Publish started in %BuildConfiguration% Mode - %date% %time%> "%LogFileName%"
ECHO.

TITLE = [%JobCount%/%TotalJobs%] %BuildMode%ing Database
ECHO [%JobCount%/%TotalJobs%] %BuildMode%ing Database
ECHO Database %BuildMode% started >> "%LogFileName%"
"%msbuildpath%\msbuild" "Prana.Database.sln" /t:%BuildMode% /p:Configuration=%BuildConfiguration% /m /nologo /noconsolelogger /fileLogger /flp:logfile="%LogFileName%";errorsonly;Append
IF %errorlevel% neq 0 (
    ECHO Error occurred while %BuildMode%ing the Database.
)
ECHO Database %BuildMode% completed >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Publishing SM Database
ECHO [%JobCount%/%TotalJobs%] Publishing SM Database
ECHO Publishing SM Database >> "%LogFileName%"
"%sqlpackagepath%\SqlPackage.exe" /a:Publish /pr:"%currentpath%Database\Prana.SecurityMaster\Prana.SecurityMaster.publish.xml" /SourceFile:"%currentpath%Database\Prana.SecurityMaster\bin\%BuildConfiguration%\Prana.SecurityMaster.dacpac" 2>>"%LogFileName%"
ECHO SM Database Published >> "%LogFileName%"
SET /a JobCount+=1

TITLE = [%JobCount%/%TotalJobs%] Publishing Client Database
ECHO [%JobCount%/%TotalJobs%] Publishing Client Database
ECHO Publishing Client Database >> "%LogFileName%"
"%sqlpackagepath%\SqlPackage.exe" /a:Publish /pr:"%currentpath%Database\Prana.NirvanaClient\Prana.NirvanaClient.publish.xml" /SourceFile:"%currentpath%Database\Prana.NirvanaClient\bin\%BuildConfiguration%\Prana.NirvanaClient.dacpac" 2>>"%LogFileName%"
ECHO Client Database Published >> "%LogFileName%"

ECHO.
ECHO %BuildMode% and Publish completed in %BuildConfiguration% Mode - %date% %time%
ECHO %BuildMode% and Publish completed in %BuildConfiguration% Mode - %date% %time%>> "%LogFileName%"

PAUSE