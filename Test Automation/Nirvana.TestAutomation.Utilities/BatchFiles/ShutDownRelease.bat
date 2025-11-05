taskkill /F /IM Prana.exe /T >> KilledBatch.txt 2>&1
taskkill /F /IM Prana.PricingService2Host.exe /T >> KilledBatch.txt 2>&1
taskkill /F /IM Prana.TradeServiceHost.exe /T >> KilledBatch.txt 2>&1
taskkill /F /IM Prana.ExpnlServiceHost.exe /T >> KilledBatch.txt 2>&1
taskkill /F /IM Prana.GreeksCalculationService.exe /T >> KilledBatch.txt 2>&1
taskkill /F /IM Nirvana.TestAutomation.AccessBridgeApp.exe /T >> KilledBatch.txt 2>&1
exit 0