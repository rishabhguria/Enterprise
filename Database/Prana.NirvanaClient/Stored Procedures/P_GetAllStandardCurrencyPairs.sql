
-- =============================================                                                                     
-- Description: Returns standard pairs from T_CurrencyStandardPairs         
-- Usage:                         
-- Exec [P_GetAllStandardCurrencyPairs]                      
-- =============================================                         
CREATE Procedure [dbo].[P_GetAllStandardCurrencyPairs]       
    
AS                        
BEGIN                                          
                     
  Select FromCurrencyID,                        
         ToCurrencyID,                                                             
         BloombergSymbol                        
   From T_CurrencyStandardPairs
   
-- =====================================================================                                                                     
-- Description: Executing [P_GetNonExistingCurrencyPairs] within this
-- SP.It returns another Table which contains Non-Existing FX Pairs
-- in our T_CurrencyStandardPairs Table.       
-- =====================================================================   

CREATE TABLE #TT
(
  LeadCurrencyID varchar(10), VsCurrencyID varchar(10)
);

INSERT INTO #TT
EXEC P_GetNonExistingCurrencyPairs
Select * From #TT
DROP TABLE #TT
                   
END 

