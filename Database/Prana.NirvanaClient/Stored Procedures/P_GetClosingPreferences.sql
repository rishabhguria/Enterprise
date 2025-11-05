/*
   Friday, September 05, 20142:58:02 PM
   Created by: Omshiv
   Purpose: Added QtyRoundOffDigits, PriceRoundOffDigits for show rounded data up to specified decimal places 
   on closing UI.
*/

CREATE PROCEDURE [dbo].[P_GetClosingPreferences]   
as                    
select  AccountingMethodData,
IsShortWithBuyandBuyToClose,
IsSellWithBuyToClose,
OverrideGlobal,
GlobalClosingAlgo,
SecondarySort,
GlobalClosingMethodology,
IsFetchDataAutomatically,
LongTermTaxRate,
ShortTermTaxRate,
QtyRoundOffDigits,
PriceRoundOffDigits,
IsAutoCloseStrategy,
GlobalClosingField,
SplitunderlyingBasedOnPosition,
AutoOptExerciseValue,
CopyOpeningTradeAttributes
from T_GlobalClosingPreferences      

