/****** Object:  Stored Procedure dbo.P_GetCompanyAUEC    Script Date: 01/05/2005 8:25:22 PM ******/
CREATE PROCEDURE dbo.P_GetCompanyAUEC
	(
		@companyID	int	
	)
AS
	
	Select CompanyAUECID, CompanyID, CA.AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, DisplayName
	From T_CompanyAUEC CA, T_AUEC A
	Where CompanyID = @companyID And CA.AUECID = A.AUECID