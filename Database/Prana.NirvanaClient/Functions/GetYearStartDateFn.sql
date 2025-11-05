/*
 Select dbo.GetYearStartDateFn()
*/

CREATE FUNCTION GetYearStartDateFn()
RETURNS DATETIME AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  

 declare @YearStartDate datetime  
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

 Set @CurrentDate = (Select dbo.AdjustBusinessDays(GetDate(),-1,@auecID))
 Set @CurrentMonth = Month(@CurrentDate)  
 Set @CurrentYear =  Year(@CurrentDate)  
 SELECT @YearStartDate = dbo.AdjustBusinessDays(
		dbo.AdjustBusinessDays(Convert(dateTime,'1-1-' + Convert(varchar(4),@CurrentYear)),-1,@auecID) 
	,1,@auecID)
 
 Return @YearStartDate
END

