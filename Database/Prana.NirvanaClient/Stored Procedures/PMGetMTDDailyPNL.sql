

/****************************************************************************    
Name :   PMGetMTDDailyPNL    
Date Created: 06-June-2007     
Purpose:  Gets the Month till date Strategy wise PNL from the DB.    
Author: Bhupesh Bareja    
Parameters:     
  @companyID int,    
  @userID int,    
  @date dateTime,    
  @ErrorMessage varchar(500)     
  , @ErrorNumber int      
Execution StateMent:     
   EXEC [PMGetMTDDailyPNL] 1, 1, '22-03-07', ' ' , 0     
    
Date Modified:     
Description:       
Modified By:       
****************************************************************************/    
CREATE PROCEDURE [dbo].[PMGetMTDDailyPNL] (    
  @companyID int,    
  @userID int,    
  @date datetime,    
  @ErrorMessage varchar(500) output,    
  @ErrorNumber int output    
      
 )    
AS    
    
SET @ErrorMessage = 'Success'    
SET @ErrorNumber = 0    
    
BEGIN TRY    
 Select EquityValue AS TotalValue, (ISNULL(CDEV.ApplicationRealizedPL, 0) +     
   ISNULL(CDEV.ApplicationUnrealizedPL, 0)) AS PNLGross,    
   round(((((ISNULL(CDEV.ApplicationRealizedPL, 0) + ISNULL(CDEV.ApplicationUnrealizedPL, 0))/EquityValue) * 100)), 2) AS PercentChange,    
   CAST(DAY(CDEV.Date) AS VARCHAR(2)) + ' ' + DATENAME(MM, CDEV.Date) AS Date,    
   TCS.StrategyName AS StrategyName,    
   --(ISNULL(CSDP.ApplicationRealizedPNL, 0) + (ISNULL(CSDP.ApplicationUnRealizedPNL, 0))) AS PNLGrossStrategy,    
   (ISNULL(CSDP.DayPNL, 0)) AS PNLGrossStrategy,    
   Logo As LogoImage    
   from T_CompanyLogo, PM_CompanyDailyEquityValue CDEV left outer join PM_CompanyStrategyDailyPNL CSDP on     
   CONVERT(VARCHAR(8), CDEV.Date, 5) = CONVERT(VARCHAR(8), CSDP.Date, 5) inner join     
   T_CompanyStrategy TCS on CSDP.StrategyID = TCS.CompanyStrategyID     
     Where CONVERT(VARCHAR(5), CDEV.Date, 11) = CONVERT(VARCHAR(5), @date, 11) AND    
      CONVERT(VARCHAR(5), CSDP.Date, 11) = CONVERT(VARCHAR(5), @date, 11)     
       order by CDEV.Date    
     
     
    
    
    
    
    
    
    
    
    
    
    
    
    
    
--Select ApplicationRealizedPNL, ApplicationUnrealizedPNL     
 -- from PM_CompanyStrategyDailyPNL PMCSDP    
  --Where Month = @month    
    
END TRY    
BEGIN CATCH     
      
 SET @ErrorMessage = ERROR_MESSAGE();    
 SET @ErrorNumber = Error_number();     
END CATCH;
