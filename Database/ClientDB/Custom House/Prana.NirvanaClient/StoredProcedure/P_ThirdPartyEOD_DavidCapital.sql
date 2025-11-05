/*  
Created By: Sandeep Singh  
Creation Date: 6 May, 2019  
Desc: End of day open positions and Day MTM PNL  
JIRA: https://jira.nirvanasolutions.com:8443/browse/CI-1564  
  
exec P_ThirdPartyEOD_DavidCapital @thirdPartyID=26,@companyFundIDs=N'1243,1241,1242',@inputDate='2019-04-29 06:01:05:817'  
,@companyID=7,@auecIDs=N'63,44,34,43,59,21,60,18,61,74,1,15,11,62,73,12,16,17,32',@TypeID=0,@dateType=0,@fileFormatID=58  
  
*/  
  
CREATE PROCEDURE [dbo].[P_ThirdPartyEOD_DavidCapital]                                                                                                                           
(                                                                                                                                          
 @ThirdPartyID int,            
 @CompanyFundIDs varchar(max),                                                                                                                                          
 @InputDate datetime,                                                                                                                                      
 @CompanyID int,                                                                                                      
 @auecIDs varchar(max),                                            
 @TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties            
 @DateType int -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                            
,@FileFormatID int            
)                                                                                                                                          
                                                                                                                                           
AS    
  
--Declare @thirdPartyID int           
--Declare @companyFundIDs varchar(max)                                                                                                                                         
--Declare @inputDate datetime                                                                                                                                      
--Declare @companyID int                                                                                                      
--Declare @auecIDs varchar(max)                                            
--Declare @TypeID int          
--Declare @dateType int                                                                                                                                        
--Declare @fileFormatID int  
  
--Set @InputDate = '04/29/2019'  
  
  
Select  
Fund As 'FundName',  
'' As Source,  
Max(CUSIPSymbol) As CUSIP,  
Max(ISINSymbol) As ISIN,  
Max(SEDOLSymbol) As SEDOL,  
Max(BloombergSymbol) As BloombergSymbol,  
Max(SecurityName) As SecurityName,  
Max(Asset) As SecurityType,  
CONVERT(VARCHAR(20), @InputDate, 101)  As PositionDate,  
Max(Side) As PositionType,  
Sum(  
Case   
 When Open_CloseTag = 'O'  
 Then TotalCost_Local  
 Else 0  
End) As TotalCost_Local,  
Sum(Case   
 When Open_CloseTag = 'O'  
 Then BeginningQuantity * SideMultiplier  
 Else 0  
End ) As Quantity,  
Max(EndingPriceLocal) As 'LastPrice',  
Max(EndingFXRate) As 'ExchangeRate',  
Sum(Case   
 When Open_CloseTag = 'O'  
 Then EndingMarketValueBase  
 Else 0  
End) As 'MarketValueBase',  
Max(Multiplier) As Multiplier,  
'' As OCCCode,  
SUM(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend) As TotalDayPNL  
InTo #TempPositionsAndPNL  
From T_MW_GenericPNL WITH (NOLOCK)  
Where Open_CloseTag <> 'Accruals'  
And Asset <> 'Cash'  
And DateDiff(Day,RunDate,@InputDate) = 0  
And Asset = 'Equity' And Side = 'Short'  
--And SedolSymbol  ='B8FW545'  
Group By Fund, Symbol  
Order By Fund,Symbol  
  
Alter Table #TempPositionsAndPNL  
Add CostPerShare Float  
  
Update #TempPositionsAndPNL   
Set CostPerShare = 0  
  
Update #TempPositionsAndPNL   
Set CostPerShare =   
Case   
When Quantity <> 0  
Then TotalCost_Local / Quantity  
Else 0  
End   
  
Select * from #TempPositionsAndPNL  
  
Drop Table #TempPositionsAndPNL   