

-- =============================================  
-- Author:  Sumit Kakra  
-- Create date: 12-Jan-2009  
-- Description: Returns Last Business Day based on DefaultAUECID. Read from T_Company table 
/*  
 Usage:   
  Exec P_GetLastBusinessDay  
  
*/  
-- =============================================  
CREATE PROCEDURE [dbo].[P_GetLastBusinessDay]  
 -- Add the parameters for the stored procedure here  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 DECLARE @companyID int
 SET @companyID = (select top 1 CompanyID from T_Company)

DECLARE @auecID int
 SET @auecID = (SELECT DefaultAUECID FROM T_Company WHERE CompanyID = @companyID)

 SELECT Convert(datetime,dbo.GetFormattedDatePart(dbo.AdjustBusinessDays(GetDate(),-1,@auecID))) As LastBusinessDay  
END  

