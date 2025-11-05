CREATE PROCEDURE dbo.P_GetCompanyAUECs

	(
		@CompanyID int
		
	)

as
SELECT   AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, DisplayName
FROM         T_AUEC 
where AUECID in
(select AUECID from  T_CompanyAUEC where CompanyID=@CompanyID) order by AssetID, UnderLyingID