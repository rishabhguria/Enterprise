  
-- =============================================    
-- Author:  Sumit Kakra    
  
-- Description: Returns Last Business Day based on DefaultAUECID. Presently Hard Coded to 1    
/*    
 Usage:     
  Exec [P_GetMonthStartForDate] '26-jan-2011'  
    
*/    
-- =============================================    
Create PROCEDURE [dbo].[P_GetMonthStartForDate]  (@date datetime)  
 -- Add the parameters for the stored procedure here    
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
     
 declare @CurrentMonth integer    
 declare @CurrentYear integer    
 declare @CurrentDate datetime    
    -- Insert statements for procedure here    
--Print @CurrentMonth    
--Print @CurrentYear    
--Print Convert(dateTime,Convert(varchar(2),@CurrentMonth) +'-1-' + Convert(varchar(4),@CurrentYear))    
  
 DECLARE @companyID int  
 SET @companyID = (select top 1 CompanyID from T_Company)  
  
 DECLARE @auecID int  
 SET @auecID = (SELECT DefaultAUECID FROM T_Company WHERE CompanyID = @companyID)  
 
/*
Made a change, corrected the @CurrentDate check from dbo.AdjustBusinessDays(@date,-1,@auecID) to dbo.AdjustBusinessDays(@date,0,@auecID)
Since we are already adjusting date once in code and here we adjust it again so month start date goes to last months date
Ankit Misra 
https://jira.nirvanasolutions.com:8443/browse/PRANA-19688
Revert it in case it is used anywhere else
*/
 Set @CurrentDate = (Select dbo.AdjustBusinessDays(@date,0,@auecID))  
 Set @CurrentMonth = Month(@CurrentDate)    
 Set @CurrentYear =  Year(@CurrentDate)    
 SELECT   
 dbo.AdjustBusinessDays(  
  dbo.AdjustBusinessDays(Convert(dateTime,Convert(varchar(2),@CurrentMonth) +'-1-' + Convert(varchar(4),@CurrentYear)),-1,@auecID)   
 ,1,@auecID)  
 As MonthStartDate    
END  