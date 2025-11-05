-- =============================================                                        
-- Modified:  <Pankaj Sharma>                                
-- Create date: <1/29/2016>                                
-- Description: <Gives MTD performance values for indexes monthly>                                
-- Sample:                                  
-- select * from dbo.F_MW_MonthlySimpleRT_ALLFunds( '1/26/2016', '1218','09/30/2014','$HGX,$NDX,$RUT,$SPX,$SPXPM,$VIX')  
-- ============================================                                        
CREATE FUNCTION [dbo].[F_MW_MonthlySimpleRT_ALLFunds_Indexes] (          
 @EndDate DATETIME                                    
 ,@fundID VARCHAR(50)          
 ,@ITD Datetime          
 ,@Indexes varchar(MAX)          
 )           
                                   
-- Declare @EndDate Datetime                                        
-- Declare @fundID varchar(50)                                       
-- Declare @ITD Datetime           
-- Declare @Indexes varchar(MAX)             
--                                       
-- Set @EndDate ='12/31/2014'                                       
-- Set @fundID ='1218,1270,1271,1286'                  
-- SET @ITD='9/30/2014'          
-- Set @Indexes ='$HGX,$NDX,$RUT,$SPX,$SPXPM,$VIX'          
          
RETURNS @MonthlyRT TABLE (                                    
 CompanyFund VARCHAR(max)                                    
 ,day1 DATETIME          
 ,day2 DATETIME          
 ,Index_Symbol varchar(MAX)          
 ,IndexName varchar(max)          
 ,ToDatePrice float          
 ,FromDatePrice float          
 ,RT FLOAT          
 ,ID INT identity(1, 1)          
 )                                    
AS                                    
BEGIN                    
          
DECLARE @TempIndexes TABLE          
(          
IndexName varchar(MAX)          
)          
insert into @TempIndexes Select Items from dbo.split(@Indexes,',')          
          
--Select * from @TempIndexes          
                  
declare @fund varchar(MAX)                                            
SET @fund = ''                                            
Select @fund = @fund + Cast(FundName as varchar(50)) +',' from T_CompanyFunds CF                                     
where CF.CompanyFundID in(Select items from split(@fundID, ','))                  
                  
Set @fund = SUBSTRING(@fund, 0, LEN(@fund))                               
--print @fund                    
                                  
DECLARE @FundTable TABLE (Fund VARCHAR(max))                                    
                                    
INSERT INTO @FundTable                                    
SELECT *                                    
FROM dbo.split(@Fund, ',')                                    
              
-----------------------------------------------------------------------------------------------------------------------------------                          
------------------Date handling in the Function starts                          
-----------------------------------------------------------------------------------------------------------------------------------                          
                                    
DECLARE @Date TABLE (                                    
 Year VARCHAR(max)                                    
 ,Month VARCHAR(max)                                    
 ,Day VARCHAR(max)                                    
 ,DATE DATETIME                                    
 )                                    
                                    
--------------------------------For ITD                            
INSERT INTO @Date (Month)                                    
 SELECT number                                    
 FROM master..spt_values                                    
 WHERE type = 'P'                                    
  AND number >= Month(@ITD)                                    
  AND number < = 12  
                            
 UPDATE @Date               
 SET [Year] = (                                    
   SELECT TOP 1 number                                    
   FROM master..spt_values                                    
   WHERE type = 'P'                                    
    AND number = year(@ITD)                                    
   )                        
where Year is NULL                            
                            
                            
                 
--------------------------------For middle years                          
If(year(@ITD)-year(@EndDate) <> 0)                        
Begin                          
 Declare @Count int                  
 Select @Count = Count(*)                             
 FROM master..spt_values                                    
 WHERE type = 'P'                                    
 AND number > year(@ITD)                            
 AND number < year(@EndDate)                                                     
 While(@Count>0)                            
 Begin                            
 Declare @Year int                            
 Select @Year = Max(Year) + 1 from @Date                            
 INSERT INTO @Date (Month)                                    
  SELECT number                                    
  FROM master..spt_values                                    
  WHERE type = 'P'                                    
   AND number >= 1                                   
   AND number <= 12                                    
                             
  UPDATE @Date                                    
  SET [Year] = (                                    
    SELECT TOP 1 number                                    
    FROM master..spt_values                                    
    WHERE type = 'P'                              
  AND number = @year                            
    )                                    
 where Year is NULL                            
                             
 Set @Count = @Count-1                            
                             
 END                            
                             
                             
 --------------------------------For YTD                            
 INSERT INTO @Date (Month)                                    
  SELECT number                                    
  FROM master..spt_values                                    
  WHERE type = 'P'                                    
   AND number >= 1                            
   AND number <= Month(@EndDate)                              
                             
                                  
                             
  UPDATE @Date                                    
  SET [Year] = (                                    
    SELECT TOP 1 number                                    
    FROM master..spt_values                                    
    WHERE type = 'P'                                    
  AND number = year(@EndDate)                                   
    )                                    
 where Year is NULL                            
