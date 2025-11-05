
CREATE PROCEDURE [dbo].[P_UpdateSecurityMasterDataForSymbol_Import]                                                                                                            
(                                                                                                            
 @Xml varchar(max),                                                                                      
 @dataSource int,                                                                                     
 @ErrorMessage varchar(500) output,                                                                                                      
 @ErrorNumber int output                                                                                                     
)                                                                                                            
As        
                                                                                               
SET @ErrorNumber = 0                                                                                                                                    
SET @ErrorMessage = 'Success'  
                
BEGIN TRY                                                                                                                     
                                                                                                            
 BEGIN TRAN TRAN1                   
                  
--Declare @Xml varchar(max)                  
--Set @Xml=''                  
                  
DECLARE @handle int                                                                                         
exec sp_xml_preparedocument @handle OUTPUT,@Xml                                                                                                   
                                                                                                      
Create TABLE tmp_#XmlItem                                                                                                      
(                                                                                                      
ExchangeID   int                                                                                        
, UnderLyingID  int                                                                                        
, AUECID    int                                                                                        
, AssetID    int                                                                                        
, CusipSymbol varchar(20)                                                                                        
, SEDOLSymbol varchar(20)                                              
, ISINSymbol   varchar(20)                                                                                         
, ReutersSymbol  varchar(100)                                                                                           
, TickerSymbol  varchar(100)                                                                                              
, BloombergSymbol varchar(20)                          
, OSIOptionSymbol varchar(25)                          
, IDCOOptionSymbol varchar(25)                          
, OpraSymbol varchar(20)                          
, UnderLyingSymbol varchar(20)                                                                                                   
, CompanyName   varchar(50)                                                                                        
, Symbol_PK      varchar(100)                                                                                        
,RoundLot bigint                                                                                        
,CurrencyID int                                                                                        
,Sector varchar(20)                                                                                        
,LongName varchar(50)                           
,PutOrCall int                                         
,StrikePrice float                                                                                      
,ExpirationDate DateTime                         
,Multiplier float                             
,Delta float                                            
, LeadCurrencyID int                                            
, VsCurrencyID  int           
,IssueDate datetime                                 
,Coupon float                                   
,MaturityDate datetime                   
,SecurityType varchar(50)                                
,AccrualBasis varchar(50)                                                                                                     
,FirstCouponDate datetime                            
,IsZero bit                            
,CouponFrequencyID int                            
,DaysToSettlement  int                        
,CutOffTime varchar(50)       
,IsNDF bit      
,FixingDate datetime    
,IsSecApproved bit
,ApprovalDate datetime
,ApprovedBy int
,Comments varchar(500) 
,UDAAssetClassID int
,UDASecurityTypeID int
,UDASectorID int
,UDASubSectorID int
,UDACountryID int                             
)                                                                     
                                           
INSERT INTO tmp_#XmlItem                                      
(                                                                                                      
 ExchangeID                                                                                     
, UnderLyingID                                                           
, AUECID                                                                                        
, AssetID                                                                                   
, CusipSymbol                                                    
, SEDOLSymbol                                                                                                    
, ISINSymbol                                                                                
, ReutersSymbol                                                                                             
, TickerSymbol                                                                                         
, BloombergSymbol                          
, OSIOptionSymbol                          
, IDCOOptionSymbol                          
, OpraSymbol                                                                                          
, UnderLyingSymbol                                                                                         
, CompanyName                                                                     
,Symbol_PK                                                                                        
,RoundLot                                                                                        
,CurrencyID                                                                         
,Sector                                                                                        
,LongName                                                                                        
,PutOrCall                                                             
,StrikePrice                                                                  
, ExpirationDate                                                                                        
, Multiplier                                                                        
,Delta                                             
, LeadCurrencyID            
,VsCurrencyID          
          
,IssueDate                                  
,Coupon                                 
,MaturityDate                          
,SecurityType                                  
,AccrualBasis                              
,FirstCouponDate                            
,IsZero                             
,CouponFrequencyID                             
,DaysToSettlement                        
,CutOffTime      
,IsNDF       
,FixingDate 
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments                   
,UDAAssetClassID 
,UDASecurityTypeID 
,UDASectorID 
,UDASubSectorID 
,UDACountryID                                                                                        
)                                                                                            
Select distinct                                                                                           
ExchangeID                                                                                                    
, UnderLyingID                                                                                                  
, AUECID                                                                                        
, AssetID                                                                                              
, CusipSymbol                                                                                                    
, SEDOLSymbol                                                                                                    
, ISINSymbol                                                                                       
, ReutersSymbol                                                                                             
, TickerSymbol                  
, BloombergSymbol                          
, OSIOptionSymbol                          
, IDCOOptionSymbol                          
,IsNull(OpraSymbol,'')                                                                      
, UnderLyingSymbol                                                                    
, CompanyName                                                                                         
,Symbol_PK                       
,RoundLot                                                                                        
,CurrencyID                                                            
,Sector                                                                                        
,LongName                                                                
,PutOrCall                                                                                        
,StrikePrice                                                                                        
,ExpirationDate                                                                          
,Multiplier                                                                         
,Delta                                               
,LeadCurrencyID                                            
,VsCurrencyID          
          
