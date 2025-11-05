-----------------------------------------------------
--Created By: Bharat raturi
--Date: 29/3/2014
--Purpose: Get the Release ID-Release Name for file setting
-------------------------------------------------------
Create Procedure [dbo].[P_GetAllReleaseIDNames]
as
select CompanyReleaseID, ReleaseName from T_CompanyReleaseDetails
