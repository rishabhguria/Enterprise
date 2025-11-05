
/*
Script Creation Date: 1 October 2015
Created by : Pankaj Sharma
Description : 

commission has to be excluded from the report as a new Column in the report is added for it
*/
Update T_reportPreferences
set 
IncludeCommissionInPNL_Equity = 0
,IncludeCommissionInPNL_EquityOption = 0
,IncludeCommissionInPNL_Futures = 0
,IncludeCommissionInPNL_FutOptions= 0
,IncludeCommissionInPNL_Swaps = 0
,IncludeCommissionInPNL_FX = 0
,IncludeCommissionInPNL_Other = 0 
where ReportID='MTM_V0'