
/********************************************
Created By: Omshiv
date: 16 Jan 2014
Desc: Get master fund Target ratio for allocation

Usage: EXEC P_GetMasterFundTargetRatio

*********************************************/
Create Procedure [dbo].[P_GetMasterFundTargetRatio]
as
select CMF.MasterFundName,CMF.CompanyMasterFundID, ISNULL(AMF.TargetRatioPct,0) as TargetRatioPct from T_CompanyMasterFunds CMF
left JOIN T_AllocationMasterfundRatio AMF on AMF.MasterFundID = CMF.CompanyMasterFundID




