CREATE PROCEDURE [dbo].[P_SaveThirdPartyBlockAllocations] @AvgPx FLOAT  
 ,@Currency NVARCHAR(8)  
 ,@ISIN VARCHAR(20)  
 ,@Cusip VARCHAR(20)  
 ,@Sedol VARCHAR(20)  
 ,@Quantity FLOAT  
 ,@SideID VARCHAR(3)  
 ,@Symbol VARCHAR(100)  
 ,@TransactTime DATETIME  
 ,@SettlDate DATETIME  
 ,@AllocID NVARCHAR(100)  
 ,@AllocTransType VARCHAR(3)  
 ,@TradeDate DATETIME  
 ,@AllocStatus VARCHAR(3)  
 ,@NetMoney FLOAT  
 ,@Commission FLOAT  
 ,@GrossTradeAmt FLOAT  
 ,@AllocationXml NTEXT  
 ,@thirdPartyBatchId INT = NULL  
 ,@runDate DATETIME = NULL  
 ,@transmissionType INT = NULL  
 ,@msgType VARCHAR(3)  
 ,@SubStatus NVARCHAR(MAX)  
 ,@Text VARCHAR(MAX) = NULL  
 ,@AllocReportId VARCHAR(MAX) = NULL  
 ,@TransmissionTime DATETIME  
 ,@FixMsg VARCHAR(MAX)  
