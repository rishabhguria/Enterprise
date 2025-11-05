

/****************************************************************************      
Name :   PMGetMonthlySummaryValues      
Date Created: 06-June-2007       
Purpose:  Gets the Monthly Summary Values from the DB.      
Author: Bhupesh Bareja      
Parameters:       
  @companyID int,      
  @userID int,      
  @date dateTime,      
  @ErrorMessage varchar(500)       
  , @ErrorNumber int        
Execution StateMent:       
   EXEC [PMGetMonthlySummaryValues] 1, 1, '22-03-07', ' ' , 0       
      
Date Modified:       
Description:         
Modified By:         
****************************************************************************/      
CREATE PROCEDURE [dbo].[PMGetMonthlySummaryValues] (      
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
 declare @beginValue float      
 --SET @beginValue = 1      
 SET @beginValue = (Select top 1 EquityValue      
   from PM_CompanyDailyEquityValue       
    WHERE Date < (Select MAX(Date) FROM PM_CompanyDailyEquityValue Where CONVERT(VARCHAR(5), Date, 11) = CONVERT(VARCHAR(5), @date, 11)) order by Date desc)      
      
 declare @currentValue float      
 SET @currentValue = (Select top 1 EquityValue      
   from PM_CompanyDailyEquityValue Where CONVERT(VARCHAR(5), Date, 11) = CONVERT(VARCHAR(5), @date, 11)      
    order by Date desc)      
      
 declare @yesterdaysEquityValue float      
 if (@beginValue IS NULL)      
 begin      
        
  set @yesterdaysEquityValue = (select baseequityvalue from       
               pm_companybaseequityvalue       
              where companyid = @companyID)      
  set @beginValue = @yesterdaysEquityValue      
 end      
 if (@currentValue IS NULL)      
 begin      
  set @currentValue = @yesterdaysEquityValue      
 end      
       
        
       
 declare @MTDPercentChange decimal(12,4)      
 Set @MTDPercentChange = (@beginValue - @currentValue) / @beginValue * 100      
       
       
 declare @strdt varchar(6)      
 set @strdt=substring(datename(m,@date),1,3)+'%'       
 declare @accrualValue float      
 SET @accrualValue = (Select AccrualValue      
   from PM_CompanyMonthlyAccruals       
    WHERE [Month] like @strdt AND datepart(yy,@date) = Year)      
       
 --declare @accrualPercent float      
 declare @accrualPercent decimal(12,2)      
 if(@currentValue = 0)      
 begin      
  set @currentValue = 1      
  set @accrualPercent = (@accrualValue / @currentValue) * 100      
 end      
 else      
 begin      
  set @accrualPercent = (@accrualValue / @currentValue) * 100      
 end      
      
 declare @monthNetReturnPercent decimal(10,2)      
 set @monthNetReturnPercent = @MTDPercentChange * @accrualPercent      
      
 /*Select @beginValue AS BeginValue, @currentValue AS CurrentValue,       
   (cast(@MTDPercentChange as varchar(3)) + ' %') AS MTDPercentChange,      
   (cast(@accrualPercent as varchar(3)) + ' %') AS AccrualValue,      
   (cast(@monthNetReturnPercent as varchar(3)) + ' %') AS MonthNetReturn*/      
      
 declare @logo binary      
       
 Set @logo = (Select Logo FROM T_CompanyLogo)      
      
  Select @beginValue AS BeginValue, @currentValue AS CurrentValue,       
   round(@accrualPercent, 2) AS AccrualPercent,      
   round(@MTDPercentChange, 2) AS MTDPercentChange,      
   --'ab' AS MonthNetReturn      
   --@monthNetReturnPercent AS MonthNetReturn      
   round(@monthNetReturnPercent, 2) AS MonthNetReturn,      
   @accrualValue AS AccrualValue,      
   Logo FROM T_CompanyLogo      
--(cast(@MTDPercentChange as varchar(8)) + ' %') AS MTDPercentChange,      
   --from PM_CompanyMonthlyEquityValue PMCMEV      
  --Where Month = @month      
      
       
      
END TRY      
BEGIN CATCH       
        
 SET @ErrorMessage = ERROR_MESSAGE();      
 SET @ErrorNumber = Error_number();       
END CATCH;
