
/*
EXEC P_LargeTraderSSummit_MTDEOD 1,'1,2','02-18-2021',1,'1,2',1,0,12,0

DESC: https://jira.nirvanasolutions.com:8443/browse/ONB-2168
*/

CREATE Procedure [dbo].[P_LargeTraderSSummit_MTDEOD]
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

DECLARE @FirstDateofMonth DateTime
Declare @EndDate DateTime

SET @FirstDateofMonth = (SELECT DATEADD(m, DATEDIFF(m, 0, @inputDate), 0)) 

Set @EndDate = @inputDate

Select 
Convert(varchar,Cast(VT.ProcessDate As Date), 101) As TradeDate,
Sum(VT.TaxLotQty) As Quantity,
--(IsNull(VT.AvgPrice * VT.TaxLotQty * [dbo].GetSideMultiplier(VT.OrderSideTagValue) * SM.Multiplier ,0)) As GrossMoney,
Sum((VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) + ([dbo].GetSideMultiplier(VT.OrderSideTagValue) * VT.TotalExpenses)) As NotionalValue

From V_TaxLots VT With (NoLock)
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_CounterParty CP On CP.CounterPartyID = VT.CounterPartyID

Where DateDiff(Day,@FirstDateofMonth,VT.AUECLocalDate) >= 0
And DateDiff(Day,VT.AUECLocalDate,@EndDate) >= 0
And CP.ShortName <> 'CorpAction'
Group By Cast(VT.ProcessDate As Date)
Order By TradeDate


	