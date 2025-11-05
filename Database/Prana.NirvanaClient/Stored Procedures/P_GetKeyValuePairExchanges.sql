-- =============================================    
-- Author:  Ashish    
-- Create date: <Create Date,,>    
-- Description: Provides the exchangeid and displayname  
-- Modification : Rajat 09 Oct 2005. From ExchangeID, DisplayName to ExchangeID,ExchangeIdentifier  
-- =============================================    
CREATE PROCEDURE P_GetKeyValuePairExchanges    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
    -- Insert statements for procedure here    
 SELECT ExchangeID, displayname from T_Exchange    
END
