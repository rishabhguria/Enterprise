                   
Create PROCEDURE [dbo].[PM_SaveCompanyAccountCashCurrencyValue]        
(                                                                          
                                
   @Xml nText --XML input                                                                  
 , @ErrorMessage varchar(500) output              
 , @ErrorNumber int output               
)              
AS                                                                           
SET @ErrorMessage = 'Success'               
SET @ErrorNumber = 0        
        
Declare @VarCashValueLocal float        
Declare @varCashValueBase float        
Declare @count int                       
Set @count=0                  
BEGIN TRAN TRAN1              
                
BEGIN TRY                  
DECLARE @handle int                                                                             
                                
exec sp_xml_preparedocument @handle OUTPUT,@Xml                
              
CREATE TABLE #XmlCashCurrency              
(                               
 BaseCurrencyID int,            
 LocalCurrencyID int,            
 CashValueBase float,           
 CashValueLocal float,          
 Date datetime,         
 AccountID int,        
 DataAdjustReq Varchar(10)        
)                                                                        
                                
INSERT INTO #XmlCashCurrency        
(                              
 BaseCurrencyID ,               
 LocalCurrencyID ,               
 CashValueBase ,                 
 CashValueLocal ,               
 Date ,                          
 AccountID,        
    DataAdjustReq              
)                                                                        
                                
SELECT            
 BaseCurrencyID ,                
 LocalCurrencyID ,               
 sum(CashValueBase) ,                
 sum(CashValueLocal) ,                
 Date ,                   
 AccountID,        
    DataAdjustReq                                
FROM                                                                        
                                
OPENXML(@handle, '//CashCurrencyValue', 2)                                                                             
                                
 WITH                          
 (                           
 BaseCurrencyID int ,            
 LocalCurrencyID int ,           
 CashValueBase float,           
 CashValueLocal float ,         
 Date datetime ,         
 AccountID int,        
    DataAdjustReq Varchar(10)                                                                                      
  )         
 Group By BaseCurrencyID,LocalCurrencyID,Date,AccountID,DataAdjustReq           
            
        
 Declare               
 @baseCurrencyID  int,              
 @localCurrencyID int,              
 @cashValueBase float,              
 @cashValueLocal float,         
 @date Datetime,        
 @AccountID int,           
 @dataAdjustReq varchar(10)           
        
              
 DECLARE CashCurrency_Cursor CURSOR FAST_FORWARD FOR                                          
 Select                
  BaseCurrencyID ,                
  LocalCurrencyID ,               
  CashValueBase ,                
  CashValueLocal ,                
  Date ,                   
  AccountID,        
  DataAdjustReq                                
 FROM  #XmlCashCurrency        
              
 Open CashCurrency_Cursor;        
        
 FETCH NEXT FROM CashCurrency_Cursor INTO                 
  @baseCurrencyID  ,              
  @localCurrencyID ,         
  @cashValueBase ,              
  @cashValueLocal ,              
  @date ,                    
  @AccountID ,           
  @dataAdjustReq           
        
WHILE @@fetch_status = 0                                          
  BEGIN         
    -- Adjust Data Case i.e required            
 If(@dataAdjustReq is not null And upper(@dataAdjustReq)='YES')        
  Begin        
   Set @count =(Select Count(*)         
    From PM_CompanyFundCashCurrencyValue where FundId=@AccountID and BaseCurrencyID=@baseCurrencyId and LocalCurrencyId=@localCurrencyID and DateDiff(d,@date,Date)=0)        
            If(@count > 0 )        
    Begin        
     Set @varCashValueLocal=(Select CashValueLocal         
      From PM_CompanyFundCashCurrencyValue where FundId=@AccountID and BaseCurrencyID=@baseCurrencyId and LocalCurrencyId=@localCurrencyID and DateDiff(d,@date,Date)=0)        
     Set @varCashValueBase=(Select CashValueBase       
      From PM_CompanyFundCashCurrencyValue where FundId=@AccountID and BaseCurrencyID=@baseCurrencyId and LocalCurrencyId=@localCurrencyID and DateDiff(d,@date,Date)=0)        
        
      Set @varCashValueLocal=@varCashValueLocal+@cashValueLocal        
      Set @varCashValueBase=@varCashValueBase+@cashValueBase        
        
      Update PM_CompanyFundCashCurrencyValue Set CashValueLocal=@varCashValueLocal,CashValueBase=@varCashValueBase        
      Where FundId=@AccountID and BaseCurrencyID=@baseCurrencyId and LocalCurrencyId=@localCurrencyID and DateDiff(d,@date,Date)=0        
    End        
        
    Else        
     Begin        
      Insert Into PM_CompanyFundCashCurrencyValue      
      (            
       Date,            
       FundID,            
       BaseCurrencyID,            
       CashValueBase,            
       LocalCurrencyID,            
       CashValueLocal            
      )               
                                    
      SELECT                                                                       
       @date,            
       @AccountID,            
       @baseCurrencyID,            
       @cashValueBase,           
       @localCurrencyID,            
       @cashValueLocal        
     End        
        
  End        
 -- Adjust Data Case not required i.e normal Case        
 Else        
  Begin        
    Delete from PM_CompanyFundCashCurrencyValue where DateDiff(d,Date,@date)=0           
    and FundID =@AccountID and BaseCurrencyID = @baseCurrencyID and LocalCurrencyID =@localCurrencyID          
           
    Insert Into PM_CompanyFundCashCurrencyValue        
    (            
     Date,            
     FundID,            
     BaseCurrencyID,            
     CashValueBase,            
     LocalCurrencyID,            
     CashValueLocal            
    )               
                                  
    SELECT                                                                       
     @date,            
     @AccountID,            
     @baseCurrencyID,            
     @cashValueBase,           
     @localCurrencyID,            
     @cashValueLocal        
  End        
        
        
        
 FETCH NEXT FROM CashCurrency_Cursor INTO                 
  @baseCurrencyID  ,              
  @localCurrencyID ,         
  @cashValueBase ,              
  @cashValueLocal ,              
  @date ,                    
  @AccountID ,           
  @dataAdjustReq         
        
END           
             
 CLOSE CashCurrency_Cursor;                                                
 DEALLOCATE CashCurrency_Cursor;          
 drop table  #XmlCashCurrency                                                                    
 --drop table #XmlTemp                              
 EXEC sp_xml_removedocument @handle                                  
                                 
 COMMIT TRANSACTION TRAN1                                                
                                 
 END TRY                                                                          
                                
 BEGIN CATCH                                         
                                 
  SET @ErrorMessage = ERROR_MESSAGE();                                            
                                 
  SET @ErrorNumber = Error_number();                                                                                                              
                                 
  ROLLBACK TRANSACTION TRAN1                                                                             
    
 END CATCH;       
      
  