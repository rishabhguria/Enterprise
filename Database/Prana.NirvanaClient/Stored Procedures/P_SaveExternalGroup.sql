                                                   
CREATE PROCEDURE [dbo].[P_SaveExternalGroup] (
	@xml NTEXT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	)
AS
SET @ErrorNumber = 0                                                                                  
SET @ErrorMessage = 'Success'                                                                                  
                                                                                  
BEGIN TRAN TRAN1                                                                                

BEGIN TRY                                                       
	DECLARE @handle INT
                                                      
	EXEC sp_xml_preparedocument @handle OUTPUT
		,@xml
                                                       
	CREATE TABLE #ExternalOrder (
		ExternalOrderID VARCHAR(50)
		,OrderSideTagValue VARCHAR(10)
		,StartQuantity FLOAT
		,Symbol VARCHAR(100)
		,VenueID INT
		,AUECID INT
		,CounterPartyID INT
		,CostBasis FLOAT
		,PositionStartDate VARCHAR(50)
		,SettlementDate VARCHAR(50)
		,ExpirationDate VARCHAR(50)
		,AssetID INT
		,UnderLyingID INT
		,ExchangeID INT
		,CurrencyID INT
		,IntPranaMsgType INT
		,StrikePrice FLOAT
		,PutOrCall INT
		,TradingAccountID INT
		,UserID INT
		,FXRate FLOAT
		,FXConversionMethodOperator CHAR(5)
		,ProcessDate VARCHAR(50)
		,OriginalPurchaseDate VARCHAR(50)
		,ImportFileID INT
		,SettlCurrency INT NULL
		,SettlCurrencyName VARCHAR(50)
        ,ChangeType INT                          
		)
                        
	INSERT INTO #ExternalOrder (
		ExternalOrderID
		,OrderSideTagValue
		,StartQuantity
		,Symbol
		,VenueID
		,AUECID
		,CounterPartyID
		,CostBasis
		,PositionStartDate
		,SettlementDate
		,ExpirationDate
		,AssetID
		,UnderLyingID
		,ExchangeID
		,CurrencyID
		,IntPranaMsgType
		,StrikePrice
		,PutOrCall
		,TradingAccountID
		,UserID
		,FXRate
		,FXConversionMethodOperator
		,ProcessDate
		,OriginalPurchaseDate
		,ImportFileID
        ,SettlCurrency 
		,SettlCurrencyName
        ,ChangeType
)                         
	SELECT ExternalOrderID
		,SideTagValue
		,NetPosition
		,Symbol
		,VenueID
		,AUECID
		,CounterPartyID
		,CostBasis
		,PositionStartDate
		,PositionSettlementDate
		,ExpirationDate
		,AssetID
		,UnderlyingID
		,ExchangeID
		,CurrencyID
		,IntPranaMsgType
		,StrikePrice
		,PutCall
		,TradingAccountID
		,UserID
		,FXRate
		,FXConversionMethodOperator
		,ProcessDate
		,OriginalPurchaseDate
		,ImportFileID
		,SettlCurrency
		,SettlCurrencyName
        ,ChangeType
	FROM OPENXML(@handle, '//PositionMaster', 2) WITH (
			ExternalOrderID VARCHAR(50)
			,SideTagValue VARCHAR(10)
			,NetPosition FLOAT
			,Symbol VARCHAR(100)
			,VenueID INT
			,AUECID INT
			,CounterPartyID INT
			,CostBasis FLOAT
			,PositionStartDate VARCHAR(50)
			,PositionSettlementDate VARCHAR(50)
			,ExpirationDate VARCHAR(50)
			,AssetID INT
			,UnderlyingID INT
			,ExchangeID INT
			,CurrencyID INT
			,IntPranaMsgType INT
			,StrikePrice FLOAT
			,PutCall INT
			,TradingAccountID INT
			,UserID INT
			,FXRate FLOAT
			,FXConversionMethodOperator CHAR(5)
			,ProcessDate VARCHAR(50)
			,OriginalPurchaseDate VARCHAR(50)
			,ImportFileID INT
			,SettlCurrency INT 
			,SettlCurrencyName VARCHAR(50)
            ,ChangeType INT
 )                      

	INSERT INTO T_TradedOrders (
		ParentClOrderID
		,ClOrderID
		,OrderSideTagValue
		,Quantity
		,Symbol
		,VenueID
		,AUECID
		,CounterPartyID
		,AvgPrice
		,AUECLocalDate
		,SettlementDate
		,ExpirationDate
		,NirvanaMsgType
		,StrikePrice
		,PutOrCall
		,TradingAccountID
		,UserID
		,FXRate
		,FXConversionMethodOperator
		,IsInternal
		,ProcessDate
		,OriginalPurchaseDate
		,CumQty
		,ImportFileID
		,SettlCurrency		
        ,ChangeType
)                               
	SELECT ExternalOrderID
		,ExternalOrderID
		,OrderSideTagValue
		,StartQuantity
		,Symbol
		,VenueID
		,AUECID
		,CounterPartyID
		,CostBasis
		,PositionStartDate
		,SettlementDate
		,ExpirationDate
		,IntPranaMsgType
		,StrikePrice
		,PutOrCall
		,TradingAccountID
		,UserID
		,FXRate
		,FXConversionMethodOperator
		,'0'
		,ProcessDate
		,OriginalPurchaseDate
		,StartQuantity
		,NULLIF(ImportFileID, 0)
		,SettlCurrency		
        ,ChangeType
	FROM #ExternalOrder
                                                                      
	DROP TABLE #ExternalOrder
                        
	EXEC sp_xml_removedocument @handle
                                                                  
COMMIT TRANSACTION TRAN1                                               
END TRY                                                          
                                                
BEGIN CATCH                                                                                     
 SET @ErrorMessage = ERROR_MESSAGE();                                                                                    
 SET @ErrorNumber = Error_number();            

 ROLLBACK TRANSACTION TRAN1                                                                                       
END CATCH;                         
  --select * from T_TradedOrders                      
  --select * from T_Group        
    
