

CREATE PROCEDURE [dbo].[P_GetKeyValuePairAUECs] AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 SELECT   
 AUECID,   
 AssetID,   
 UnderlyingID,   
 ExchangeID,   
 BaseCurrencyID,  
 OtherCurrencyID  
  
 from T_AUEC    
  
END
