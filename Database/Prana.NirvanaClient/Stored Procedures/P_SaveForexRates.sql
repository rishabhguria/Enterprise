--- Author : Rajat  
---Date : 27 Sep 2006
--- Updates the new values of forex conversion factor corresponding to "from and to currency ids"
CREATE procedure [dbo].[P_SaveForexRates]  
(  
@fromCurrencyID int,  
@toCurrencyID int, 
@conversionFactor float  
)  
as  

update T_CurrencyConversion  
set  
ConversionFactor = @conversionFactor
where  
FromCurrencyID = @fromCurrencyID and ToCurrencyId = @toCurrencyID 