CREATE PROCEDURE [dbo].[PMGetEquityValueForCompanyAndDay] (                                                
  @CompanyID int,                                                
  @Date Datetime                                                 
 )  
AS
	SELECT 	
		EquityValue
	FROM 
		PM_CompanyDailyEquityValue
	WHERE
		CompanyID = @CompanyID
		AND
		DATEADD(day, DATEDIFF(day, 0, Date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0) 
