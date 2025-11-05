-- =============================================  
-- Author:  Rajat  
-- Create date: 23 Nov 2006  
-- Description: Get all of the Exchange mappings for a datasource  
-- =============================================  
CREATE PROCEDURE PMGetDataSourceExchangeMappings  
  
 @ThirdPartyID int   
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT first.DataSourceExchangeID, first.DataSourceExchangeName, first.ApplicationExchangeId, second.DisplayName as ApplicationExchangeName, second.FullName as ApplicationExchangeFullName  
 from PM_DataSourceExchanges first inner join T_Exchange second on first.ApplicationExchangeID = second.ExchangeID   
 where first.ThirdPartyID = @ThirdPartyID  
   
END  
  
  
  