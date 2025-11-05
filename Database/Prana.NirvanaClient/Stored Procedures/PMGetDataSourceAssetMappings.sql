  
-- =============================================  
-- Author:  Rajat  
-- Create date: 21 Nov 2006  
-- Description: Get all of the assets mapping for a datasource  
-- =============================================  
CREATE PROCEDURE PMGetDataSourceAssetMappings  
  
 @ThirdPartyID int   
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT first.DataSourceAssetID, first.DataSourceAssetName, first.ApplicationAssetId, second.AssetName as ApplicationAssetName  
 from PM_DataSourceAssets first inner join T_Asset second on first.ApplicationAssetID = second.AssetID   
 where first.ThirdPartyID = @ThirdPartyID  
   
END  
  
  
  