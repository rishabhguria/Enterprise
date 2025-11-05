create PROCEDURE [dbo].[P_SaveSecurityMasterDataForSymbol] (  
 @Xml NVARCHAR(max)  
 ,@IsAutoUpdateDerivateUDA bit = 1
 ,  
 --@dataSource int,                                                                                                       
 @ErrorMessage VARCHAR(500) OUTPUT  
 ,@ErrorNumber INT OUTPUT  
 )  
AS  
SET @ErrorNumber = 0  
SET @ErrorMessage = 'Success'  
  
BEGIN TRY  
 BEGIN TRAN TRAN1  
  
 DECLARE @handle INT  
  
 EXEC sp_xml_preparedocument @handle OUTPUT  
  ,@Xml  
  
 CREATE TABLE #XmlItem (  
  ExchangeID INT  
  ,UnderLyingID INT  
  ,AUECID INT  
  ,AssetID INT  
  ,CusipSymbol VARCHAR(20)  
  ,SEDOLSymbol VARCHAR(20)  
  ,ISINSymbol VARCHAR(20)  
  ,ReutersSymbol VARCHAR(100)  
  ,TickerSymbol VARCHAR(100)  
  ,FactSetSymbol VARCHAR(200) 
  ,ActivSymbol VARCHAR(200) 
  ,BloombergSymbol VARCHAR(200)
  ,OSIOptionSymbol VARCHAR(25)  
  ,IDCOOptionSymbol VARCHAR(25)  
  ,OpraSymbol VARCHAR(20)  
  ,UnderLyingSymbol VARCHAR(100)  
  ,CompanyName VARCHAR(50)  
  ,Symbol_PK VARCHAR(100)  
  ,RoundLot DECIMAL(28,10)  
  ,CurrencyID INT  
  ,Sector VARCHAR(20)  
  ,LongName VARCHAR(50)  
  ,PutOrCall INT  
  --,MaturityMonth varchar(50)                                                                                                    
  --,SettleMentDate int                                                                                                          
  ,StrikePrice FLOAT  
  ,ExpirationDate DATETIME  
  ,Multiplier FLOAT --need to discuss can be float                                                     
  ,SymbolType INT  
  ,Delta FLOAT  
  ,LeadCurrencyID INT  
  ,VsCurrencyID INT  
  ,IssueDate DATETIME  
  ,Coupon FLOAT --datatype of coupon is changed to the float as it was saved only in int type  Narendra Jangir 2012 Nov 20                                      
  ,MaturityDate DATETIME  
  ,BondType VARCHAR(50)  
  ,AccrualBasis VARCHAR(50)  
  ,FirstCouponDate DATETIME  
  ,IsZero BIT  
  ,CouponFrequency INT  
  ,DaysToSettlement INT  
  ,CutOffTime VARCHAR(50)  
  ,IsNDF BIT  
  ,FixingDate DATETIME  
  ,ProxySymbol VARCHAR(100)  
  ,IsSecApproved BIT  
  ,ApprovalDate DATETIME  
  ,ApprovedBy VARCHAR(100)  
  ,Comments VARCHAR(500)  
  ,CreatedBy VARCHAR(100)  
  ,ModifiedBy VARCHAR(100)  
  ,PrimarySymbology INT  
  ,BBGID VARCHAR(20)  
  ,StrikePriceMultiplier FLOAT  
  ,SourceOfDataID INT  
  ,EsignalOptionRoot VARCHAR(100)  
  ,BloombergOptionRoot VARCHAR(100)  
  ,IsCurrencyFuture BIT  
  ,CollateralType VARCHAR(50)
  ,SharesOutstanding FLOAT
  ,BloombergSymbolWithExchangeCode VARCHAR(200)

  )  
  
 INSERT INTO #XmlItem (  
  ExchangeID  
  ,UnderLyingID  
  ,AUECID  
  ,AssetID  
  ,CusipSymbol  
  ,SEDOLSymbol  
  ,ISINSymbol  
  ,ReutersSymbol  
  ,TickerSymbol    
  ,FactSetSymbol
  ,ActivSymbol
  ,BloombergSymbol  
  ,OSIOptionSymbol  
  ,IDCOOptionSymbol  
  ,OpraSymbol  
  ,UnderLyingSymbol  
  ,CompanyName  
  ,Symbol_PK  
  ,RoundLot  
  ,CurrencyID  
  ,Sector  
  ,LongName  
  ,PutOrCall  
  --,MaturityMonth                                                                                                           
  --,SettleMentDate                                                                          
  ,StrikePrice  
  ,ExpirationDate  
  ,Multiplier  
  ,SymbolType  
  ,Delta  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,IssueDate  
  ,Coupon  
  ,MaturityDate  
  ,BondType  
  ,AccrualBasis  
  ,FirstCouponDate  
  ,IsZero  
  ,CouponFrequency  
  ,DaysToSettlement  
  ,CutOffTime  
  ,IsNDF  
  ,FixingDate  
  ,ProxySymbol  
  ,IsSecApproved  
  ,ApprovalDate  
  ,ApprovedBy  
  ,Comments  
  ,CreatedBy  
  ,ModifiedBy  
  ,PrimarySymbology  
  ,BBGID  
  ,StrikePriceMultiplier  
  ,SourceOfDataID  
  ,EsignalOptionRoot  
  ,BloombergOptionRoot  
  ,IsCurrencyFuture  
  ,CollateralType
  ,SharesOutstanding
  ,BloombergSymbolWithExchangeCode
  )  
 SELECT DISTINCT ExchangeID  
  ,UnderLyingID  
  ,AUECID  
  ,AssetID  
  ,CusipSymbol  
  ,SEDOLSymbol  
  ,ISINSymbol  
  ,ReutersSymbol  
  ,TickerSymbol     
  ,FactSetSymbol
  ,ActivSymbol
  ,BloombergSymbol  
  ,OSIOptionSymbol  
  ,IDCOOptionSymbol  
  ,isnull(OpraSymbol, '')  
  ,UnderLyingSymbol  
  ,CompanyName  
  ,Symbol_PK  
  ,RoundLot  
  ,CurrencyID  
  ,Sector  
  ,LongName  
  ,PutOrCall  
  --,MaturityMonth                                         
  --,SettleMentDate                                                                     
  ,StrikePrice  
  ,ExpirationDate  
  ,Multiplier  
  ,SymbolType  
  ,Delta  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,IssueDate  
  ,Coupon  
  ,MaturityDate  
  ,BondTypeID  
  ,AccrualBasisID  
  ,FirstCouponDate  
  ,IsZero  
  ,CouponFrequencyID  
  ,DaysToSettlement  
  ,CutOffTime  
  ,IsNDF  
  ,FixingDate  
  ,ProxySymbol  
  ,IsSecApproved  
  ,ApprovalDate  
  ,ApprovedBy  
  ,Comments  
  ,CreatedBy  
  ,ModifiedBy  
  ,PrimarySymbology  
  ,BBGID  
  ,StrikePriceMultiplier  
  ,SourceOfDataID  
  ,EsignalOptionRoot  
  ,BloombergOptionRoot  
  ,IsCurrencyFuture  
  ,CollateralTypeID 
  ,SharesOutstanding
  ,BloombergSymbolWithExchangeCode

 FROM OPENXML(@handle, '//', 3) WITH (  
   ExchangeID INT  
   ,UnderLyingID INT  
   ,AUECID INT  
   ,AssetID INT  
   ,CusipSymbol VARCHAR(20)  
   ,SedolSymbol VARCHAR(20)  
   ,ISINSymbol VARCHAR(20)  
   ,ReutersSymbol VARCHAR(100)  
   ,TickerSymbol VARCHAR(100)    
   ,FactSetSymbol VARCHAR(200)
   ,ActivSymbol	VARCHAR(200)
   ,BloombergSymbol VARCHAR(200)  
   ,OSIOptionSymbol VARCHAR(25)  
   ,IDCOOptionSymbol VARCHAR(25)  
   ,OpraSymbol VARCHAR(20)  
   ,UnderLyingSymbol VARCHAR(100)  
   ,CompanyName VARCHAR(50)  
   ,Symbol_PK VARCHAR(100)  
   ,RoundLot DECIMAL(28,10)  
   ,CurrencyID INT  
   ,Sector VARCHAR(20)  
   ,LongName VARCHAR(50)  
   ,PutOrCall INT  
   --,MaturityMonth varchar(50)                                                                                                          
   --,SettleMentDate int                                                                           
   ,StrikePrice FLOAT  
   ,ExpirationDate DATETIME  
   ,Multiplier FLOAT  
   ,SymbolType INT  
   ,Delta FLOAT  
   ,LeadCurrencyID INT  
   ,VsCurrencyID INT  
   ,IssueDate DATETIME  
   ,Coupon FLOAT  
   ,MaturityDate DATETIME  
   ,BondTypeID VARCHAR(50)  
   ,AccrualBasisID VARCHAR(50)  
   ,FirstCouponDate DATETIME  
   ,IsZero BIT  
   ,CouponFrequencyID INT  
   ,DaysToSettlement INT  
   ,CutOffTime VARCHAR(50)  
   ,IsNDF BIT  
   ,FixingDate DATETIME  
   ,ProxySymbol VARCHAR(100)  
   ,IsSecApproved BIT  
   ,ApprovalDate DATETIME  
   ,ApprovedBy VARCHAR(100)  
   ,Comments VARCHAR(500)  
   ,CreatedBy VARCHAR(100)  
   ,ModifiedBy VARCHAR(100)  
   ,PrimarySymbology INT  
   ,BBGID VARCHAR(20)  
   ,StrikePriceMultiplier FLOAT  
   ,SourceOfDataID INT  
   ,EsignalOptionRoot VARCHAR(100)  
   ,BloombergOptionRoot VARCHAR(100)  
   ,IsCurrencyFuture BIT  
   ,CollateralTypeID VARCHAR(50)
   ,SharesOutstanding FLOAT
   ,BloombergSymbolWithExchangeCode VARCHAR(200)
   )  
  
 --select * from    #XmlItem        
 CREATE TABLE #temp_uda (  
  Symbol VARCHAR(100)  
  ,AssetID INT  
  ,SecurityTypeID INT  
  ,SectorID INT  
  ,SubSectorID INT  
  ,CountryID INT  
  ,UnderlyingSymbol VARCHAR(100)  
  )  
  
 INSERT INTO #temp_uda (  
  Symbol  
  ,AssetID  
  ,SecurityTypeID  
  ,SectorID  
  ,SubSectorID  
  ,CountryID  
  ,UnderlyingSymbol  
  )  
 SELECT Symbol  
  ,AssetID  
  ,SecurityTypeID  
  ,SectorID  
  ,SubSectorID  
  ,CountryID  
  ,UnderlyingSymbol  
 FROM OPENXML(@handle, '//SymbolUDAData', 2) WITH (  
   Symbol VARCHAR(100)  
   ,AssetID INT  
   ,SecurityTypeID INT  
   ,SectorID INT  
   ,SubSectorID INT  
   ,CountryID INT  
   ,UnderlyingSymbol VARCHAR(100)  
   )  
  
 --select * from    #temp_uda                                                                                         
 SELECT *  
 INTO #tmp_XmlItem  
 FROM #XmlItem  
 WHERE SymbolType = 0  
  AND TickerSymbol IS NOT NULL  
  AND TickerSymbol NOT IN (  
   SELECT TickerSymbol  
   FROM T_SMSymbolLookUpTable  
   )  
  
 INSERT INTO T_SMSymbolLookUpTable (  
  TickerSymbol  
  ,UnderLyingID  
  ,AUECID  
  ,AssetID  
  ,ISINSymbol  
  ,SEDOLSymbol  
  --,ReutersSymbol                                                                                                             
  ,BloombergSymbol   
  ,FactSetSymbol
  ,ActivSymbol 
  ,OSISymbol  
  ,IDCOSymbol  
  ,OpraSymbol  
  ,UnderlyingSymbol  
  ,CusipSymbol  
  ,ExchangeID  
  ,Symbol_PK  
  ,CreationDate  
  ,CurrencyID  
  ,Sector  
  ,DataSource  
  ,RoundLot  
  ,ProxySymbol  
  ,IsSecApproved  
  ,ApprovalDate  
  ,ApprovedBy  
  ,Comments  
  ,UDAAssetClassID  
  ,UDASecurityTypeID  
  ,UDASectorID  
  ,UDASubSectorID  
  ,UDACountryID  
  ,CreatedBy  
  --,ModifiedBy    
  ,PrimarySymbology  
  ,BBGID  
  ,StrikePriceMultiplier  
  ,EsignalOptionRoot  
  ,BloombergOptionRoot 
  ,SharesOutstanding 
  ,BloombergSymbolWithExchangeCode
  )  
 SELECT TickerSymbol  
  ,UnderLyingID  
  ,AUECID  
  ,#tmp_XmlItem.AssetID  
  ,ISINSymbol  
  ,SEDOLSymbol  
  --,ReutersSymbol                              
  ,BloombergSymbol   
  ,FactSetSymbol
  ,ActivSymbol 
  ,OSIOptionSymbol  
  ,IDCOOptionSymbol  
  ,OpraSymbol  
  ,#tmp_XmlItem.UnderLyingSymbol  
  ,CusipSymbol  
  ,ExchangeID  
  ,Symbol_PK  
  ,getdate()  
  ,CurrencyID  
  ,Sector  
  ,SourceOfDataID  
  ,RoundLot  
  ,ProxySymbol  
  ,IsSecApproved  
  ,ApprovalDate  
  ,ApprovedBy  
  ,Comments  
  ,#temp_uda.AssetID  
  ,#temp_uda.SecurityTypeID  
  ,#temp_uda.SectorID  
  ,#temp_uda.SubSectorID  
  ,#temp_uda.CountryID  
  ,CreatedBy  
  --,ModifiedBy    
  ,PrimarySymbology  
  ,BBGID  
  ,StrikePriceMultiplier  
  ,EsignalOptionRoot  
  ,BloombergOptionRoot  
  ,SharesOutstanding
  ,BloombergSymbolWithExchangeCode
 FROM #tmp_XmlItem  
 LEFT JOIN #temp_uda ON #temp_uda.Symbol = #tmp_XmlItem.TickerSymbol  
  