,IssueDate                                  
,Coupon                                  
,MaturityDate                                 
,SecurityTypeID                                
,AccrualBasisID                                                                      
,FirstCouponDate                            
,IsZero                          
,CouponFrequencyID                             
,DaysToSettlement                                                                                                                    
,CutOffTime        
,IsNDF       
,FixingDate  
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments                                                              
,UDAAssetClassID 
,UDASecurityTypeID 
,UDASectorID 
,UDASubSectorID 
,UDACountryID                                                                                                     
                                                         
FROM  OPENXML(@handle, '//',3)         
 WITH                                                                                                        
(                                                               
  ExchangeID   int                                                                                        
, UnderLyingID   int                                                                                        
, AUECID    int                                                                       
, AssetID    int                                                                                        
, CusipSymbol   varchar(20)           
, SedolSymbol   varchar(20)                                                                                         
, ISINSymbol   varchar(20)                                                                                         
, ReutersSymbol   varchar(100)                                                
, TickerSymbol   varchar(100)                                                                                         
, BloombergSymbol varchar(20)                             
, OSIOptionSymbol varchar(25)                          
, IDCOOptionSymbol varchar(25)                          
, OpraSymbol varchar(20)                                                                               
, UnderLyingSymbol  varchar(20)                                                                                         
, CompanyName   varchar(50)                                                              
,Symbol_PK varchar(100)                                                                                        
,RoundLot bigint                                                                            
,CurrencyID int                                                                                        
,Sector varchar(20)                                                                                        
,LongName varchar(50)                                                                                        
,PutOrCall int                                                                                        
,StrikePrice float                                                                                   
,ExpirationDate DateTime                         
,Multiplier float                                                                         
,Delta float                                            
, LeadCurrencyID int                                            
,VsCurrencyID  int          
,IssueDate datetime                                 
,Coupon float                                   
,MaturityDate datetime                                 
,SecurityTypeID varchar(50)                                  
,AccrualBasisID varchar(50)                            
,FirstCouponDate datetime                            
,IsZero bit                            
,CouponFrequencyID int                            
,DaysToSettlement  int                        
,CutOffTime varchar(50)      
,IsNDF bit      
,FixingDate datetime     
,IsSecApproved bit
,ApprovalDate datetime
,ApprovedBy int
,Comments varchar(500) 
,UDAAssetClassID int
,UDASecurityTypeID int
,UDASectorID int
,UDASubSectorID int
,UDACountryID int                                                         
)                                                       
                  
Delete from tmp_#XmlItem where TickerSymbol is null                  
                  
Update tmp_#XmlItem                  
Set Symbol_PK = T_SMSymbolLookUpTable.Symbol_PK                  
From T_SMSymbolLookUpTable                  
Inner Join tmp_#XmlItem On tmp_#XmlItem.TickerSymbol=T_SMSymbolLookUpTable.TickerSymbol                  
                  
--select * from tmp_#XmlItem                  
                  
-- Common Data for All Assets                  
UPDATE  T_SMSymbolLookUpTable                                            
                                                         
