@echo off
rem This batch program starts a java application and passes in the commandline arguements

rem set PATH=\path\to\jvm\bin

set JP=..\..\..\..\lib\


set cp=%cp%;%JP%\fixCore.jar
set cp=%cp%;%JP%\fixFilePersistence.jar
set cp=%cp%;%JP%\fixUniversalServer.jar
set cp=%cp%;%JP%\fixSocketAdapter.jar

java -version
@echo on

java -mx256M -classpath "%cp%"  %*
pause
exit
