
CREATE PROCEDURE [dbo].[P_CombineAccountClass_EOD_K3E] 
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


--Declare
--  @thirdPartyID INT  
-- ,@companyFundIDs VARCHAR(max)  
-- ,@inputDate DATETIME  
-- ,@companyID INT  
-- ,@auecIDs VARCHAR(max)  
-- ,@TypeID INT  
-- ,@dateType INT         
-- ,@fileFormatID INT  


-- set @thirdPartyID=65
-- set @companyFundIDs=N'3,4,1'
-- set @inputDate='2021-11-02 05:50:03'
-- set @companyID=7
-- set @auecIDs=N'1,15,62'
-- set @TypeID=0
-- set @dateType=0
-- set @fileFormatID=136




DECLARE @Fund TABLE (FundID INT)  
DECLARE @AUECID TABLE (AUECID INT)  
                                                                         
INSERT INTO @Fund  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@companyFundIDs, ',')  
  
INSERT INTO @AUECID  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@auecIDs, ',')  


CREATE TABLE #VT (
	  --TIRORDERID varchar(50)
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
	 ,TIRORDERID varchar(50),
	 Symbol_PK varchar(50)
	)

INSERT INTO #VT
SELECT
--VT.TaxLotID as TaxLotID,
--Replace(Cast((convert(varchar, VT.ProcessDate, 10)  + Cast(Max(SM.Symbol_PK) As Varchar(20))) As Varchar(200)),'-','') As TIRORDERID,
 CF.FundShortName AS AccountName,
 VT.Symbol As Symbol,  
 VT.OrderSideTagValue AS SideID , 
 VT.AUECLocalDate As TradeDate,
 VT.SettlementDate As SettlementDate,
 VT.ProcessDate,   
 VT.TaxLotQty As [AllocatedQty],
 SM.SEDOLSymbol As SEDOL,
 Currency.CurrencySymbol AS CurrencySymbol,
 COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency,
 CP.Shortname AS CounterParty, 
 VT.AvgPrice As AvgPrice, 
 ISNULL(VT.Commission,0) AS CommissionCharged,
 ISNULL(VT.SoftCommission,0) AS SoftCommissionCharged,
 Replace(Cast((convert(varchar, ProcessDate, 10)  + Cast(Symbol_PK As Varchar(20))) As Varchar(200)),'-','') as TIRORDERID ,
  SM.Symbol_PK AS Symbol_PK

FROM V_Taxlots VT  
Inner Join @Fund F On F.FundID = VT.FundID
Inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID  
Inner JOIN T_Side S ON S.SideTagValue = VT.OrderSideTagValue  
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
 


UPDATE #VT
SET AccountName = CASE 
		WHEN AccountName='TD Levered'
			THEN 'TD'
		WHEN AccountName='TD Unlevered'
			THEN 'TD'		
		ELSE AccountName
		END    

--select * from #VT where Symbol='CLRM'

--UPDATE #VT
--SET TIRORDERID=Replace(Cast((convert(varchar, ProcessDate, 10)  + Cast(Symbol_PK As Varchar(20))) As Varchar(200)),'-','')
--Replace(Cast((convert(varchar, VT.ProcessDate, 10)  + Cast(Max(SM.Symbol_PK) As Varchar(20))) As Varchar(200)),'-','') As TIRORDERID

Select  
AccountName,
Symbol AS Symbol,
S.Side as Side,
 Convert(varchar, VT.TradeDate, 101) As TradeDate,
 Convert(varchar, VT.SettlementDate, 101) As SettlementDate, 
 Max(SEDOL),
VT.CurrencySymbol AS CurrencySymbol,
SettlCurrency AS SettlCurrency,
CounterParty,
ISNULL(Sum(VT.AllocatedQty),0) AS AllocatedQty,
Sum(VT.AllocatedQty * VT.AvgPrice) / NULLIF(Sum(VT.AllocatedQty),0) As AveragePrice,
--ISNULL(Sum(VT.AvgPrice),0) AS AveragePrice,
ISNULL(Sum(VT.Commission),0) AS CommissionCharged,
ISNULL(Sum(VT.SoftCommission),0) AS SoftCommissionCharged,
VT.TIRORDERID as TIRORDERID
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
   GROUP BY
   AccountName,
   Symbol,
   TradeDate,
   VT.SettlementDate,
   VT.ProcessDate,
  --ProcessDate,
   Side,
   CounterParty,
   Vt.CurrencySymbol,
    SettlCurrency, 
    VT.TIRORDERID 
                                                             
--Order by TaxlotId  
drop table #VT