
-- =============================================    
-- Modified By  :  Rajat    
-- Modified Date: 11-01-2008    
-- Description  : Gets the conversion factor from the supplied auec's currencyid(Traded Auec id) to the base currency     
--    for a particular date and for supplied company,  
--    Important : Returns 1 if conversion factor is not available  
-- =============================================    
CREATE FUNCTION [dbo].[GetConversionFactor] (    
 @CompanyId int ,    
 @TradedCurrencyID int,  
 @Date datetime      
)    
RETURNS float    
AS    
BEGIN    
    
    
declare @var float    
set @var = null    
set @var =    
(    
SELECT     
    
 CC.ConversionFactor as ConversionFactor     
    
FROM       
  T_CURRENCYCONVERSION CC      
join     
T_COMPANY C1 on C1.BaseCurrencyID=CC.FROMCURRENCYID    
 
WHERE      
 C1.CompanyID = @CompanyId and CC.ToCurrencyID =@TradedCurrencyID  and dbo.GetFormattedDatePart(Date) = dbo.GetFormattedDatePart(@Date)   
)    
     
RETURN ( isnull(@var,1))    
END