if(@IsAutoUpdateDerivateUDA = 1)  
BEGIN
 -- DERIVED SYMBOLS      
    UPDATE T_SMSymbolLookUpTable  
    SET --UDAAssetClassID = #temp_uda.AssetID  
      UDASecurityTypeID = #temp_uda.SecurityTypeID  
     ,UDASectorID = #temp_uda.SectorID  
     ,UDASubSectorID = #temp_uda.SubSectorID  
     ,UDACountryID = #temp_uda.CountryID  
    FROM T_SMSymbolLookUpTable AS SM  
    INNER JOIN #temp_uda ON SM.UnderLyingSymbol = #temp_uda.Symbol
    INNER JOIN #XmlItem XI ON XI.TickerSymbol = #temp_uda.Symbol--only update for underlying symbols      
    WHERE  
     --( CHARINDEX('ROOT',#temp_uda.CompanyName) < 0 )  -- not a root symbol    
     --and     
     (  
      SM.TickerSymbol NOT IN (  
       SELECT #temp_uda.Symbol  
       FROM #temp_uda  
       )  
       AND XI.SymbolType=1
      ) -- update only for UDA not not set specifically                  
END     
 INSERT INTO T_SMReuters (  
  AUECID  
  ,ExchangeID  
  ,ReutersSymbol  
  ,Symbol_PK  
  ,IsPrimaryExchange  
  )  
 SELECT AUECID  
  ,ExchangeID  
  ,ReutersSymbol  
  ,Symbol_PK  
  ,1  
 FROM #tmp_XmlItem  
  
 INSERT INTO T_SMEquityNonHistoryData (  
  CompanyName  
  ,RoundLot  
  ,Symbol_PK  
  ,Delta  
  ,Multiplier  
  )  
 SELECT LongName  
  ,RoundLot  
  ,Symbol_PK  
  ,Delta  
  ,Multiplier  
 FROM #tmp_XmlItem  
 WHERE (  
   AssetID IN (  
    1  
    ,9  
    ,14  
    )  
   )  
  
 INSERT INTO T_SMOptiondata (  
  --MaturityMonth,                                                                                                           
  Multiplier  
  ,Symbol_PK  
  ,Strike  
  ,[Type]  
  ,  
  --SettlementDate,                                       
  ContractName  
  ,ExpirationDate  
  ,LeveragedFactor  
  ,IsCurrencyFuture  
  )  
 SELECT  
  --MaturityMonth,                                                                                                
  Multiplier  
  ,Symbol_PK  
  ,StrikePrice  
  ,PutOrCall  
  ,  
  --SettlementDate,                                                                                     
  LongName  
  ,ExpirationDate  
  ,Delta  
  ,IsCurrencyFuture  
 FROM #tmp_XmlItem  
 WHERE (  
   AssetID IN (  
    2  
    ,4  
    ,10
    )  
   )  
  
 INSERT INTO T_SMFutureData (  
  ExpirationDate  
  ,Multiplier  
  ,ContractName  
  ,LongName  
  ,Symbol_PK  
  ,CutOffTime  
  ,LeveragedFactor  
  ,IsCurrencyFuture  
  )  
 SELECT ExpirationDate  
  ,Multiplier  
  ,LongName  
  ,LongName  
  ,Symbol_PK  
  ,CutOffTime  
  ,Delta  
  ,IsCurrencyFuture  
 FROM #tmp_XmlItem  
 WHERE AssetID = 3  
  
 --select * from T_SMFxData                                                              
 INSERT INTO T_SMFxData (  
  --MaturityMonth,                                                                                              
  Symbol_PK  
  ,  
  --SettlementDate,                                                                                                          
  LongName  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,IsNDF  
  ,FixingDate  
  ,Multiplier  
  ,ExpirationDate  
  ,LeveragedFactor  
  )  
 SELECT Symbol_PK  
  ,LongName  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,IsNDF  
  ,FixingDate  
  ,Multiplier  
  ,ExpirationDate  
  ,Delta  
 FROM #tmp_XmlItem  
 WHERE (AssetID = 5)  
  
 --select * from T_SMFXForwardData                                                              
 INSERT INTO T_SMFXForwardData (  
  --MaturityMonth,                                                  
  Symbol_PK  
  ,  
  --SettlementDate,                                              
  LongName  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,ExpirationDate  
  ,Multiplier  
  ,IsNDF  
  ,FixingDate  
  ,LeveragedFactor  
  )  
 SELECT Symbol_PK  
  ,LongName  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,ExpirationDate  
  ,Multiplier  
  ,IsNDF  
  ,FixingDate  
  ,Delta  
 FROM #tmp_XmlItem  
 WHERE (AssetID = 11)  
  
 INSERT INTO T_SMIndexData (  
  LongName  
  ,Symbol_PK  
  ,LeveragedFactor  
  )  
 SELECT LongName  
  ,Symbol_PK  
  ,Delta  
 FROM #tmp_XmlItem  
 WHERE (AssetID IN (7))  
  
 INSERT INTO T_SMFixedIncomeData (  
  BondDescription  
  ,IssueDate  
  ,Coupon  
  ,MaturityDate  
  ,BondTypeID  
  ,AccrualBasisID  
  ,Symbol_PK  
  ,FirstCouponDate  
  ,IsZero  
  ,CouponFrequencyID  
  ,DaysToSettlement  
  ,Multiplier  
  ,LeveragedFactor  
  ,CollateralTypeID
  )  
 SELECT LongName  
  ,IssueDate  
  ,Coupon  
  ,MaturityDate  
  ,BondType  
  ,AccrualBasis  
  ,Symbol_PK  
  ,FirstCouponDate  
  ,IsZero  
  ,CouponFrequency  
  ,DaysToSettlement  
  ,Multiplier  
  ,Delta  
  ,CollateralType
 FROM #tmp_XmlItem  
 WHERE (  
   AssetID IN (  
    8  
    ,13  
    )  
   )  
  
 UPDATE T_SMSymbolLookUpTable  
 SET TickerSymbol = #XmlItem.TickerSymbol  
  ,UnderLyingID = #XmlItem.UnderLyingID  
  ,AUECID = #XmlItem.AUECID  
  ,AssetID = #XmlItem.AssetID  
  ,ISINSymbol = #XmlItem.ISINSymbol  
  ,SEDOLSymbol = #XmlItem.SEDOLSymbol  
  ,BloombergSymbol = #XmlItem.BloombergSymbol      
  ,FactSetSymbol = #XmlItem.FactSetSymbol
  ,ActivSymbol = #XmlItem.ActivSymbol
  ,CusipSymbol = #XmlItem.CusipSymbol  
  ,OSISymbol = #XmlItem.OSIOptionSymbol  
  ,IDCOSymbol = #XmlItem.IDCOOptionSymbol  
  ,OpraSymbol = #XmlItem.OpraSymbol  
  ,UnderlyingSymbol = #XmlItem.UnderlyingSymbol  
  ,ExchangeID = #XmlItem.ExchangeID  
  ,CurrencyID = #XmlItem.CurrencyID  
  ,Sector = #XmlItem.Sector  
  ,ModifiedDate = getdate()  
  ,RoundLot = #XmlItem.RoundLot  
  ,ProxySymbol = #XmlItem.ProxySymbol  
  ,IsSecApproved = #XmlItem.IsSecApproved  
  ,ApprovalDate = #XmlItem.ApprovalDate  
  ,ApprovedBy = #XmlItem.ApprovedBy  
  ,Comments = #XmlItem.Comments  
  ,UDAAssetClassID = #temp_uda.AssetID  
  ,UDASecurityTypeID = #temp_uda.SecurityTypeID  
  ,UDASectorID = #temp_uda.SectorID  
  ,UDASubSectorID = #temp_uda.SubSectorID  
  ,UDACountryID = #temp_uda.CountryID  
  ,CreatedBy = #XmlItem.CreatedBy  
  ,ModifiedBy = #XmlItem.ModifiedBy  
  ,PrimarySymbology = #XmlItem.PrimarySymbology  
  ,BBGID = #XmlItem.BBGID  
  ,StrikePriceMultiplier = #XmlItem.StrikePriceMultiplier  
  ,EsignalOptionRoot = #XmlItem.EsignalOptionRoot  
  ,BloombergOptionRoot = #XmlItem.BloombergOptionRoot  
  ,SharesOutstanding = #XmlItem.SharesOutstanding
  ,BloombergSymbolWithExchangeCode= #XmlItem.BloombergSymbolWithExchangeCode
 FROM #XmlItem  
 LEFT JOIN #temp_uda ON #temp_uda.Symbol = #XmlItem.TickerSymbol  
 WHERE #XmlItem.Symbol_PK = T_SMSymbolLookUpTable.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
 UPDATE T_SMReuters  
 SET AUECID = #XmlItem.AUECID  
  ,ExchangeID = #XmlItem.ExchangeID  
  ,ReutersSymbol = #XmlItem.ReutersSymbol  
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMReuters.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  AND T_SMReuters.ISPrimaryExchange = 'true'  
  
 --Modified by - omshiv, insert row in T_SMReuters if row not exists in table, only in update security case.              
 INSERT INTO T_SMReuters (  
  AUECID  
  ,ExchangeID  
  ,ReutersSymbol  
  ,Symbol_PK  
  ,IsPrimaryExchange  
  )  
 SELECT XI.AUECID  
  ,XI.ExchangeID  
  ,XI.ReutersSymbol  
  ,XI.Symbol_PK  
  ,1  
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND XI.Symbol_pk NOT IN (  
   SELECT symbol_pk  
   FROM T_SMReuters  
   )  
  
 --Select * from T_SMReuters    
 DELETE  
 FROM T_SMEquityNonHistoryData  
 WHERE Symbol_pk IN (  
   SELECT symbol_pk  
   FROM #XmlItem  
   WHERE AssetID NOT IN (  
     1  
     ,9  
     ,14  
     )  
   )  
  
 DELETE  
 FROM T_SMOptionData  
 WHERE Symbol_pk IN (  
   SELECT symbol_pk  
   FROM #XmlItem  
   WHERE AssetID NOT IN (  
     2  
     ,4  
     ,10
     )  
   )  
  
 DELETE  
 FROM T_SMFutureData  
 WHERE Symbol_pk IN (  
   SELECT symbol_pk  
   FROM #XmlItem  
   WHERE AssetID NOT IN (3)  
   )  
  
 DELETE  
 FROM T_SMIndexData  
 WHERE Symbol_pk IN (  
   SELECT symbol_pk  
   FROM #XmlItem  
   WHERE AssetID NOT IN (7)  
   )  
  
 DELETE  
 FROM T_SMFXForwardData  
 WHERE Symbol_pk IN (  
   SELECT symbol_pk  
   FROM #XmlItem  
   WHERE AssetID NOT IN (11)  
   )  
  
 INSERT INTO T_SMEquityNonHistoryData (  
  CompanyName  
  ,RoundLot  
  ,Symbol_PK  
  ,Delta  
  ,Multiplier  
  )  
 SELECT XI.LongName  
  ,XI.RoundLot  
  ,XI.Symbol_PK  
  ,XI.Delta  
  ,XI.Multiplier  
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND (  
   XI.AssetID IN (  
    1  
    ,9  
    ,14  
    )  
   )  
  AND XI.Symbol_pk NOT IN (  
   SELECT Symbol_pk  
   FROM T_SMEquityNonHistoryData  
   )  
  
 INSERT INTO T_SMOptiondata (  
  --MaturityMonth,                                                                  
  Multiplier  
  ,Symbol_PK  
  ,Strike  
  ,[Type]  
  ,  
  --SettlementDate,                                                                                                          
  ContractName  
  ,ExpirationDate  
  ,LeveragedFactor  
  ,IsCurrencyFuture  
  )  
 SELECT  
  --MaturityMonth,                                                                                                           
  XI.Multiplier  
  ,XI.Symbol_PK  
  ,XI.StrikePrice  
  ,XI.PutOrCall  
  ,  
  --SettlementDate,                                                                                                          
  XI.LongName  
  ,XI.ExpirationDate  
  ,XI.Delta  
  ,XI.IsCurrencyFuture  
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND (  
   XI.AssetID IN (  
    2  
    ,4  
    ,10
    )  
   )  
  AND XI.Symbol_pk NOT IN (  
   SELECT Symbol_pk  
   FROM T_SMOptiondata  
   )  
  
 INSERT INTO T_SMFutureData (  
  ExpirationDate  
  ,Multiplier  
  ,ContractName  
  ,LongName  
  ,Symbol_PK  
  ,CutOffTime  
  ,LeveragedFactor  
  ,IsCurrencyFuture  
  )  
 SELECT XI.ExpirationDate  
  ,XI.Multiplier  
  ,XI.LongName  
  ,XI.LongName  
  ,XI.Symbol_PK  
  ,XI.CutOffTime  
  ,XI.Delta  
  ,XI.IsCurrencyFuture  
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND XI.AssetID IN (3)  
  AND XI.Symbol_pk NOT IN (  
   SELECT Symbol_pk  
   FROM T_SMFutureData  
   )  
  
 INSERT INTO T_SMFxData (  
  Symbol_PK  
  ,LongName  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,IsNDF  
  ,FixingDate  
  ,Multiplier  
  ,ExpirationDate  
  ,LeveragedFactor  
  )  
 SELECT XI.Symbol_PK  
  ,XI.LongName  
  ,XI.LeadCurrencyID  
  ,XI.VsCurrencyID  
  ,XI.IsNDF  
  ,XI.FixingDate  
  ,XI.Multiplier  
  ,XI.ExpirationDate  
  ,XI.Delta  
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND XI.AssetID = 5  
  AND XI.Symbol_pk NOT IN (  
   SELECT Symbol_pk  
   FROM T_SMFxData  
   )  
  
 INSERT INTO T_SMFXForwardData (  
  Symbol_PK  
  ,LongName  
  ,LeadCurrencyID  
  ,VsCurrencyID  
  ,ExpirationDate  
  ,Multiplier  
  ,IsNDF  
  ,FixingDate  
  ,LeveragedFactor  
  )  
 SELECT XI.Symbol_PK  
  ,XI.LongName  
  ,XI.LeadCurrencyID  
  ,XI.VsCurrencyID  
  ,XI.ExpirationDate  
  ,XI.Multiplier  
  ,XI.IsNDF  
  ,XI.FixingDate  
  ,XI.Delta  
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND XI.AssetID = 11  
  AND XI.Symbol_pk NOT IN (  
   SELECT Symbol_pk  
   FROM T_SMFXForwardData  
   )  
  
 INSERT INTO T_SMIndexData (  
  LongName  
  ,Symbol_PK  
  ,LeveragedFactor  
  )  
 SELECT XI.LongName  
  ,XI.Symbol_PK  
  ,XI.Delta  
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND (XI.AssetID IN (7))  
  AND XI.Symbol_pk NOT IN (  
   SELECT Symbol_pk  
   FROM T_SMIndexData  
   )  
  
 INSERT INTO T_SMFixedIncomeData (  
  BondDescription  
  ,IssueDate  
  ,Coupon  
  ,MaturityDate  
  ,BondTypeID  
  ,AccrualBasisID  
  ,Symbol_PK  
  ,FirstCouponDate  
  ,IsZero  
  ,CouponFrequencyID  
  ,DaysToSettlement  
  ,Multiplier  
  ,LeveragedFactor  
  ,CollateralTypeID
  )  
 SELECT XI.LongName  
  ,XI.IssueDate  
  ,XI.Coupon  
  ,XI.MaturityDate  
  ,XI.BondType  
  ,XI.AccrualBasis  
  ,XI.Symbol_PK  
  ,XI.FirstCouponDate  
  ,XI.IsZero  
  ,XI.CouponFrequency  
  ,XI.DaysToSettlement  
  ,XI.Multiplier  
  ,XI.Delta  
  ,XI.CollateralType
 FROM #XmlItem XI  
 INNER JOIN T_SMSymbolLookUpTable SM ON SM.Symbol_PK = XI.Symbol_PK  
 WHERE XI.SymbolType = 1  
  AND (  
   XI.AssetID IN (  
    8  
    ,13  
    )  
   )  
  AND XI.Symbol_pk NOT IN (  
   SELECT Symbol_pk  
   FROM T_SMFixedIncomeData  
   )  
  
 UPDATE T_SMEquityNonHistoryData  
 SET CompanyName = #XmlItem.LongName  
  ,RoundLot = isnull(#XmlItem.RoundLot, T_SMEquityNonHistoryData.RoundLot)  
  ,Delta = #XmlItem.Delta  
  ,Multiplier = #XmlItem.Multiplier  
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMEquityNonHistoryData.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
 UPDATE T_SMOptiondata  
 SET Multiplier = #XmlItem.Multiplier  
  ,Strike = #XmlItem.StrikePrice  
  ,[Type] = #XmlItem.PutOrCall  
  ,ContractName = #XmlItem.LongName  
  ,ExpirationDate = #XmlItem.ExpirationDate  
  ,Leveragedfactor = #XmlItem.Delta  
  ,IsCurrencyFuture = #XmlItem.IsCurrencyFuture  
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMOptiondata.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
 UPDATE T_SMFutureData  
 SET ExpirationDate = #XmlItem.ExpirationDate  
  ,Multiplier = #XmlItem.Multiplier  
  ,ContractName = #XmlItem.LongName  
  ,LongName = #XmlItem.LongName  
  ,LeveragedFactor = #XmlItem.Delta  
  ,IsCurrencyFuture = #XmlItem.IsCurrencyFuture  
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMFutureData.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
 UPDATE T_SMFxData  
 SET LongName = #XmlItem.LongName  
  ,LeadCurrencyID = #XmlItem.LeadCurrencyID  
  ,VsCurrencyID = #XmlItem.VsCurrencyID  
  ,IsNDF = #XmlItem.IsNDF  
  ,FixingDate = #XmlItem.FixingDate  
  ,Multiplier = #XmlItem.Multiplier  
  ,ExpirationDate = #XmlItem.ExpirationDate  
  ,LeveragedFactor = #XmlItem.Delta  
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMFxData.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
 UPDATE T_SMFXForwardData  
 SET LongName = #XmlItem.LongName  
  ,LeadCurrencyID = #XmlItem.LeadCurrencyID  
  ,VsCurrencyID = #XmlItem.VsCurrencyID  
  ,ExpirationDate = #XmlItem.ExpirationDate  
  ,Multiplier = #XmlItem.Multiplier  
  ,IsNDF = #XmlItem.IsNDF  
  ,FixingDate = #XmlItem.FixingDate  
  ,LeveragedFactor = #XmlItem.Delta  
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMFXForwardData.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
 UPDATE T_SMIndexData  
 SET LongName = #XmlItem.LongName  
  ,LeveragedFactor = #XmlItem.Delta  
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMIndexData.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
 UPDATE T_SMFixedIncomeData  
 SET BondDescription = #XmlItem.LongName  
  ,IssueDate = #XmlItem.IssueDate  
  ,Coupon = #XmlItem.Coupon  
  ,MaturityDate = #XmlItem.MaturityDate  
  ,BondTypeID = #XmlItem.BondType  
  ,AccrualBasisID = #XmlItem.AccrualBasis  
  ,Symbol_PK = #XmlItem.Symbol_PK  
  ,FirstCouponDate = #XmlItem.FirstCouponDate  
  ,IsZero = #XmlItem.IsZero  
  ,CouponFrequencyID = #XmlItem.CouponFrequency  
  ,DaysToSettlement = #XmlItem.DaysToSettlement  
  ,Multiplier = #XmlItem.Multiplier  
  ,LeveragedFactor = #XmlItem.Delta  
  ,CollateralTypeID = #XmlItem.CollateralType
 FROM #XmlItem  
 WHERE #XmlItem.Symbol_PK = T_SMFixedIncomeData.Symbol_PK  
  AND #XmlItem.SymbolType = 1  
  
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
 SELECT TSM.Symbol_PK, COALESCE(ENHD.CompanyName, OD.ContractName, FD.ContractName, FXD.LongName, FXFD.LongName, FID.BondDescription, 'Undefined') AS Issuer  
 INTO #IssuerTable  
 FROM #TempDynamicUDADataXmlSymbolWise TSM  
   LEFT JOIN T_SMSymbolLookUpTable SM  ON SM.TickerSymbol = (SELECT UnderLyingSymbol FROM T_SMSymbolLookUpTable WHERE Symbol_PK = TSM.Symbol_PK)   
   LEFT JOIN T_SMEquityNonHistoryData ENHD ON ENHD.Symbol_PK = SM.Symbol_PK   
   LEFT JOIN T_SMOptionData OD    ON OD.Symbol_PK = SM.Symbol_PK   
   LEFT JOIN T_SMFutureData FD    ON FD.Symbol_PK = SM.Symbol_PK   
   LEFT JOIN T_SMFxData FXD    ON FXD.Symbol_PK = SM.Symbol_PK   
   LEFT JOIN T_SMFxForwardData FXFD  ON FXFD.Symbol_PK = SM.Symbol_PK   
   LEFT JOIN T_SMFixedIncomeData FID  ON FID.Symbol_PK = SM.Symbol_PK  
  
 DECLARE @SymbolPK BIGINT, @Issuer VARCHAR(100)  
  
 WHILE EXISTS(SELECT 1 FROM #IssuerTable)  
 BEGIN  
  SET @SymbolPK = (SELECT TOP 1 Symbol_PK FROM #IssuerTable)  
  SELECT @Issuer = Issuer FROM #IssuerTable WHERE Symbol_PK = @SymbolPK  
  
  UPDATE #TempDynamicUDADataXmlSymbolWise  
  SET  UDAData.modify('delete /DynamicUDAs/Issuer')  
  WHERE UDAData.exist('/DynamicUDAs/Issuer') = 1 AND  
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
  
 -- Updating dynamic UDAs of Derived Symbols      
    
-- SELECT SM.Symbol_PK, DUDA.UDAData, 0 AS FundID INTO #TempUpdateDerivativesDynamicUDA  
-- FROM T_SMSymbolLookUpTable AS SM  
-- INNER JOIN #XmlItem X ON SM.UnderLyingSymbol = X.TickerSymbol  
-- LEFT JOIN T_UDA_DynamicUDAData DUDA ON X.Symbol_PK = DUDA.Symbol_PK  
-- WHERE SM.TickerSymbol NOT IN (  
--   SELECT #XmlItem.TickerSymbol  
--   FROM #XmlItem  
--   WHERE #XmlItem.TickerSymbol IS NOT NULL  
--   )  
  
-- WHILE EXISTS( SELECT 1 FROM #TempUpdateDerivativesDynamicUDA)  
-- BEGIN  
--  SET @Symbol_PK = (SELECT TOP 1 Symbol_PK FROM #TempUpdateDerivativesDynamicUDA)  
--  SELECT @UDAData = UDAData, @FundID = FundID FROM #TempUpdateDerivativesDynamicUDA WHERE Symbol_PK = @Symbol_PK  
--  
--  IF(@UDAData IS NOT NULL)  
--  BEGIN  
--   -- call procedure to store dynamic UDA data  
--   EXEC P_UDA_SaveDynamicUDAData  
--   @Symbol_PK, @UDAData, @FundID  
--  END  
--  
--  DELETE FROM #TempUpdateDerivativesDynamicUDA WHERE Symbol_PK = @Symbol_PK  
--   
-- END  
  
if(@IsAutoUpdateDerivateUDA = 1)  
BEGIN

    DECLARE @UpdateDerivativestring VARCHAR(MAX)  
    SET  @UpdateDerivativestring = ''  
    SELECT @UpdateDerivativestring = @UpdateDerivativestring + ' DUDA.[' + Tag + '] = UDA.[' + Tag + '],'  
    FROM T_UDA_DynamicUDA   
     
    SET  @UpdateDerivativestring = SUBSTRING(@UpdateDerivativestring,0,LEN(@UpdateDerivativestring))  
     
    EXEC('  
    UPDATE DUDA  
    SET ' + @UpdateDerivativestring +   
    ' FROM T_SMSymbolLookUpTable AS SM  
    INNER JOIN #XmlItem X ON SM.UnderLyingSymbol = X.TickerSymbol  
    INNER JOIN T_UDA_DynamicUDAData UDA ON X.Symbol_PK = UDA.Symbol_PK  
    INNER JOIN T_UDA_DynamicUDAData DUDA ON DUDA.Symbol_PK = SM.Symbol_PK AND DUDA.FundID = 0  
    WHERE SM.TickerSymbol NOT IN (  
      SELECT #XmlItem.TickerSymbol  
      FROM #XmlItem  
      WHERE #XmlItem.TickerSymbol IS NOT NULL  
      )')   
 END    
 DROP TABLE #TempDynamicUDADataXmlSymbolWise --, #TempUpdateDerivativesDynamicUDA  
  
 DROP TABLE #tmp_XmlItem  
  ,#XmlItem  
  ,#temp_uda  
  
 EXEC sp_xml_removedocument @handle  
  
 COMMIT TRANSACTION TRAN1  
END TRY  
  
BEGIN CATCH  
 SET @ErrorMessage = ERROR_MESSAGE();  
 SET @ErrorNumber = Error_number();  
  
 ROLLBACK TRANSACTION TRAN1  
END CATCH;  