SET                    
--,AUECID = tmp_#XmlItem.AUECID                                                                                                
--UnderLyingID  =                  
-- Case                   
-- When tmp_#XmlItem.UnderLyingID <> -2147483648 And tmp_#XmlItem.UnderLyingID <> 0                  
-- Then tmp_#XmlItem.UnderLyingID                   
-- Else T_SMSymbolLookUpTable.UnderLyingID                   
-- End                                                                                           
--,AssetID =                   
-- Case                   
-- When tmp_#XmlItem.AssetID <> -2147483648 And tmp_#XmlItem.AssetID <> 0                  
-- Then tmp_#XmlItem.AssetID                   
-- Else T_SMSymbolLookUpTable.AssetID                   
-- End                  
-- ,ExchangeID =                   
-- Case                   
-- When tmp_#XmlItem.ExchangeID <> -2147483648 And tmp_#XmlItem.ExchangeID <> 0                  
-- Then tmp_#XmlItem.ExchangeID                   
-- Else T_SMSymbolLookUpTable.ExchangeID                   
-- End                  
--,  
CurrencyID  =                    
 Case                   
 When tmp_#XmlItem.CurrencyID is not null And tmp_#XmlItem.CurrencyID <> -2147483648 And tmp_#XmlItem.CurrencyID <> 0                  
 Then tmp_#XmlItem.CurrencyID                  
 Else T_SMSymbolLookUpTable.CurrencyID                    
 End                  
,ISINSymbol =                  
 Case                   
 When tmp_#XmlItem.ISINSymbol is not null And tmp_#XmlItem.ISINSymbol <> ''                  
 Then tmp_#XmlItem.ISINSymbol                  
 Else T_SMSymbolLookUpTable.ISINSymbol                     
 End                  
,SEDOLSymbol =                  
 Case                   
 When tmp_#XmlItem.SEDOLSymbol is not null And tmp_#XmlItem.SEDOLSymbol <> ''                  
 Then tmp_#XmlItem.SEDOLSymbol                   
 Else T_SMSymbolLookUpTable.SEDOLSymbol                   
 End                  
,BloombergSymbol =                  
 Case                   
 When tmp_#XmlItem.BloombergSymbol is not null And tmp_#XmlItem.BloombergSymbol <> ''                  
 Then tmp_#XmlItem.BloombergSymbol                   
 Else T_SMSymbolLookUpTable.BloombergSymbol                   
 End                  
,CusipSymbol =                  
 Case                   
 When tmp_#XmlItem.CusipSymbol is not null And tmp_#XmlItem.CusipSymbol <> ''                  
 Then tmp_#XmlItem.CusipSymbol                   
 Else T_SMSymbolLookUpTable.CusipSymbol                   
 End                  
, OSISymbol =                  
 Case                   
 When tmp_#XmlItem.OSIOptionSymbol is not null And tmp_#XmlItem.OSIOptionSymbol <> ''                  
 Then tmp_#XmlItem.OSIOptionSymbol                    
 Else T_SMSymbolLookUpTable.OSISymbol                  
 End                  
, IDCOSymbol =                  
 Case                   
 When tmp_#XmlItem.IDCOOptionSymbol is not null And tmp_#XmlItem.IDCOOptionSymbol <> ''                  
 Then tmp_#XmlItem.IDCOOptionSymbol                  
 Else T_SMSymbolLookUpTable.IDCOSymbol                    
 End                  
, OpraSymbol =                  
 Case                   
 When tmp_#XmlItem.OpraSymbol is not null And tmp_#XmlItem.OpraSymbol <> ''                  
 Then tmp_#XmlItem.OpraSymbol                  
 Else T_SMSymbolLookUpTable.OpraSymbol                    
 End                  
,UnderlyingSymbol =                   
 Case                   
 When tmp_#XmlItem.UnderlyingSymbol is not null And tmp_#XmlItem.UnderlyingSymbol <> ''                  
 Then tmp_#XmlItem.UnderlyingSymbol                   
 Else T_SMSymbolLookUpTable.UnderlyingSymbol                   
 End                                                                                  
,Sector =                  
 Case                   
 When tmp_#XmlItem.Sector is not null And tmp_#XmlItem.Sector <> ''                  
 Then tmp_#XmlItem.Sector                   
 Else T_SMSymbolLookUpTable.Sector                   
 End                  
,RoundLot =                  
 Case                   
  When tmp_#XmlItem.RoundLot is not null And tmp_#XmlItem.RoundLot <> 0  And tmp_#XmlItem.RoundLot <> -2147483648                
  Then tmp_#XmlItem.RoundLot                   
  Else T_SMSymbolLookUpTable.RoundLot                   
 End
