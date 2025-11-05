taskkill /F /IM Prana.PricingService2Host.exe /T >> KilledBatch.txt 2>&1
taskkill /F /IM Prana.GreeksCalculationService.exe /T >> KilledBatch.txt 2>&1
exit 0