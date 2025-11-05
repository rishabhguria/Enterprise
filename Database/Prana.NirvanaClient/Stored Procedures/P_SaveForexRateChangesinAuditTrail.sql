CREATE PROCEDURE [dbo].[P_SaveForexRateChangesinAuditTrail]     
(       
@tradeAuditCollection NTEXT    
)       
AS       
  BEGIN try       
      DECLARE @handle INT       
      
      EXEC Sp_xml_preparedocument       
        @handle output,       
        @tradeAuditCollection       
      
      -- Audit Trail Temporary Table that will contain Forex Rate changed entries                 
      CREATE TABLE #Temp_AuditEntries       
        (       
           ActionDate    DATETIME,       
           OriginalDate  DATETIME,       
           GroupID       VARCHAR(50),       
           Action        VARCHAR(50),       
           OriginalValue FLOAT,       
           Comment       VARCHAR(50),       
           CompanyUserId INT,       
           Symbol        VARCHAR(50),       
           Level1ID      INT       
        )       
      
      INSERT INTO #Temp_AuditEntries       
                  (ActionDate,       
                   OriginalDate,       
                   GroupID,       
                   Action,       
                   OriginalValue,       
                   Comment,       
                   CompanyUserId,       
                   Symbol,       
                   Level1ID)       
      SELECT AUECLocalDate,       
             OriginalDate,       
             GroupID,       
             Action,       
             OriginalValue,       
             Comment,       
             CompanyUserId,       
             Symbol,       
             Level1ID       
      FROM   OPENXML(@handle, '/NewDataSet/Sheet1', 2)       
                WITH (AUECLocalDate DATETIME,       
                      OriginalDate  DATETIME,       
                      GroupID       VARCHAR(50),       
                      Action        VARCHAR(50),       
                      OriginalValue FLOAT,       
                      Comment       VARCHAR(50),       
                      CompanyUserId INT,       
                      Symbol        VARCHAR(50),       
                      Level1ID      INT)       
      
      -- Insert Data into Audit Trail Table directly for Forex Rate Fund Wise changed entries                 
      INSERT INTO T_TradeAudit       
                  (ActionDate,       
                   GroupID,       
                   TaxlotID,       
                   TaxlotClosingID,       
                   Action,       
                   OriginalValue,       
                   Comment,       
                   CompanyUserId,       
                   Symbol,       
                   FundID,       
                   OrderSideTagvalue,       
                   OriginalDate,       
                   Isprocessed)       
      SELECT ActionDate,       
             GroupID,       
             NULL,       
             NULL,       
             '87',--Audit action id corresponding to Forex Rate Changed (Fund Wise)       
             OriginalValue,       
             Comment,       
             CompanyUserId,       
             Symbol,       
             Level1ID,       
             NULL,       
             OriginalDate,       
             0       
      FROM   #Temp_AuditEntries       
      WHERE  #Temp_AuditEntries.Level1ID <> 0       
      
      -- Delete Forex Rate Fund Wise changed entries from Temporary Table                 
      DELETE FROM #Temp_AuditEntries       
      WHERE  Level1ID <> 0       
      
      -- Temporary Table that will contain Forex Rate changed entries including CurrencyID      
      CREATE TABLE #T_TempFxRatesAudit       
        (       
           ID            INT IDENTITY (1, 1),       
           AUECLocalDate DATETIME,       
           OriginalDate  DATETIME,       
           Action        VARCHAR(50),       
           OriginalValue FLOAT,       
           Comment       VARCHAR(50),       
           CompanyUserId INT,       
           Symbol        VARCHAR(50),       
           Level1ID      INT,       
           CurrencyID    INT       
        )       
      
      INSERT INTO #T_TempFxRatesAudit      
      SELECT ActionDate,       
             TA.OriginalDate,       
             TA.Action,       
             OriginalValue,       
             Comment,       
             CompanyUserId,       
             Symbol,       
             Level1ID,       
      CSP.FromCurrencyID AS CurrencyID       
      FROM   #Temp_AuditEntries TA       
             INNER JOIN T_CurrencyStandardPairs CSP       
                     ON TA.Symbol = CSP.eSignalSymbol       
      WHERE  CSP.FromCurrencyID <> 1       
      
      INSERT INTO #T_TempFxRatesAudit       
      SELECT ActionDate,       
             OriginalDate,       
             TA.Action,       
             OriginalValue,       
             Comment,       
             CompanyUserId,       
             Symbol,       
             Level1ID,       
             CSP.ToCurrencyID AS CurrencyID       
      FROM   #Temp_AuditEntries TA       
             INNER JOIN T_CurrencyStandardPairs CSP       
                     ON TA.Symbol = CSP.eSignalSymbol       
      WHERE  CSP.ToCurrencyID <> 1       
      
  
      -- Temp Table that will contain all changed forex rate data for which any symbol is open in given date    
      CREATE TABLE #OpenCurrencies       
        (       
           FundID        INT,       
           Symbol        VARCHAR(100),       
           OriginalDate  DATETIME,       
           OriginalValue FLOAT,       
           CompanyUserId INT       
        )       
      
	  SELECT DISTINCT cal.FundID, cal.CurrencyID 
	  INTO #FundsWithCurrencyExists	 
	  FROM T_LastCalculatedBalanceDate cal WITH(NOLOCK)
	  INNER JOIN T_SubAccounts sub WITH(NOLOCK) ON cal.SubAcID = sub.SubAccountID
	  INNER JOIN T_TransactionType ty WITH(NOLOCK) ON sub.TransactionTypeID = ty.TransactionTypeID
	  INNER JOIN T_CompanyFunds fund WITH(NOLOCK) ON cal.FundID = fund.CompanyFundID
	  WHERE ty.TransactionTypeAcronym IN ('Cash', 'ACB')
                
            INSERT INTO #OpenCurrencies(FundID,Symbol,OriginalDate,OriginalValue,CompanyUserId)                             
	  Select DiSTINCT FundID, Symbol, OriginalDate, OriginalValue, CompanyUserId From #FundsWithCurrencyExists fund
	  INNER JOIN #T_TempFxRatesAudit temp ON fund.CurrencyID = temp.CurrencyID	     
      
      -- Delete Data from #OpenCurrencies that is already in Audit Trail Table                 
      DELETE OC       
      FROM  #OpenCurrencies OC       
      INNER JOIN T_TradeAudit TA       
      ON OC.FundID = TA.FundID AND OC.Symbol = TA.Symbol AND OC.OriginalDate = TA.OriginalDate       
      WHERE  TA.Action = 87       
     
      INSERT INTO T_TradeAudit       
                  (ActionDate,       
                   GroupID,       
                   TaxlotID,       
                   TaxlotClosingID,       
                   Action,       
                   OriginalValue,       
                   Comment,       
                   CompanyUserId,       
                   Symbol,       
                   FundID,       
                   OrderSideTagvalue,       
                   OriginalDate,       
                   Isprocessed)       
      SELECT Getdate(),       
             -2147483648,       
             NULL,       
             NULL,       
             50,       
             OriginalValue,       
             'Forex Rate Changed',       
             CompanyUserId,       
             Symbol,       
             FundID,       
             NULL,       
             OriginalDate,       
             0       
     FROM   #OpenCurrencies 
	 UNION
      SELECT ActionDate,
             -2147483648,
             NULL,       
             NULL,
             Action,
             OriginalValue,       
             Comment,       
             CompanyUserId,       
             Symbol,       
             Level1ID,
             NULL,       
             OriginalDate,       
             0  
     FROM   #Temp_AuditEntries	    
      
      EXEC Sp_xml_removedocument       
        @handle       
  END try       
      
  BEGIN catch       
  END catch;       
      
  DROP TABLE #Temp_AuditEntries, #OpenCurrencies, #T_TempFxRatesAudit 