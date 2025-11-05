title 'Rule Mediator Engine'
color F2
reg add HKCU\Console /v QuickEdit /t REG_DWORD /d 0 /f
cls
java -Xms512M -jar prana.ruleEngineMediator.jar true 