AS  
BEGIN  
 BEGIN TRY  
 BEGIN TRANSACTION T1  
  
  DECLARE @jobId INT;  
  
  IF @msgType = 'J'  
  BEGIN  
   IF EXISTS (  
     SELECT TOP 1 1  
     FROM T_ThirdPartyDailyJobs  
     WHERE ThirdPartyBatchId = @thirdPartyBatchId  
      AND CAST(BatchRunDate AS DATE) = cast(@runDate AS DATE)  
      AND TransmissionType = @transmissionType  
     )  
   BEGIN  
    SET @jobId = (  
      SELECT TOP 1 JobId  
      FROM T_ThirdPartyDailyJobs  
      WHERE ThirdPartyBatchId = @thirdPartyBatchId  
       AND CAST(BatchRunDate AS DATE) = cast(@runDate AS DATE)  
       AND TransmissionType = @transmissionType  
      )  
   END  
   ELSE  
   BEGIN  
    INSERT INTO T_ThirdPartyDailyJobs (  
     ThirdPartyBatchId  
     ,BatchRunDate  
     ,TransmissionType  
     )  
    VALUES (  
     @thirdPartyBatchId  
     ,@runDate  
     ,@transmissionType  
     )  
  
    SET @jobId = SCOPE_IDENTITY();  
   END  
  END  
  ELSE  
  BEGIN  
   SET @jobId = (  
     SELECT TOP 1 JobId  
     FROM T_ThirdPartyAllocationBlocks  
     WHERE AllocID = @AllocID  
     )  
  END  
  
  DECLARE @BlockId INT;  
  
  IF @msgType = 'J'  
   AND RIGHT(@AllocID, 1) = 'C'  
  BEGIN  
   DECLARE @allocIdWithoutSuffix VARCHAR(100) = LEFT(@AllocID, LEN(@AllocID) - 1);  
  
   SET @BlockId = (  
     SELECT TOP 1 BlockId  
     FROM T_ThirdPartyAllocationBlocks  
     WHERE AllocID IN (  
       @allocIdWithoutSuffix  
       ,@allocIdWithoutSuffix + 'N'  
       )  
      AND MsgType = 'J'  
     );  
  
   UPDATE T_ThirdPartyAllocationBlocks  
   SET AllocID = @AllocID  
    ,AllocStatus = '9'  
    ,SubStatus = '27'  
   WHERE LEFT(@AllocID, LEN(@AllocID) - 1) IN (  
     LEFT(AllocID, LEN(AllocID) - 1)  
     ,AllocID  
     )  
  END  
  ELSE  
  BEGIN  
   DECLARE @InsertionRequired BIT = 1;  
  
   IF (  
     @msgType IN (  
      'AK'  
      ,'AU'  
      ,'AS'  
      ,'AT'  
      )  
     AND EXISTS (  
      SELECT TOP 1 1  
      FROM T_ThirdPartyAllocationBlocks  
      WHERE AllocID = @AllocID  
       AND MsgType = @msgType  
      )  
     )  
    OR (  
     @msgType = 'J'  
     AND RIGHT(@allocId, 1) = 'R'  
     )  
   BEGIN  
    SET @InsertionRequired = 0;  
   END  
  
   IF @InsertionRequired = 1  
   BEGIN  
    INSERT INTO T_ThirdPartyAllocationBlocks (  
     JobId  
     ,MsgType  
     ,AvgPx  
     ,Currency  
     ,ISIN  
     ,Cusip  
     ,Sedol  
     ,Quantity  
     ,SideID  
     ,Symbol  
     ,TransactTime  
     ,SettlDate  
     ,AllocID  
     ,AllocTransType  
     ,TradeDate  
     ,AllocStatus  
     ,NetMoney  
     ,Commission  
     ,GrossTradeAmt  
     ,SubStatus  
     ,[Text]  
     ,AllocReportId  
     ,TransmissionTime  
     )  
    SELECT @jobId  
     ,@msgType  
     ,@AvgPx  
     ,(  
      SELECT CurrencyID  
      FROM T_Currency  
      WHERE CurrencySymbol = @Currency  
      )  
     ,@ISIN  
     ,@Cusip  
     ,@Sedol  
     ,@Quantity  
     ,@SideID  
     ,@Symbol  
     ,@TransactTime  
     ,@SettlDate  
     ,@AllocID  
     ,@AllocTransType  
     ,@TradeDate  
     ,@AllocStatus  
     ,@NetMoney  
     ,@Commission  
     ,@GrossTradeAmt  
     ,@SubStatus  
     ,@Text  
     ,@AllocReportId  
     ,@TransmissionTime  
  
    SET @BlockId = SCOPE_IDENTITY();  
  
    IF (@msgType = 'J')  
    BEGIN  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET JBlockId = BlockId  
     WHERE BlockId = @BlockId  
    END  
    ELSE  
    BEGIN  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET JBlockId = (  
       SELECT TOP 1 BlockId  
       FROM T_ThirdPartyAllocationBlocks  
       WHERE AllocId = @AllocID  
        AND MsgType = 'J'  
       )  
     WHERE BlockId = @BlockId  
    END  
   END  
   ELSE  
   BEGIN  
    IF (  
      @msgType = 'J'  
      AND RIGHT(@allocId, 1) = 'R'  
      )  
    BEGIN  
     SELECT BlockId  
     INTO #BlockIdTemp  
     FROM T_ThirdPartyAllocationBlocks  
     WHERE LEFT(@AllocId, LEN(@AllocId) - 1) IN (  
       AllocId  
       ,LEFT(AllocId, LEN(AllocId) - 1)  
       )  
  
     DELETE  
     FROM T_ThirdPartyAllocationMessages  
     WHERE BlockId IN (  
       SELECT BlockId  
       FROM #BlockIdTemp  
       )  
  
     DELETE  
     FROM T_ThirdPartyAllocationBlocks  
     WHERE BlockId IN (  
       SELECT BlockId  
       FROM #BlockIdTemp  
       )  
      AND MsgType <> 'J'  
  
     UPDATE [dbo].[T_ThirdPartyAllocationBlocks]  
     SET [AvgPx] = @AvgPx  
      ,[Currency] = (  
       SELECT CurrencyID  
       FROM T_Currency  
       WHERE CurrencySymbol = @Currency  
       )  
      ,[ISIN] = @ISIN  
      ,[Cusip] = @Cusip  
      ,[Sedol] = @Sedol  
      ,[Quantity] = @Quantity  
      ,[SideID] = @SideId  
      ,[Symbol] = @Symbol  
      ,[TransactTime] = @TransactTime  
      ,[SettlDate] = @SettlDate  
      ,[AllocID] = @AllocID  
      ,[AllocTransType] = @AllocTransType  
      ,[TradeDate] = @TradeDate  
      ,[AllocStatus] = @AllocStatus  
      ,[NetMoney] = @NetMoney  
      ,[Commission] = @Commission  
      ,[GrossTradeAmt] = @GrossTradeAmt  
      ,[SubStatus] = @SubStatus  
      ,[Text] = @Text  
      ,[TransmissionTime] = @TransmissionTime  
     WHERE AllocID = LEFT(@AllocId, LEN(@AllocId) - 1)  
      AND MsgType = 'J'  
    END  
  
    SET @BlockId = (  
      SELECT TOP 1 BlockId  
      FROM T_ThirdPartyAllocationBlocks  
      WHERE AllocID = @AllocID  
       AND MsgType = @msgType  
      )  
   END  
  
   DECLARE @handle INT  
  
   EXEC sp_xml_preparedocument @handle OUTPUT  
    ,@AllocationXml  
  
   CREATE TABLE #AllocationTemp (  
    IndividualAllocID NVARCHAR(100)  
    ,AllocAccount VARCHAR(100)  
    ,AllocQty FLOAT  
    ,AllocAvgPx FLOAT  
    ,Commission FLOAT  
    ,[Misc Fees] FLOAT  
    ,NetMoney FLOAT  
    ,AllocID NVARCHAR(100)  
    ,MatchStatus VARCHAR(MAX)  
    ,AllocText VARCHAR(MAX)  
    ,TradeDate DATETIME  
    ,ConfirmID VARCHAR(MAX)  
    ,ConfirmRefID VARCHAR(MAX)  
    ,ConfirmTransType VARCHAR(1)  
    ,ConfirmType VARCHAR(1)  
    ,ConfirmStatus VARCHAR(1)  
    ,AffirmStatus VARCHAR(1)  
    ,ConfirmRejReason VARCHAR(1)  
    )  
  
   DECLARE @InsertionRequiredInTaxlotLevel BIT = 1;  
  
   INSERT INTO #AllocationTemp (  
    IndividualAllocID  
    ,AllocAccount  
    ,AllocQty  
    ,AllocAvgPx  
    ,Commission  
    ,[Misc Fees]  
    ,NetMoney  
    ,AllocID  
    ,MatchStatus  
    ,AllocText  
    ,TradeDate  
    ,ConfirmID  
    ,ConfirmRefID  
    ,ConfirmTransType  
    ,ConfirmType  
    ,ConfirmStatus  
    ,AffirmStatus  
    ,ConfirmRejReason  
    )  
   SELECT IndividualAllocID  
    ,AllocAccount  
    ,AllocQty  
    ,AllocAvgPx  
    ,Commission  
    ,MiscFeeAmt  
    ,AllocNetMoney  
    ,AllocID  
    ,MatchStatus  
    ,AllocText  
    ,TradeDate  
    ,ConfirmID  
    ,ConfirmRefID  
    ,ConfirmTransType  
    ,ConfirmType  
    ,ConfirmStatus  
    ,AffirmStatus  
    ,ConfirmRejReason  
   FROM OPENXML(@handle, '/DocumentElement/Allocation', 2) WITH (  
     IndividualAllocID NVARCHAR(100)  
     ,AllocAccount VARCHAR(100)  
     ,AllocQty FLOAT  
     ,AllocAvgPx FLOAT  
     ,Commission FLOAT  
     ,MiscFeeAmt FLOAT  
     ,AllocNetMoney FLOAT  
     ,AllocID NVARCHAR(100)  
     ,MatchStatus VARCHAR(MAX)  
     ,AllocText VARCHAR(MAX)  
     ,TradeDate DATETIME  
     ,ConfirmID VARCHAR(MAX)  
     ,ConfirmRefID VARCHAR(MAX)  
     ,ConfirmTransType VARCHAR(1)  
     ,ConfirmType VARCHAR(1)  
     ,ConfirmStatus VARCHAR(1)  
     ,AffirmStatus VARCHAR(1)  
     ,ConfirmRejReason VARCHAR(1)  
     ) AS A  
  
   IF (@msgType = 'AU')  
   BEGIN  
    DECLARE @IndividualAllocIdForAU NVARCHAR(100) = (  
      SELECT TOP 1 AM.IndividualAllocId  
      FROM T_ThirdPartyAllocationMessages AM  
      JOIN #AllocationTemp AT ON AM.ConfirmID = AT.ConfirmID  
      )  
  
    UPDATE #AllocationTemp  
    SET IndividualAllocID = @IndividualAllocIdForAU  
   END  
  
   IF (  
     @InsertionRequired = 0  
     AND EXISTS (  
      SELECT TOP 1 1  
      FROM T_ThirdPartyAllocationMessages AM  
      JOIN #AllocationTemp T ON AM.IndividualAllocID = T.IndividualAllocID  
       AND AM.BlockId = @BlockId  
      )  
     )  
   BEGIN  
    UPDATE T_ThirdPartyAllocationMessages  
    SET AllocAccount = T.AllocAccount  
     ,AllocQty = T.AllocQty  
     ,AllocAvgPx = T.AllocAvgPx  
     ,Commission = T.Commission  
     ,[Misc Fees] = T.[Misc Fees]  
     ,NetMoney = T.NetMoney  
     ,MatchStatus = T.MatchStatus  
     ,AllocText = T.AllocText  
     ,TradeDate = T.TradeDate  
     ,ConfirmID = T.ConfirmID  
     ,ConfirmRefID = T.ConfirmRefID  
     ,ConfirmTransType = T.ConfirmTransType  
     ,ConfirmType = T.ConfirmType  
     ,ConfirmStatus = T.ConfirmStatus  
     ,AffirmStatus = T.AffirmStatus  
     ,ConfirmRejReason = T.ConfirmRejReason  
    FROM T_ThirdPartyAllocationMessages AM  
    JOIN #AllocationTemp T ON AM.IndividualAllocID = T.IndividualAllocID  
    WHERE AM.BlockId = @BlockId  
  
    SET @InsertionRequiredInTaxlotLevel = 0;  
   END  
  
   IF (@InsertionRequiredInTaxlotLevel = 1)  
   BEGIN  
    INSERT INTO T_ThirdPartyAllocationMessages (  
     BlockId  
     ,IndividualAllocID  
     ,AllocAccount  
     ,AllocQty  
     ,AllocAvgPx  
     ,Commission  
     ,[Misc Fees]  
     ,NetMoney  
     ,MatchStatus  
     ,AllocText  
     ,TradeDate  
     ,ConfirmID  
     ,ConfirmRefID  
     ,ConfirmTransType  
     ,ConfirmType  
     ,ConfirmStatus  
     ,AffirmStatus  
     ,ConfirmRejReason  
     )  
    SELECT @BlockId  
     ,CASE   
      WHEN @msgType = 'AU'  
       THEN @IndividualAllocIdForAU  
      ELSE IndividualAllocID  
      END  
     ,AllocAccount  
     ,AllocQty  
     ,AllocAvgPx  
     ,Commission  
     ,[Misc Fees]  
     ,NetMoney  
     ,MatchStatus  
     ,AllocText  
     ,TradeDate  
     ,ConfirmID  
     ,ConfirmRefID  
     ,ConfirmTransType  
     ,ConfirmType  
     ,ConfirmStatus  
     ,AffirmStatus  
     ,ConfirmRejReason  
    FROM #AllocationTemp A  
   END  
  
   IF (@msgType = 'P')  
   BEGIN  
    IF (@AllocStatus = '1')  
    BEGIN  
     UPDATE AM  
     SET MatchStatus = '16'  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
     WHERE AB.AllocID = @AllocID  
    END  
    ELSE IF (  
      (  
       SELECT COUNT(1)  
       FROM #AllocationTemp  
       ) > 0  
      )  
    BEGIN  
     UPDATE AM  
     SET MatchStatus = TEMP.MatchStatus  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN #AllocationTemp TEMP ON AM.IndividualAllocID = TEMP.IndividualAllocID  
     IF (  
        SELECT COUNT(DISTINCT MatchStatus)  
        FROM T_ThirdPartyAllocationMessages AM  
        WHERE BlockId = @BlockId  
        ) > 1  
      BEGIN  
       SET @SubStatus = '17';  
      END  
      ELSE  
      BEGIN  
       SET @SubStatus = (  
         SELECT TOP 1 MatchStatus  
         FROM T_ThirdPartyAllocationMessages AM  
         WHERE BlockId = @BlockId  
         )  
      END  
    END  
    ELSE IF (@AllocStatus = '10')  
    BEGIN  
     UPDATE AM  
     SET MatchStatus = @SubStatus  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
     WHERE AB.AllocID = @AllocID  
    END  
    ELSE  
    BEGIN  
     UPDATE AM  
     SET MatchStatus = '5'  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
     WHERE AB.AllocID = @AllocID  
    END  
  
    UPDATE T_ThirdPartyAllocationBlocks  
    SET AllocStatus = @AllocStatus  
     ,SubStatus = @SubStatus  
    WHERE AllocID = @AllocID;  
   END  
  
   IF (@msgType = 'AK')  
   BEGIN  
    UPDATE T_ThirdPartyAllocationMessages  
    SET MatchStatus = TEMP.MatchStatus  
    FROM T_ThirdPartyAllocationMessages AM  
    JOIN #AllocationTemp TEMP ON AM.IndividualAllocID = TEMP.IndividualAllocID  
    WHERE TEMP.MatchStatus <> '5'  
  
    IF EXISTS (  
      SELECT TOP 1 1  
      FROM #AllocationTemp TEMP  
      WHERE TEMP.ConfirmStatus <> '1'  
      )  
    BEGIN  
     DECLARE @newSubstatus VARCHAR(MAX);  
     DECLARE @newBlockstatus VARCHAR(MAX);  
     DECLARE @TotalJTaxlots INT = (  
       SELECT COUNT(1)  
       FROM T_ThirdPartyAllocationMessages AM  
       JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
       WHERE AB.AllocID = @AllocID  
        AND AB.MsgType = 'J'  
       )  
     DECLARE @TotalAKTaxlots INT = (  
       SELECT COUNT(1)  
       FROM T_ThirdPartyAllocationMessages AM  
       JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
       WHERE AB.AllocID = @AllocID  
        AND AB.MsgType = 'AK'  
       )  
  
     IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '22'  
       )  
     BEGIN  
      SET @newBlockstatus = '2'  
      SET @newSubstatus = '7'  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus IN (  
         '15'  
         ,'16'  
         )  
       )  
     BEGIN  
      SET @newBlockstatus = '2';  
  
      IF (  
        SELECT COUNT(DISTINCT MatchStatus)  
        FROM T_ThirdPartyAllocationMessages AM  
        WHERE BlockId = @BlockId  
         AND MatchStatus IN (  
          '15'  
          ,'16'  
          )  
        ) > 1  
      BEGIN  
       SET @newSubstatus = '13';  
      END  
      ELSE  
      BEGIN  
       SET @newSubstatus = (  
         SELECT TOP 1 MatchStatus  
         FROM T_ThirdPartyAllocationMessages AM  
         WHERE BlockId = @BlockId  
          AND MatchStatus IN (  
           '15'  
           ,'16'  
           )  
         )  
      END  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus IN (  
         '9'  
         ,'10'  
         ,'11'  
         ,'12'  
         ,'13'  
         ,'23'  
         )  
       )  
     BEGIN  
      SET @newBlockstatus = '8';  
  
      IF (  
        SELECT COUNT(DISTINCT MatchStatus)  
        FROM T_ThirdPartyAllocationMessages AM  
        WHERE BlockId = @BlockId  
         AND MatchStatus IN (  
          '9'  
          ,'10'  
          ,'11'  
          ,'12'  
          ,'13'  
          ,'23'  
          )  
        ) > 1  
      BEGIN  
       SET @newSubstatus = '13';  
      END  
      ELSE  
      BEGIN  
       SET @newSubstatus = (  
         SELECT TOP 1 MatchStatus  
         FROM T_ThirdPartyAllocationMessages AM  
         WHERE BlockId = @BlockId  
          AND MatchStatus IN (  
           '9'  
           ,'10'  
           ,'11'  
           ,'12'  
           ,'13'  
           ,'23'  
           )  
         )  
      END  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '8'  
       )  
     BEGIN  
      SET @newBlockstatus = '8'  
      SET @newSubstatus = '4'  
     END  
     ELSE IF NOT EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus <> '6'  
       )  
      AND (@TotalJTaxlots = @TotalAKTaxlots)  
     BEGIN  
      SET @newBlockstatus = '0'  
      SET @newSubstatus = '0'  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '6'  
       )  
     BEGIN  
      SET @newBlockstatus = '-1'  
      SET @newSubstatus = '3'  
     END  
  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET AllocStatus = @newBlockstatus  
      ,SubStatus = @newSubstatus  
     WHERE AllocID = @AllocID;  
    END  
   END  
  
   IF (@msgType = 'AU')  
   BEGIN  
    UPDATE #AllocationTemp  
    SET MatchStatus = AM.MatchStatus  
    FROM T_ThirdPartyAllocationMessages AM  
    JOIN #AllocationTemp TEMP ON AM.ConfirmID = TEMP.ConfirmID  
    WHERE TEMP.AffirmStatus = '1'  
  
    UPDATE #AllocationTemp  
    SET MatchStatus = '22'  
    WHERE AffirmStatus = '2'  
  
    UPDATE #AllocationTemp  
    SET MatchStatus = CASE   
      WHEN AM.MatchStatus = '6'  
       THEN '6'  
      ELSE CASE   
        WHEN AM.MatchStatus = '8'  
         THEN '20'  
        ELSE '21'  
        END  
      END  
    FROM T_ThirdPartyAllocationMessages AM  
    JOIN #AllocationTemp TEMP ON AM.ConfirmID = TEMP.ConfirmID  
    WHERE TEMP.AffirmStatus = '3'  
  
    UPDATE T_ThirdPartyAllocationMessages  
    SET MatchStatus = TEMP.MatchStatus  
    FROM T_ThirdPartyAllocationMessages AM  
    JOIN #AllocationTemp TEMP ON AM.IndividualAllocID = TEMP.IndividualAllocID  
  
    IF EXISTS (  
      SELECT TOP 1 1  
      FROM #AllocationTemp TEMP  
      WHERE TEMP.AffirmStatus <> '1'  
      )  
    BEGIN  
     DECLARE @LatestJBlockId INT = (  
       SELECT TOP 1 BlockId  
       FROM T_ThirdPartyAllocationBlocks  
       WHERE AllocID = @AllocID  
        AND MsgType = 'J'  
       ORDER BY BlockId DESC  
       )  
     DECLARE @LatestAUBlockId INT = (  
       SELECT TOP 1 BlockId  
       FROM T_ThirdPartyAllocationBlocks  
       WHERE AllocID = @AllocID  
        AND MsgType = 'AU'  
       ORDER BY BlockId DESC  
       )  
     DECLARE @JTaxlots INT = (  
       SELECT COUNT(1)  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE AM.BlockId = @LatestJBlockId  
       )  
     DECLARE @TotalAUTaxlots INT = (  
       SELECT COUNT(1)  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE AM.BlockId = @LatestAUBlockId  
       )  
     DECLARE @TotalPendingTaxlots INT = (  
       SELECT COUNT(1)  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE AM.BlockId = @LatestJBlockId  
        AND MatchStatus = '5'  
       )  
     DECLARE @newSubstatusAU VARCHAR(MAX);  
     DECLARE @newBlockstatusAU VARCHAR(MAX);  
  
     IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '22'  
       )  
     BEGIN  
      SET @newBlockstatusAU = '2'  
      SET @newSubstatusAU = '7'  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus IN (  
         '15'  
         ,'16'  
         )  
       )  
     BEGIN  
      SET @newBlockstatusAU = '2';  
  
      IF (  
        SELECT COUNT(DISTINCT MatchStatus)  
        FROM T_ThirdPartyAllocationMessages AM  
        WHERE BlockId = @BlockId  
         AND MatchStatus IN (  
          '15'  
          ,'16'  
          )  
        ) > 1  
      BEGIN  
       SET @newSubstatusAU = '13';  
      END  
      ELSE  
      BEGIN  
       SET @newSubstatusAU = (  
         SELECT TOP 1 MatchStatus  
         FROM T_ThirdPartyAllocationMessages AM  
         WHERE BlockId = @BlockId  
          AND MatchStatus IN (  
           '15'  
           ,'16'  
           )  
         )  
      END  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus IN (  
         '9'  
         ,'10'  
         ,'11'  
         ,'12'  
         ,'13'  
         ,'23'  
         )  
       )  
     BEGIN  
      SET @newBlockstatusAU = '8';  
  
      IF (  
        SELECT COUNT(DISTINCT MatchStatus)  
        FROM T_ThirdPartyAllocationMessages AM  
        WHERE BlockId = @BlockId  
         AND MatchStatus IN (  
          '9'  
          ,'10'  
          ,'11'  
          ,'12'  
          ,'13'  
          ,'23'  
          )  
        ) > 1  
      BEGIN  
       SET @newSubstatusAU = '13';  
      END  
      ELSE  
      BEGIN  
       SET @newSubstatusAU = (  
         SELECT TOP 1 MatchStatus  
         FROM T_ThirdPartyAllocationMessages AM  
         WHERE BlockId = @BlockId  
          AND MatchStatus IN (  
           '9'  
           ,'10'  
           ,'11'  
           ,'12'  
           ,'13'  
           ,'23'  
           )  
         )  
      END  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '8'  
       )  
     BEGIN  
      SET @newBlockstatusAU = '8'  
      SET @newSubstatusAU = '4'  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '21'  
       )  
     BEGIN  
      SET @newBlockstatusAU = CASE   
        WHEN @TotalPendingTaxlots = 0  
         THEN '0'  
        ELSE '-1'  
        END  
      SET @newSubstatusAU = CASE   
        WHEN @TotalPendingTaxlots = 0  
         THEN '6'  
        ELSE '3'  
        END  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '20'  
       )  
     BEGIN  
      SET @newBlockstatusAU = CASE   
        WHEN @TotalPendingTaxlots = 0  
         THEN '0'  
        ELSE '-1'  
        END  
      SET @newSubstatusAU = CASE   
        WHEN @TotalPendingTaxlots = 0  
         THEN '5'  
        ELSE '3'  
        END  
     END  
     ELSE IF NOT EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus <> '6'  
       )  
      AND (@JTaxlots = @TotalAUTaxlots)  
     BEGIN  
      SET @newBlockstatusAU = '0'  
      SET @newSubstatusAU = '0'  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '6'  
       )  
     BEGIN  
      SET @newBlockstatusAU = '-1'  
      SET @newSubstatusAU = '3'  
     END  
  
     DECLARE @AllocationIdForAU NVARCHAR(100) = (  
       SELECT TOP 1 AllocId  
       FROM T_ThirdPartyAllocationBlocks AB  
       JOIN T_ThirdPartyAllocationMessages AM ON AB.BlockId = AM.BlockId  
        AND AM.BlockId = @LatestJBlockId  
       )  
  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET AllocStatus = @newBlockstatusAU  
      ,SubStatus = @newSubstatusAU  
     WHERE AllocID = @AllocationIdForAU;  
    END  
   END  
  
   IF (@msgType = 'AS')  
   BEGIN  
    IF (@AllocStatus = '0')  
    BEGIN  
     DECLARE @newSubstatusAS VARCHAR(MAX);  
     DECLARE @newBlockstatusAS VARCHAR(MAX);  
  
     UPDATE AM  
     SET AM.MatchStatus = TEMP.MatchStatus  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN #AllocationTemp TEMP ON AM.IndividualAllocID = TEMP.IndividualAllocID  
  
     IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus IN (  
         '9'  
         ,'10'  
         ,'11'  
         ,'12'  
         ,'13'  
         ,'23'  
         )  
       )  
     BEGIN  
      SET @newBlockstatusAS = '8';  
  
      IF (  
        SELECT COUNT(DISTINCT MatchStatus)  
        FROM T_ThirdPartyAllocationMessages AM  
        WHERE BlockId = @BlockId  
         AND MatchStatus IN (  
          '9'  
          ,'10'  
          ,'11'  
          ,'12'  
          ,'13'  
          ,'23'  
          )  
        ) > 1  
      BEGIN  
       SET @newSubstatusAS = '13';  
      END  
      ELSE  
      BEGIN  
       SET @newSubstatusAS = (  
         SELECT TOP 1 MatchStatus  
         FROM T_ThirdPartyAllocationMessages AM  
         WHERE BlockId = @BlockId  
          AND MatchStatus IN (  
           '9'  
           ,'10'  
           ,'11'  
           ,'12'  
           ,'13'  
           ,'23'  
           )  
         )  
      END  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus = '8'  
       )  
     BEGIN  
      SET @newBlockstatusAS = '8'  
      SET @newSubstatusAS = '4'  
     END  
     ELSE IF NOT EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockId  
        AND MatchStatus <> '18'  
       )  
     BEGIN  
      SET @newBlockstatusAS = '0'  
      SET @newSubstatusAS = '0'  
     END  
  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET AllocStatus = @newBlockstatusAS  
      ,SubStatus = @newSubstatusAS  
     WHERE AllocID = @AllocID  
    END  
    ELSE IF (@AllocStatus <> '3')  
    BEGIN  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET AllocStatus = @AllocStatus  
      ,SubStatus = @SubStatus  
     WHERE AllocId = @AllocID  
  
     IF (@AllocStatus = '1')  
     BEGIN  
      UPDATE AM  
      SET MatchStatus = @SubStatus  
      FROM T_ThirdPartyAllocationMessages AM  
      JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
      WHERE AB.AllocID = @AllocID  
     END  
    END  
   END  
  
   IF (@msgType = 'AT')  
   BEGIN  
    DECLARE @BlockIdASMsg INT = (  
      SELECT BlockId  
      FROM T_ThirdPartyAllocationBlocks  
      WHERE AllocID = @AllocID  
       AND MsgType = 'AS'  
      )  
  
    IF (@AllocStatus = '0')  
    BEGIN  
     UPDATE AM  
     SET MatchStatus = '6'  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
     WHERE AB.AllocID = @AllocID  
      AND AM.MatchStatus = '18';  
  
     UPDATE AM  
     SET MatchStatus = '20'  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
     WHERE AB.AllocID = @AllocID  
      AND AM.MatchStatus = '8';  
  
     UPDATE AM  
     SET MatchStatus = '21'  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
     WHERE AB.AllocID = @AllocID  
      AND AM.MatchStatus IN (  
       '9'  
       ,'10'  
       ,'11'  
       ,'12'  
       ,'13'  
       ,'23'  
       );  
  
     DECLARE @newSubstatusAT VARCHAR(MAX);  
     DECLARE @newBlockstatusAT VARCHAR(MAX);  
  
     IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockIdASMsg  
        AND MatchStatus = '21'  
       )  
     BEGIN  
      SET @newBlockstatusAT = '0'  
      SET @newSubstatusAT = '6'  
     END  
     ELSE IF EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockIdASMsg  
        AND MatchStatus = '20'  
       )  
     BEGIN  
      SET @newBlockstatusAT = '0'  
      SET @newSubstatusAT = '5'  
     END  
     ELSE IF NOT EXISTS (  
       SELECT TOP 1 1  
       FROM T_ThirdPartyAllocationMessages AM  
       WHERE BlockId = @BlockIdASMsg  
        AND MatchStatus <> '6'  
       )  
     BEGIN  
      SET @newBlockstatusAT = '0'  
      SET @newSubstatusAT = '0'  
     END  
  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET AllocStatus = @newBlockstatusAT  
      ,SubStatus = @newSubstatusAT  
     WHERE AllocID = @AllocID  
    END  
  
    IF (@AllocStatus = '4')  
    BEGIN  
     UPDATE AM  
     SET MatchStatus = '15'  
     FROM T_ThirdPartyAllocationMessages AM  
     JOIN T_ThirdPartyAllocationBlocks AB ON AM.BlockId = AB.BlockId  
     WHERE AB.AllocID = @AllocID  
  
     UPDATE T_ThirdPartyAllocationBlocks  
     SET AllocStatus = '1'  
      ,SubStatus = '14'  
     WHERE AllocID = @AllocID  
    END  
   END  
  END  
  
  DECLARE @AllocationId NVARCHAR(100);  
  DECLARE @Direction NVARCHAR(50);  
  DECLARE @Description NVARCHAR(50);  
  
  SET @BlockId = (  
    SELECT TOP 1 JBlockId  
    FROM T_ThirdPartyAllocationBlocks  
    WHERE BlockId = @BlockId  
    )  
  
  -- Determine @AllocationId based on @msgType              
  IF @msgType = 'AK'  
   OR @msgType = 'AU'  
  BEGIN  
   SET @AllocationId = (  
       SELECT TOP 1 IndividualAllocID  
       FROM #AllocationTemp  
       )  
  END  
  ELSE  
  BEGIN  
   SET @AllocationId = @AllocID  
  END  
  
  -- Set the direction based on message type          
  SET @Direction = CASE   
    WHEN @msgType IN (  
      'J'  
      ,'AU'  
      ,'AT'  
      )  
     THEN 'FROM Nirvana TO Broker'  
    WHEN @msgType IN (  
      'P'  
      ,'AK'  
      ,'AS'  
      )  
     THEN 'FROM Broker TO Nirvana'  
    END;  
  -- Set the description based on message type and other conditions          
  SET @Description = CASE   
    WHEN @msgType = 'AS'  
     THEN 'Allocation Report Recvd (AS)'  
    WHEN @msgType = 'AT'  
     THEN 'Allocation Report Ack (AT)'  
    WHEN @msgType = 'AU'  
     THEN 'Affirmation message AU'  
    WHEN @msgType = 'AK'  
     THEN 'Confirmation message AK'  
    WHEN @msgType = 'J'  
     THEN CASE   
       WHEN @AllocTransType = 0  
        THEN 'Block New (J)'  
       WHEN @AllocTransType = 1  
        THEN 'Block Replaced (J)'  
       WHEN @AllocTransType = 2  
        THEN 'Block Canceled (J)'  
       END  
    WHEN @msgType = 'P'  
     THEN CASE   
       WHEN @SubStatus = 2  
        THEN 'Block Accepted (P)'  
       WHEN @SubStatus = 1  
        THEN 'Block Received (P)'  
       ELSE 'Block Rejected (P)'  
       END  
    END;  
  
  -- Insert the data into T_ThirdPartyJobFixMessages table          
  INSERT INTO T_ThirdPartyJobFixMessages (  
   BlockId  
   ,AllocationID  
   ,TransmissionTime  
   ,Direction  
   ,Description  
   ,FIXMsg  
   )  
  VALUES (  
   @BlockId  
   ,@AllocationId  
   ,@TransmissionTime  
   ,@Direction  
   ,@Description  
   ,@FixMsg  
   );  
  
  COMMIT TRANSACTION T1  
  
  SELECT TOP 1 ThirdPartyBatchId  
   ,AllocStatus  
  FROM T_ThirdPartyDailyJobs DJ  
  INNER JOIN T_ThirdPartyAllocationBlocks AB ON DJ.JobId = AB.JobId  
  WHERE AB.AllocID = @AllocID  
 END TRY  
  
 BEGIN CATCH  
  ROLLBACK TRANSACTION T1  
 END CATCH  
END