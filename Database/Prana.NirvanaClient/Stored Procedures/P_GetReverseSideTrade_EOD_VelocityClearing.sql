
CREATE PROCEDURE [dbo].[P_GetReverseSideTrade_EOD_VelocityClearing] 
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
 

--set @thirdPartyID=74
--set @companyFundIDs=N'59,60,1,58,4,14,15,13,63,16,17,18,12,6,7,38,39,72,40,41,42,22,45,46,47,48,49,43,44,73,74,75,76,77,78,79,80,81,82,62,54,53,52,51,88,89,87,2,86,85,3,71,84,83,56,55,61,50,57'
--set @inputDate='2021-11-08 07:23:04'
--set @companyID=7
--set @auecIDs=N'20,30,34,43,59,21,18,180,202,74,1,15,230,62,73,12,158,17,81'
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=127



DECLARE @Fund TABLE (FundID INT)  
DECLARE @AUECID TABLE (AUECID INT)  
                                                                         
INSERT INTO @Fund  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@companyFundIDs, ',')  
  
INSERT INTO @AUECID  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@auecIDs, ',')  


CREATE TABLE #VT (
	 TaxLotID varchar(50)
	 ,AccountName VARCHAR(300)	
	,Symbol VARCHAR(100)
 	,OrderTypeTagValue VARCHAR(3)
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
VT.TaxLotID as TaxLotID,
 CF.FundShortName AS AccountName,
 VT.Symbol As Symbol,  
 VT.OrderTypeTagValue,
 VT.OrderSideTagValue AS SideID, 
 VT.AUECLocalDate As TradeDate,
 VT.SettlementDate As SettlementDate,
 VT.ProcessDate   AS ProcessDate,
 VT.TaxLotQty As [AllocatedQty],
 SM.SEDOLSymbol As SEDOL,
 Currency.CurrencySymbol AS CurrencySymbol,
 COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency,
 CP.Shortname AS CounterParty, 
 VT.AvgPrice As AvgPrice, 
 ISNULL(VT.Commission,0) AS CommissionCharged,
 ISNULL(VT.SoftCommission,0) AS SoftCommissionCharged,
 AST.AssetName AS Asset,
 'C@N@' AS MEMOFIELD

FROM V_Taxlots VT  
Inner Join @Fund F On F.FundID = VT.FundID
Inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID  
Inner JOIN T_Side S ON S.SideTagValue = VT.OrderSideTagValue
Inner JOIN T_Asset AST ON AST.AssetID = VT.AssetID  
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol 
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID

WHERE DateDiff(DAY, (  
CASE  
WHEN @DateType = 1  
THEN VT.AUECLocalDate  
ELSE VT.ProcessDate  
END  
   ), @inputdate) = 0 
 
 -----Generate apposite side trade----

 INSERT INTO #VT
SELECT
VT.TaxLotID as TaxLotID,
 CF.FundShortName AS AccountName,
 VT.Symbol As Symbol,
 VT.OrderTypeTagValue,
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
 
 --VT.OrderSideTagValue AS SideID,
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
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK
LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot
LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID
LEFT JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID

WHERE DateDiff(DAY, (  
CASE  
WHEN @DateType = 1  
THEN VT.AUECLocalDate  
ELSE VT.ProcessDate  
END  
   ), @inputdate) = 0 
    
Select 
VT.TaxLotID as TaxlotId,  
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
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol 
WHERE DateDiff(DAY, (  
CASE  
WHEN @DateType = 1  
THEN VT.TradeDate  
ELSE VT.ProcessDate  
END  
   ), @inputdate) = 0                          
                                                                     
Order by TaxlotId  
drop table #VT