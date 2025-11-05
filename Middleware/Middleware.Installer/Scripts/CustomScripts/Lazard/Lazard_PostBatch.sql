
/****************************************
Author: Ankit
Creation Date: 12th August,2013
Description :Used to modify customized changes In Lazard MW

Execution Method:
P_MW_PostMWBatch '2012-1-1','2013-5-1'
****************************************/

CREATE Proc P_MW_PostMWBatch
(
@StartDate datetime,
@EndDate datetime
)

As

Set NoCount On

--Update All UDA Country to "Currency" for CASH
Update 
T_MW_GenericPNL
Set 
UDACountry = 'Currency'
where
Asset = 'CASH'
and 
datediff(d,@Startdate , rundate)>=0
and
datediff(d,rundate,@EndDate)<=0