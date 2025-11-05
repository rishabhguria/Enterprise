Create PROCEDURE [dbo].[P_GetBlotterData_McMorgan_RBC_EOD]   
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
-- ,@companyFundIDs VARCHAR(max)  
-- ,@inputDate DATETIME  
-- ,@companyID INT  
-- ,@auecIDs VARCHAR(max)  
-- ,@TypeID INT  
-- ,@dateType INT                                                                                                                    
-- ,@fileFormatID INT  
-- ,@includeSent BIT = 1

----Set @inputDate = '2023-12-06'


--set @thirdPartyID=95
--set @companyFundIDs=N'96,8,'
--set @inputDate='2025-04-23 07:04:13.733'
--set @companyID=7
--set @auecIDs=N'1,15,62,'
--set @TypeID=0
--set @dateType=1
--set @fileFormatID=51

DECLARE @Fund TABLE (FundID INT)  
  
INSERT INTO @Fund  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@companyFundIDs, ',')  
  
  Create Table #Temp_BT
  (
  AccountName Varchar(100)
  , Side Varchar(20)
  , Quantity Float
  , Symbol Varchar(100)
  , Price Float
  , GroupID Varchar(50)
  , CustomizedOrderby Int
  ) 
  Insert InTo #Temp_BT
  Select CF.FundName As AccountName,
  S.Side As Side,
  --(L1.Percentage/100)*G.Quantity As Quantity,
  ROUND((L1.Percentage / 100) * G.Quantity, 0) AS Quantity,
 -- G.Quantity As Quantity, 
  G.Symbol As Symbol,
  G.AvgPrice as Price,
  G.GroupID,
  1 As CustomizedOrderby 
  From T_Group G  
  Inner Join T_TradedOrders TOR WITH(NOLOCK) on G.GroupID = TOR.GroupID
  Inner Join T_Sub Sub WITH(NOLOCK) on Sub.ClOrderID = TOR.CLOrderID
  Inner Join T_FundAllocation L1 WITH(NOLOCK) On L1.GroupID = G.GroupID    
   Inner Join T_CompanyFunds CF WITH(NOLOCK) On CF.CompanyFundID = L1.FundID
   Inner Join T_Side S WITH(NOLOCK) On S.SideTagValue = G.OrderSideTagValue
   Inner Join T_OrderType OT WITH(NOLOCK) on G.OrderTypeTagValue = OT.OrderTypeTagValue 
   Inner Join T_TimeInForce TIF WITH(NOLOCK) on TIF.TimeInForceTagValue = Sub.TimeInForce 
   WHERE G.Quantity > 0  
   And Datediff(Day, G.AUECLocalDate, @inputdate) = 0 

Select * From #Temp_BT
Order By GroupID, CustomizedOrderby, AccountName,Symbol,Side
Drop Table #Temp_BT