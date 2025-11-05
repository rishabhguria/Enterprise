
/***********************************************
Modified by: Disha Sharma  
Date :10 July 2015  
Desc: Saved Dynamic UDAs xml

Modified By:	Disha Sharma
Date:			08-14-2015
Description:	Modified to use columns in T_UDA_DynamicUDAData instead of Dynamic UDA XML  
***********************************************/

Create PROCEDURE [dbo].[P_SaveSecurityMasterDataForSymbol_Import]                                                                                                  
(                                                                                                  
 @Xml varchar(max),                                                                            
 @dataSource int,                                                                           
 @ErrorMessage varchar(500) output,                                                                                                                                           
 @ErrorNumber int output                                                                                           
)                                                                                                  
As
SET @ErrorNumber = 0                                                                                                                                      
SET @ErrorMessage = 'Success'           
                                                                                                                  
BEGIN TRY                                                                                                           
                                                                                                  
 BEGIN TRAN TRAN1                                                                                 
                
DECLARE @handle int                                                                               
exec sp_xml_preparedocument @handle OUTPUT,@Xml                                              
                                                                                            
Create TABLE #tmp_XmlItem                                                                                            
(                                                                                            
ExchangeID   int                                                                              
, UnderLyingID  int                                                                              
, AUECID    int                                                                              
, AssetID    int                                                                              
, CusipSymbol Varchar(20)                                                                              
, SEDOLSymbol Varchar(20)                                                                                             
, ISINSymbol  Varchar(20)                                                                               
, ReutersSymbol Varchar(100)                                                                                 
, TickerSymbol  varchar(100)                                                                                    
, BloombergSymbol varchar(200)                
, OSIOptionSymbol varchar(25)                
, IDCOOptionSymbol varchar(25)                
, OpraSymbol varchar(20)                
, UnderLyingSymbol varchar(100)                                                                                         
, CompanyName   varchar(50)                                                                              
, Symbol_PK      varchar(100)                                                                              
,RoundLot DECIMAL(28,10)                                                                              
,CurrencyID int                                                                              
,Sector varchar(20)                                                                              
,LongName varchar(50)                                                                         
,PutOrCall int                                                         
,StrikePrice float                                                                              
,ExpirationDate datetime               
,Multiplier float                        
,SymbolType int                                                                              
,Delta float                                  
, LeadCurrencyID int                                  
, VsCurrencyID  int      
      
,IssueDate datetime                             
,Coupon float                               
,MaturityDate datetime                             
,SecurityType varchar(50)                            
,AccrualBasis varchar(50)                                                                                                 
,FirstCouponDate datetime                        
,IsZero bit                        
,CouponFrequency int                        
,DaysToSettlement  int                    
,CutOffTime varchar(50)  
,IsNDF bit
,FixingDate datetime
,IsSecApproved bit
,ApprovalDate datetime
,ApprovedBy varchar(100)
,Comments varchar(500)
,CreatedBy varchar(100)
,ModifiedBy varchar(100)
,PrimarySymbology int
,BBGID varchar(20) 
                        
)                                                           
                                 
