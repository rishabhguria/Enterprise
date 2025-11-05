

/****** 
	Created by : Ram Shankar Yadav
	Desc : Get specified AUEC details with respective Names
	Date : 30 Jan 2007
 ******/
CREATE PROCEDURE [dbo].[P_GetAUECDetailsForAUECID]
( @AUECID int)
AS
SELECT   
	auec.AUECID, 
	auec.DisplayName,
	auec.AssetID, 
	asset.AssetName,
	auec.UnderLyingID,
	underlying.UnderlyingName,
	auec.ExchangeID, 
	exchange.DisplayName,
	auec.BaseCurrencyID,	
	currency.CurrencyName,
	currency.CurrencySymbol	
FROM         
	T_AUEC as auec,
	T_ASSET as asset,
	T_Currency as currency,
	T_Underlying as underlying,
	T_Exchange as exchange
Where
	auec.AssetID = asset.AssetID AND
	auec.UnderLyingID = underlying.UnderLyingID AND
	auec.ExchangeID = exchange.ExchangeID AND
	auec.BaseCurrencyID = currency.CurrencyID AND
	auec.AUECID = @AUECID
order by 
	auec.AssetID, auec.UnderLyingID, auec.ExchangeID, BaseCurrencyID
