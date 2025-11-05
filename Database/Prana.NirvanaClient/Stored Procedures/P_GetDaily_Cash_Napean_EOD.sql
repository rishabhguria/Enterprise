/*

JIRA: https://jira.nirvanasolutions.com:8443/browse/ONB-5768

DESC: Constellation-Tareo account integration
*/
CREATE Procedure [dbo].[P_GetDaily_Cash_Napean_EOD]                        
(                                 
@ThirdPartyID int,                                            
@CompanyFundIDs varchar(max),                                                                                                                                                                          
@InputDate datetime,                                                                                                                                                                      
@CompanyID int,                                                                                                                                      
@AUECIDs varchar(max),                                                                            
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
@FileFormatID int                                  
)                                  
AS      
    
--Declare @ThirdPartyID int,                                            
--	@CompanyFundIDs varchar(max),                                                                                                                                                                          
--	@InputDate datetime,                                                                                                                                                                      
--	@CompanyID int,                                                                                                                                      
--	@AUECIDs varchar(max),                                                                            
--	@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
--	@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
--	@FileFormatID int
 
--set @thirdPartyID=86
--set @companyFundIDs=N'24'
--set @inputDate='08-4-2020'
--set @companyID=7
--set @auecIDs=N'20,43,21,18,61,74,1,15,11,62,73,12,80,32,81'
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=167

Declare @Fund Table                                                           
(                
FundID int                      
)  

Insert into @Fund                                                                                                    
Select Cast(Items As Int) from dbo.Split(@companyFundIDs,',') 

Select               
CF.FundName As AccountName,              
CC.CurrencySymbol As Symbol,   
EODCash.CashValueLocal As CashValueLocal, 
CC.CurrencySymbol As LocalCurrency, 
'Cash' As AssetClass,
EODCash.Date AS TradeDate
 
Into #TempEODCash             
From PM_CompanyFundCashCurrencyValue EODCash
Inner Join @Fund Fund On Fund.FundID = EODCash.FundID
inner join T_CompanyFunds CF on EODCash.FundID = CF.CompanyFundID
inner join T_Currency CC on EODCash.LocalCurrencyID=CC.CurrencyID
 Where datediff(d,EODCash.Date,@Inputdate)=0
  ------------------------------------------------
 -- Select * from #TempEODCash
    
Select               
EODCash.AccountName As AccountName,              
EODCash.Symbol, 
Sum(EODCash.CashValueLocal) As CashValueLocal,
LocalCurrency As LocalCurrency, 
Min(EODCash.AssetClass) As AssetClass,
Convert(varchar,EODCash.TradeDate,112) AS TradeDate
InTo #TempGroupedCashTable

From #TempEODCash EODCash  
Group By EODCash.AccountName,EODCash.Symbol, EODCash.LocalCurrency,TradeDate

Select * From #TempGroupedCashTable
Order By AccountName,Symbol 

Drop Table #TempGroupedCashTable,#TempEODCash
