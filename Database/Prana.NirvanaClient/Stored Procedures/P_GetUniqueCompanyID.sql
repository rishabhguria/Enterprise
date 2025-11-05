
CREATE PROCEDURE [dbo].[P_GetUniqueCompanyID]
AS
BEGIN
	select Top 1 CompanyID from T_Company where CompanyID > 0
END
