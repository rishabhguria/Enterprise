/***********************************************    
Modified By: Om shiv    
Date: 10-12-2013    
Desc.:  Maching Exchange on future root data updating/ Adding   
  
Modified By: Disha Sharma    
Date: 06-13-2015    
Desc.:  Save Dynamic UDAs in T_UDA_DynamicUDAData table   
  
Modified By:- Disha Sharma  
Date: 07-22-2015    
Desc.: Modified condition to insert new symbols   
***********************************************/  
CREATE PROCEDURE [dbo].[P_SaveFutureRootData] (
 @Xml VARCHAR(max)  
 ,@ErrorMessage VARCHAR(500) OUTPUT  
 ,@ErrorNumber INT OUTPUT  
 )  
AS  
SET @ErrorNumber = 0  
SET @ErrorMessage = 'Success'  
  
BEGIN TRY  
 BEGIN TRAN TRAN1  
  
 -- SAVES PSSYMBOL AND MULTIPLIERS                                                                        
 DECLARE @handle INT  
  
 EXEC sp_xml_preparedocument @handle OUTPUT  
  ,@Xml  
  
 --DELETE FROM T_FutureMultipliers    --modified by Omshiv, Have commented.line. ???  why we deleting whole data from T_FutureMultipliers for only updating ROOT data. - omshiv    
 CREATE TABLE #XmlItem (  
  Symbol VARCHAR(100)  
  ,Multiplier FLOAT  
  ,PSSymbol VARCHAR(20)  
  ,UnderlyingSymbol VARCHAR(100)  
  ,CutOffTime VARCHAR(50)  
  ,Exchange VARCHAR(50)  
  ,ProxyRoot VARCHAR(50)  
  ,UDAAssetClassID INT  
  ,UDASecurityTypeID INT  
  ,UDASectorID INT  
  ,UDASubSectorID INT  
  ,UDACountryID INT  
  ,IsCurrencyFuture BIT  
  ,DynamicUDA XML
  ,BBGYellowKey VARCHAR(20)
  ,BBGRoot VARCHAR(100)
  )  
  
 -- Temp table to store xml as XML type  
 CREATE TABLE #XmlContainer (  
  XmlString XML  
 )  
  
 -- Creating a table to remove warning  
 CREATE TABLE TempDynamicUDAData(  
  Symbol VARCHAR(100)  
  ,Exchange VARCHAR(100)  
 )  
  
 INSERT INTO #XmlContainer (XmlString) VALUES (@Xml)  
  
 INSERT INTO #XmlItem (  
  Symbol  
  ,Multiplier  
  ,PSSymbol  
  ,UnderlyingSymbol  
  ,CutOffTime  
  ,Exchange  
  ,ProxyRoot  
  ,UDAAssetClassID  
  ,UDASecurityTypeID  
  ,UDASectorID  
  ,UDASubSectorID  
  ,UDACountryID  
  ,IsCurrencyFuture  
  ,DynamicUDA
  ,BBGYellowKey
  ,BBGRoot
  )  
 SELECT Symbol  
  ,Multiplier  
  ,PSSymbol  
  ,UnderlyingSymbol  
  ,CutOffTime  
  ,Exchange  
  ,ProxyRoot  
  ,UDAAssetClassID  
  ,UDASecurityTypeID  
  ,UDASectorID  
  ,UDASubSectorID  
  ,UDACountryID  
  ,IsCurrencyFuture  
  ,NULL
  ,BBGYellowKey
  ,BBGRoot
 FROM OPENXML(@handle, '//DocumentElement/Table', 3) WITH (  
   Symbol VARCHAR(100)  
   ,Multiplier FLOAT  
   ,PSSymbol VARCHAR(100)  
   ,UnderlyingSymbol VARCHAR(100)  
   ,CutOffTime VARCHAR(50)  
   ,Exchange VARCHAR(50)  
   ,ProxyRoot VARCHAR(50)  
   ,UDAAssetClassID INT  
   ,UDASecurityTypeID INT  
   ,UDASectorID INT  
   ,UDASubSectorID INT  
   ,UDACountryID INT  
   ,IsCurrencyFuture BIT
   ,BBGYellowKey VARCHAR(20)
   ,BBGRoot VARCHAR(100)
   )  
  
 -- Update Dynamic UDA data   
 DECLARE @UDAColString VARCHAR(MAX)  
 SET  @UDAColString = ' '  
 SELECT @UDAColString = @UDAColString +' ALTER TABLE TempDynamicUDAData ADD '+ '[' + Tag + ']'  + ' VARCHAR(100) '   
 FROM T_UDA_DynamicUDA  
   
 -- Create table to store dynamic UDAs  
 EXEC(@UDAColString)  
  
 DECLARE @UDAString  VARCHAR(MAX)  
 DECLARE @TempTableStr NVARCHAR(MAX)  
 SET  @UDAString = 'X.XmlCol.value(''(Symbol)[1]'', ''varchar(100)'') As Symbol, X.XmlCol.value(''(Exchange)[1]'', ''varchar(100)'') As Exchange'  
  
 SELECT @UDAString = @UDAString + ',' + ' X.XmlCol.value(''(' + Tag  + ')[1]'', ''varchar(100)'') As [' + Tag +']'  
 FROM T_UDA_DynamicUDA  
  
 SET @TempTableStr = 'INSERT INTO TempDynamicUDAData SELECT ' + @UDAString + ' FROM #XmlContainer XC CROSS APPLY XC.XmlString.nodes(''/DocumentElement/Table'')  X(XmlCol)'  
 EXEC(@TempTableStr)  
  
 CREATE TABLE #TempDynamicUDADataXmlSymbolWise ( Symbol VARCHAR(100), UDAData XML, Exchange VARCHAR(50))  
 INSERT INTO #TempDynamicUDADataXmlSymbolWise  
 SELECT  
  DUDA2.Symbol,  
  (  
   SELECT   
    *  
   FROM TempDynamicUDAData DUDA1  
   WHERE DUDA1.Symbol = DUDA2.Symbol  
     AND (  
      ( DUDA1.Exchange IS NOT NULL AND DUDA1.Exchange = DUDA2.Exchange)  
      OR ( DUDA1.Exchange IS NULL AND DUDA2.Exchange IS NULL )  
     )  
   FOR XML PATH (''), ROOT('DynamicUDAs')  
  ),  
  DUDA2.Exchange  
 FROM TempDynamicUDAData DUDA2  
  
 -- SELECT * FROM TempDynamicUDAData  
 -- SELECT * FROM #TempDynamicUDADataXmlSymbolWise  
  
 -- Remove Tags with Undefined or Default Value of dynamic UDA  
 SELECT Tag INTO #TempDynamicUDANames FROM T_UDA_DynamicUDA  
 WHILE EXISTS(SELECT 1 FROM #TempDynamicUDANames)  
 BEGIN  
  DECLARE @tagName VARCHAR(100)  
  SET @tagName = (SELECT TOP 1 Tag FROM #TempDynamicUDANames)  
  
  UPDATE #TempDynamicUDADataXmlSymbolWise  
  SET  UDAData.modify('delete /DynamicUDAs/*[local-name()=sql:variable("@tagName")]')  
  FROM T_UDA_DynamicUDA  
  WHERE ( UDAData.value('(/DynamicUDAs/*[local-name()=sql:variable("@tagName")]/text())[1]','VARCHAR(100)') = DefaultValue  
    OR UDAData.value('(/DynamicUDAs/*[local-name()=sql:variable("@tagName")]/text())[1]','VARCHAR(100)') = 'Undefined' )  
    AND Tag = @tagName  
  
  DELETE FROM #TempDynamicUDANames WHERE Tag = @tagName  
 END  
  
 -- Updating DynamicUDA in #XmlItem table  
 WHILE EXISTS( SELECT 1 FROM #TempDynamicUDADataXmlSymbolWise)  
 BEGIN  
  DECLARE @Symbol VARCHAR(100), @UDAData XML, @Exchange VARCHAR(50)  
  
  SELECT TOP 1 @Symbol = Symbol, @Exchange = Exchange,@UDAData = UDAData FROM #TempDynamicUDADataXmlSymbolWise  
  
  IF(@UDAData IS NOT NULL)  
  BEGIN  
   SET  @UDAData.modify('delete /DynamicUDAs/Symbol')  
   SET  @UDAData.modify('delete /DynamicUDAs/Exchange')  
  
   UPDATE #XmlItem  
   SET  DynamicUDA = @UDAData  
   WHERE Symbol = @Symbol  
     AND (  
      ( Exchange IS NOT NULL AND Exchange = @Exchange)  
      OR ( Exchange IS NULL AND @Exchange IS NULL )  
     )  
     
  END  
  
  DELETE FROM #TempDynamicUDADataXmlSymbolWise WHERE Symbol = @Symbol AND (  
      ( Exchange IS NOT NULL AND Exchange = @Exchange)  
      OR ( Exchange IS NULL AND @Exchange IS NULL )  
     )  
   
 END  
  
 DROP TABLE TempDynamicUDAData  
 DROP TABLE #XmlContainer  
 DROP TABLE #TempDynamicUDADataXmlSymbolWise  
  
  
 UPDATE T_FutureMultipliers  
 SET Symbol = #XmlItem.Symbol  
  ,Multiplier = #XmlItem.Multiplier  
  ,PSSymbol = #XmlItem.PSSymbol  
  ,UnderlyingSymbol = #XmlItem.UnderlyingSymbol  
  ,CutOffTime = #XmlItem.CutOffTime  
  ,Exchange = #XmlItem.Exchange  
  ,ProxyRoot = #XmlItem.ProxyRoot  
  ,UDAAssetClassID = #XmlItem.UDAAssetClassID  
  ,UDASecurityTypeID = #XmlItem.UDASecurityTypeID  
  ,UDASectorID = #XmlItem.UDASectorID  
  ,UDASubSectorID = #XmlItem.UDASubSectorID  
  ,UDACountryID = #XmlItem.UDACountryID  
  ,IsCurrencyFuture = #XmlItem.IsCurrencyFuture  
  ,DynamicUDA = #XmlItem.DynamicUDA
  ,BBGYellowKey = #XmlItem.BBGYellowKey
  ,BBGRoot = #XmlItem.BBGRoot
 --select *    
 FROM T_FutureMultipliers  
 INNER JOIN #XmlItem ON #XmlItem.Symbol = T_FutureMultipliers.Symbol  
  AND (  
   (  
    T_FutureMultipliers.Exchange IS NOT NULL  
    AND #XmlItem.Exchange = T_FutureMultipliers.Exchange  
    )  
   OR (  
    T_FutureMultipliers.Exchange IS NULL  
    AND #XmlItem.Exchange IS NULL  
    )  
   )  
   
 --select * from #XmlItem;               
 INSERT INTO T_FutureMultipliers (  
  Symbol  
  ,Multiplier  
  ,PSSymbol  
  ,UnderlyingSymbol  
  ,CutOffTime  
  ,Exchange  
  ,ProxyRoot  
  ,UDAAssetClassID  
  ,UDASecurityTypeID  
  ,UDASectorID  
  ,UDASubSectorID  
  ,UDACountryID  
  ,IsCurrencyFuture  
  ,DynamicUDA 
  ,BBGYellowKey
  ,BBGRoot
  )  
 SELECT Symbol  
  ,Multiplier  
  ,PSSymbol  
  ,UnderlyingSymbol  
  ,CutOffTime  
  ,Exchange  
  ,ProxyRoot  
  ,UDAAssetClassID  
  ,UDASecurityTypeID  
  ,UDASectorID  
  ,UDASubSectorID  
  ,UDACountryID  
  ,IsCurrencyFuture  
  ,DynamicUDA
  ,BBGYellowKey
  ,BBGRoot
 FROM #XmlItem XI  
 WHERE NOT EXISTS (  
   SELECT 1  
   FROM T_FutureMultipliers  
   WHERE T_FutureMultipliers.Symbol = XI.Symbol  
    AND (  
     (  
      T_FutureMultipliers.Exchange IS NOT NULL  
      AND T_FutureMultipliers.Exchange = XI.Exchange  
      )  
     OR (  
      T_FutureMultipliers.Exchange IS NULL  
      AND XI.Exchange IS NULL  
      )  
     )  
   )  
  
 --      
 --          
 ---- UPDATE MUTIPLIERS And CutOffTime IN T_SMFutureData TABLE          
 DECLARE @temp TABLE (  
  Symbol_PK BIGINT  
  ,TickerSymbol VARCHAR(100)  
  ,RootSymbol VARCHAR(100)  
  ,Suffix VARCHAR(100)  
  ,Multiplier FLOAT  
  ,UnderlyingSymbol VARCHAR(100)  
  ,CutOffTime VARCHAR(50)  
  ,Exchange VARCHAR(50)  
  ,ExchangeSM VARCHAR(50)  
  ,ProxyRoot VARCHAR(50)  
  ,ProxySymbol VARCHAR(100)  
  ,UDAAssetClassID INT  
  ,UDASecurityTypeID INT  
  ,UDASectorID INT  
  ,UDASubSectorID INT  
  ,UDACountryID INT  
  ,AssetID INT  
  ,IsCurrencyFuture BIT  
  ,DynamicUDA XML  
  )  
  
 INSERT INTO @temp  
 SELECT lookup.Symbol_PK  
  ,lookup.TickerSymbol  
  ,SUBSTRING(lookup.TickerSymbol, 1, CHARINDEX(' ', lookup.TickerSymbol)) AS symbol  
  ,SUBSTRING(lookup.TickerSymbol, CHARINDEX(' ', lookup.TickerSymbol) + 1, 20) AS Suffix  
  ,FutMulti.Multiplier  
  ,FutMulti.UnderlyingSymbol  
  ,FutMulti.CutOffTime  
  ,FutMulti.Exchange  
  ,CASE   
   WHEN CHARINDEX('-', lookup.TickerSymbol) > 0  
    THEN SUBSTRING(lookup.TickerSymbol, CHARINDEX('-', lookup.TickerSymbol) + 1, 3)  
   ELSE ''  
   END AS exchangeSM  
  ,FutMulti.ProxyRoot  
  ,(FutMulti.ProxyRoot + ' ' + SUBSTRING(lookup.TickerSymbol, CHARINDEX(' ', lookup.TickerSymbol) + 1, 20)) AS ProxySymbol  
  ,  
  --lookup.TickerSymbol    
  FutMulti.UDAAssetClassID  
  ,FutMulti.UDASecurityTypeID  
  ,FutMulti.UDASectorID  
  ,FutMulti.UDASubSectorID  
  ,FutMulti.UDACountryID  
  ,AssetID  
  ,FutMulti.IsCurrencyFuture  
  ,FutMulti.DynamicUDA  
 FROM T_SMSymbolLookUpTable AS lookup  
 INNER JOIN #XmlItem AS FutMulti ON FutMulti.Symbol = SUBSTRING(lookup.TickerSymbol, 1, CHARINDEX(' ', lookup.TickerSymbol))  
 -- To avoid the symbols for which exchange is NULL in T_Future root Data,    
 -- For those symbols where -Exchange suffix does not exist we match an empty string    
 WHERE (  
   (  
    CHARINDEX('-', lookup.TickerSymbol) > 0  
    AND FutMulti.Exchange = SUBSTRING(lookup.TickerSymbol, CHARINDEX('-', lookup.TickerSymbol) + 1, 3)  
    )  
   OR (  
     ((CHARINDEX('-', lookup.TickerSymbol) <= 0) AND  
     FutMulti.Exchange IS NULL)  
     OR  
     ((CHARINDEX('-', lookup.TickerSymbol) <= 0) AND  
      FutMulti.Exchange = (SELECT DisplayName FROM T_Exchange WHERE T_Exchange.ExchangeID = lookup.ExchangeID) AND  
      NOT EXISTS (SELECT 1 FROM T_FutureMultipliers WHERE Symbol = FutMulti.Symbol AND Exchange IS NULL)  
     )  
    )  
   )  
  AND (  
   AssetID = 3  
   OR AssetID = 4  
   )  
  
-- SELECT *  
-- FROM @temp  
  
 ----  Updating fields in T_SMoptionData for futures    
 UPDATE T_SMFutureData  
 SET T_SMFutureData.Multiplier = TEMP.Multiplier  
  ,T_SMFutureData.CutOffTime = TEMP.CutOffTime  
  ,T_SMFutureData.IsCurrencyFuture = TEMP.IsCurrencyFuture  
 FROM @temp AS TEMP  
 WHERE TEMP.Symbol_PK = T_SMFutureData.Symbol_PK  
  
 -- why we have multiple queries for updating for multiple column updates in same table    
 -- null check should be on code level (send correct data to DB) - om shiv    
 --UPDATE T_SMFutureData       
 --SET T_SMFutureData.CutOffTime = Temp.CutOffTime         
 --FROM @temp as Temp          
 --WHERE Temp.Symbol_PK = T_SMFutureData.Symbol_PK and Temp.CutOffTime is not null             
 --  Updating fields in T_SMoptionData for future options      
 UPDATE T_SMoptionData  
 SET T_SMOptionData.Multiplier = TEMP.Multiplier  
 FROM @temp AS TEMP  
 WHERE TEMP.Symbol_PK = T_SMOptionData.Symbol_PK  
  AND TEMP.Multiplier IS NOT NULL  
  
 UPDATE T_SMoptionData  
 SET T_SMOptionData.IsCurrencyFuture = TEMP.IsCurrencyFuture  
 FROM @temp AS TEMP  
 WHERE TEMP.Symbol_PK = T_SMOptionData.Symbol_PK  
  AND TEMP.IsCurrencyFuture IS NOT NULL  
  
 --  Updating fields in T_SMSymbolLookUpTable         
 UPDATE T_SMSymbolLookUpTable  
 SET T_SMSymbolLookUpTable.UnderLyingSymbol = CASE   
   WHEN TEMP.UnderlyingSymbol IS NOT NULL  
    AND TEMP.UnderlyingSymbol <> ''  
    THEN TEMP.UnderlyingSymbol  
   ELSE T_SMSymbolLookUpTable.UnderLyingSymbol  
   END  
  ,T_SMSymbolLookUpTable.ProxySymbol = CASE   
   WHEN TEMP.ProxySymbol IS NOT NULL  
    AND TEMP.ProxySymbol <> ''  
    THEN TEMP.ProxySymbol  
   ELSE T_SMSymbolLookUpTable.ProxySymbol  
   END  
--  ,T_SMSymbolLookUpTable.UDAAssetClassID = TEMP.UDAAssetClassID  
  ,T_SMSymbolLookUpTable.UDASecurityTypeID = TEMP.UDASecurityTypeID  
  ,T_SMSymbolLookUpTable.UDASectorID = TEMP.UDASectorID  
  ,T_SMSymbolLookUpTable.UDASubSectorID = TEMP.UDASubSectorID  
  ,T_SMSymbolLookUpTable.UDACountryID = TEMP.UDACountryID  
 --     
 FROM @temp AS TEMP  
 WHERE TEMP.Symbol_PK = T_SMSymbolLookUpTable.Symbol_PK  
  
 -- Updating dynamic UDAs for future and options for root   
 SELECT SM.Symbol_PK, TEMP.DynamicUDA, 0 AS FundID INTO #UpdateUDAData  
 FROM @temp TEMP  
 INNER JOIN T_SMSymbolLookUpTable SM ON TEMP.Symbol_PK = SM.Symbol_PK  

-- WHILE EXISTS(SELECT 1 FROM #UpdateUDAData)  
-- BEGIN  
--  DECLARE @SymbolPK BIGINT, @UDAXML XML, @FundID INT  
--  SET  @SymbolPK = (SELECT TOP 1 Symbol_PK from #UpdateUDAData)  
--  SELECT @UDAXML = DynamicUDA FROM #UpdateUDAData WHERE Symbol_PK = @SymbolPK  
--  SELECT @FundID = FundID  FROM #UpdateUDAData WHERE Symbol_PK = @SymbolPK  
--  
--  EXEC P_UDA_SaveDynamicUDAData   
--  @Symbol_PK = @SymbolPK, @UDAData = @UDAXML, @FundID = @FundID  
--  
--  DELETE FROM #UpdateUDAData WHERE Symbol_PK = @SymbolPK  
-- END  

/* Commented this while loop as it is doing saving data one by one for fundwise and 
symbolwise for Dynamic UDAs, and it is taking much time and hence data is not getting saved.
Optimized it using Join in the below section and it is much faster.
-(Ankit Misra PRANA-15927)*/

Select 
TUA.Symbol_PK,  
TUA.FundID 
Into #TempMissingFundWiseSymbols
from #UpdateUDAData TUA
Left Outer Join T_UDA_DynamicUDAData DUDA ON DUDA.Symbol_PK = TUA.Symbol_PK AND DUDA.FundID = TUA.FundID
Where DUDA.Symbol_PK IS NULL

INSERT INTO T_UDA_DynamicUDAData  
(  
Symbol_PK,   
UDAData,  
FundID  
)
Select 
Symbol_PK,   
NULL,  
FundID  
From #TempMissingFundWiseSymbols

Declare @updateDynamicUdaQuery Varchar(max)

Set  @updateDynamicUdaQuery = 'Update T_UDA_DynamicUDAData Set'
Select  @updateDynamicUdaQuery = @updateDynamicUdaQuery + ' [' + Tag + '] = TUA.DynamicUDA.value(''(/DynamicUDAs/' + Tag  + ')[1]'', ''varchar(100)''),'  FROM T_UDA_DynamicUDA
Set @updateDynamicUdaQuery = SUBSTRING(@updateDynamicUdaQuery,0,LEN(@updateDynamicUdaQuery)) 	
Set @updateDynamicUdaQuery = @updateDynamicUdaQuery + 'from #UpdateUDAData TUA
Inner Join T_UDA_DynamicUDAData DUDA ON DUDA.Symbol_PK = TUA.Symbol_PK AND DUDA.FundID = TUA.FundID'
  

  
 DROP TABLE #UpdateUDAData,#TempMissingFundWiseSymbols 
 -- why we have multiple queries for updating for multiple column updates in same table            
 ---- null check should be on code level (send correct data to DB) - om shiv    
 --UPDATE T_SMSymbolLookUpTable          
 --SET  T_SMSymbolLookUpTable.ProxySymbol = Temp.ProxySymbol    
 --FROM @temp as Temp          
 --WHERE Temp.Symbol_PK = T_SMSymbolLookUpTable.Symbol_PK and Temp.ProxySymbol is not null          
 --            
 COMMIT TRANSACTION TRAN1
EXEC sp_xml_removedocument @handle
END TRY  
  
BEGIN CATCH  
 SET @ErrorMessage = ERROR_MESSAGE();  
 SET @ErrorNumber = Error_number();  
  
 ROLLBACK TRANSACTION TRAN1  
END CATCH;