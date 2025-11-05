/*
Author: Abhilekha Sharma
Creation Date: 11 June 2020
Description: Get Trade Attributes and User Name in EOD for Maple Rock
JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-1098

Modified by: Amit Kumar
Modified Date: 12 Dec 2022
Jira: https://jira.nirvaasolutions.com:8443/browse/CI-5139

exec [P_GetTradeAttriAndUser_EOD_MapleRock] @thirdPartyID=56,@companyFundIDs=N'1,1280,1281,1282,1283,1284,1347,1285,1286,1287,1348,1288,1289,1290,1291,1292,1293,1294,1379,1349,1295,1378,1296,1297,1298,1299,1300,1350,1301,1302,1351,1303,1304,1305,1306,1352



,1307,1308,1353,1309,1310,1311,1354,1312,1355,1313,1314,1315,1316,1356,1317,1318,1319,1320,1321,1322,1357,1323,1324,1358,1325,1377,1326,1327,1328,1329,1330,1331,1373,1332,1374,1372,1359,1333,1334,1335,1336,1337,1360,1361,1338,1362,1363,1339,1340,1364,1341



,1342,1365,1343,1344,1366,1345,1367,1346,1368,1375,1376,1369,1370,1371',@inputDate='2020-06-10 02:52:43',
@companyID=7,@auecIDs=N'1,15',@TypeID=0,@dateType=0,@fileFormatID=123

*/ 
  
CREATE PROCEDURE [dbo].[P_GetTradeAttriAndUser_EOD_MapleRock] 
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

--Declare @thirdPartyID INT  
-- ,@companyFundIDs VARCHAR(max)  
-- ,@inputDate DATETIME  
-- ,@companyID INT  
-- ,@auecIDs VARCHAR(max)  
-- ,@TypeID INT  
-- ,@dateType INT         
-- ,@fileFormatID INT
 
-- Set @thirdPartyID = 56 
-- set @companyFundIDs=N'1254,1239,1264,1265,1260,1213,1258,1214,1259,1251,1266,1268,1267,1255,1238,1257,1250,1249,1256'
-- Set @inputDate = '08/03/2022'  
-- Set @companyID = 7
-- Set @auecIDs =N'152,20,164,69,65,71,67,76,63,102,29,55,53,47,49,44,34,43,56,59,31,54,45,108,21,60,18,61,74,1,15,11,62,73,105,12,80,90,16,100,19,32,33,81'
-- Set @TypeID  = 0  
-- Set @dateType = 0          
-- Set @fileFormatID = 123  

   
DECLARE @Fund TABLE (FundID INT)  
DECLARE @AUECID TABLE (AUECID INT)  
                                                                         
INSERT INTO @Fund  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@companyFundIDs, ',')  
  
INSERT INTO @AUECID  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@auecIDs, ',')  

 
SELECT
 CF.FundShortName AS AccountName
 ,VT.Symbol  
 ,T_Side.Side AS Side  
 ,Convert(varchar, VT.AUECLocalDate, 101) As TradeDate
 ,Convert(varchar, VT.ProcessDate, 101) As ProcessDate  
 ,SM.CompanyName  
 ,CASE 
	WHEN (VT.TradeAttribute1='' Or VT.TradeAttribute1 is NULL)
	THEN IsNull(SUB.TradeAttribute1,'')
	ELSE VT.TradeAttribute1
	END AS TradeAttribute1
 ,CASE 
	WHEN (VT.TradeAttribute2='' Or VT.TradeAttribute2 is NULL)
	THEN IsNull(SUB.TradeAttribute2,'')
	ELSE VT.TradeAttribute2
	END AS TradeAttribute2
 ,CASE 
	WHEN (VT.TradeAttribute3='' Or VT.TradeAttribute3 is NULL)
	THEN IsNull(SUB.TradeAttribute3,'')
	ELSE VT.TradeAttribute3
	END AS TradeAttribute3
 ,CASE 
	WHEN (VT.TradeAttribute4='' Or VT.TradeAttribute4 is NULL)
	THEN IsNull(SUB.TradeAttribute4,'')
	ELSE VT.TradeAttribute4
	END AS TradeAttribute4
 ,CASE 
	WHEN (VT.TradeAttribute5='' Or VT.TradeAttribute5 is NULL)
	THEN IsNull(SUB.TradeAttribute5,'')
	ELSE VT.TradeAttribute5
	END AS TradeAttribute5
,CASE 
	WHEN (VT.TradeAttribute6='' Or VT.TradeAttribute6 is NULL)
	THEN IsNull(SUB.TradeAttribute6,'')
	ELSE VT.TradeAttribute6
	END AS TradeAttribute6
 --,IsNull(UT.ShortName,'') As UserName
 ,IsNull(UT.ShortName,'') As UserName
FROM V_Taxlots VT  
inner join T_TradedOrders TRO on TRO.GroupID=VT.GroupID
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
Inner Join @Fund F On F.FundID = VT.FundID
Inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID  
Inner JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue  
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol  
Left Outer JOIN T_Sub AS SUB ON SUB.ClOrderID=TRO.CLOrderID
Left Outer JOIN T_CompanyUser AS UT ON UT.UserID = Sub.UserID
WHERE DateDiff(DAY,VT.AUECLocalDate, @inputdate) = 0  
And VT.OrderSideTagValue = '5'
AND VT.TransactionSource=1

UNION ALL

SELECT
 CF.FundShortName AS AccountName
 ,VT.Symbol  
 ,T_Side.Side AS Side  
 ,Convert(varchar, VT.AUECLocalDate, 101) As TradeDate
 ,Convert(varchar, VT.ProcessDate, 101) As ProcessDate  
 ,SM.CompanyName  
 ,IsNull(VT.TradeAttribute1,'')
 ,IsNull(VT.TradeAttribute2,'')
 ,IsNull(VT.TradeAttribute3,'')
 ,IsNull(VT.TradeAttribute4,'')
 ,IsNull(VT.TradeAttribute5,'')
 ,IsNull(VT.TradeAttribute6,'')
 --,IsNull(UT.ShortName,'') As UserName
 ,IsNull(UT.ShortName,'') As UserName
FROM V_Taxlots VT  
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
Inner Join @Fund F On F.FundID = VT.FundID
Inner JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID  
Inner JOIN T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue  
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol  
Left Outer JOIN T_CompanyUser AS UT ON UT.UserID = VT.UserID
WHERE DateDiff(DAY,VT.AUECLocalDate, @inputdate) = 0  
And VT.OrderSideTagValue = '5'
AND VT.TransactionSource=4

--ORDER BY TaxlotId