,IsSecApproved=                  
 Case                   
 When tmp_#XmlItem.IsSecApproved is not null And tmp_#XmlItem.ExpirationDate <> 0
 Then tmp_#XmlItem.IsSecApproved                  
 Else 0                    
 End 
,
ApprovalDate=                  
 Case                   
 When tmp_#XmlItem.ApprovalDate is not null And tmp_#XmlItem.ApprovalDate <> '' And tmp_#XmlItem.ApprovalDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.ApprovalDate <> '1/1/1800 12:00:00 AM'                 
 Then tmp_#XmlItem.ApprovalDate                  
 Else '1/1/1800 12:00:00 AM'                    
 End 

,ApprovedBy=                  
 Case                   
 When tmp_#XmlItem.ApprovedBy <> -2147483648 And tmp_#XmlItem.ApprovedBy <> 0 And tmp_#XmlItem.ApprovedBy <> 1                  
 Then tmp_#XmlItem.ApprovedBy                   
 Else 0                       
 End 
,Comments=                  
 Case                   
 When tmp_#XmlItem.Comments is not null And tmp_#XmlItem.Comments <> ''
 Then tmp_#XmlItem.Comments                  
 Else ''                    
 End    
 
 ,UDAAssetClassID=                  
 Case                   
 When tmp_#XmlItem.UDAAssetClassID is not null And tmp_#XmlItem.UDAAssetClassID <> -2147483648 And tmp_#XmlItem.UDAAssetClassID <> 0                  
 Then tmp_#XmlItem.UDAAssetClassID                  
 Else T_SMSymbolLookUpTable.UDAAssetClassID                    
 End

,UDASecurityTypeID=                  
 Case                   
 When tmp_#XmlItem.UDASecurityTypeID is not null And tmp_#XmlItem.UDASecurityTypeID <> -2147483648 And tmp_#XmlItem.UDASecurityTypeID <> 0                  
 Then tmp_#XmlItem.UDASecurityTypeID                  
 Else T_SMSymbolLookUpTable.UDASecurityTypeID                    
 End

,UDASectorID=                  
 Case                   
 When tmp_#XmlItem.UDASectorID is not null And tmp_#XmlItem.UDASectorID <> -2147483648 And tmp_#XmlItem.UDASectorID <> 0                  
 Then tmp_#XmlItem.UDASectorID                  
 Else T_SMSymbolLookUpTable.UDASectorID                    
 End

,UDASubSectorID=                  
 Case                   
 When tmp_#XmlItem.UDASubSectorID is not null And tmp_#XmlItem.UDASubSectorID <> -2147483648 And tmp_#XmlItem.UDASubSectorID <> 0                  
 Then tmp_#XmlItem.UDASubSectorID                  
 Else T_SMSymbolLookUpTable.UDASubSectorID                    
 End

,UDACountryID=                  
 Case                   
 When tmp_#XmlItem.UDACountryID is not null And tmp_#XmlItem.UDACountryID <> -2147483648 And tmp_#XmlItem.UDACountryID <> 0                  
 Then tmp_#XmlItem.UDACountryID                  
 Else T_SMSymbolLookUpTable.UDACountryID                    
 End
                                                                                     
From  tmp_#XmlItem           
Where  tmp_#XmlItem.Symbol_PK = T_SMSymbolLookUpTable.Symbol_PK                    
                  
-- Non History data for Equity                  
Update T_SMEquityNonHistoryData                                                                                                  
Set                                                                           
Multiplier  =                  
 Case                   
 When tmp_#XmlItem.Multiplier <> -2147483648 And tmp_#XmlItem.Multiplier <> 0 And tmp_#XmlItem.Multiplier <> 1                  
 Then tmp_#XmlItem.Multiplier            
 Else T_SMEquityNonHistoryData.Multiplier                  
 End                   
,CompanyName=                  
 Case                   
 When tmp_#XmlItem.LongName is not null And tmp_#XmlItem.LongName <> ''                  
 Then tmp_#XmlItem.LongName                    
 Else T_SMEquityNonHistoryData.CompanyName                  
 End                  
