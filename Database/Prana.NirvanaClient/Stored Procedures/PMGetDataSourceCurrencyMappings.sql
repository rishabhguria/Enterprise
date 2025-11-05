  
-- =============================================  
-- Author:  Rajat  
-- Create date: 23 Nov 2006  
-- Description: Get all of the Currency mappings for a datasource  
-- =============================================  
CREATE PROCEDURE PMGetDataSourceCurrencyMappings  
  
 @ThirdPartyID int   
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT first.DataSourceCurrencyID, first.DataSourceCurrencyName, first.ApplicationCurrencyId, second.CurrencySymbol as ApplicationCurrencyName, second.CurrencyName as ApplicationCurrencyFullName  
 from PM_DataSourceCurrencies first inner join T_Currency second on first.ApplicationCurrencyID = second.CurrencyID   
 where first.ThirdPartyID = @ThirdPartyID  
   
END  
  
  
  