END                        
                                    
UPDATE @Date                                    
SET Day = CASE                                     
  WHEN Month = 1                                    
   THEN 31                                    
  WHEN Month = 2                                    
   THEN CASE                                     
     WHEN Year % 4 = 0                                    
      THEN 29                                    
     ELSE 28                                    
     END                                    
  WHEN Month = 3                                    
   THEN 31                                    
  WHEN Month = 4                                    
   THEN 30                                    
  WHEN Month = 5                                    
   THEN 31                                    
  WHEN Month = 6                                    
   THEN 30                                    
  WHEN Month = 7                                    
   THEN 31                                    
  WHEN Month = 8                                    
   THEN 31                                    
  WHEN Month = 9                                    
  THEN 30                                    
  WHEN Month = 10                                    
   THEN 31                                    
  WHEN Month = 11                                    
   THEN 30                                    
  WHEN Month = 12                                    
   THEN 31                                    
  END                      
                                    
UPDATE @Date                                    
SET DATE = (                                    
  SELECT CAST(CAST(Year AS VARCHAR) + '-' + CAST(Month AS VARCHAR) + '-' + CAST(Day AS VARCHAR) AS DATETIME)                                    
  )                                    
WHERE DATE IS NULL                  
                                    
UPDATE @Date                                    
SET DATE = @EndDate                                    
WHERE DATE = (                                    
  SELECT max(DATE)                                    
  FROM @Date                                    
  )                                    
                                    
UPDATE @Date                              
SET DATE = dbo.AdjustBusinessDays(DATE, - 1, 11)                                    
WHERE dbo.isbusinessday(DATE, 1) = 0 --set date to previous business day.    
  
Delete from @Date where Year = Year(@Enddate) and Month >Month(@Enddate)                                      
                
-----------------------------------------------------------------------------------------------------------------------------------                          
------------------Date handling in the Function ends                          
-----------------------------------------------------------------------------------------------------------------------------------                          
                          
INSERT INTO @MonthlyRT (                         
 day1----------from date      
,day2----------to date      
 ,CompanyFund          
,IndexName          
,Index_Symbol            
 )                                    
SELECT CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(DATE) - 1), DATE), 101)                                    
 ,DATE                                    
 ,Fund                                    
,IndexName+ ' MTD' as IndexName          
,IndexName          
FROM @Date                                    
CROSS JOIN @FundTable          
CROSS JOIN @TempIndexes                                    
ORDER BY DATE                                    
            
Update @MonthlyRT            
SET day1=CashMgmtStartDate from @MonthlyRT RT             
inner join T_CompanyFunds CF on CF.FundName = RT.CompanyFund            
inner JOIN T_cashPreferences CP on CP.FundID = CF.CompanyFundID and datediff(d,day1,CashMgmtStartDate)>0            
        
update @MonthlyRT set day1 = dbo.AdjustBusinessDays(day1,-1,11) where dbo. isbusinessday(day1,1)=0        
          
update @MonthlyRT set FromDatePrice=                                                        
(select [close] from [Historical].[Historicals].[dbo].dailybars                                               
where Date=day1 and Symbol=Index_Symbol)    
    
update @MonthlyRT set FromDatePrice=                                                        
(select [close] from [Historical].[Historicals].[dbo].dailybars                                               
where Date=dbo.AdjustBusinessDays(day1,-1,11) and Symbol=Index_Symbol)    
where FromDatePrice IS NULL    
      
      
update @MonthlyRT set ToDatePrice=      
(select [close] from [Historical].[Historicals].[dbo].dailybars      
where Date=day2 and Symbol=Index_Symbol)      
          
                                                        
                                                        
update @MonthlyRT set RT=ToDatePrice/FromDatePrice-1           
          
                                    
-------------------------------Update historical performance from client file--------------------------------------                                        
UPDATE @MonthlyRT                                    
SET RT = COALESCE(Performance,RT,0)          
FROM T_MW_PerformanceOnBoard                                    
WHERE DataType = IndexName                                    
 AND Datediff(d,AsofDate,day2)=0                      
 AND CompanyFund = Fund          
 and ISBenchMark = 1           
          
UPDATE @MonthlyRT                                    
SET RT = IsNull(RT,0)          
          
          
          
                                    
                                 
--Insert into @MonthlyRT                     
--(CompanyFund,day1,day2,Pnl ,ExpFee,CF ,IncentiveFee,ManagementFee,PriorEquityGross,PriorEquityNet,GrossRT,NetRT)                    
--values                    
--('Clover Street - Fund','2014-10-01 00:00:00.000','2014-10-31 00:00:00.000',0,0,0,0,0,0,0,110,220)                    
            
--UPDATE @MonthlyRT                                    
--SET day1 = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(day2) - 1), day2), 101)                                    
            
--Select * from @MonthlyRT order by indexname          
              
          
                                    
---------------------------------------------------------------------------------------------------------------                                        
 RETURN                                    
END 