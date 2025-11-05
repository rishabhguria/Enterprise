
/***********************************************
Created By: Om shiv
Date: 10-12-2013
Desc.: Get Advanced search filter SM data, 

Modified by: Sumeet Kumar 
Date :8 JULY 2015  
Desc: Adding Option IsCurrencyFuture and Future IsCurrencyFuture Fields
Link: http://jira.nirvanasolutions.com:8080/browse/PRANA-9418

Modified by: Disha Sharma  
Date :10 July 2015  
Desc: Returned Dynamic UDAs xml  

 Modified By:	Disha Sharma
 Date:		08-14-2015
 Description:	Modified to use columns in T_UDA_DynamicUDAData instead of Dynamic UDA XML

 Modified By:	Bhupendra Singh Bora
 Date:		09-17-2015
 Description:	Modified condition to search lead and Vs CurrecyID
***********************************************/
CREATE PROCEDURE [dbo].[P_AdvancedSearchSecMaster]                                                                
(                                                                
@condition VARCHAR(max),
@startIndex int  ,
@endIndex int                                                          
)  
as

DECLARE @SQL_SCRIPT VARCHAR(MAX)

CREATE TABLE #TempSecMasterDatatable
(
	AssetID INT,UnderLyingID INT,ExchangeID INT,CurrencyID INT,TickerSymbol VARCHAR(100),UnderLyingSymbol VARCHAR(100),ReutersSymbol VARCHAR(100),                                                 
	ISINSymbol VARCHAR(100),SedolSymbol VARCHAR(100),CusipSymbol VARCHAR(100),BloombergSymbol VARCHAR(100),OSISymbol VARCHAR(100), IDCOSymbol VARCHAR(100), OPRASymbol VARCHAR(100),                                    
	LongName VARCHAR(500),Delta FLOAT, Sector VARCHAR(100),Symbol_PK BIGINT,OPTMultiplier FLOAT,[Type] INT,StrikePrice FLOAT,OptionName VARCHAR(100),                          
	FUTMultiplier FLOAT,CutOffTime VARCHAR(100),FutureName VARCHAR(100),AUECID INT,OPTExpiration DATETIME,FUTExpiration DATETIME,LeadCurrencyID INT,VsCurrencyID INT,    
	FxContractName VARCHAR(100),FXForwardMultiplier FLOAT,IndexLongName VARCHAR(200), Multiplier FLOAT,IssueDate DATETIME,Coupon FLOAT,MaturityDate DATETIME,BondTypeID INT,CollateralTypeID INT,    
	AccrualBasisID INT,FixedIncomeLongName VARCHAR(100),FirstCouponDate DATETIME,IsZero BIT,CouponFrequencyID INT,DaysToSettlement INT,FIMultiplier FLOAT,    
	CreationDate DATETIME,ModifiedDate DATETIME,IsNDF BIT,FixingDate DATETIME, FxMultiplier FLOAT, FxExpirationDate DATETIME, RoundLot DECIMAL(28,10),ProxySymbol VARCHAR(100),
	IsSecApproved BIT,ApprovalDate DATETIME,ApprovedBy VARCHAR(100),Comments VARCHAR(500),UDAAssetClassID INT,UDASecurityTypeID INT,UDASectorID INT,UDASubSectorID INT,UDACountryID INT,
	OPTIsCurrencyFuture BIT, FUTIsCurrencyFuture BIT, DynamicUDA XML, FactSetSymbol VARCHAR(100), ActivSymbol VARCHAR(100)
)


--print @condition

SET @SQL_SCRIPT  = '
INSERT INTO #TempSecMasterDatatable 
  ( AssetID ,UnderLyingID,ExchangeID ,CurrencyID,TickerSymbol,UnderLyingSymbol,ReutersSymbol,                                                 
	ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol,OSISymbol, IDCOSymbol, OPRASymbol,                                    
	LongName,Delta,Sector,Symbol_PK,OPTMultiplier,[Type],StrikePrice,OptionName,                          
	FUTMultiplier,CutOffTime,FutureName,AUECID,OPTExpiration,FUTExpiration,LeadCurrencyID,VsCurrencyID,    
	FxContractName,FXForwardMultiplier,IndexLongName, Multiplier,IssueDate,Coupon,MaturityDate,BondTypeID,CollateralTypeID,    
	AccrualBasisID,FixedIncomeLongName,FirstCouponDate,IsZero,CouponFrequencyID,DaysToSettlement,FIMultiplier,    
	CreationDate,ModifiedDate,IsNDF,FixingDate, FxMultiplier, FxExpirationDate, RoundLot,ProxySymbol,
	IsSecApproved,ApprovalDate,ApprovedBy,Comments,UDAAssetClassID,UDASecurityTypeID,UDASectorID,UDASubSectorID,UDACountryID, 
	OPTIsCurrencyFuture,FUTIsCurrencyFuture,DynamicUDA,FactSetSymbol,ActivSymbol )

