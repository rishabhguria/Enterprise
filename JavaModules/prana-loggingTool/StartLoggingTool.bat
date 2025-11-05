title 'Logging Tool'
color F2
reg add HKCU\Console /v QuickEdit /t REG_DWORD /d 0 /f
cls
java -Xms512M -jar prana.loggingTool.jar true 
