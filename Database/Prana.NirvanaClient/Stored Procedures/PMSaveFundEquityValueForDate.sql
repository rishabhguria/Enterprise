

/*  
Comment: Not used for now. As this functionality is used to generate DailyEquityValueReport which is not asked for in the Yunzei release.  

Author  : Rajat  
Date  : 04 Nov 2007  
Description : It saves the day's current equity value  
  
*/  
  
CREATE PROCEDURE [dbo].[PMSaveFundEquityValueForDate] (  
 @date datetime,  
 @companyId int  
)  
AS  
Begin  
  
 Declare @yesterdaysEquityValue float   
 Declare @dayPnlForDate float   
     
 select @dayPnlForDate = DayPNL from PM_CompanyFundDailyPNL where dbo.GetFormattedDatePart(Date) = dbo.GetFormattedDatePart(@date)  
  
 SET @yesterdaysEquityValue = (Select Top(1) EquityValue      
     from PM_CompanyDailyEquityValue       
     where DATEDIFF(day, @date,date) = -1)      
       
 if(@yesterdaysEquityValue IS NULL)      
 begin      
  set @yesterdaysEquityValue = (Select BaseEquityValue FROM PM_CompanyBaseEquityValue WHERE CompanyID = @companyId)      
 end      
            
 -- TODO : Need to check if @yesterdaysEquityValue is still null, then give message to user saying "Please enter the base equity value"  
  
 if exists(select * from PM_CompanyDailyEquityValue where dbo.GetFormattedDatePart(Date) = dbo.GetFormattedDatePart(@date) AND PM_CompanyDailyEquityValue.CompanyID = @companyID)  
  Begin  
   Update PM_CompanyDailyEquityValue   
   SET  PM_CompanyDailyEquityValue.EquityValue = (ISNULL(@yesterdaysEquityValue,0) + IsNull(@dayPnlForDate,0))     
   WHERE dbo.GetFormattedDatePart(Date) = dbo.GetFormattedDatePart(@date) AND PM_CompanyDailyEquityValue.CompanyID = @companyID      
  End  
 else  
  Begin  
   Insert into PM_CompanyDailyEquityValue(Date,CompanyID,EquityValue)  
   values (@date,@companyId,(ISNULL(@yesterdaysEquityValue,0) + IsNull(@dayPnlForDate,0)))  
  End  
  
END
