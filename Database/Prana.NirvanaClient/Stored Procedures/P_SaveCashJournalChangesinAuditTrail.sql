CREATE PROCEDURE [dbo].[P_SaveCashJournalChangesinAuditTrail]
(
@cashAuditCollection NTEXT 
)
AS                                                                                                                                                                                                                                                           
  BEGIN TRY                 
      DECLARE @handle INT                 
                
      EXEC Sp_xml_preparedocument                 
        @handle OUTPUT,                 
        @cashAuditCollection                 
                
      -- Audit Trail Temporary Table that will contain Mark Price changed entries                
      CREATE TABLE #Temp_AuditEntries                 
        (
			TransactionDate	DATETIME,
			CurrencyName	VARCHAR(100),
			AccountId		INT,
			Comments		VARCHAR(MAX),
			ActionDate		DATETIME, 
			UserId			INT,
			CashAccountID		INT,		
			Symbol			VARCHAR(100),
			DR				MONEY,		
			CR				MONEY,		
			FXRate		    FLOAT		
        )                 
                
      INSERT INTO #Temp_AuditEntries                 
                  (	TransactionDate,
					CurrencyName,
					AccountId,
					Comments,
					ActionDate, 
					UserId,
					CashAccountID,
					Symbol,
					DR,
					CR,
					FXRate)                 
      SELECT	TransactionDate,
				CurrencyName,
				AccountId,
				Comments,
				ActionDate,
				UserId,
				CashAccountID,
				Symbol,
				DR,
				CR,
				FXRate                 
      FROM OPENXML(@handle, '/NewDataSet/Sheet1', 2)                 
                WITH ( TransactionDate	DATETIME,                
                       CurrencyName		VARCHAR(100),                 
					   AccountId		INT,
                       Comments			VARCHAR(max),
					   ActionDate		DATETIME,
					   UserId			INT,
					   CashAccountId	INT,		
					   Symbol			VARCHAR(100),
					   DR				MONEY,		
					   CR				MONEY,		
					   FXRate		    FLOAT		
					 )  

	INSERT INTO T_CashjournalAudit
		 (	TransactionDate,                 
             Currency,                 
             AccountId,                 
             Comments,
			 IsProcessed,
			 ActionDate,
			 UserId,
			 CashAccountID,
			 Symbol,
			 DR,
			 CR,
			 FXRate)
	SELECT TransactionDate,                 
             CurrencyName,                 
             AccountId,                 
             Comments,
			 0, -- this will be unprocessed data
			ActionDate, 
			UserId,
			CashAccountID,
			Symbol,
			DR,
			CR,
			FXRate      
	FROM	#Temp_AuditEntries

	EXEC Sp_xml_removedocument                 
        @handle      
               
  END TRY                 
                
  BEGIN CATCH                 
  END CATCH;       