INSERT INTO #tmp_XmlItem                            
(                                                                                            
ExchangeID                                                                           
, UnderLyingID                                                 
, AUECID                                                                              
, AssetID                                                                         
, CusipSymbol                                          
, SEDOLSymbol                                                                                          
, ISINSymbol                                                                      
, ReutersSymbol                                                                                   
, TickerSymbol                                                                               
, BloombergSymbol                
, OSIOptionSymbol                
, IDCOOptionSymbol                
, OpraSymbol                                                                                
, UnderLyingSymbol                                                                               
, CompanyName                                                           
,Symbol_PK                                                                              
,RoundLot                                                                              
,CurrencyID                                                               
,Sector                                                                              
,LongName                                                                              
,PutOrCall                                                   
,StrikePrice                                                        
, ExpirationDate                                                                              
, Multiplier                                                              
,SymbolType                                                                              
,Delta                                   
, LeadCurrencyID                                  
,VsCurrencyID        
      
,IssueDate                              
,Coupon                             
,MaturityDate                      
,SecurityType                              
,AccrualBasis                          
  ,FirstCouponDate                        
,IsZero                         
,CouponFrequency                         
,DaysToSettlement                    
,CutOffTime 
,IsNDF 
,FixingDate
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments  
,CreatedBy 
,ModifiedBy
,PrimarySymbology
,BBGID                                                                                
)                                                                                  
Select distinct                                       
ExchangeID                                                                                          
, UnderLyingID                                                                                        
, AUECID                                                                              
, AssetID                                                                                         
, CusipSymbol                                                                                          
, SEDOLSymbol                                                                                          
, ISINSymbol                                                                             
, ReutersSymbol                                                                                   
, TickerSymbol                                                                               
, BloombergSymbol                
, OSIOptionSymbol                
, IDCOOptionSymbol                
, IsNull(OpraSymbol,'')      
, UnderLyingSymbol                                                              
, CompanyName                                                                               
,Symbol_PK                  
,RoundLot                                                                              
,CurrencyID                                                                              
,Sector                                                            
,LongName                                                      
,PutOrCall                                                                              
,StrikePrice                                                                              
,ExpirationDate                                                                
,Multiplier                                                               
,SymbolType                                                                             
,Delta                                     
, LeadCurrencyID                                  
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
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments  
,CreatedBy 
,ModifiedBy
,PrimarySymbology
,BBGID                                                                                               
                                               
FROM  OPENXML(@handle, '//',3)                                                                      
 WITH                                                                                              
(                                                     
ExchangeID   int                                                                              
, UnderLyingID   int                                                                              
, AUECID    int                                                             
, AssetID    int                                                                              
, CusipSymbol   varchar(20)                                                                              
, SedolSymbol   varchar(20)                                                                               
, ISINSymbol   varchar(20)                                                                               
, ReutersSymbol   varchar(100)                                      
, TickerSymbol   varchar(100)                                           
, BloombergSymbol varchar(200)                   
, OSIOptionSymbol varchar(25)                
, IDCOOptionSymbol varchar(25)                
, OpraSymbol varchar(20)                                                                     
, UnderLyingSymbol  varchar(100)                                                                               
, CompanyName   varchar(50)                                                                              
,Symbol_PK varchar(100)                                                                              
,RoundLot DECIMAL(28,10)                             
,CurrencyID int                                                                              
,Sector varchar(20)                                                                              
,LongName varchar(50)                                                                              
,PutOrCall int                                                                              
,StrikePrice float                                                                         
,ExpirationDate datetime                                                                              
,Multiplier float                                                               
,SymbolType  int                                                                            
,Delta float                                  
, LeadCurrencyID int                                  
,VsCurrencyID  int       
      
,IssueDate datetime                             
,Coupon float                               
,MaturityDate datetime                             
,BondTypeID varchar(50)                              
,AccrualBasisID varchar(50)                        
,FirstCouponDate datetime                        
,IsZero bit                        
,CouponFrequencyID int                        
,DaysToSettlement  int                    
,CutOffTime varchar(50) 
,IsNDF bit
,FixingDate datetime
,IsSecApproved bit
,ApprovalDate datetime
,ApprovedBy varchar(100)
,Comments varchar(500)
,CreatedBy varchar(100)
,ModifiedBy varchar(100)
,PrimarySymbology int
,BBGID varchar(20)                                               
)                                                                
      
CREATE TABLE #temp_uda                                                                                
(                                                                                
Symbol varchar(100),                                                                                
AssetID int,  
SecurityTypeID int,  
SectorID int,  
SubSectorID int,  
CountryID int,  
UnderlyingSymbol varchar(100)                                                                        
)                                                                                
                                                                                
                                                                          
                                                                                
INSERT INTO #temp_uda                                                                                
(                                                                                
Symbol,                                                                                
AssetID,  
SecurityTypeID,  
SectorID,  
SubSectorID,  
CountryID,  
UnderlyingSymbol                                                                                
)                                                                                 
SELECT                                                                                 
Symbol,                                                                                
AssetID,  
SecurityTypeID,  
SectorID,  
SubSectorID,  
CountryID,  
UnderlyingSymbol                                                                            
FROM  OPENXML(@handle, '//SymbolUDAData',2)                                                                                                                                                                    
WITH                                                                                    
(                                                                                        
Symbol varchar(100),                                                                                
AssetID int,  
SecurityTypeID int,  
SectorID int,  
SubSectorID int,  
CountryID int,  
UnderlyingSymbol varchar(100)                                                                               
)

