CREATE PROCEDURE [dbo].[P_GetBlotterData_McMorgan_RBC] 
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
AS

--Declare @ThirdPartyID INT
--	,@companyFundIDs VARCHAR(max)
--	,@inputDate DATETIME
--	,@companyID INT
--	,@auecIDs VARCHAR(max)
--	,@TypeID INT
--	,@dateType INT                                                                                                                  
--	,@fileFormatID INT
--	,@includeSent BIT = 1


--Set @inputDate = '2023-12-06'

DECLARE @Fund TABLE (FundID INT)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')


--Declare @AccountCount Int
--Set @AccountCount = 0
--Set @AccountCount = (Select Count(*) From @Fund)


--If( @AccountCount = 0 Or @AccountCount IS Null)
--Begin
--Insert InTo @Fund
--Select CompanyFundID From T_CompanyFunds 
--End


Create Table #Temp_BT
(
AccountName Varchar(100),
Side Varchar(20),
Quantity Float,
Symbol Varchar(100),
Price Float,
GroupID Varchar(50),
CustomizedOrderby Int
)

Insert InTo #Temp_BT
Select
CF.FundName As AccountName,
S.Side As Side,
G.Quantity As Quantity,
G.Symbol As Symbol,
G.AvgPrice as Price,
G.GroupID,
1 As CustomizedOrderby

From T_Group G
--INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID
--INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = L1.FundID
Inner Join T_TradedOrders TOR on G.GroupID = TOR.GroupID
Inner Join T_Sub Sub on Sub.ClOrderID = TOR.CLOrderID
Inner Join T_FundAllocation L1 On L1.GroupID = G.GroupID  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = L1.FundID
Inner Join T_Side S On S.SideTagValue = G.OrderSideTagValue
Inner Join T_OrderType OT on G.OrderTypeTagValue = OT.OrderTypeTagValue 
Inner Join T_TimeInForce TIF on TIF.TimeInForceTagValue = Sub.TimeInForce
WHERE G.Quantity > 0
And Datediff(Day, G.AUECLocalDate, @inputdate) = 0

Select *
From #Temp_BT
Order By GroupID, CustomizedOrderby, AccountName,Symbol,Side 

Drop Table #Temp_BT