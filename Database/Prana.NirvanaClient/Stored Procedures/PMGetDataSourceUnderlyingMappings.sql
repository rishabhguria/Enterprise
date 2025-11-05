-- =============================================  
-- Author:  Rajat  
-- Create date: 23 Nov 2006  
-- Description: Get all of the Underlying mappings for a datasource  
-- =============================================  
CREATE PROCEDURE PMGetDataSourceUnderlyingMappings  
  
 @ThirdPartyID int   
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT first.DataSourceUnderlyingID, first.DataSourceUnderlyingName, first.ApplicationUnderlyingId, second.UnderlyingName as ApplicationUnderlyingName  
 from PM_DataSourceUnderlyings first inner join T_Underlying second on first.ApplicationUnderlyingID = second.UnderlyingID   
 where first.ThirdPartyID = @ThirdPartyID  
   
END  
  
  
  