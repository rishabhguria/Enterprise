-----------------------------------------------------------------
--Modified BY: Bhavana
--Date: 22-may-14
--Purpose: Get the sorted record by fund name
-----------------------------------------------------------------

CREATE procedure [dbo].[P_GetCompanyFundsForSetup] 
@companyID int
as
--select 
--CompanyFundID,
--FundName,
--FundShortName,
--LocalCurrency,
--FundInceptionDate,
--FundOnBoardDate,
--ClosingMethodology,
--LockDate,
--CompanyThirdPartyID,
--SecSortCriteriaID,
--PostingLockScheduleID,
--CompanyID
--from T_CompanyFunds where CompanyID=@companyID
select 
T_CompanyFunds.CompanyFundID,
FundName,
FundShortName,
LocalCurrency,
FundInceptionDate,
FundOnBoardDate,
ClosingMethodology,
tab1.LockDate,
CompanyThirdPartyID,
SecSortCriteriaID,
PostingLockScheduleID,
CompanyID,
IsSwapAccount,
T_CompanyFunds.IsActive
from T_CompanyFunds
--------------------------------------------------
left JOIN
(SELECT t1.companyFundID, ActualActionDate as LockDate
from
(SELECT CompanyFundID, MAX(ExecutionTime) as executiontime from T_AdminAuditTrail GROUP BY CompanyFundID) as t1
inner join
(SELECT CompanyFundID, ActualActionDate, ExecutionTime from T_AdminAuditTrail) as t2
on t1.executiontime=t2.ExecutionTime
--ORDER BY t1.CompanyFundID
) AS tab1
ON
T_CompanyFunds.CompanyFundID=tab1.CompanyFundID 
--------------------------------------------------
where CompanyID=@companyID order BY T_CompanyFunds.FundName

