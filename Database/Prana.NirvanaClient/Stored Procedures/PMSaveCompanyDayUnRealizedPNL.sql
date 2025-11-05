

CREATE PROCEDURE [dbo].[PMSaveCompanyDayUnRealizedPNL] (                                                  
  @CompanyID int,                                                  
  @Date Datetime,   
  @UnRealizedPNL float  
 )    
AS  
 SELECT   
  EquityValue, ApplicationUnRealizedPL   
 FROM   
  PM_CompanyDailyEquityValue  
 WHERE   
   companyID = @companyID  
   AND   
   DATEADD(day, DATEDIFF(day, 0, Date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0);  
     
 IF(@@rowcount = 0)    
 BEGIN  
  Declare @yesterdaysEquityValue float;   
  SET @yesterdaysEquityValue  =   
          (SELECT   
             EquityValue   
			FROM    
             PM_CompanyDailyEquityValue   
           WHERE   
           companyID = @CompanyID   
           AND   
           DATEADD(day, DateDiff(Day, 0, date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0)  
          )  
          
  IF  (@yesterdaysEquityValue is null)   
   BEGIN   
    set @yesterdaysEquityValue = (select baseequityvalue from   
               pm_companybaseequityvalue   
              where companyid = @companyID)  
   END  
  declare @yesterdaysUnrealizedPNL float;
	SET @yesterdaysUnrealizedPNL  =   
          (SELECT   
             ApplicationUnrealizedPL   
          FROM    
             PM_CompanyDailyEquityValue   
           WHERE   
           companyID = @CompanyID   
           AND   
           DATEADD(day, DateDiff(Day, 0, date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0)  
          ) 
	declare @tempEquityValue float
	set @tempEquityValue = 0
	set @tempEquityValue = ISNULL(@yesterdaysEquityValue, 0) + ISNULL(@UnRealizedPNL, 0) - ISNULL(@yesterdaysUnrealizedPNL, 0)
  INSERT INTO   
    PM_CompanyDailyEquityValue (Date, ApplicationUNRealizedPL, CompanyID, EquityValue)   
  VALUES   
	(DATEADD(day, DATEDIFF(day, 0, @date), 0), @UnRealizedPNL, @CompanyID, @tempEquityValue)  
 END  
 ELSE  
  BEGIN  
   UPDATE PM_CompanyDailyEquityValue   
     SET ApplicationUnRealizedPL = ApplicationUnRealizedPL + @UnRealizedPNL , EquityValue = ISNULL(EquityValue, 0) + @UnRealizedPNL  
   WHERE  
    companyID = @companyID  
   AND   
   DATEADD(day, DATEDIFF(day, 0, Date), 0) = DATEADD(day, DATEDIFF(day, 0, @date), 0);  
  END  

