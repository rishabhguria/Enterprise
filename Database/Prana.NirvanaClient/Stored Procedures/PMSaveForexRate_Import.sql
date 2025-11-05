 CREATE PROCEDURE [dbo].[PMSaveForexRate_Import]                                
 (                                
   @Xml nText                                
 , @ErrorMessage varchar(500) output                                
 , @ErrorNumber int output    
 , @OldValues  xml output                               
 )                                
AS

SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0
                                
BEGIN TRAN TRAN1
                                 
                                
BEGIN TRY                                
                                
DECLARE @handle int
EXEC sp_xml_preparedocument	@handle OUTPUT, @Xml
           
  DECLARE @DeletedFXRates  TABLE                                                                                          
  (                                                                                           
    CurrencyPairID int                                                                      
   ,ConversionRate float     
   ,Date datetime                                   
   ,AccountID int                                     
   )                         
                                
  CREATE TABLE #TempForexRate                                                                                       
  (                                                                                       
    FromCurrencyID int                                
   ,ToCurrencyID int                                   
   ,Date datetime                              
   ,ConversionRate float                                 
   ,AccountID int                                 
   )
INSERT INTO #TempForexRate (FromCurrencyID
	,ToCurrencyID
	,Date
	,ConversionRate
	,AccountID)
	SELECT
		BaseCurrencyID,
		SettlementCurrencyID,
		Date,
		ForexPrice,
		AccountID
	FROM OPENXML(@handle, '//ForexPriceImport', 2)
	WITH
	(
	BaseCurrencyID int
	,SettlementCurrencyID int
	,Date datetime
	,ForexPrice float
	,AccountID int
	)
             
            
Declare @masterTableId int
SET @masterTableId = 0
            
            
Declare @count2 int
SET @count2 = 0
              
            
Declare                   
	@fromCurrencyID  int,                  
	@toCurrencyID int,                  
	@date Datetime,             
	@conversionRate float,
	@accountid int
                        
DECLARE CurrencyConversion_Cursor CURSOR FAST_FORWARD FOR SELECT
	FromCurrencyID,
	ToCurrencyID,
	Date,
	ConversionRate,
    AccountID

FROM #TempForexRate
            
            
Open CurrencyConversion_Cursor;
                                            
                                            
FETCH NEXT FROM CurrencyConversion_Cursor INTO                     
	@fromCurrencyID ,                  
	@toCurrencyID ,                  
	@date,                  
	@conversionRate,
	@accountid;
             
            
WHILE @@fetch_status = 0                                              
  BEGIN

SET @count2 = (SELECT
	CSP.CurrencyPairID
FROM T_CurrencyStandardPairs CSP
WHERE CSP.FromCurrencyID = @fromCurrencyID
AND CSP.ToCurrencyID = @toCurrencyID)
            
            
if(@count2>0)            
BEGIN
DELETE T_CurrencyConversionRate
OUTPUT DELETED.CurrencyPairID_FK,DELETED.ConversionRate,DELETED.Date,DELETED.FundID INTO @DeletedFXRates  
WHERE DATEDIFF(D, T_CurrencyConversionRate.Date, @date) = 0 AND T_CurrencyConversionRate.CurrencyPairID_FK = @count2
And T_CurrencyConversionRate.FundID = @accountid

INSERT INTO T_CurrencyConversionRate (CurrencyPairID_FK,
ConversionRate,Date,FundID)
	SELECT
		@count2,
		@conversionRate,
		@date,
        @accountid 
END
            
               
            
FETCH NEXT FROM CurrencyConversion_Cursor INTO                
	@fromCurrencyID ,                  
	@toCurrencyID ,                  
	@date,                  
	@conversionRate,
 	@accountid;
             
            
END
              
CLOSE CurrencyConversion_Cursor;
                                              
DEALLOCATE CurrencyConversion_Cursor;
DROP TABLE #TempForexRate

EXEC sp_xml_removedocument @handle
   
Set @OldValues =  
(  
SELECT   
CSP.FromCurrencyID,  
CSP.ToCurrencyID,  
Date,  
AccountID,  
ConversionRate  
FROM  
@DeletedFXRates DFX  
inner join T_CurrencyStandardPairs CSP  
ON DFX.CurrencyPairID = CSP.CurrencyPairID   
FOR XML PATH('DeletedFXRate')      
)                               
                                
COMMIT TRANSACTION TRAN1
                                
                                
END TRY                                
BEGIN CATCH
SET @ErrorMessage = ERROR_MESSAGE();
                                
print @errormessage
SET @ErrorNumber = ERROR_NUMBER();
                        
 ROLLBACK TRANSACTION TRAN1
                                 
END CATCH;
