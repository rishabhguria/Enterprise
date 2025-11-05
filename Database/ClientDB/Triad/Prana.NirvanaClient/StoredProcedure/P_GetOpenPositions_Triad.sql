/*
exec P_GetOpenPositions_Triad @thirdPartyID=86,@companyFundIDs=N'20,4,30,19,13,14,15,16,31,32,17,18,21,22,23,24,25,26,27,28,29,3,1',
@inputDate='2019-07-02 08:20:39:000',@companyID=7,@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',@TypeID=0,@dateType=0,@fileFormatID=167
*/

ALTER Procedure [dbo].[P_GetOpenPositions_Triad]                        
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
CF.FundName As AccountName,              
PT.Symbol,              
Case                      
	When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                      
	Then 'Long'                      
	Else 'Short'                      
End as PositionSide,  
PT.TaxlotOpenQty As Qty,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) TotalCost_Local, 
SM.Multiplier As Multiplier,  
SM.OSISymbol As OSISymbol,    
A.AssetName As Asset,
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier 
Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
Inner join T_CompanyMasterFundSubAccountAssociation CMF on CMF.CompanyFundID = CF.CompanyFundID                      
Inner join T_companyMasterFunds MF on MF.CompanyMasterFundID = CMF.CompanyMasterFundID  
Where PT.Taxlot_PK In
(
	Select Max(Taxlot_PK) From PM_Taxlots
	Where DateDiff(Day,AUECModifiedDate,@InputDate) >= 0
	Group By TaxlotID
)
And PT.TaxlotOpenQty > 0
And A.AssetName <> 'PrivateEquity'

Select               
Temp.AccountName As AccountName,              
Temp.Symbol,              
Temp.PositionSide As PositionSide,          
Sum(Temp.Qty * Temp.SideMultiplier) As Qty,              
Sum(Temp.TotalCost_Local) As TotalCost_Local,            
Max(Temp.Multiplier) As Multiplier,              
Max(Temp.OSISymbol) As OSISymbol,        
Asset As Asset         
Into #TempTable              
From #TempOpenPositionsTable Temp    
Group By Temp.AccountName,Temp.Symbol,Temp.PositionSide,Temp.Asset

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


Drop Table #TempOpenPositionsTable, #TempTable