,Delta  =                  
 Case                   
 When tmp_#XmlItem.Delta <> -2147483648 And tmp_#XmlItem.Delta <> 0 And tmp_#XmlItem.Delta <> 1                  
 Then tmp_#XmlItem.Delta                   
 Else T_SMEquityNonHistoryData.Delta                  
 End                   
From  tmp_#XmlItem where  tmp_#XmlItem.Symbol_PK = T_SMEquityNonHistoryData.Symbol_PK                   
                  
--Option specific Data                  
                  
Update T_SMOptiondata                                                                                                  
Set                                                                                                   
Multiplier  =                  
 Case                   
 When tmp_#XmlItem.Multiplier <> -2147483648 And tmp_#XmlItem.Multiplier <> 0 And tmp_#XmlItem.Multiplier <> 1                  
 Then tmp_#XmlItem.Multiplier                   
 Else T_SMOptiondata.Multiplier                  
 End                   
,ContractName=                  
 Case                   
 When tmp_#XmlItem.LongName is not null And tmp_#XmlItem.LongName <> ''                  
 Then tmp_#XmlItem.LongName                    
 Else T_SMOptiondata.ContractName                  
 End                  
,Expirationdate=                  
 Case                   
 When tmp_#XmlItem.ExpirationDate is not null And tmp_#XmlItem.ExpirationDate <> '' And tmp_#XmlItem.ExpirationDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.ExpirationDate <> '1/1/1800 12:00:00 AM'                 
 Then tmp_#XmlItem.ExpirationDate                  
 Else T_SMOptiondata.Expirationdate                    
 End                  
,Strike=                  
 Case                   
 When tmp_#XmlItem.StrikePrice <> -2147483648 And tmp_#XmlItem.StrikePrice <> 0                  
 Then tmp_#XmlItem.StrikePrice                  
 Else T_SMOptiondata.Strike                    
 End  

                
From  tmp_#XmlItem where  tmp_#XmlItem.Symbol_PK = T_SMOptiondata.Symbol_PK                   
                  
-- Future Data                  
Update T_SMFutureData                  
SET                    
Multiplier  =                  
 Case                   
 When tmp_#XmlItem.Multiplier <> -2147483648 And tmp_#XmlItem.Multiplier <> 0 And tmp_#XmlItem.Multiplier <> 1                  
 Then tmp_#XmlItem.Multiplier                   
Else T_SMFutureData.Multiplier                  
 End                   
,ContractName=                  
 Case                   
 When tmp_#XmlItem.LongName is not null And tmp_#XmlItem.LongName <> ''                  
 Then tmp_#XmlItem.LongName                    
Else T_SMFutureData.ContractName                  
 End                  
,ExpirationDate=                  
 Case                   
 When tmp_#XmlItem.ExpirationDate is not null And tmp_#XmlItem.ExpirationDate <> '' And tmp_#XmlItem.ExpirationDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.ExpirationDate <> '1/1/1800 12:00:00 AM'                 
 Then tmp_#XmlItem.ExpirationDate                  
 Else T_SMFutureData.ExpirationDate                    
 End                  
From  tmp_#XmlItem where  tmp_#XmlItem.Symbol_PK = T_SMFutureData.Symbol_PK                   
           
                  
-- Fx Data                  
Update T_SMFxData                                                                                        
Set                                                                       
LongName=                  
 Case                   
 When tmp_#XmlItem.LongName is not null And tmp_#XmlItem.LongName <> ''                  
 Then tmp_#XmlItem.LongName                    
 Else T_SMFxData.LongName                  
 End                  
,LeadCurrencyID=                  
 Case                   
 When tmp_#XmlItem.LeadCurrencyID <> -2147483648 And tmp_#XmlItem.LeadCurrencyID <> 0                  
 Then tmp_#XmlItem.LeadCurrencyID                  
 Else T_SMFxData.LeadCurrencyID                    
 End                          
,VsCurrencyID =                  
 Case                   
 When tmp_#XmlItem.VsCurrencyID <> -2147483648 And tmp_#XmlItem.VsCurrencyID <> 0                  
 Then tmp_#XmlItem.VsCurrencyID                  
 Else T_SMFxData.VsCurrencyID                    
 End          
,FixingDate=                  
 Case                   
 When tmp_#XmlItem.FixingDate is not null And tmp_#XmlItem.FixingDate <> '' And tmp_#XmlItem.FixingDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.FixingDate <> '1/1/1800 12:00:00 AM'             
 Then tmp_#XmlItem.FixingDate                    
 Else T_SMFxData.FixingDate                  
 End    
