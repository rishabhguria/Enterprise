CREATE PROCEDURE [dbo].[P_GetKeyValuePairSM] As                    
BEGIN                    
SELECT CurrencyID, CurrencySymbol from T_Currency      
select AUECId, AssetId from T_AUEC  
SELECT AUECID,AssetID,UnderlyingID,ExchangeID,BaseCurrencyID,OtherCurrencyID from T_AUEC   
SELECT AUECID, ExchangeIdentifier,T_Asset.AssetName from T_AUEC join T_Asset on T_AUEC.AssetID =  T_Asset.AssetID       
END
