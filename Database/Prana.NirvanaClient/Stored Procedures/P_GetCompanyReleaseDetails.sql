
CREATE procedure [dbo].[P_GetCompanyReleaseDetails]
as
SELECT DISTINCT TR.CompanyReleaseID,TM.CompanyID,TR.ReleaseName,TR.IP,TR.ReleasePath,TR.ClientDB_Name,TR.SMDB_Name
from
T_CompanyReleaseDetails TR inner join
T_CompanyReleaseMapping TM on TR.CompanyReleaseID = TM.ReleaseID