,ExpirationDate=                  
 Case                   
 When tmp_#XmlItem.ExpirationDate is not null And tmp_#XmlItem.ExpirationDate <> '' And tmp_#XmlItem.ExpirationDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.ExpirationDate <> '1/1/1800 12:00:00 AM'                 
 Then tmp_#XmlItem.ExpirationDate                  
 Else T_SMFxData.ExpirationDate                    
 End     
,Multiplier  =                  
 Case                   
 When tmp_#XmlItem.Multiplier <> -2147483648 And tmp_#XmlItem.Multiplier <> 0 And tmp_#XmlItem.Multiplier <> 1                  
 Then tmp_#XmlItem.Multiplier                   
Else T_SMFxData.Multiplier                  
 End                             
From  tmp_#XmlItem where  tmp_#XmlItem.Symbol_PK = T_SMFxData.Symbol_PK                    
                              
Update T_SMFXForwardData                                                                                        
Set                                                                       
LongName=                  
 Case                   
 When tmp_#XmlItem.LongName is not null And tmp_#XmlItem.LongName <> ''                  
 Then tmp_#XmlItem.LongName                    
 Else T_SMFXForwardData.LongName                  
 End                  
,LeadCurrencyID=                  
 Case                  
 When tmp_#XmlItem.LeadCurrencyID <> -2147483648 And tmp_#XmlItem.LeadCurrencyID <> 0                  
 Then tmp_#XmlItem.LeadCurrencyID                  
 Else T_SMFXForwardData.LeadCurrencyID                    
 End                                            
,VsCurrencyID=                  
 Case                   
 When tmp_#XmlItem.VsCurrencyID <> -2147483648 And tmp_#XmlItem.VsCurrencyID <> 0                  
 Then tmp_#XmlItem.VsCurrencyID                  
 Else T_SMFXForwardData.VsCurrencyID                    
 End                   
,ExpirationDate=                  
 Case                   
 When tmp_#XmlItem.ExpirationDate is not null And tmp_#XmlItem.ExpirationDate <> '' And tmp_#XmlItem.ExpirationDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.ExpirationDate <> '1/1/1800 12:00:00 AM'                 
 Then tmp_#XmlItem.ExpirationDate                  
 Else T_SMFXForwardData.ExpirationDate                    
 End                  
,Multiplier  =                  
 Case                   
 When tmp_#XmlItem.Multiplier <> -2147483648 And tmp_#XmlItem.Multiplier <> 0 And tmp_#XmlItem.Multiplier <> 1                  
 Then tmp_#XmlItem.Multiplier                   
 Else T_SMFXForwardData.Multiplier                  
 End       
 ,FixingDate=                  
 Case                   
 When tmp_#XmlItem.FixingDate is not null And tmp_#XmlItem.FixingDate <> '' And tmp_#XmlItem.FixingDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.FixingDate <> '1/1/1800 12:00:00 AM'             
 Then tmp_#XmlItem.FixingDate                    
 Else T_SMFXForwardData.FixingDate                  
 End                        
From  tmp_#XmlItem where  tmp_#XmlItem.Symbol_PK = T_SMFXForwardData.Symbol_PK                    
                  
Update T_SMIndexData                                                                                        
Set                                                                                         
LongName=                  
 Case                   
 When tmp_#XmlItem.LongName is not null And tmp_#XmlItem.LongName <> ''                  
 Then tmp_#XmlItem.LongName                    
 Else T_SMIndexData.LongName              
 End                            
From  tmp_#XmlItem where  tmp_#XmlItem.Symbol_PK = T_SMIndexData.Symbol_PK            
          
Update T_SMFixedIncomeData                                                                                        
Set                                                                                         
BondDescription=                  
 Case                   
 When tmp_#XmlItem.LongName is not null And tmp_#XmlItem.LongName <> ''                  
 Then tmp_#XmlItem.LongName                    
 Else T_SMFixedIncomeData.BondDescription                  
 End           
,IssueDate=                  
 Case                   
 When tmp_#XmlItem.IssueDate is not null And tmp_#XmlItem.IssueDate <> '' And tmp_#XmlItem.IssueDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.IssueDate <> '1/1/1800 12:00:00 AM'                
 Then tmp_#XmlItem.IssueDate                    
 Else T_SMFixedIncomeData.IssueDate                  
 End            
