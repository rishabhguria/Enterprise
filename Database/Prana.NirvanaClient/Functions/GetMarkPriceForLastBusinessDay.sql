
-- ==============================================================                           
-- Description: This function returns a table with Mark Prices 
-- for all symbols based on prices available on last business day.                       
-- select * from dbo.[GetMarkPriceForLastBusinessDay]()            
-- ============================================================== 
            
CREATE FUNCTION [dbo].[GetMarkPriceForLastBusinessDay]              
(                            
)              
RETURNS @DayMarkPrices TABLE                                 
(  
 Symbol varchar(50),                            
 FinalMarkPrice float,    
 FundID int     
)    
                                  
AS                                
BEGIN      
          
;WITH Date(Symbol, MaxDateForAvailablePrice)
AS
(
SELECT Symbol,
CASE WHEN DATEDIFF(d, MAX(Date), GETDATE()) = 0 
     THEN DATEADD(dd, -1, MAX(Date))
	 ELSE MAX(Date)
	 END AS MaxDateForAvailablePrice 
FROM PM_DayMarkPrice WHERE FinalMarkPrice IS NOT NULL AND FinalMarkPrice<>0

GROUP BY Symbol
)

INSERT INTO @DayMarkPrices 
select DISTINCT Date.Symbol, FinalMarkPrice, FundID FROM PM_DayMarkPrice
inner JOIN Date ON Date.Symbol = PM_DayMarkPrice.Symbol
WHERE PM_DayMarkPrice.Date = Date.MaxDateForAvailablePrice               
              
RETURN               

END   
