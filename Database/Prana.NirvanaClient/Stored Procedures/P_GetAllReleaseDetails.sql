-----------------------------------------------------------------
--Created BY: Bhavana
--Date: 2-june-14
--Purpose: Get the release details
-----------------------------------------------------------------
CREATE procedure [dbo].[P_GetAllReleaseDetails]
as
SELECT DISTINCT T_CompanyReleaseDetails.CompanyReleaseID, ISNULL(T_CompanyReleaseMapping.CompanyID,0), ISNULL(T_CompanyFunds.CompanyFundID,0), 
T_CompanyReleaseDetails.ReleaseName, T_CompanyReleaseDetails.IP,
T_CompanyReleaseDetails.ReleasePath, T_CompanyReleaseDetails.ClientDB_Name, T_CompanyReleaseDetails.SMDB_Name,ISNULL(T_ImportFileSettings.ReleaseID,0) as IsInUse
FROM T_CompanyReleaseDetails LEFT JOIN
T_CompanyFunds ON T_CompanyReleaseDetails.CompanyReleaseID = T_CompanyFunds.FundReleaseID LEFT JOIN
T_CompanyReleaseMapping ON T_CompanyReleaseDetails.CompanyReleaseID = T_CompanyReleaseMapping.ReleaseID
LEFT JOIN
T_ImportFileSettings ON T_ImportFileSettings.ReleaseID = T_CompanyFunds.FundReleaseID
ORDER BY  T_CompanyReleaseDetails.CompanyReleaseID

