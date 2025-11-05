CREATE PROCEDURE [dbo].[P_GetFundWiseExecutingBroker]
@companyID int
AS
	SELECT FundId,
	BrokerId
	FROM T_FundWiseExecutingBroker
	WHERE CompanyId = @companyID