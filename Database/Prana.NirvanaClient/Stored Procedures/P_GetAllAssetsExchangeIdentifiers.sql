
--=============================================        
-- Author:  Rajat      
-- Create date: <Create Date,,>        
-- Description: Provides the exchangeid and displayname      
--=============================================        
CREATE PROCEDURE [dbo].[P_GetAllAssetsExchangeIdentifiers] AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;        
        
    -- Insert statements for procedure here        
 SELECT AssetID, ExchangeIdentifier
--,T_Asset.AssetName 
from T_AUEC
-- join T_Asset on T_AUEC.AssetID =  T_Asset.AssetID     
END
