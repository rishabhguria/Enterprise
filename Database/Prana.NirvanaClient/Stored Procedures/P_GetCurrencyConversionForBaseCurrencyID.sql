

/****************************************************************************                
Date Modified: 18-Feb-2008                 
Purpose:  It gets the tocurrencyid and conversionfactor for the supplied date and the                 
   fromcurrencyid (which is basecurrencyid)      
Author: Rajat Tandon        
      
Date Modified: <08-Jan-2008>                 
Description:     <Get the Data Datewise>                 
Modified By:     <Sandeep Singh>                
      
Name :   [P_GetCurrencyConversionForBaseCurrencyID]                
Date Created: 4-Dec-2006                 
Purpose:  Get currency conversion for specified base currency                
Author: Ram Shankar Yadav                
Select * from T_CurrencyConversion        
        
Parameters:                 
  @BaseCurrencyID int                
                
Execution StateMent:                 
                   
P_GetCurrencyConversionForBaseCurrencyID '2008-03-08'               
Select * from  Temp        
****************************************************************************/                
              
CREATE PROCEDURE [dbo].[P_GetCurrencyConversionForBaseCurrencyID] (                
 @Date DateTime             
)                
AS         
Create table #Temp      
(      
 ToCurrencyID int,    
 FromCurrencyID int,      
 Date datetime      
)      
      
--insert into #Temp      
--select ToCurrencyID,FromCurrencyID max(Date) from T_CurrencyStandardPairs     
--Inner Join T_CurrencyConversionRate on     
--T_CurrencyStandardPairs.CurrencyPairID=T_CurrencyConversionRate.CurrencyPairID_FK    
--where FromCurrencyID = @BaseCurrencyID and dbo.GetFormattedDatePart(Date) <= dbo.GetFormattedDatePart(@Date)      
--group by ToCurrencyID        
--      
--select C.ToCurrencyID, CCR.ConversionRate from T_CurrencyStandardPairs C      
--inner join #Temp temp on C.ToCurrencyID = temp.ToCurrencyID    
--inner join  T_CurrencyConversionRate CCR on C.CurrencyPairID=CCR.CurrencyPairID_FK    
--and CCR.Date = temp.Date      
--where C.FromCurrencyID = @BaseCurrencyID      
    
insert into #Temp      
select ToCurrencyID,FromCurrencyID, max(Date) from T_CurrencyStandardPairs     
Inner Join T_CurrencyConversionRate on     
T_CurrencyStandardPairs.CurrencyPairID=T_CurrencyConversionRate.CurrencyPairID_FK    
where Datediff(d,Date,@Date) = 0
group by ToCurrencyID,FromCurrencyID        
      
--select C.FromCurrencyID,C.ToCurrencyID, CCR.ConversionRate ,CCR.Date from T_CurrencyStandardPairs C      
--inner join #Temp temp on C.ToCurrencyID = temp.ToCurrencyID    
--inner join  T_CurrencyConversionRate CCR on C.CurrencyPairID=CCR.CurrencyPairID_FK    
--and CCR.Date = temp.Date      
    
select C.FromCurrencyID,C.ToCurrencyID, CCR.ConversionRate ,CCR.Date, eSignalSymbol  from T_CurrencyStandardPairs C      
inner join  T_CurrencyConversionRate CCR on C.CurrencyPairID=CCR.CurrencyPairID_FK    
inner join #Temp temp on C.FromCurrencyID = temp.FromCurrencyID and  C.ToCurrencyID=temp.ToCurrencyID   
where CCR.Date=temp.Date    
          
drop table #Temp

