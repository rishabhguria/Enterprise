
Create PROCEDURE [dbo].[P_DeleteMarketDataTypesForUser]
(
		@companyUserID int		
)
AS
	delete from T_CompanyUserMarketDataTypes where companyUserID = @companyUserID
