-- =============================================  
-- Created by: Bharat raturi
-- Date: 16 may 2014
-- Purpose: get  Unapproved Mark Prices from DB 
-- Usage: P_GetUnApprovedMarkPrices '08/05/2014','08/06/2014'
-- =============================================
CREATE procedure [dbo].[P_GetUnApprovedMarkPrices]
@startDate Datetime,
@endDate Datetime
as
select 
DayMarkPriceID, Date, Symbol, FundID, FundName, FinalMarkPrice, AmendedMarkPrice 
from PM_DayMarkPrice inner JOIN T_CompanyFunds on FundID=CompanyFundID 
where (Date BETWEEN @startDate and @endDate) and isApproved=0  

