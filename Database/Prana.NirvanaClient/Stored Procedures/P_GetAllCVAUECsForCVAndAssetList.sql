
/****** Object:  Stored Procedure dbo.P_GetAllCVAUECsForCVAndAssetList    Script Date: 10/18/2007 8:42:22 PM ******/          
CREATE PROCEDURE [dbo].[P_GetAllCVAUECsForCVAndAssetList] (          
  @assetList varchar(200),    
  @cvID int            
 )          
AS          
           
  exec ('Select 0, 0, CVAUEC.AUECID, AssetID, UnderLyingID, ExchangeID, BaseCurrencyID, 0 FROM T_CVAUEC CVAUEC inner join T_AUEC AUEC ON CVAUEC.AUECID = AUEC.AUECID Where CVAUEC.AuecID in (Select AuecID FROM T_AUEC WHERE convert(varchar(5), AssetID) in ('
  
 + @assetList + ')) AND CounterPartyVenueID = ' + @cvID + '')
