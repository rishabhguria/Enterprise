
/*
EXEC P_LargeTraderVelocityReporting_DailyEOD 1,'1,2','02-18-2021',1,'1,2',1,0,12,0

DESC: https://jira.nirvanasolutions.com:8443/browse/ONB-4493
*/

CREATE Procedure [dbo].[P_LargeTraderVelocityReporting_DailyEOD]
(
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties 
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
)
As

SET NOCOUNT ON;

--Declare @inputDate DateTime
--Set @inputDate = '02-18-2021'

--DECLARE @Fund TABLE (FundID INT)

--INSERT INTO @Fund
--SELECT Cast(Items AS INT)
--FROM dbo.Split(@companyFundIDs, ',')


DECLARE @FirstDateofMonth DateTime
Declare @EndDate DateTime

SET @FirstDateofMonth = (SELECT DATEADD(m, DATEDIFF(m, 0, @inputDate), 0)) 

Set @EndDate = @inputDate

Select 
Convert(varchar,Cast(VT.ProcessDate As Date), 101) As TradeDate,
Sum(VT.TaxLotQty) As Quantity,

T_CompanyFunds.FundName As AccountName,
--(IsNull(VT.AvgPrice * VT.TaxLotQty * [dbo].GetSideMultiplier(VT.OrderSideTagValue) * SM.Multiplier ,0)) As GrossMoney,

Convert(money,CASE             
	WHEN VT.CurrencyID <> T_CompanyFunds.LocalCurrency            
	THEN CASE    
	WHEN IsNull(VT.FXRate, 0) <> 0            
	THEN CASE             
	WHEN VT.FXConversionMethodOperator = 'M'            
	THEN sum((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0) )           
	WHEN VT.FXConversionMethodOperator = 'D'            
	AND VT.FXRate > 0            
	THEN sum((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / NULLIF(VT.FXRate,0))         
	END 
	ELSE 0          
	END            
	ELSE sum((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses))
	END)  AS NetNotionalValueBase,

Sum((VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) + ([dbo].GetSideMultiplier(VT.OrderSideTagValue) * VT.TotalExpenses)) As NotionalValue

InTo #Temp
From V_TaxLots VT With (NoLock)
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_CounterParty CP On CP.CounterPartyID = VT.CounterPartyID
Inner Join T_CompanyFunds on T_CompanyFunds.CompanyFundID=VT.FundID 

Where DateDiff(Day,@FirstDateofMonth,VT.AUECLocalDate) >= 0
And DateDiff(Day,VT.AUECLocalDate,@EndDate) >= 0
And CP.ShortName <> 'CorpAction'
Group By Cast(VT.ProcessDate As Date),VT.CurrencyID,VT.FXRate, VT.FXConversionMethodOperator, T_CompanyFunds.LocalCurrency,T_CompanyFunds.FundName
Order By TradeDate

select 'Total' As TradeDate,
Sum(Quantity) As Quantity,
'AVT51209' As AccountName,
Sum(NetNotionalValueBase) As NetNotionalValueBase,
'' As NotionalValue
Into #Temp1
From #Temp where AccountName='AVT51209'

select 'Average' As TradeDate,
AVG(Quantity) As Quantity,
'AVT51209' As AccountName,
AVG(NetNotionalValueBase) As NetNotionalValueBase,
'' As NotionalValue
Into #Temp2
From #Temp where AccountName='AVT51209'

Create table #TempFinal(
TradeDate varchar(max),
Quantity float,
AccountName varchar(100),
NetNotionalValueBase float,
NotionalValue float
) 

Insert into #TempFinal 
select * from #Temp

Insert into #TempFinal 
select * from #Temp1

Insert into #TempFinal 
select * from #Temp2

select * from #TempFinal

drop table #Temp,#Temp1,#Temp2,#TempFinal



