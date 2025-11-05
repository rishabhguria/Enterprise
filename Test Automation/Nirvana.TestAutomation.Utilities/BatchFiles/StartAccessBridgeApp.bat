taskkill /F /IM Nirvana.TestAutomation.AccessBridgeApp.exe /T >> KilledBatch.txt 2>&1
cd..
call "Nirvana.TestAutomation.AccessBridgeApp.exe"
exit 0