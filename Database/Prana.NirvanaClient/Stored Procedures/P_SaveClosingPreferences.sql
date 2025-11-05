/*
   Friday, September 05, 20142:58:02 PM
   Created by: Omshiv
   Purpose: Added QtyRoundOffDigits, PriceRoundOffDigits for saving in T_GlobalClosingPreferences
*/

CREATE procedure [dbo].[P_SaveClosingPreferences]                  
(                  
@accMethodData varbinary(max),        
@isShortWithBuyandBuyToClose bit,        
@isSellWithBuyToClose bit,        
@overrideGlobal bit,        
@GlobalClosingAlgo int ,      
@SecondarySort int,    
@GlobalClosingMethodology int,
@isFetchDataAutomatically  bit,
@longTermTaxRate float,
@shortTermTaxRate float,
@QtyRoundOffDigits int,
@PriceRoundOffDigits int,
@IsAutoCloseStrategy int,
@GlobalClosingField int,
@SplitunderlyingBasedOnPosition bit,
@AutoOptExerciseValue float,
@CopyOpeningTradeAttributes bit
)                  
as                  
           
if((select count(*) from T_GlobalClosingPreferences) > 0 )               
begin           
update T_GlobalClosingPreferences          
set           
AccountingMethodData = @accMethodData,        
IsShortWithBuyandBuyToClose = @isShortWithBuyandBuyToClose,        
IsSellWithBuyToClose = @isSellWithBuyToClose,        
OverrideGlobal =@overrideGlobal,        
GlobalClosingAlgo=@GlobalClosingAlgo,        
SecondarySort =@SecondarySort,      
GlobalClosingMethodology = @GlobalClosingMethodology,
IsFetchDataAutomatically=@isFetchDataAutomatically,  
LongTermTaxRate=@longTermTaxRate,  
ShortTermTaxRate=@shortTermTaxRate,
QtyRoundOffDigits=@QtyRoundOffDigits,  
PriceRoundOffDigits=@PriceRoundOffDigits,
IsAutoCloseStrategy=@IsAutoCloseStrategy,
GlobalClosingField=@GlobalClosingField,
SplitunderlyingBasedOnPosition=@SplitunderlyingBasedOnPosition,
AutoOptExerciseValue=@AutoOptExerciseValue,
CopyOpeningTradeAttributes=@CopyOpeningTradeAttributes
end          
else          
begin          
insert into T_GlobalClosingPreferences(AccountingMethodData,IsShortWithBuyandBuyToClose,IsSellWithBuyToClose,OverrideGlobal,GlobalClosingAlgo,SecondarySort,GlobalClosingMethodology,IsFetchDataAutomatically,LongTermTaxRate,ShortTermTaxRate,QtyRoundOffDigits,PriceRoundOffDigits,IsAutoCloseStrategy,GlobalClosingField,SplitunderlyingBasedOnPosition,AutoOptExerciseValue)                   
values(@accMethodData,@isShortWithBuyandBuyToClose,@isSellWithBuyToClose,@overrideGlobal,@GlobalClosingAlgo,@SecondarySort,@GlobalClosingMethodology,@isFetchDataAutomatically,@longTermTaxRate,@shortTermTaxRate,@QtyRoundOffDigits,@PriceRoundOffDigits,@IsAutoCloseStrategy,@GlobalClosingField,@SplitunderlyingBasedOnPosition,@AutoOptExerciseValue)           
end          
  
