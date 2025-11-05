

Create PROCEDURE [dbo].[P_UpdateSecurityMasterData]           
(              
  @Xml nText        ,                                                                             
 @ErrorMessage varchar(500) output    ,                                         
 @ErrorNumber int output       
)              
AS                             
                            
  SET @ErrorNumber = 0                            
SET @ErrorMessage = 'Success'                       
                            
BEGIN TRAN TRAN1                          
BEGIN TRY                            
                           
  
DECLARE @handle int        
  
                                                 
exec sp_xml_preparedocument @handle OUTPUT,@xml  
Create TABLE #XmlItemUpdate                
(                
 ExchangeID   int  
, UnderlyingID  int  
, AUECID    int  
, AssetID    int  
, CusipSymbol       varchar(20)  
, SEDOLSymbol   varchar(20)                 
, ISINSymbol   varchar(20)   
, ReutersSymbol  varchar(20)     
, TickerSymbol  varchar(100)        
, BloombergSymbol varchar(20)      
, UnderlyingSymbol varchar(20)             
, CompanyName   varchar(50)  
,  Symbol_PK      varchar(100)  
,RoundLot bigint ,
CurrencyID int
,IsSecApproved bit
,ApprovalDate datetime
,ApprovedBy int
,Comments varchar(500) 
)     
             
INSERT INTO #XmlItemUpdate                
(                
 ExchangeID    
, UnderlyingID    
, AUECID   
,   AssetID    
, CusipSymbol     
, SEDOLSymbol    
, ISINSymbol    
, ReutersSymbol   
, TickerSymbol   
, BloombergSymbol  
, UnderlyingSymbol  
, CompanyName      
,   Symbol_PK   
,RoundLot  
,CurrencyID
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments 
)                
Select distinct               
  ExchangeID              
, UnderlyingID            
, AUECID  
, AssetID             
, CusipSymbol              
, SEDOLSymbol              
, ISINSymbol             
, ReutersSymbol       
, TickerSymbol   
, BloombergSymbol    
, UnderlyingSymbol   
, CompanyName   
, Symbol_PK   
,RoundLot  
,CurrencyID
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments
 
FROM  OPENXML(@handle, '//SecMasterTable',2)                                                                                                
 WITH                  
(        
  ExchangeID   int  
, UnderlyingID   int  
, AUECID    int  
, AssetID    int  
, CusipSymbol   varchar(20)  
, SEDOLSymbol   varchar(20)   
, ISINSymbol   varchar(20)   
, ReutersSymbol   varchar(20)   
, TickerSymbol   varchar(100)   
, BloombergSymbol varchar(20)    
, UnderlyingSymbol  varchar(20)   
, CompanyName   varchar(50)  
  
,Symbol_PK varchar(100)  
,RoundLot bigint  
,CurrencyID int
,IsSecApproved bit
,ApprovalDate datetime
,ApprovedBy int
,Comments varchar(500)

)  
  
update T_SMSymbolLookUpTable            
set 
--T_SMSymbolLookUpTable.TickerSymbol=COALESCE(#XmlItemUpdate.TickerSymbol,T_SMSymbolLookUpTable.TickerSymbol),
T_SMSymbolLookUpTable.ISINSymbol=COALESCE(#XmlItemUpdate.ISINSymbol,T_SMSymbolLookUpTable.ISINSymbol),
T_SMSymbolLookUpTable.SEDOLSymbol=COALESCE(#XmlItemUpdate.SEDOLSymbol,T_SMSymbolLookUpTable.SEDOLSymbol) , 
--T_SMSymbolLookUpTable.ReutersSymbol=COALESCE(#XmlItemUpdate.ReutersSymbol,T_SMSymbolLookUpTable.ReutersSymbol)  ,
T_SMSymbolLookUpTable.BloombergSymbol=COALESCE(#XmlItemUpdate.BloombergSymbol,T_SMSymbolLookUpTable.BloombergSymbol)  ,
T_SMSymbolLookUpTable.CusipSymbol=COALESCE(#XmlItemUpdate.CusipSymbol,  T_SMSymbolLookUpTable.CusipSymbol),
T_SMSymbolLookUpTable.CurrencyID=COALESCE(#XmlItemUpdate.CurrencyID,  T_SMSymbolLookUpTable.CurrencyID),
ModifiedDate=getdate(),
T_SMSymbolLookUpTable.IsSecApproved= COALESCE(#XmlItemUpdate.IsSecApproved,  T_SMSymbolLookUpTable.IsSecApproved),
T_SMSymbolLookUpTable.ApprovalDate= #XmlItemUpdate.ApprovalDate, 
T_SMSymbolLookUpTable.ApprovedBy= #XmlItemUpdate.ApprovedBy,
T_SMSymbolLookUpTable.Comments= #XmlItemUpdate.Comments

from #XmlItemUpdate  
where #XmlItemUpdate.TickerSymbol=T_SMSymbolLookUpTable.TickerSymbol  
           
  
            
            
Drop Table #XmlItemUpdate  
EXEC sp_xml_removedocument @handle     
COMMIT TRANSACTION TRAN1                              
                              
                             
END TRY                              
BEGIN CATCH                               
       SET @ErrorMessage = ERROR_MESSAGE();                              
 SET @ErrorNumber = Error_number();                      
 ROLLBACK TRANSACTION TRAN1                                 
END CATCH;            
  