,Coupon=                  
 Case                   
 When tmp_#XmlItem.Coupon is not null And tmp_#XmlItem.Coupon <> -2147483648  And tmp_#XmlItem.Coupon <> 0               
 Then tmp_#XmlItem.Coupon                    
 Else T_SMFixedIncomeData.Coupon                  
 End          
,MaturityDate=                  
 Case                   
 When tmp_#XmlItem.MaturityDate is not null And tmp_#XmlItem.MaturityDate <> '' And tmp_#XmlItem.MaturityDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.MaturityDate <> '1/1/1800 12:00:00 AM'             
 Then tmp_#XmlItem.MaturityDate                    
 Else T_SMFixedIncomeData.MaturityDate                  
 End          
,BondTypeID=                  
 Case                   
 When tmp_#XmlItem.SecurityType is not null And tmp_#XmlItem.SecurityType <> -2147483648  And tmp_#XmlItem.SecurityType <> 0               
 Then tmp_#XmlItem.SecurityType                    
 Else T_SMFixedIncomeData.BondTypeID                  
 End           
,AccrualBasisID=                  
 Case                   
 When tmp_#XmlItem.AccrualBasis is not null And tmp_#XmlItem.AccrualBasis <> -2147483648  And tmp_#XmlItem.AccrualBasis <> 0               
 Then tmp_#XmlItem.AccrualBasis                    
 Else T_SMFixedIncomeData.AccrualBasisID                  
 End            
,FirstCouponDate=                  
 Case                   
 When tmp_#XmlItem.FirstCouponDate is not null And tmp_#XmlItem.FirstCouponDate <> '' And tmp_#XmlItem.FirstCouponDate <> '1800-01-01 12:00:00.000' And tmp_#XmlItem.FirstCouponDate <> '1/1/1800 12:00:00 AM'             
 Then tmp_#XmlItem.FirstCouponDate                    
 Else T_SMFixedIncomeData.FirstCouponDate                  
 End           
,CouponFrequencyID=               
 Case                   
 When tmp_#XmlItem.CouponFrequencyID is not null And tmp_#XmlItem.CouponFrequencyID <> -2147483648  And tmp_#XmlItem.CouponFrequencyID <> 0               
 Then tmp_#XmlItem.CouponFrequencyID                    
 Else T_SMFixedIncomeData.CouponFrequencyID                  
 End             
,DaysToSettlement=                  
 Case                   
 When tmp_#XmlItem.DaysToSettlement is not null And tmp_#XmlItem.DaysToSettlement <> -2147483648  And tmp_#XmlItem.DaysToSettlement <> 0               
 Then tmp_#XmlItem.DaysToSettlement                    
 Else T_SMFixedIncomeData.DaysToSettlement                  
 End           
,Multiplier  =                  
 Case                   
 When tmp_#XmlItem.Multiplier <> -2147483648 And tmp_#XmlItem.Multiplier <> 0 And tmp_#XmlItem.Multiplier <> 1                  
 Then tmp_#XmlItem.Multiplier                   
 Else T_SMFixedIncomeData.Multiplier                  
 End           
-- IsZero is a bit value, need to ckeck                              
From  tmp_#XmlItem where  tmp_#XmlItem.Symbol_PK = T_SMFixedIncomeData.Symbol_PK                  
                 
                  
                   
