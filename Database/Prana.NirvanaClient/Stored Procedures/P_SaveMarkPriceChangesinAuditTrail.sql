CREATE PROCEDURE [dbo].[P_SaveMarkPriceChangesinAuditTrail]     
(                 
 @tradeAuditCollection NTEXT    
)                 
AS                                                                                                                                                                                                                                                           
  BEGIN try                 
      DECLARE @handle INT                 
                
      EXEC Sp_xml_preparedocument                 
        @handle output,                 
        @tradeAuditCollection                 
                
      -- Audit Trail Temporary Table that will contain Mark Price changed entries                
      CREATE TABLE #Temp_AuditEntries                 
        (                 
           AUECLocalDate DATETIME,                 
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
                  (AUECLocalDate,                 
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
                WITH ( AUECLocalDate DATETIME,                 
                       OriginalDate  DATETIME,                 
                       GroupID       VARCHAR(50),                 
                       Action        VARCHAR(50),                 
                       OriginalValue FLOAT,                 
                       Comment       VARCHAR(50),                 
                       CompanyUserId INT,                 
                       Symbol        VARCHAR(50),                 
                       Level1ID      INT )                 
                   
      -- Insert Data into Audit Trail Table directly for Mark Price Fund Wise changed entries                 
      INSERT INTO T_TradeAudit                 
                  (ActionDate,                 
                   GroupId,                 
                   TaxlotId,                 
                   TaxlotClosingId,                 
                   Action,                 
                   OriginalValue,                 
                   Comment,                 
                   CompanyUserId,                 
                   Symbol,                 
                   FundID,                 
                   OrderSideTagValue,                 
                   OriginalDate,                 
                   IsProcessed)                 
      SELECT AUECLocalDate,                 
             GroupId,                 
             NULL,                 
             NULL,                 
             '86',-- This is the AuditActionID corresponding to Mark Price Changed (Fund Wise)                 
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
    
      -- Delete Mark Price Fund Wise changed entries from Temporary Table                
      DELETE FROM #Temp_AuditEntries                 
      WHERE  Level1ID <> 0                 
    
      -- Temp Table that will contain disticnt datewise Mark Price changed entries     
      -- It will hold the comma separated symbols date wise.               
      CREATE TABLE #MarkPriceData                 
        (         
           ID            INT IDENTITY(1, 1),                 
           OriginalValue FLOAT,                 
           CompanyUserID INT,                 
           OriginalDate  DATETIME,                 
           Symbols       VARCHAR(max)                 
        )                 
                
      INSERT INTO #MarkPriceData                 
                  (OriginalValue,                 
                   CompanyUserID,                 
                   OriginalDate,                 
                   Symbols)                 
      SELECT Main.OriginalValue,                 
             Main.CompanyUserID,                 
             Main.OriginalDate,                 
             LEFT(Main.#Temp_AuditEntries, Len(Main.#Temp_AuditEntries) - 1) AS                 
             "Symbols"                 
      FROM   (SELECT DISTINCT 0 as OriginalValue,    --https://jira.nirvanasolutions.com:8443/browse/CI-3956             
                              AE2.CompanyUserID,                 
                              AE2.OriginalDate,                 
                              (SELECT AE1.Symbol + ', ' AS [text()]                 
                               FROM   dbo.#Temp_AuditEntries AE1                 
                               WHERE  AE1.OriginalDate = AE2.OriginalDate                 
                               ORDER  BY AE1.OriginalDate                 
                               FOR xml path ('')) [#Temp_AuditEntries]                 
              FROM   dbo.#Temp_AuditEntries AE2) [Main]                 
      ORDER  BY Main.OriginalDate                 
    
      -- This table will be used to fetch all the open positions date wise as there might be multiple days' mark price changes.               
      CREATE TABLE #PM_Taxlots                 
        (                 
           FundID        INT,                 
           Symbol        VARCHAR(100),                 
           OriginalDate  DATETIME,                 
           OriginalValue FLOAT,                 
           CompanyUserID INT                 
        )                 
                 
      -- Data Insertion in Temp Table(#pm_taxlots) using while loop                
      DECLARE @ID INT                 
      DECLARE @Date DATETIME                 
      DECLARE @SymbolList VARCHAR(max)                 
      DECLARE @OriginalValue FLOAT                 
      DECLARE @CompanyUserId INT                 
                      
      DECLARE @Symbols TABLE                 
      (                 
           Symbols VARCHAR(max)                 
      );                 
                
      SET @ID = 1                 
                
      WHILE (@ID <= (SELECT Count(*) FROM #MarkPriceData))                                            
        BEGIN        
            
            SELECT @Date = OriginalDate, @SymbolList = Symbols, @OriginalValue = OriginalValue, @CompanyUserId = CompanyUserID     
            From #MarkPriceData Where ID = @ID                   
                
            INSERT INTO @Symbols                 
            SELECT Items AS Symbols                 
            FROM  dbo.Split(@SymbolList, ',')                            
      
            INSERT INTO #PM_Taxlots (fundid,Symbol,OriginalDate,OriginalValue,CompanyUserID)                    
            SELECT DISTINCT FundID,Symbol,@Date,@OriginalValue,@CompanyUserId        
            FROM   PM_Taxlots                 
            WHERE  TaxLotOpenQty <> 0                 
                   AND TaxLot_PK IN (SELECT Max(TaxLot_PK) FROM PM_Taxlots WHERE  Datediff(d,PM_Taxlots.AUECModifiedDate,@Date) >= 0                   
                                     GROUP  BY TaxlotID)                
           AND Symbol IN(SELECT dbo.Trim(Symbols) FROM @Symbols)                                   
           
            DELETE FROM @Symbols                         
            SET @ID = @ID + 1                 
        END                 
    
      -- Delete Data from #pm_taxlots that is already in Mark Price Table for same date and fund.               
      DELETE PMT FROM #PM_Taxlots PMT                
      INNER JOIN T_TradeAudit TA  WITH (NOLOCK)               
      ON PMT.FundID = TA.FundID                     
      AND PMT.Symbol = TA.Symbol                                
      AND PMT.OriginalDate = TA.OriginalDate                                   
      Where TA.Action = 86                                     
                
      INSERT INTO T_TradeAudit                 
                  (ActionDate,                 
                   GroupId,                 
                   TaxlotId,                 
                   TaxlotClosingId,                 
                   Action,                 
                   OriginalValue,                 
                   Comment,                 
                   CompanyUserId,                 
                   Symbol,                 
                   FundID,                 
                   OrderSideTagValue,                 
                   OriginalDate,                 
                   IsProcessed)                 
      SELECT Getdate(),                 
             -2147483648,                 
             NULL,                 
             NULL,                 
             49,                 
             AE.OriginalValue,                 
             'Mark Price Changed',                 
             PMT.CompanyUserId,                 
             PMT.Symbol,                 
             FundID,                 
             NULL,                 
             PMT.OriginalDate,                 
             0                 
      FROM   #PM_Taxlots PMT
	  inner join #Temp_AuditEntries AE on pmt.OriginalDate=AE.OriginalDate and PMT.Symbol=AE.Symbol            
           
      EXEC Sp_xml_removedocument                 
        @handle      
               
  END try                 
                
  BEGIN catch                 
  END catch;                 
                
  DROP TABLE #Temp_AuditEntries, #PM_Taxlots, #MarkPriceData 