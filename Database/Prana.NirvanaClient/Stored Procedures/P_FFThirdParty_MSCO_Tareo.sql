
CREATE PROCEDURE [dbo].[P_FFThirdParty_MSCO_Tareo] 
 (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
	)
AS

--Declare @thirdPartyID INT
--	,@companyFundIDs VARCHAR(max)
--	,@inputDate DATETIME
--	,@companyID INT
--	,@auecIDs VARCHAR(max)
--	,@TypeID INT
--	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                            
--	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
--	,@fileFormatID INT
--	,@includeSent INT = 0

--Set @thirdPartyID=56
--Set @companyFundIDs=N'8,2,3,4,5,6,7,14,11,10,9,12,13,1'
--Set @inputDate='2022-11-08 06:40:26'
--Set @companyID=7
--Set @auecIDs=N'59,18,180,202,203,1,15,62,12,158,32'
--Set @TypeID=0
--Set @dateType=0
--Set @fileFormatID=120

Declare @Fund Table                                                               
(                    
FundID int
)  
    
Insert into @Fund                                                                                                        
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')

SELECT PT.Taxlot_PK        
InTo #TempTaxlotPK             
FROM PM_Taxlots PT              
Inner Join @Fund Fund on Fund.FundID = PT.FundID                  
Where PT.Taxlot_PK in                                           
(                                                                                                    
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                       
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                          
 group by TaxlotId                             
)                                                                                              
And PT.TaxLotOpenQty > 0

Select
CF.FundName As AccountName,
PT.Symbol,
T_Side.Side as Side,
T_CounterParty.ShortName as CounterParty,     
SM.ISINSymbol As ISINSymbol,                                          
SM.CUSIPSymbol As CUSIPSymbol,                                          
SM.SEDOLSymbol As SEDOLSymbol,                                          
SM.BloombergSymbol As BloombergSymbol,
SM.ReutersSymbol As RIC,                                          
SM.CompanyName As CompanyName,
SM.OSISymbol As OSISymbol,
SM.UnderLyingSymbol,
SM.StrikePrice,
CONVERT(VARCHAR(10), SM.ExpirationDate, 112) As ExpirationDate,
SM.PutOrCall,               
Curr.CurrencySymbol,    
PT.TaxlotOpenQty As OpenPosition,
IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) As MarkPrice,
CONVERT(VARCHAR(10), PT.AUECModifiedDate, 112) AS TradeDate,
IsNull((PT.TaxlotOpenQty * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) AS MarketValue,   
Case
		When dbo.GetSideMultiplier(PT.OrderSideTagValue)=1
		Then 'Long'
		Else 'Short'
End As LongShort,  
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
PT.AvgPrice As AveragePrice,
A.AssetName As Asset,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) As TotalCost_Local,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) As TotalCost_ForWtedPrice,
SM.Multiplier,
SM.BondTypeID

Into #TempOpenPositionsTable             
From PM_Taxlots PT    
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK     
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol    
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID      
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID   
INNER JOIN T_Group G ON G.GroupID = PT.GroupID
Inner Join T_CounterParty on T_CounterParty.CounterPartyID = G.CounterPartyID
Inner Join T_Side ON dbo.T_Side.SideTagValue = PT.OrderSideTagValue
Inner Join T_Asset A On A.AssetID = SM.AssetID
  
 LEFT OUTER JOIN PM_DayMarkPrice AS MP ON (      
   MP.Symbol = PT.Symbol      
   AND DateDiff(d, MP.[DATE], @InputDate) = 0      
   AND MP.FundID = PT.FundID      
   )      
 LEFT OUTER JOIN PM_DayMarkPrice AS MP1 ON (      
   MP1.Symbol = PT.Symbol      
   AND DateDiff(d, MP1.[DATE], @InputDate) = 0      
   AND MP1.FundID = 0      
   )     
Where PT.TaxlotOpenQty > 0
And A.AssetID in (1,2,8)
And CF.FundName='Mountain Cove MS 038CAOJ72'

Select
AccountName ,
Side As Side ,
Max(CounterParty) As CounterParty ,
Symbol ,
Max(CurrencySymbol) As CurrencySymbol ,
Sum(OpenPosition * SideMultiplier) As OpenPosition ,
CONVERT(VARCHAR(10), @InputDate, 101) As TradeDate ,
Max(ISINSymbol) As ISINSymbol ,
Max(CUSIPSymbol) As CUSIPSymbol ,
Max(SEDOLSymbol) As SEDOLSymbol ,
Max(BloombergSymbol) As BloombergSymbol ,
Max(RIC) As RIC ,
Max(CompanyName) As CompanyName ,
Max(OSISymbol) As OSISymbol ,
Max(UnderLyingSymbol) As UnderLyingSymbol,
Max(StrikePrice) As StrikePrice,
Max(ExpirationDate) As ExpirationDate,
Max(PutOrCall) As PutOrCall,
Sum(MarketValue) As MarketValue ,
Max(MarkPrice) As MarkPrice ,
LongShort As LongShort ,
Max(AveragePrice) As AveragePrice,
Asset As Asset,
Sum(TotalCost_Local) As TotalCost_Local,
Sum(TotalCost_ForWtedPrice) As TotalCost_ForWtedPrice,
Max(Multiplier) As Multiplier,
Max(BondTypeID) As BondTypeID

InTo #Temp_GroupedData
From #TempOpenPositionsTable as Temp
Group By Asset, Temp.AccountName,Temp.Symbol, Temp.Side, Temp.LongShort

Update #Temp_GroupedData      
Set AveragePrice = 
Case 
When OpenPosition <> 0 And Multiplier <> 0
Then ((TotalCost_ForWtedPrice / OpenPosition) /Multiplier)
Else 0
End

Update #Temp_GroupedData      
Set TotalCost_Local  =
Case 
	When Asset='Equity'
	  Then CASE
	  when OpenPosition<>0
	  THEN Round(Abs(TotalCost_Local/OpenPosition),4)
	  ELSE 0
	  END
	   WHEN Asset='EquityOption'
	    THEN CASE
	     When OpenPosition<>0
	     THEN Round(Abs(TotalCost_Local/OpenPosition)/100,4)
	   ELSE 0
	   END 
	   WHEN Asset='FixedIncome'
	    THEN CASE
	     When OpenPosition<>0
	     THEN Round(Abs(TotalCost_Local/OpenPosition)*100,4)
	   ELSE 0
	   END
End


select * from #Temp_GroupedData 

Drop Table #TempOpenPositionsTable,#Temp_GroupedData,#TempTaxlotPK


