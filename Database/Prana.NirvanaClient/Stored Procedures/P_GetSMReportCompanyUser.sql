
CREATE PROCEDURE [dbo].[P_GetSMReportCompanyUser]
AS
select TU.UserID as UserID , ISNULL(TU.ShortName,'') + '_' + ISNULL((TC.Name),'') as CompanyUser 
FROM T_CompanyUser TU inner JOIN T_Company TC ON TU.CompanyID = TC.CompanyID 

