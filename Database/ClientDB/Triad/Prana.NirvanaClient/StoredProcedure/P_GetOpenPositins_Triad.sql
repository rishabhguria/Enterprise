Create Procedure [dbo].[P_GetOpenPositions_Triad]                        
(                                 
@ThirdPartyID int,                                            
@CompanyFundIDs varchar(max),                                                                                                                                                                          
@InputDate datetime,                                                                                                                                                                      
@CompanyID int,                                                                                                                                      
@AUECIDs varchar(max),                                                                            
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
@FileFormatID int                                  
)                                  
AS      
    
--Declare @InputDate DateTime    
--Set @InputDate = '06-13-2017'     
    
Select     
Fund As AccountName,    
Symbol,    
Side As PositionSide,
Sum(BeginningQuantity * SideMultiplier) As Qty,    
Sum(TotalCost_Local) As TotalCost_Local,  
Max(Multiplier) As Multiplier,    
Max(OSISymbol) As OSISymbol  
Into #TempTable    
From T_MW_GenericPNL    
Where Open_CloseTag = 'O' And Asset <> 'Cash' And DateDiff(Day,RunDate,@InputDate) = 0    
--And Fund ='MAC Fund I LP A Partnership'   
--And Symbol = 'AMSC'   
Group By Fund,Symbol,Side    
  
Alter Table #TempTable  
Add AvgPrice Float Null  
  
UPdate #TempTable  
Set AvgPrice = 0.0  
  
UPdate #TempTable  
Set AvgPrice =   
Case    
When Qty <> 0 And Multiplier <> 0    
Then (TotalCost_Local/Qty) /Multiplier    
Else 0    
End    
From #TempTable    
  
Select * from #TempTable  
Order By AccountName,Symbol   
  
Drop Table #TempTable 