-- =============================================
-- Author:		Ankit Jain
-- Create date: 19 Nov 2015
-- Description:	Get Company User Email IDs
-- =============================================
CREATE PROCEDURE [dbo].[P_CA_GetCompanyUserEmailIds] 
	
	@CompanyId INT
AS
BEGIN
	SELECT
	COMPANYUSER.UserId, 
	COMPANYUSER.EMail
    
FROM 
T_CompanyUser AS COMPANYUSER
WHERE COMPANYUSER.CompanyId = @CompanyId

END
