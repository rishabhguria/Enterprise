
-- =============================================    
-- Author:  Ashish Poddar    
-- Create date: Create Date, 28 Jan, 2008    
-- Description: To fetch the most recent Conversion Factors (closest to the date specified) for all currencies available in the database.    
-- Let's say you want to fetch the conversion factor for date 25thJan.     
-- If one of the currency conversion factors is zero then this sp will find the data for closest previous date for which non-zero value is avaialble.     
     
/* Methodology: Get the list of currencies and the maximum date for which a Non-Zero conversion factor     
  is available in the database.    
  Then join that list again with the Currency conversion table and select the conversion factor    
  for these currencies.  */    
-- =============================================    
CREATE FUNCTION [dbo].[GetNonZeroConversionFactorForDate] (     
 @date datetime    
)    
RETURNS TABLE     
AS    
RETURN     
(    
    
Select     
 Temp.FCID as FCID,    
 Temp.TCID as TCID,    
 Temp.DateNew as DateCC,    
 CC.ConversionRate FROM  
 (Select CSP.FromCurrencyID,CSP.ToCurrencyID,CSR.ConversionRate, CSR.Date  
from T_CurrencyStandardPairs CSP INNER JOIN T_CurrencyConversionRate CSR   
 ON CSP.CurrencyPairID = CSR.CurrencyPairID_FK) AS CC     
join     
 ( /* Get the list of currencies and the maximum date for which a Non-Zero conversion factor     
  is available in the database.    
  Then join that list again with the Currency conversion table and select the conversion factor    
  for these currencies.  */    
  SELECT FromCurrencyID AS FCID     
    ,TocurrencyID as TCID    
    ,max(Date)as DateNew    
  from T_CurrencyStandardPairs CSP INNER JOIN T_CurrencyConversionRate CSR   
 ON CSP.CurrencyPairID = CSR.CurrencyPairID_FK     
  where ConversionRate > 0 and Date <= @date    
  group by fromcurrencyid, TocurrencyID )    
  as Temp     
    
on Temp.FCID = CC.FromCurrencyID and Temp.TCID = cc.ToCurrencyID and Temp.DateNew = cc.Date    
    
)
