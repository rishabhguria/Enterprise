/*          
-- Author : Dileep                                                  
-- Description : It provides data to corp action module for applying the corporate action           
--Modified By :Ishant Kathuria        
--Date:20th dec 2011                                   
-- Description : Returns  UserID as well                                                                                           
Modified By: Sandeep Singh    
Date: Dec 19, 2013    
Desc: Attribute1-6, LotId and ExternalID 

Modified By: Sandeep Singh    
Date: NOV 23, 2014    
Desc: TransactionType    
*/    
CREATE PROCEDURE [dbo].[PMGetOpenPositionforUndoPreview]                                                        
(                                                                                                                              
 @corpactionIDs varchar(max)                                                                      
)                                                                                                                              
As                                                                                                                                  
Begin                                                                                                                  
-- Get UTC Date. UTC Date is stored corresponding to AUECID 0 in @AUECDatesTable   
Create Table #SecMasterTable  
(  
TickerSymbol Varchar(200),  
Multiplier Float,
CompanyName Varchar(200)   
)  
  
Insert InTo #SecMasterTable  
Select  
TickerSymbol,  
Multiplier ,
CompanyName   
From V_SecMasterData  
  
                                                                                                             
Select                                                        
PT.TaxLotID as TaxLotID,                                                                                                                                              
PT.OrderSideTagValue as SideID, -- WE INSERT 0 FOR BUY SIDE AND 1 FOR SELL SHORT IN PM_NETPOSITIONS    
PT.Symbol as Symbol ,                                                
PT.GroupID as GroupID,                                                                                                                                        
PT.TaxLotOpenQty as TaxLotOpenQty ,                                                  
PT.AvgPrice as AvgPrice ,                                                                                                                                                                     
PT.FundID as FundID,                  
PT.Level2ID as Level2ID,                
G.AUECID as AUECID,                
G.AUECLocalDate as AUECLocalDate,                                                                       
PT.TaxLot_PK as TaxLot_PK,                              
G.AssetID,               
G.UnderLyingID,              
G.ExchangeID,                   
G.CurrencyID,              
PT.OpenTotalCommissionandFees,--this is open commission and closed commission sum is not necessarily equals to total commission                
PT.ClosedTotalCommissionandFees,              
G.OrderTypeTagValue,              
G.CounterPartyID,              
G.VenueID,              
G.TradingAccountID,              
G.CumQty,              
G.AllocatedQty,              
G.ListID,              
G.ISProrataActive,              
G.AutoGrouped,              
G.IsManualGroup,              
G.Description,              
G.FXRate,              
G.FXConversionMethodOperator,        
G.ProcessDate,        
G.OriginalPurchaseDate,         
G.UserId,      
PT.TradeAttribute1,      
PT.TradeAttribute2,      
PT.TradeAttribute3,      
PT.TradeAttribute4,      
PT.TradeAttribute5,                    
PT.TradeAttribute6,    
PT.LotID,    
PT.ExternalTransID,   
SM.Multiplier As AssetMultiplier, ---- This value is not in use while undo preview but number of parameters should be equal 
G.TransactionType,  ---- This value is not in use while undo preview but number of parameters should be equal  
SM.CompanyName,    
0 AS NotionalValue_Local,
0 AS NotionalValue_Base,
G.SettlementDate,   
CT.ExDate,  
CT.PayoutDate,
PT.AdditionalTradeAttributes
From PM_Taxlots PT                           
Inner Join                                                                                                                                
(Select distinct CaTaxlots.ParentRow_Pk from dbo.Split(@corpactionIDs,',') Splt                
Inner join PM_CorpActionTaxlots CaTaxlots on CaTaxlots.CorpActionID = Splt.Items) newTaxlots on newTaxlots.ParentRow_Pk = PT.Taxlot_PK                         
Inner join  T_Group G on G.GroupID=PT.GroupID                                                                                                                  
inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID   
Inner Join #SecMasterTable SM On SM.TickerSymbol = PT.Symbol                                     
LEFT OUTER JOIN T_CashTransactions CT ON CT.TaxLotID = PT.TaxLotID                                      
                                    
                                                                    
Drop Table #SecMasterTable                                                             
End    
  
