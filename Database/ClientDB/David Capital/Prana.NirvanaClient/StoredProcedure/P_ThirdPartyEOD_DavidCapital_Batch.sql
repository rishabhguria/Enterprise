/*    
  
Created By: Sandeep Singh    
  
Creation Date: 6 May, 2019    
  
Desc: End of day open positions and Day MTM PNL    
  
JIRA: https://jira.nirvanasolutions.com:8443/browse/CI-1608    
  
    
  
exec P_ThirdPartyEOD_DavidCapital @thirdPartyID=26,@companyFundIDs=N'1243,1241,1242',@inputDate='2019-04-29 06:01:05:817'    
  
,@companyID=7,@auecIDs=N'63,44,34,43,59,21,60,18,61,74,1,15,11,62,73,12,16,17,32',@TypeID=0,@dateType=0,@fileFormatID=58    
  
    
  
*/    
  
    
  
CREATE PROCEDURE [dbo].[P_ThirdPartyEOD_DavidCapital_Batch]                                                                                                                             
  
(                                                                                                                                            
  
 @CompanyFundIDs VARCHAR(max)                                    
 ,@InputDate DATETIME                                    
  )                                      
AS                        
                      
SET NOCOUNT On                                  
                                  
                    
                            
If (@InputDate = '')      
Begin      
 Set @InputDate = GetDate()      
End      
         
                                 
                                  
DECLARE @Fund TABLE                               
(                              
  FundID INT                              
)                              
      
If (@CompanyFundIDs Is NULL Or @CompanyFundIDs = '')                                                                  
 Insert InTo @Fund                                                                  
  Select                          
  CompanyFundID as FundID                           
  From T_CompanyFunds                                                                  
Else                                                                  
 INSERT INTO @Fund                                                                  
  SELECT Cast(Items AS INT)                              
  FROM dbo.Split(@companyFundIDs, ',')       
  
  
Select    
  
Fund As 'FUNDNAME',    
  
'' As SOURCE,    
  
Max(CUSIPSymbol) As CUSIP,    
  
Max(ISINSymbol) As ISIN,    
  
Max(SEDOLSymbol) As SEDOL,    
  
Max(BloombergSymbol) As BloombergTicker,    
  
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
  
cast(Max(EndingPriceLocal) as decimal(38,2)) As 'LastPrice',    
  
cast(Max(EndingFXRate) as decimal(38,2)) As 'ExchangeRate',    
  
Sum(Case     
  
 When Open_CloseTag = 'O'    
  
 Then EndingMarketValueBase    
  
 Else 0    
  
End) As 'PorfolioBaseCurrency_MV',    
  
Max(Multiplier) As Multiplier,    
  
'' As OCCCode,    
  
cast(SUM(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend) as decimal(38,2)) As TotalDayPNL    
  
InTo #TempPositionsAndPNL    
  
From T_MW_GenericPNL WITH (NOLOCK)    
  
Where Open_CloseTag <> 'Accruals'    
  
And Asset <> 'Cash'    
  
And DateDiff(Day,RunDate,@InputDate) = 0    
  
And Asset = 'Equity' And Side = 'Short'  
  
  
  
Group By Fund, Symbol    
  
Order By Fund,Symbol    
  
    
  
Alter Table #TempPositionsAndPNL    
  
Add Price Float    
  
    
  
Update #TempPositionsAndPNL     
  
Set Price = 0    
  
    
  
Update #TempPositionsAndPNL     
  
Set Price =     
  
Case     
  
When Quantity <> 0    
  
Then cast(TotalCost_Local / Quantity as decimal(38,2))  
  
Else 0    
  
End     
  
    
  
Select FUNDNAME,SOURCE,CUSIP,ISIN,SEDOL,BloombergTicker,SecurityName,SecurityType,PositionDate,PositionType,Price,Quantity,LastPrice,ExchangeRate,PorfolioBaseCurrency_MV,Multiplier,OCCCode,TotalDayPNL  from #TempPositionsAndPNL    
  
    
  
Drop Table #TempPositionsAndPNL 