--Select * from T_SMSymbolLookUpTable                  
--Inner Join tmp_#XmlItem On tmp_#XmlItem.Symbol_PK=T_SMSymbolLookUpTable.Symbol_PK                  
--Where tmp_#XmlItem.OSIOptionSymbol is not null                   
--and  tmp_#XmlItem.OSIOptionSymbol <> ''                  
--                  
--Update  T_SMReuters                                                                          
--SET                                           
----AUECID =#XmlItem.AUECID ,                                                                          
--ExchangeID=#XmlItem.ExchangeID,                                                                          
--ReutersSymbol=#XmlItem.ReutersSymbol                                                                         
--from  #XmlItem where  #XmlItem.Symbol_PK = T_SMReuters.Symbol_PK                   
                  
                  
-- Save Dynamic UDAs XML to T_UDA_DynamicUDAData table
	CREATE TABLE #TempDynamicUDADataXmlSymbolWise ( Symbol_PK BIGINT, UDAData XML, FundID INT)

	INSERT INTO #TempDynamicUDADataXmlSymbolWise ( Symbol_PK, UDAData, FundID)
	SELECT Symbol_PK, DynamicUDAs, 0
	FROM OPENXML(@handle, '//', 3) WITH (
			Symbol_PK VARCHAR(100)
			,DynamicUDAs XML
			)
	-- Deleting data for symbols where any dynamic UDA is not defined
	DELETE FROM #TempDynamicUDADataXmlSymbolWise WHERE UDAData IS NULL

	-- check for issuer field, if it is same as underlying symbols's description, then remove it
	SELECT	TSM.Symbol_PK, COALESCE(ENHD.CompanyName, OD.ContractName, FD.ContractName, FXD.LongName, FXFD.LongName, FID.BondDescription, 'Undefined') AS Issuer
	INTO	#IssuerTable
	FROM	#TempDynamicUDADataXmlSymbolWise TSM
			LEFT JOIN T_SMSymbolLookUpTable SM		ON SM.TickerSymbol = (SELECT UnderLyingSymbol FROM T_SMSymbolLookUpTable WHERE Symbol_PK = TSM.Symbol_PK) 
			LEFT JOIN T_SMEquityNonHistoryData ENHD	ON ENHD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMOptionData OD				ON OD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFutureData FD				ON FD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFxData FXD				ON FXD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFxForwardData FXFD		ON FXFD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFixedIncomeData FID		ON FID.Symbol_PK = SM.Symbol_PK
SET QUOTED_IDENTIFIER ON
	DECLARE @SymbolPK BIGINT, @Issuer VARCHAR(100)

	WHILE EXISTS(SELECT 1 FROM #IssuerTable)
	BEGIN
		SET	@SymbolPK	= (SELECT TOP 1 Symbol_PK FROM #IssuerTable)
		SELECT @Issuer	= Issuer FROM #IssuerTable WHERE Symbol_PK = @SymbolPK

		UPDATE	#TempDynamicUDADataXmlSymbolWise
		SET		UDAData.modify('delete /DynamicUDAs/Issuer')
		WHERE	UDAData.exist('/DynamicUDAs/Issuer') = 1 AND
				UDAData.value('(/DynamicUDAs/Issuer/text())[1]','VARCHAR(100)') = @Issuer AND
				Symbol_PK = @SymbolPK

		DELETE FROM #IssuerTable WHERE Symbol_PK = @SymbolPK
	END   

	-- insert dynamic UDAs in T_UDA_DynamicUDAData table
	WHILE EXISTS( SELECT 1 FROM #TempDynamicUDADataXmlSymbolWise)
	BEGIN
		DECLARE @Symbol_PK BIGINT, @UDAData XML, @FundID INT

		SET @Symbol_PK = (SELECT TOP 1 Symbol_PK FROM #TempDynamicUDADataXmlSymbolWise)
		SELECT @UDAData = UDAData, @FundID = FundID FROM #TempDynamicUDADataXmlSymbolWise WHERE Symbol_PK = @Symbol_PK

		IF(@UDAData IS NOT NULL)
		BEGIN
			SET @UDAData.modify('delete /DynamicUDAs/Symbol')

			-- call procedure to store dynamic UDA data
			EXEC P_UDA_SaveDynamicUDAData
			@Symbol_PK, @UDAData, @FundID
		END

		DELETE FROM #TempDynamicUDADataXmlSymbolWise WHERE Symbol_PK = @Symbol_PK
	
	END	
SET QUOTED_IDENTIFIER OFF               
DROP TABLE #TempDynamicUDADataXmlSymbolWise
DROP TABLE tmp_#XmlItem
DROP TABLE #IssuerTable                      
                  
EXEC sp_xml_removedocument @handle                                                            
COMMIT TRANSACTION TRAN1                                           
                                            
                                                                                                                   
END TRY                                                                        
BEGIN CATCH                                                                                                          
SET @ErrorMessage = ERROR_MESSAGE();                                            
SET @ErrorNumber = Error_number();                                                                                                            
ROLLBACK TRANSACTION TRAN1                                                        
END CATCH;
