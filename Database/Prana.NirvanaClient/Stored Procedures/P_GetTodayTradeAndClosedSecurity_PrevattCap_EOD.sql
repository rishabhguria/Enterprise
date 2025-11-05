CREATE Procedure [dbo].[P_GetTodayTradeAndClosedSecurity_PrevattCap_EOD]                      
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

--Declare @ThirdPartyID int,                                            
--@CompanyFundIDs varchar(max),                                                                                                                                                                          
--@InputDate datetime,                                                                                                                                                                      
--@CompanyID int,                                                                                                                                      
--@AUECIDs varchar(max),                                                                            
--@TypeID int,                                     
--@DateType int,
--@FileFormatID int  

--Set @ThirdPartyID = 1                                            
--Set @CompanyFundIDs = N'5,2,3,1,4'                                                                                                                                                                  
--Set @InputDate =  '2024-05-28'		                                                                                                                                                                 
--Set @CompanyID = 1                                                                                                                                     
--set @AUECIDs = '1'                                                                            
--Set @TypeID = 0
--Set @DateType = 0                                                                                                                                                                         
--Set @FileFormatID = 0 


DECLARE @PriorBusinessDay DATETIME
--Set @CurrentDate = '2024-03-15 00:00:00.000'--'2024-05-08 00:00:00.000'-- DateAdd(Day, - 1, GetDate())

Set @InputDate = DateAdd(Day, - 1, @InputDate)

Set @PriorBusinessDay  = DateAdd(Day, - 1, @InputDate)

Declare @Fund Table                                                                     
(                          
FundID int                                
)            
          
Insert into @Fund                                                                                                              
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',') 

---- Get Today traded Securities
Select Distinct 
Symbol,
Convert(varchar(10),VT.AUECLocalDate,101) As TradeDate,
Cast('' As Varchar(10)) As ClosingDate
InTo #Temp_TodaySecurity
From V_Taxlots VT With(NoLock)
Inner Join @Fund Fund on Fund.FundID = VT.FundID 
Where DateDiff(Day,VT.AUECLocalDate,@InputDate) = 0
Group By VT.Symbol, Convert(varchar(10),VT.AUECLocalDate,101)

--Select *
--From #Temp_TodaySecurity

---- Get yesterday Positions
Select 
PT.Symbol, 
Min(G.AUECLocalDate) As TradeDate
InTo #Temp_OpenSecurities_Yesterday
From PM_Taxlots PT With(NoLock)
Inner Join @Fund Fund on Fund.FundID = PT.FundID 
Inner Join T_Group G On G.GroupID = PT.GroupID 
Where PT.Taxlot_PK IN
(
	Select Max(Taxlot_PK) From PM_Taxlots 
	Where dateDiff(Day,AUECModifiedDate, @PriorBusinessDay) >= 0
	Group By TaxlotID
)
And PT.TaxLotOpenQty > 0
Group By PT.Symbol 
Order By PT.Symbol

Delete TTrade
From #Temp_TodaySecurity TTrade
Inner Join #Temp_OpenSecurities_Yesterday YOP On YOP.Symbol = TTrade.Symbol

---- Get Open Positions
Select 
PT.Symbol, 
Min(G.AUECLocalDate) As TradeDate
InTo #Temp_OpenSecurities_Today
From PM_Taxlots PT With(NoLock)
Inner Join @Fund Fund on Fund.FundID = PT.FundID 
Inner Join T_Group G On G.GroupID = PT.GroupID 
Where PT.Taxlot_PK IN
(
	Select Max(Taxlot_PK) From PM_Taxlots 
	Where dateDiff(Day,AUECModifiedDate, @InputDate) >= 0
	Group By TaxlotID
)
And PT.TaxLotOpenQty > 0
Group By PT.Symbol 
Order By PT.Symbol

---- Get Today's closed Data
Select Distinct 
PT.Symbol AS Symbol, 
Cast('' As Varchar(10)) As TradeDate,
Convert(varchar(10),@InputDate,101) As ClosingDate
InTo #Temp_ClosedData_CurrentDate
FROM PM_TaxlotClosing PTC With(NoLock)
INNER JOIN PM_Taxlots PT With(NoLock) ON	PTC.PositionalTaxlotID = PT.TaxlotID AND PTC.TaxLotClosingId = PT.TaxLotClosingId_Fk
INNER JOIN PM_Taxlots PT1 With(NoLock) ON PTC.ClosingTaxlotID = PT1.TaxlotID AND PTC.TaxLotClosingId = PT1.TaxLotClosingId_Fk
Inner Join @Fund Fund on Fund.FundID = PT.FundID 
WHERE DateDiff(Day, PTC.AUECLocalDate, @InputDate) = 0
Group By PT.Symbol 
Order By PT.Symbol

---- Remove Symbols from Closing temp table which are still Open
Delete C
From #Temp_ClosedData_CurrentDate C
Inner Join #Temp_OpenSecurities_Today OP On OP.Symbol = C.Symbol

---- Update Closing Date for a symbol which is Open and Closed same day
Update OP
Set OP.ClosingDate = C.ClosingDate 
From #Temp_TodaySecurity OP 
Inner Join #Temp_ClosedData_CurrentDate C On C.Symbol = OP.Symbol
And OP.TradeDate = C.ClosingDate 

---- Add Data in the final table
Insert InTO #Temp_TodaySecurity
Select *
From #Temp_ClosedData_CurrentDate
Where Symbol Not In
(
Select Symbol From #Temp_TodaySecurity
)

Select 
T.Symbol ,
T.TradeDate,
T.ClosingDate,
SM.SEDOLSymbol,
SM.ISINSymbol ,
SM.CUSIPSymbol,
SM.OSISymbol,
A.AssetName As AssetClass
From #Temp_TodaySecurity T
Inner Join V_SecMasterData SM With(NoLock) On SM.TickerSymbol = T.Symbol 
Inner Join T_Asset A On A.AssetID = SM.AssetId
Order By TradeDate,Symbol 


Drop Table #Temp_ClosedData_CurrentDate ,#Temp_OpenSecurities_Today,#Temp_TodaySecurity,#Temp_OpenSecurities_Yesterday