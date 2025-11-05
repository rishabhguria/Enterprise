For Realized Pnl report we must have to add an entry 'OpenOriginalPurchaseDate' in 'T_MW_ReportPreferences' table in Client Database, otherwise "SearchBy" dropdown will be inactive.
Must Run T_MW_Gettransaction latest from dev middleware
For Realized ACA you have to Change the Reportid from parameter with the name 'Realized_MW_ACA' and run the following script 'NirvanaCode\SourceCode\Dev\Middleware\VS2010\CSBatch\Middleware.Installer\Scripts\Middleware\Data\D_MW_Realized_ACA_MW.sql'
In REALIZED PNL REPORT visibility of 'Total Realized P&L (Base)' should be visible