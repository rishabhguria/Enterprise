



/****** Object:  Stored Procedure dbo.P_GetAUECName    Script Date: 03/21/2006 4:00:23 PM ******/
CREATE PROCEDURE [dbo].[P_GetAUECName]	
(
	@auecID int		
)
AS

Declare @assetName varchar(50), @underLyingName varchar(50), @exchangeName varchar(50)
Select @assetName = AssetName, @underLyingName = UnderLyingName, @exchangeName = T_Exchange.DisplayName
From T_Asset, T_Underlying, T_Exchange, T_AUEC 
Where T_AUEC.AUECID = @auecID
And T_AUEC.AssetID  = T_Asset.AssetID
And T_AUEC.UnderlyingID  = T_Underlying.UnderlyingID
And T_AUEC.ExchangeID  = T_Exchange.ExchangeID
Select @assetName, @underLyingName , @exchangeName

	/*Declare @assetID int, @underLyingID int, @auecExchangeID int, @exchangeID int
	
	Select @assetID = assetID, @underLyingID = underlyingID, @auecExchangeID = AUECExchangeID
	From T_AUEC
	Where auecID = @auecID
	
	Select @exchangeID = ExchangeID From T_AUECExchange Where AUECExchangeID = @auecExchangeID
	
	Declare @assetName varchar(50), @underLyingName varchar(50), @exchangeName varchar(50)
	
	Select @assetName = AssetName From T_Asset Where AssetID = @assetID
	
	Select @underLyingName = UnderLyingName From T_UnderLying Where UnderLyingID = @underLyingID
	
	Select @exchangeName = DisplayName From T_Exchange Where ExchangeID = @exchangeID
	
	Select @assetName, @underLyingName, @exchangeName*/
	
	
	
	/* select e.DisplayName, b.AssetName, c.UnderLyingName from 
T_AUEC a inner join T_Asset b on a.AssetId = b.AssetID
inner join T_UnderLying c on a.UnderLyingID = c.UnderLyingID
inner join T_AUECExchange d on a.AUECExchangeID = d.AUECExchangeID
inner join T_Exchange e on d.ExchangeID = e.ExchangeID
where a.AUECID = 51 */



