CREATE PROCEDURE [dbo].[PMGetCompanyBaseEquityValues]
	
AS
BEGIN
	
Select CompanyBaseEquityValueID,CompanyID,BaseEquityValue,
 BaseEquityDate from PM_CompanyBaseEquityValue
	
END
