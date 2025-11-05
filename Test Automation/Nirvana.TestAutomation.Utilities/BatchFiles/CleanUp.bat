echo off
taskkill /F /IM Prana.exe /T >> KilledBatch.txt 2>&1
taskkill /F /IM Prana.TradeServiceHost.exe /T >> KilledBatch.txt 2>&1
exit 0