select  AssetID,UnderLyingID,ExchangeID ,CurrencyID,TickerSymbol,UnderLyingSymbol,ReutersSymbol,                                                 
  ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol,OSISymbol, IDCOSymbol, OPRASymbol,                                    
  LongName,Delta,Sector,Symbol_PK,OPTMultiplier,[Type],StrikePrice,OptionName,                          
  FUTMultiplier,CutOffTime,FutureName,AUECID,OPTExpiration,FUTExpiration,LeadCurrencyID,VsCurrencyID,    
  FxContractName,FXForwardMultiplier,IndexLongName, Multiplier,IssueDate,Coupon,MaturityDate,BondTypeID,CollateralTypeID,    
  AccrualBasisID,FixedIncomeLongName,FirstCouponDate,IsZero,CouponFrequencyID,DaysToSettlement,FIMultiplier,    
  CreationDate,ModifiedDate,IsNDF,FixingDate, FxMultiplier, FxExpirationDate, RoundLot,ProxySymbol,
  IsSecApproved,ApprovalDate,ApprovedBy,Comments,UDAAssetClassID,UDASecurityTypeID,UDASectorID,UDASubSectorID,UDACountryID,OPTIsCurrencyFuture,
  FUTIsCurrencyFuture,NULL As DynamicUDA,FactSetSymbol,ActivSymbol
from                                                        
       (                                            
     select ROW_NUMBER() OVER (ORDER BY SM.Symbol_PK)AS Row,SM.AssetID,SM.UnderLyingID,T_SMReuters.ExchangeID ,
		SM.CurrencyID,SM.TickerSymbol,SM.UnderLyingSymbol,T_SMReuters.ReutersSymbol,ISINSymbol,SedolSymbol,CusipSymbol,
		BloombergSymbol,OSISymbol, IDCOSymbol, OPRASymbol,ENHD.CompanyName as LongName,Delta,Sector,SM.Symbol_PK,
		OPT.Multiplier as OPTMultiplier,OPT.[Type],OPT.Strike as StrikePrice,OPT.ContractName as OptionName,                             
		FUT.Multiplier as FUTMultiplier,FUT.CutOffTime,FUT.ContractName as FutureName,SM.AUECID,
		OPT.ExpirationDate as OPTExpiration,IsNull(FUT.ExpirationDate,FxForwardData.ExpirationDate) as FUTExpiration,
		IsNull(FxData.LeadCurrencyID,FxForwardData.LeadCurrencyID) as LeadCurrencyID,                                      
		IsNull(FxData.VsCurrencyID,FxForwardData.VsCurrencyID) as VsCurrencyID,                                      
		IsNull(FxData.LongName,FxForwardData.LongName) as FxContractName,FxForwardData.Multiplier as FXForwardMultiplier,
		IndexData.LongName as   IndexLongName, ENHD.Multiplier,FixedIncomeData.IssueDate,FixedIncomeData.Coupon,
		FixedIncomeData.MaturityDate,FixedIncomeData.BondTypeID,FixedIncomeData.CollateralTypeID,FixedIncomeData.AccrualBasisID,
		FixedIncomeData.BondDescription as FixedIncomeLongName,  FixedIncomeData.FirstCouponDate,FixedIncomeData.IsZero,
		FixedIncomeData.CouponFrequencyID,FixedIncomeData.DaysToSettlement,FixedIncomeData.Multiplier as FIMultiplier,
		SM.CreationDate,SM.ModifiedDate, IsNull(FxData.IsNDF,FxForwardData.IsNDF) as IsNDF,
		IsNull(FxData.FixingDate,FxForwardData.FixingDate) as FixingDate,FxData.Multiplier as FxMultiplier, 
		FxData.ExpirationDate as  FxExpirationDate,SM.RoundLot as RoundLot,SM.ProxySymbol as ProxySymbol,
		SM.IsSecApproved,SM.ApprovalDate,SM.ApprovedBy,SM.Comments,SM.UDAAssetClassID,SM.UDASecurityTypeID,SM.UDASectorID,SM.UDASubSectorID,SM.UDACountryID ,OPT.IsCurrencyFuture As OPTIsCurrencyFuture
		,FUT.IsCurrencyFuture As FUTIsCurrencyFuture,SM.FactSetSymbol,SM.ActivSymbol                    

from T_SMSymbolLookUpTable as SM                                                               
		join T_SMReuters  on SM.Symbol_PK=T_SMReuters.Symbol_PK                                                           
		left outer join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK                                                                
		left outer  join T_SMoptionData as OPT on  SM.Symbol_PK=OPT.Symbol_PK                                                            
		left outer  join T_SMFutureData as FUT on  SM.Symbol_PK=FUT.Symbol_PK                                                               
		left outer  join T_SMFxData as FxData on  SM.Symbol_PK=FxData.Symbol_PK                                            
		left outer  join T_SMFxForwardData as FxForwardData on  SM.Symbol_PK=FxForwardData.Symbol_PK                                         
		left outer  join T_SMIndexData as IndexData on  SM.Symbol_PK=IndexData.Symbol_PK                                         
		left outer  join T_SMFixedIncomeData as FixedIncomeData on  SM.Symbol_PK=FixedIncomeData.Symbol_PK    
		left outer  join T_UDA_DynamicUDAData AS DUDA ON DUDA.Symbol_PK = SM.Symbol_PK 
                               
where ISPrimaryExchange =''true'' and '+@condition+'  
)
as TempSM  
   
