title 'Basket Compliance Service'
color F2
reg add HKCU\Console /v QuickEdit /t REG_DWORD /d 0 /f
cls
java -Xms1024M -jar prana.basketComplianceService.jar true
