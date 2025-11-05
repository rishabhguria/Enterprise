
/****** Object:  Stored Procedure dbo.GetAllCVAUECsForCompanyAndAssetList    Script Date: 10/15/2007 8:32:22 PM ******/        
CREATE PROCEDURE [dbo].[P_GetAllCVAUECsForAssetList] (        
  @assetList varchar(200)          
 )        
AS        
         
  exec ('Select AUECID, 0, 0, 0, 0, 0, 0, 0 FROM T_CVAUEC Where AuecID in (Select AuecID FROM T_AUEC WHERE convert(varchar(5), AssetID) in (' + @assetList + '))')