-- Delete a row where Ticker Symbol is null as Ticker Symbol is mandatory          
Delete FROM #tmp_XmlItem  Where TickerSymbol is null           
        
--Delete data from Temp table for those symbols which are already in the Sec Master DB        
Delete #tmp_XmlItem Where #tmp_XmlItem.TickerSymbol in (Select TickerSymbol From T_SMSymbolLookUpTable)                  
          
--Check for Duplicate Symbol, remove if duplicate symbol          
;WITH SecMasterCTE(ExchangeID, UnderlyingID, AUECID,AssetID,TickerSymbol, Ranking)          
AS          
(          
SELECT          
 ExchangeID, UnderlyingID, AUECID,AssetID,TickerSymbol,          
Ranking = DENSE_RANK() OVER(PARTITION BY ExchangeID, UnderlyingID, AUECID,AssetID,TickerSymbol ORDER BY NEWID() ASC)          
FROM #tmp_XmlItem          
)          
DELETE FROM SecMasterCTE          
WHERE Ranking > 1          
          
                                                     
INSERT INTO T_SMSymbolLookUpTable                                                                                        
(                  
 TickerSymbol                                                                                         
,UnderLyingID                                                                           
,AUECID                                                 
,AssetID                                                                                  
,ISINSymbol                                         
,SEDOLSymbol                                                                                         
,BloombergSymbol                
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
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments ,
UDAAssetClassID , 
UDASecurityTypeID,                                    
UDASectorID ,
UDASubSectorID,
UDACountryID  
,CreatedBy 
,ModifiedBy
,PrimarySymbology
,BBGID                                         
)                                                                                        
SELECT                                             
TickerSymbol                                                                                         
,UnderLyingID                                        
,AUECID                                                                                 
,#tmp_XmlItem.AssetID                               
,ISINSymbol                                                                                          
,SEDOLSymbol                                                                             
,BloombergSymbol                
,OSIOptionSymbol                
,IDCOOptionSymbol                
,CASE           
 When OpraSymbol is null           
 Then ''            
 Else OpraSymbol           
End As OpraSymbol                                                                                   
,#tmp_XmlItem.UnderLyingSymbol                                                                               
,CusipSymbol                                                             
,ExchangeID                                                                                    
,Symbol_PK                                                            
,GetDate()                                                                              
,CurrencyID                                                        
,Sector                                                                          
,@dataSource
,IsSecApproved
,ApprovalDate 
,ApprovedBy 
,Comments,                                                                             
#temp_uda.AssetID,  
#temp_uda.SecurityTypeID,  
#temp_uda.SectorID,  
#temp_uda.SubSectorID,  
#temp_uda.CountryID  
,CreatedBy 
,ModifiedBy
,PrimarySymbology
,BBGID                                                                                            
FROM                                                 
#tmp_XmlItem 
left JOIN #temp_uda on #temp_uda.Symbol =#tmp_XmlItem.TickerSymbol                                                                  
                                                                 
INSERT INTO T_SMReuters                                                                
(                                                                
AUECID,                                                                
ExchangeID,                                                                
ReutersSymbol,                                                                
Symbol_PK,                                                                
IsPrimaryExchange                                                                
)                                                                
Select                                                        
AUECID,                                              
ExchangeID,                                                                
ReutersSymbol,                                                                
Symbol_PK,                                                                
1                                                     
from  #tmp_XmlItem                                                                 
       
-- insert data for Equiry(1) and PrivateEquity(9)                                                                            
INSERT INTO T_SMEquityNonHistoryData                                            
(                                                                                        
 CompanyName                                                                             
,RoundLot                    
,Symbol_PK                                                                               
,Delta              
,Multiplier                                                                         
)                                                                                        
SELECT                                                                                          
 LongName                                                                                   
,RoundLot                                                                                  
,Symbol_PK                                                                       
,Delta            
,Multiplier                                                                          
FROM         
#tmp_XmlItem  where  (AssetID in (1,9,14) )                                        
           