WHERE Row >= '+cast(@startIndex as varchar)+' AND Row <= '+cast(@endIndex as varchar) 

--print   @SQL_SCRIPT                                


EXEC (@SQL_SCRIPT)

-- Get Dynamic UDA data only for the selected rows in result set, PRANA-11280
UPDATE	TSM
SET		TSM.DynamicUDA = (SELECT * FROM T_UDA_DynamicUDAData WHERE Symbol_PK = TSM.Symbol_PK FOR XML PATH ('DynamicUDAs'))
FROM	#TempSecMasterDatatable TSM

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/IndexID')

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/Symbol_PK')

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/UDAData')

UPDATE	#TempSecMasterDatatable
SET		DynamicUDA.modify('delete /DynamicUDAs/FundID')

SELECT	TSM.Symbol_PK, 
		CASE
			WHEN TSM.AssetID IN (5,11)
			THEN 
				(TSM.UnderLyingSymbol)
			ELSE
				(COALESCE(ENHD.CompanyName, OD.ContractName, FD.ContractName, FXD.LongName, FXFD.LongName, FID.BondDescription, 'Undefined'))
		END AS Issuer
INTO	#IssuerTable
FROM	#TempSecMasterDatatable TSM	
		LEFT JOIN T_SMSymbolLookUpTable SM		ON SM.TickerSymbol = (SELECT UnderLyingSymbol FROM T_SMSymbolLookUpTable WHERE Symbol_PK = TSM.Symbol_PK) 
		LEFT JOIN T_SMEquityNonHistoryData ENHD	ON ENHD.Symbol_PK = SM.Symbol_PK 
		LEFT JOIN T_SMOptionData OD				ON OD.Symbol_PK = SM.Symbol_PK 
		LEFT JOIN T_SMFutureData FD				ON FD.Symbol_PK = SM.Symbol_PK 
		LEFT JOIN T_SMFxData FXD				ON FXD.Symbol_PK = SM.Symbol_PK 
		LEFT JOIN T_SMFxForwardData FXFD		ON FXFD.Symbol_PK = SM.Symbol_PK 
		LEFT JOIN T_SMFixedIncomeData FID		ON FID.Symbol_PK = SM.Symbol_PK 

DECLARE @Symbol_PK BIGINT, @Issuer VARCHAR(100)

WHILE EXISTS(SELECT 1 FROM #IssuerTable)
BEGIN
	SET	@Symbol_PK	= (SELECT TOP 1 Symbol_PK FROM #IssuerTable)
	SELECT @Issuer	= Issuer FROM #IssuerTable WHERE Symbol_PK = @Symbol_PK

	IF((SELECT DynamicUDA.exist('/DynamicUDAs/Issuer') FROM #TempSecMasterDatatable WHERE Symbol_PK = @Symbol_PK) = 1)
	BEGIN
		UPDATE	#TempSecMasterDatatable
		SET		DynamicUDA.modify('replace value of (/DynamicUDAs/Issuer/text())[1] with sql:variable("@Issuer")')
		WHERE	DynamicUDA.value('(/DynamicUDAs/Issuer/text())[1]','VARCHAR(100)') IS NULL
				AND	Symbol_PK = @Symbol_PK
				AND DynamicUDA IS NOT NULL
	END
	ELSE
	BEGIN		
		UPDATE	#TempSecMasterDatatable
		SET		DynamicUDA = '<DynamicUDAs />'
		WHERE	Symbol_PK = @Symbol_PK
				AND DynamicUDA IS NULL

		UPDATE	#TempSecMasterDatatable
		SET		DynamicUDA.modify('insert <Issuer>{sql:variable("@Issuer")}</Issuer> into (/DynamicUDAs)[1]')
		WHERE	DynamicUDA.value('(/DynamicUDAs/Issuer/text())[1]','VARCHAR(100)') IS NULL
				AND	Symbol_PK = @Symbol_PK
				AND DynamicUDA IS NOT NULL
	END

	DELETE FROM #IssuerTable WHERE Symbol_PK = @Symbol_PK
END

SELECT * FROM #TempSecMasterDatatable

DROP TABLE #TempSecMasterDatatable, #IssuerTable

--P_AdvancedSearchSecMaster 'tickersymbol=''MSFT'' and AssetID=1',1,10 
