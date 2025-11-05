/****** Object:  Stored Procedure dbo.P_GetAllAssets    Script Date: 11/17/2005 9:50:21 AM

Created By : Pooja Porwal
Description : ADD Equity Swap as separate Asset Class
Jira :http://jira.nirvanasolutions.com:8080/browse/PRANA-12757
******/   

CREATE PROCEDURE dbo.P_GetAllAssets_IncludingEquitySwap  
AS  

Declare @EquitySwapId Int    
select  @EquitySwapId=MAX(AssetId)+1 from T_Asset     

 Select AssetID, AssetName, Comment  
 From T_Asset  

 UNION ALL  
  SELECT @EquitySwapId, 'EquitySwap', ''   

order by Assetname
  
  
  