-- insert data for EquityOption(2) And FutureOption(4)                                                                            
INSERT INTO T_SMOptiondata                                                                              
(                                                                     
Multiplier,                                                                              
Symbol_PK,                                
Strike,                                                 
[Type],                                                       
ContractName,                                                
Expirationdate                                                                              
)                                                                              
Select                                                                               
Multiplier,                                                                              
Symbol_PK,                                                              
StrikePrice,                                 
PutOrCall,                                                                              
LongName ,                                                
Expirationdate                                                                             
                                                                              
From #tmp_XmlItem where  (AssetID in (2,4))                                  
                          
-- Insert Data for Future Contract(3)                                                     
INSERT INTO T_SMFutureData                                                                              
(                                                         
ExpirationDate,                                                                               
Multiplier,                                                                              
ContractName,                                                                              
LongName,                             
Symbol_PK,      
CutOffTime                                                                   
)                                                                              
Select                                                                               
Expirationdate,                                             
Multiplier,                                                                
LongName,                                                                              
LongName,                                                                              
Symbol_PK ,      
CutOffTime                                                                                 
From #tmp_XmlItem                                                                               
where AssetID= 3                            
                                  
--Insert Data for FX(5)                   
INSERT INTO T_SMFxData                                                                              
(                                                                     
Symbol_PK,                                                                              
LongName,                                  
LeadCurrencyID,                                  
VsCurrencyID,
IsNDF, 
FixingDate,
Multiplier,
ExpirationDate                                                                           
)                                    
Select                                 
Symbol_PK,                                                              
LongName ,                                  
LeadCurrencyID,                                  
VsCurrencyID,
IsNDF, 
FixingDate,
Multiplier,
ExpirationDate                                                  
                                                                              
From #tmp_XmlItem where  (AssetID =5)                                    
                                  
--insert data for FX Forward(11)                                  
INSERT INTO T_SMFXForwardData                                  
(                                                                  
Symbol_PK,                                                                              
LongName,                                  
LeadCurrencyID,                                  
VsCurrencyID,                          
ExpirationDate,                    
Multiplier,
IsNDF, 
FixingDate                                              
)                                                                              
Select                                 
Symbol_PK,                                                              
LongName ,                                  
LeadCurrencyID,                                  
VsCurrencyID,                                                
ExpirationDate,                    
Multiplier,
IsNDF, 
FixingDate                                                                               
From #tmp_XmlItem where  (AssetID =11)                          
                       
-- For Indices(7)              
INSERT INTO T_SMIndexData                                                                              
(                                                                               
LongName,                                                                              
Symbol_PK                                                                         
)                                                                              
Select                                                                              
LongName,                                                                              
Symbol_PK                             
From #tmp_XmlItem where  (AssetID in (7))       
      
