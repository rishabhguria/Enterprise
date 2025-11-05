/*        
Creation Date: March 1, 2018        
Purpose: It is customized for Traid client to get open postions for all funds wise and grouped only positions for a master fund named Sloman 
       
exec P_TP_GetOpenPositions_AccountAndMaster_Triad 
@thirdPartyID=86,
@companyFundIDs=N'20,4,30,19,13,14,15,16,31,32,17,18,21,22,23,24,25,26,27,28,29,3,1',
@inputDate='2019-07-02 08:20:39:000',
@companyID=7,
@auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81',
@TypeID=0,
@dateType=0,
@fileFormatID=169 

Modifed By Sandeep Singh    
JIRA: https://jira.nirvanasolutions.com:8443/browse/PRANA-31673    
Desc: Triad: Please add Alec account on REDI position Third-party file    
ModifiedDate: 11 Feb, 2019   
FileFormatID=168   
      
*/ 

CREATE Procedure [dbo].[P_TP_GetOpenPositions_AccountAndMaster_Triad]                                    
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
        
--Declare @ThirdPartyID int                                                      
--Declare @CompanyFundIDs varchar(max)                                                                                                                                                                                    
--Declare @InputDate datetime                                                                                                                                                                               
--Declare @CompanyID int                                                                                                                                                
--Declare @AUECIDs varchar(max)                                                                                      
--Declare @TypeID int  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                      
--Declare @DateType int -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                    

  
--Declare @FileFormatID int         
        
--Set @ThirdPartyID = 86                                                       
--Set @CompanyFundIDs = '20,4,30,19,13,14,15,16,31,32,17,18,21,22,23,24,25,26,27,28,29,3,1'        
--Set @InputDate = '2019-07-02'    
--Set @CompanyID = 7                                                                
--Set @AUECIDs =  '20,43,21,18,61,74,1,15,11,62,73,12,80,32,81'                                                                                     
--Set @TypeID =0   -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                                      
--Set @DateType = 0 -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                                      
--Set @FileFormatID = 169   

Select         
CMF.MasterFundName         
InTo #TempMasterAccountDataNames        
From T_CompanyMasterFunds CMF           
Where CMF.MasterFundName In  ('Sloman','Alec Rutherfurd')  


Select               
CF.FundName As AccountName,              
PT.Symbol,              
Case                      
	When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                      
	Then 'Long'                      
	Else 'Short'                      
End as PositionSide,  
PT.TaxlotOpenQty As Qty,
----CASE                      
----	WHEN A.AssetName = 'FixedIncome'
----	Then PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue) 
----	Else (PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) 
----End As TotalCost_Local,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) As TotalCost_Local,  
SM.Multiplier As Multiplier,  
SM.OSISymbol As OSISymbol,    
1 As OrderID,    
MF.MasterFundName As MasterFund,
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier 
Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
Left outer join T_CompanyMasterFundSubAccountAssociation CMF on CMF.CompanyFundID = CF.CompanyFundID                      
Left outer join T_companyMasterFunds MF on MF.CompanyMasterFundID = CMF.CompanyMasterFundID  
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
1 As OrderID,    
Temp.MasterFund         
Into #TempTable              
From #TempOpenPositionsTable Temp    
Group By Temp.MasterFund,Temp.AccountName,Temp.Symbol,Temp.PositionSide

-- Calculate weighted average price    
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


---- All accounts data in table #TempAccount      
Select *         
Into #TempAccount         
From #TempTable            
Order By AccountName,Symbol         
        
---- Delete data other that master funds which is in table #TempMasterAccountDataNames    
---- Now #TempTable has only data for master funds which are in #TempMasterAccountDataNames    
    
Delete #TempTable        
Where MasterFund         
Not In (Select MasterFundName From #TempMasterAccountDataNames)        
       
---- Update Account Name as Master Fund in #TempTable    
Update #TempTable        
Set AccountName =           
 Case         
  When MasterFund IN (Select MasterFundName From #TempMasterAccountDataNames)     
  Then MasterFund     
-- When MasterFund = 'Alec Rutherfurd'      
--  Then 'Alec Rutherfurd'       
  Else AccountName        
 End         
From #TempTable           
        
Select         
AccountName,        
Symbol,        
PositionSide,        
Sum(Qty) As Qty,              
Sum(TotalCost_Local) As TotalCost_Local,            
Max(Multiplier) As Multiplier,              
Max(OSISymbol) As OSISymbol ,     
Case     
 When AccountName = 'Sloman'    
 Then 2       
 When AccountName = 'Alec Rutherfurd'    
 Then 3    
 Else 1    
End As OrderID,     
MasterFund,       
CAST(0 AS FLOAT) As AvgPrice    
Into #TempMasterAccountData         
From #TempTable      
Inner Join #TempMasterAccountDataNames TempMF On TempMF.MasterFundName = #TempTable.MasterFund    
Group By MasterFund,AccountName, Symbol, PositionSide        
        
UPdate #TempMasterAccountData            
Set AvgPrice =             
 Case              
  When Qty <> 0 And Multiplier <> 0              
  Then (TotalCost_Local/Qty) /Multiplier              
  Else 0              
 End              
From #TempMasterAccountData     
       
    
Select *         
Into #TemFinalData        
From #TempAccount        
        
Insert Into #TemFinalData        
Select * From #TempMasterAccountData        
         
Select     
AccountName,    
Symbol,    
PositionSide,    
Qty,    
TotalCost_Local,    
Multiplier,    
OSISymbol,    
OrderID,    
AvgPrice    
From #TemFinalData        
Order By OrderID, AccountName,Symbol

Drop Table #TempOpenPositionsTable
Drop Table #TempAccount,#TempMasterAccountData,#TempTable,#TemFinalData,#TempMasterAccountDataNames
