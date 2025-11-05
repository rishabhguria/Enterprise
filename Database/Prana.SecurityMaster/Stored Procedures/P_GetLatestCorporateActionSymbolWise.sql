/*
 Author : Ishant Kathuria                                             
 Description : It provides the latest corporateactio on the symbol 
 Modified By Sandeep Singh, Date: Mar 15, 2014
 Desc: check IsNull(NewSymbol, Symbol) is removed, now only Symbol is picking
 Reason: Same day two different CA applied on same symbol i.e.Say on Mar 15, 2014, Symbol:DELL, CA:SPLIT, Effective Date: May-12-2014.
 Same Symbol DELL, CA: Name Change to new Symbol say DELL NEW, Effective Date: May-15-2014.
 Now Undo CA, query "IsNull(NewSymbol, Symbol) as Symbol" will return 'DELL NEW' and 'DELL'. After that 
 Delete from #TempCA where symbol not in (Select Items from dbo.Split(@symbols,',')) will delete 'DELL NEW' and 'DELL' will be selected
 'DELL' has only one row that is for SPLIT. So SPLIT will be undone first and/Or there will be wrong message on UI. 
 This action has been taken and SP modified
*/
CREATE PROCEDURE [dbo].[P_GetLatestCorporateActionSymbolWise]                                                  
(                                                                                                                        
 @symbols varchar(max)                                                                
)                                                                                                                        
As                                                                                                                            
Begin                                                                                                            
  
--Select CorpActionID, CorpActionTypeId, IsNull(NewSymbol, Symbol) as Symbol, UTCInsertiontime into #TempCA   
--from v_corpactiondata where IsApplied = 'true'  
--order by UTCInsertiontime desc

Select CorpActionID, CorpActionTypeId,  Symbol as Symbol, UTCInsertiontime into #TempCA   
from v_corpactiondata where IsApplied = 'true'  
order by UTCInsertiontime desc  
    
Delete from #TempCA where symbol not in (Select Items from dbo.Split(@symbols,','))  
  
Select TempCA.CorpActionID, TempCA.CorpActionTypeId, TempCA.Symbol, TempCA.UTCInsertiontime   
from #TempCA as TempCA inner join    
(Select Symbol, Max(UTCInsertionTime) as UTCInsertionTime from #TempCA Group by Symbol) as LatestCA  
on TempCA.Symbol =  LatestCA.Symbol and TempCA.UTCInsertionTime =  LatestCA.UTCInsertionTime  
  
Drop table #TempCA  
                                                                                                   
End   
  
