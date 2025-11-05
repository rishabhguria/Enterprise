CREATE PROCEDURE dbo.P_GetClientAUECs
(
		@ClientID int
		
	)

as
SELECT   AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, DisplayName
FROM         T_AUEC 
where AUECID in
(
select AUECID from  T_CompanyAUEC where CompanyAUECID in
(select CompanyAUECID from T_CompanyClientAUEC where CompanyClientID=@ClientID)


) order by AssetID, UnderLyingID, ExchangeID, BaseCurrencyID