-- insert Data for FixedIncome(8)      
INSERT INTO T_SMFixedIncomeData                           
(                                                                                         
BondDescription,                                                                                                   
IssueDate,                              
Coupon,                              
MaturityDate,                              
BondTypeID,                              
AccrualBasisID,                                                                                                  
Symbol_PK,                        
FirstCouponDate,                        
IsZero,                        
CouponFrequencyID,                        
DaysToSettlement,              
Multiplier                        
                                                                                          
)                                                                                                  
Select                                                                          
LongName,                                                                                
IssueDate,                              
Coupon,                              
MaturityDate,                              
SecurityType,                              
AccrualBasis,                                                                                                  
Symbol_PK,                                                  
FirstCouponDate,            
IsZero,                        
CouponFrequency,                        
DaysToSettlement,              
Multiplier                                                                                                   
From #tmp_XmlItem where  (AssetID in (8,13))           

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
	SELECT	TSM.Symbol_PK, COALESCE(ENHD.CompanyName, OD.ContractName, FD.ContractName, FXD.LongName, FXFD.LongName, FID.BondDescription, 'Undefined') AS Issuer
	INTO	#IssuerTable
	FROM	#TempDynamicUDADataXmlSymbolWise TSM
			LEFT JOIN T_SMSymbolLookUpTable SM		ON SM.TickerSymbol = (SELECT UnderLyingSymbol FROM T_SMSymbolLookUpTable WHERE Symbol_PK = TSM.Symbol_PK) 
			LEFT JOIN T_SMEquityNonHistoryData ENHD	ON ENHD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMOptionData OD				ON OD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFutureData FD				ON FD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFxData FXD				ON FXD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFxForwardData FXFD		ON FXFD.Symbol_PK = SM.Symbol_PK 
			LEFT JOIN T_SMFixedIncomeData FID		ON FID.Symbol_PK = SM.Symbol_PK

	DECLARE @SymbolPK BIGINT, @Issuer VARCHAR(100)

	WHILE EXISTS(SELECT 1 FROM #IssuerTable)
	BEGIN
		SET	@SymbolPK	= (SELECT TOP 1 Symbol_PK FROM #IssuerTable)
		SELECT @Issuer	= Issuer FROM #IssuerTable WHERE Symbol_PK = @SymbolPK

		UPDATE	#TempDynamicUDADataXmlSymbolWise
		SET		UDAData.modify('delete /DynamicUDAs/Issuer')
		WHERE	UDAData.exist('/DynamicUDAs/Issuer') = 1 AND
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

--	SELECT SM.Symbol_PK, DUDA.UDAData, 0 AS FundID INTO #TempUpdateDerivativesDynamicUDA
--	FROM T_SMSymbolLookUpTable AS SM
--	INNER JOIN #tmp_XmlItem X ON SM.UnderLyingSymbol = X.TickerSymbol
--	LEFT JOIN T_UDA_DynamicUDAData DUDA ON X.Symbol_PK = DUDA.Symbol_PK
--	WHERE SM.TickerSymbol NOT IN (
--			SELECT #tmp_XmlItem.TickerSymbol
--			FROM #tmp_XmlItem
--			WHERE #tmp_XmlItem.TickerSymbol IS NOT NULL
--			)
--
--	WHILE EXISTS( SELECT 1 FROM #TempUpdateDerivativesDynamicUDA)
--	BEGIN
--		SET @Symbol_PK = (SELECT TOP 1 Symbol_PK FROM #TempUpdateDerivativesDynamicUDA)
--		SELECT @UDAData = UDAData, @FundID = FundID FROM #TempUpdateDerivativesDynamicUDA WHERE Symbol_PK = @Symbol_PK
--
--		IF(@UDAData IS NOT NULL)
--		BEGIN
--			-- call procedure to store dynamic UDA data
--			EXEC P_UDA_SaveDynamicUDAData
--			@Symbol_PK, @UDAData, @FundID
--		END
--
--		DELETE FROM #TempUpdateDerivativesDynamicUDA WHERE Symbol_PK = @Symbol_PK
--	
--	END

	DECLARE	@UpdateDerivativestring VARCHAR(MAX)
	SET		@UpdateDerivativestring = ''
	SELECT	@UpdateDerivativestring = @UpdateDerivativestring + ' DUDA.[' + Tag + '] = UDA.[' + Tag + '],'
	FROM	T_UDA_DynamicUDA 

	SET		@UpdateDerivativestring = SUBSTRING(@UpdateDerivativestring,0,LEN(@UpdateDerivativestring))

	SELECT('
	UPDATE	DUDA
	SET	' + @UpdateDerivativestring +	
	' FROM	T_SMSymbolLookUpTable AS SM
	INNER JOIN #XmlItem X ON SM.UnderLyingSymbol = X.TickerSymbol
	INNER JOIN T_UDA_DynamicUDAData UDA ON X.Symbol_PK = UDA.Symbol_PK
	INNER JOIN T_UDA_DynamicUDAData DUDA ON DUDA.Symbol_PK = SM.Symbol_PK AND DUDA.FundID = 0
	WHERE SM.TickerSymbol NOT IN (
			SELECT #XmlItem.TickerSymbol
			FROM #XmlItem
			WHERE #XmlItem.TickerSymbol IS NOT NULL
			)')	

	DROP TABLE #TempDynamicUDADataXmlSymbolWise --, #TempUpdateDerivativesDynamicUDA

	DROP TABLE #tmp_XmlItem
		,#temp_uda                                                              
                                                                      
EXEC sp_xml_removedocument @handle                                                  
COMMIT TRANSACTION TRAN1                                 
                                  
                                                                                                         
END TRY                                                                                                          
BEGIN CATCH                                                       
SET @ErrorMessage = ERROR_MESSAGE();                                                                                                          
SET @ErrorNumber = Error_number();                                                                                                  
ROLLBACK TRANSACTION TRAN1                                              
END CATCH;

