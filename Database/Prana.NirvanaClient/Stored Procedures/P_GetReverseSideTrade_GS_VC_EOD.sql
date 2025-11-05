CREATE PROCEDURE [dbo].[P_GetReverseSideTrade_GS_VC_EOD] 

(  
 @thirdPartyID INT  
 ,@companyFundIDs VARCHAR(max)  
 ,@inputDate DATETIME  
 ,@companyID INT  
 ,@auecIDs VARCHAR(max)  
 ,@TypeID INT  
 ,@dateType INT         
 ,@fileFormatID INT  
 )  
AS

--declare 
-- @thirdPartyID INT  
-- ,@companyFundIDs VARCHAR(max)  
-- ,@inputDate DATETIME  
-- ,@companyID INT  
-- ,@auecIDs VARCHAR(max)  
-- ,@TypeID INT  
-- ,@dateType INT         
-- ,@fileFormatID INT  
 

--set  @thirdPartyID=79
--set  @companyFundIDs=N'5,11,94,93,95'
--set  @inputDate='2022-07-19 09:41:15'
--set  @companyID=7
--set  @auecIDs=N'63,59,180,1,15,73,32'
--set  @TypeID=0
--set  @dateType=0
--set  @fileFormatID=142



DECLARE @Fund TABLE (FundID INT)  
DECLARE @AUECID TABLE (AUECID INT)  
                                                                         
INSERT INTO @Fund  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@companyFundIDs, ',')  
  
INSERT INTO @AUECID  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@auecIDs, ',')  


CREATE TABLE #VT (	
     AccountName VARCHAR(300)	
	 ,Symbol VARCHAR(100) 	
	,SideID VARCHAR(20)
	,TradeDate DATETIME
	,SettlementDate DATETIME
	 ,ProcessDate DATETIME
	,AllocatedQty FLOAT
	,SEDOL VARCHAR(20)
	,CurrencySymbol VARCHAR(100)
	,SettlCurrency VARCHAR(100)
	 ,CounterParty VARCHAR(100)
	,AvgPrice FLOAT	
	,Commission FLOAT	
	,SoftCommission FLOAT
    , Asset VARCHAR(20)
	,MEMOFIELD VARCHAR(20)	
	)

INSERT INTO #VT
SELECT
'A45C1209' AS AccountName,
 VT.Symbol As Symbol,  
 VT.OrderSideTagValue AS SideID, 
 Max(VT.AUECLocalDate) As TradeDate,
 Max(VT.SettlementDate) As SettlementDate,
 max(VT.ProcessDate)   AS ProcessDate,
 Sum(VT.TaxLotQty) As [AllocatedQty],
 Max(SM.SEDOLSymbol) As SEDOL,
 max(Currency.CurrencySymbol) AS CurrencySymbol,
 Max(COALESCE(TC.CurrencySymbol, 'None')) AS SettlCurrency,
 Max(CP.Shortname) AS CounterParty, 
 Sum(VT.TaxLotQty * VT.AvgPrice) / NULLIF(Sum(VT.TaxLotQty),0) As AvgPrice,
 sum(ISNULL(VT.Commission,0) )AS CommissionCharged,
 sum(ISNULL(VT.SoftCommission,0)) AS SoftCommissionCharged,
 max(AST.AssetName) AS Asset,
 'C@N@' AS MEMOFIELD

FROM V_Taxlots VT  
--Inner Join @Fund F On F.FundID = VT.FundID
--Inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID  
Inner JOIN T_Side S ON S.SideTagValue = VT.OrderSideTagValue
Inner JOIN T_Asset AST ON AST.AssetID = VT.AssetID  
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol 
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID

WHERE DateDiff(DAY, (  
CASE  
WHEN @DateType = 1  
THEN VT.AUECLocalDate  
ELSE VT.ProcessDate  
END  
   ), @inputdate) = 0 
  
  Group by VT.OrderSideTagValue,VT.Symbol,VT.CounterPartyID

 
 -----Generate apposite side trade----

 INSERT INTO #VT
SELECT
CF.FundShortName AS AccountName,
 VT.Symbol As Symbol, 
 Case 
	when VT.OrderSideTagValue='1'
	then '2'
	when VT.OrderSideTagValue='2'
	then '1'
	when VT.OrderSideTagValue='5'
	then 'B' 
	when VT.OrderSideTagValue='B'
	then '5'	
	else VT.OrderSideTagValue
 end  AS SideID,
 
 VT.AUECLocalDate As TradeDate,
 VT.SettlementDate As SettlementDate,
 VT.ProcessDate,   
 VT.TaxLotQty As [AllocatedQty],
 SM.SEDOLSymbol As SEDOL,
 Currency.CurrencySymbol AS CurrencySymbol,
 COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency,
 CP.ShortName AS CounterParty, 
 VT.AvgPrice As AvgPrice, 
 ISNULL(VT.Commission,0) AS CommissionCharged,
 ISNULL(VT.SoftCommission,0) AS SoftCommissionCharged,
 AST.AssetName AS Asset,
 'AL@C' AS MEMOFIELD

FROM V_Taxlots VT  
Inner Join @Fund F On F.FundID = VT.FundID
Inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID  
Inner JOIN T_Side S ON S.SideTagValue = VT.OrderSideTagValue  
Inner JOIN T_Asset AST ON AST.AssetID = VT.AssetID
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol 
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID

WHERE DateDiff(DAY, (  
CASE  
WHEN @DateType = 1  
THEN VT.AUECLocalDate  
ELSE VT.ProcessDate  
END  
   ), @inputdate) = 0 

    
Select 
AccountName,
Symbol,
S.Side as Side,
 Convert(varchar, VT.TradeDate, 101) As TradeDate,
 Convert(varchar, VT.SettlementDate, 101) As SettlementDate, 
VT.AllocatedQty AS AllocatedQty,
SEDOL,
VT.CurrencySymbol,
SettlCurrency,
VT.CounterParty,
ISNULL(VT.AvgPrice,0) AS AveragePrice,
ISNULL(VT.Commission,0) AS CommissionCharged,
ISNULL(VT.SoftCommission,0) AS SoftCommissionCharged,
Vt.Asset,
MEMOFIELD
from #VT VT 
Inner JOIN T_Side S ON S.SideTagValue = VT.SideID 
WHERE DateDiff(DAY, (  
CASE  
WHEN @DateType = 1  
THEN VT.TradeDate  
ELSE VT.ProcessDate  
END  
   ), @inputdate) = 0                          
                                                                     
Order by Symbol ,MEMOFIELD desc
drop table #VT