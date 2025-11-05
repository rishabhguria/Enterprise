title 'Esper Calculation Engine'
color F2
reg add HKCU\Console /v QuickEdit /t REG_DWORD /d 0 /f
cls
java -Xms1024M -XX:+UseCompressedOops -jar prana.esperCalculator.jar true 
