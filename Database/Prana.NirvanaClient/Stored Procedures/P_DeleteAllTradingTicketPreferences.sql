
CREATE PROC P_DeleteAllTradingTicketPreferences (
 @CompanyUserID int,
 @AssetID int,
 @UnderlyingID int,
 @CounterPArtyID int,
 @VenueID int  
	)
AS
delete from T_TradingTicketPrefrencesSettings  
where CompanyUserID = @CompanyUserID  
and AssetID=@assetid
and UnderlyingID=@underlyingID
and counterpartyID=@counterpartyID
and VenueID=@VenueID
