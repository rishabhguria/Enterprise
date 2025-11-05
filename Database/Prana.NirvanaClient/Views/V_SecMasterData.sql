/*=============================================    
-- Modified By: om shiv    
-- Modification Date: June, 2014    
-- Description: Added columns SM.ProxySymbol,SM.IsSecApproved, SM.ApprovalDate,    
SM.ApprovedBy,SM.CreatedBy, SM.ModifiedBy,SM.BBGID, SM.CreationDate,SM.ModifiedDate,    
SM.DataSource    
-- =============================================*/  
-- =============================================    
-- Modified By: om shiv    
-- Modification Date: NOV, 2013    
-- Description: Get UDA details from T_SMSymbolLookUpTable    
-- =============================================    
CREATE VIEW [dbo].[V_SecMasterData]  
AS  
SELECT SM.AUECID  
 ,SM.TickerSymbol  
 ,COALESCE(NonHistoryData.CompanyName, OptionData.ContractName, FutureData.ContractName, FxData.LongName, FxForwardData.LongName, FixedIncomeData.BondDescription) AS CompanyName  
 ,CASE   
  -- AssetID : 2 - EquityOption; 4 - FutureOption; 10 - FXOption    
  WHEN SM.AssetID = 2  
   OR SM.AssetID = 4  
   OR SM.AssetID = 10  
   THEN 0  
  ELSE 1  
  END AS Delta  
 ,ISNULL(T_UDAAssetClass.AssetName, 'Undefined') AS AssetName  
 ,ISNULL(T_UDASecurityType.SecurityTypeName, 'Undefined') AS SecurityTypeName  
 ,ISNULL(T_UDASector.SectorName, 'Undefined') AS SectorName  
 ,ISNULL(T_UDASubSector.SubSectorName, 'Undefined') AS SubSectorName  
 ,ISNULL(T_UDACountry.CountryName, 'Undefined') AS CountryName  
 ,SM.UnderLyingSymbol  
 ,SM.BloombergSymbol  
 ,SM.ISINSymbol  
 ,SM.SEDOLSymbol  
 ,SM.CUSIPSymbol  
 ,CASE   
  -- AssetID : 2 - EquityOption; 4 - FutureOption; 10 - FXOption    
  WHEN SM.AssetID = 2  
   OR SM.AssetID = 4  
   OR SM.AssetID = 10  
   THEN CASE OptionData.Type  
     WHEN 1  
      THEN 'CALL'  
     WHEN 0  
      THEN 'PUT'  
     ELSE ''  
     END  
  ELSE ''  
  END AS PutOrCall  
 ,COALESCE(OptionData.Strike, 0) AS StrikePrice  
 ,COALESCE(NonHistoryData.Multiplier, OptionData.Multiplier, FutureData.Multiplier, FxData.Multiplier, FxForwardData.Multiplier, FixedIncomeData.Multiplier) AS Multiplier  
 ,COALESCE(Reuters.ReutersSymbol, '') AS ReutersSymbol  
 ,COALESCE(OptionData.ExpirationDate, FutureData.ExpirationDate, FxData.ExpirationDate, FxForwardData.ExpirationDate, FixedIncomeData.MaturityDate, '1/1/1800') AS ExpirationDate  
 ,COALESCE(FxData.LeadCurrencyID, FxForwardData.LeadCurrencyID, 0) AS LeadCurrencyID  
 ,COALESCE(FxData.VsCurrencyID, FxForwardData.VsCurrencyID, 0) AS VsCurrencyID  
 ,COALESCE(CurrencyFxData1.CurrencySymbol, CurrencyFxForwardData1.CurrencySymbol, '') AS LeadCurrency  
 ,COALESCE(CurrencyFxData2.CurrencySymbol, CurrencyFxForwardData2.CurrencySymbol, '') AS VsCurrency  
 ,SM.CurrencyID  
 ,SM.OSISymbol  
 ,SM.IDCOSymbol  
 ,SM.OpraSymbol  
 ,SM.AssetId  
 ,COALESCE(FixedIncomeData.Coupon, 0) AS Coupon  
 ,COALESCE(FixedIncomeData.IssueDate, '1/1/1800') AS IssueDate  
 ,COALESCE(FixedIncomeData.MaturityDate, '1/1/1800') AS MaturityDate  
 ,COALESCE(FixedIncomeData.FirstCouponDate, '1/1/1800') AS FirstCouponDate  
 ,COALESCE(FixedIncomeData.CouponFrequencyID, 0) AS CouponFrequencyID  
 ,COALESCE(FixedIncomeData.AccrualBasisID, 0) AS AccrualBasisID  
 ,COALESCE(FixedIncomeData.BondTypeID, 0) AS BondTypeID  
 ,COALESCE(FixedIncomeData.IsZero, 0) AS IsZero  
 ,COALESCE(FxData.IsNDF, FxForwardData.IsNDF, 0) AS IsNDF  
 ,COALESCE(FxData.FixingDate, FxForwardData.FixingDate, '1/1/1800') AS FixingDate  
 ,CASE   
  --Kuldeep A.: In case of Index options, the asset id is also 2(which is for Equity Option) as in our application     
  --we treat them as Equity Option but in this case Leveraged Factor is to be picked from their underlying which is Index    
  --so changed the following condition.    
  WHEN SM.AssetID = 2  
   THEN COALESCE(UnderlyingSMData.Delta, UnderlyingIndexData.LeveragedFactor, 1)  
  WHEN SM.AssetID = 4  
   THEN COALESCE(UnderlyingFutureData.LeveragedFactor, 1)  
  WHEN SM.AssetID = 10  
   THEN COALESCE(UnderlyingFXData.LeveragedFactor, 1)  
  WHEN SM.AssetID = 3  
   THEN COALESCE(FutureData.leveragedfactor, 1)  
  WHEN SM.AssetID = 5  
			THEN COALESCE(FxData.leveragedfactor, 1)
		WHEN SM.AssetID = 8
   THEN COALESCE(FixedIncomeData.leveragedfactor, 1)  
  WHEN SM.AssetID = 11  
   THEN COALESCE(FxForwardData.leveragedfactor, 1)  
  ELSE COALESCE(UnderlyingSMData.Delta, 1)  
  END AS UnderlyingDelta  
 ,SM.ProxySymbol  
 ,SM.IsSecApproved  
 ,SM.ApprovalDate  
 ,SM.ApprovedBy  
 ,SM.CreatedBy  
 ,SM.ModifiedBy  
 ,SM.BBGID  
 ,SM.CreationDate  
 ,SM.ModifiedDate  
 ,SM.DataSource  
 ,SM.PrimarySymbology  
 ,SM.Symbol_PK  
 ,SM.SharesOutstanding  
 ,COALESCE(OptionData.IsCurrencyFuture, FutureData.IsCurrencyFuture, 0) As IsCurrencyFuture  
 ,FixedIncomeData.CollateralTypeID AS CollateralTypeID  
 ,SM.ActivSymbol  
 ,SM.FactSetSymbol  
 ,SM.BloombergSymbolWithExchangeCode  
 ,SM.RoundLot
FROM V_UniqueSymbol_Prana  
INNER JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable AS SM ON V_UniqueSymbol_Prana.Symbol = SM.TickerSymbol
 AND SM.AssetID != 7 --exclude index data    
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDAAssetClass AS T_UDAAssetClass ON T_UDAAssetClass.AssetID = SM.UDAAssetClassID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASecurityType AS T_UDASecurityType ON T_UDASecurityType.SecurityTypeID = SM.UDASecurityTypeID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASector AS T_UDASector ON T_UDASector.SectorID = SM.UDASectorID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASubSector AS T_UDASubSector ON T_UDASubSector.SubSectorID = SM.UDASubSectorID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDACountry AS T_UDACountry ON SM.UDACountryID = T_UDACountry.CountryID -- left outer join  
	--Modified by - omshiv, get UDA from T_SMSymbolLookUpTable as merged UDA to secMaster  
	--LEFT OUTER JOIN V_GetSymbolUDAData AS UDA  
	-- ON UDA.TickerSymbol = SM.TickerSymbol  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMReuters AS Reuters ON Reuters.Symbol_PK = SM.Symbol_PK
	AND Reuters.ISPrimaryExchange = 'true'
-----------------------------------------------------------------------------------------------------------------------  
-- For Equity  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMEquityNonHistoryData AS NonHistoryData ON NonHistoryData.Symbol_PK = SM.Symbol_PK
-----------------------------------------------------------------------------------------------------------------------  
-- For Option  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMOptionData AS OptionData ON OptionData.Symbol_PK = SM.Symbol_PK
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable UnderlyingSM ON SM.UnderlyingSymbol = UnderlyingSM.TickerSymbol
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMEquityNonHistoryData AS UnderlyingSMData ON UnderlyingSMData.Symbol_PK = UnderlyingSM.Symbol_PK
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFutureData AS UnderlyingFutureData ON UnderlyingFutureData.Symbol_PK = UnderlyingSM.Symbol_PK
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFxData AS UnderlyingFXData ON UnderlyingFXData.Symbol_PK = UnderlyingSM.Symbol_PK
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMIndexData AS UnderlyingIndexData ON UnderlyingIndexData.Symbol_PK = UnderlyingSM.Symbol_PK
-----------------------------------------------------------------------------------------------------------------------  
-- For Future  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFutureData AS FutureData ON FutureData.Symbol_PK = SM.Symbol_PK
-----------------------------------------------------------------------------------------------------------------------  
-- For Fx  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFxData AS FxData ON FxData.Symbol_PK = SM.Symbol_PK
LEFT OUTER JOIN T_Currency AS CurrencyFxData1 ON CurrencyFxData1.CurrencyID = FxData.LeadCurrencyID
LEFT OUTER JOIN T_Currency AS CurrencyFxData2 ON CurrencyFxData2.CurrencyID = FxData.VsCurrencyID
-----------------------------------------------------------------------------------------------------------------------  
-- For FxForward  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFxForwardData AS FxForwardData ON FxForwarddata.Symbol_PK = SM.Symbol_PK
LEFT OUTER JOIN T_Currency AS CurrencyFxForwardData1 ON CurrencyFxForwardData1.CurrencyID = FxForwardData.LeadCurrencyID
LEFT OUTER JOIN T_Currency AS CurrencyFxForwardData2 ON CurrencyFxForwardData2.CurrencyID = FxForwardData.VsCurrencyID
-----------------------------------------------------------------------------------------------------------------------  
-- For FixedIncome  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFixedIncomeData AS FixedIncomeData ON FixedIncomeData.Symbol_PK = SM.Symbol_PK
-----------------------------------------------------------------------------------------------------------------------  

UNION

-- For Indices (Its not traded but it has to be used)  
SELECT SM.AUECID
	,SM.TickerSymbol
	,IndexData.LongName AS CompanyName
	,1 AS Delta
	,ISNULL(T_UDAAssetClass.AssetName, 'Undefined') AS AssetName
	,ISNULL(T_UDASecurityType.SecurityTypeName, 'Undefined') AS SecurityTypeName
	,ISNULL(T_UDASector.SectorName, 'Undefined') AS SectorName
	,ISNULL(T_UDASubSector.SubSectorName, 'Undefined') AS SubSectorName
	,ISNULL(T_UDACountry.CountryName, 'Undefined') AS CountryName
	,
	-- UDA.AssetName,  
	-- UDA.SecurityTypeName,  
	-- UDA.SectorName,  
	-- UDA.SubSectorName,  
	-- UDA.CountryName,  
	SM.UnderLyingSymbol
	,SM.BloombergSymbol
	,SM.ISINSymbol
	,SM.SEDOLSymbol
	,SM.CUSIPSymbol
	,'' AS PutOrCall
	,0 AS StrikePrice
	,1 AS Multiplier
	,Reuters.ReutersSymbol AS ReutersSymbol
	,'1/1/1800' AS ExpirationDate
	,0 AS LeadCurrencyID
	,0 AS VsCurrencyID
	,'' AS LeadCurrency
	,'' AS VsCurrency
	,SM.CurrencyID
	,SM.OSISymbol
	,SM.IDCOSymbol
	,SM.OpraSymbol
	,SM.AssetId
	,0 AS Coupon
	,'1/1/1800' AS IssueDate
	,'1/1/1800' AS MaturityDate
	,'1/1/1800' AS FirstCouponDate
	,0 AS CouponFrequencyID
	,0 AS AccrualBasisID
	,0 AS BondTypeID
	,0 AS IsZero
	,0 AS IsNDF
	,'1/1/1800' AS FixingDate
	,isnull(IndexData.Leveragedfactor, 1) AS UnderlyingDelta
	,SM.ProxySymbol
	,SM.IsSecApproved
	,SM.ApprovalDate
	,SM.ApprovedBy
	,SM.CreatedBy
	,SM.ModifiedBy
	,SM.BBGID
	,SM.CreationDate
	,SM.ModifiedDate
	,SM.DataSource
	,SM.PrimarySymbology
	,SM.Symbol_PK
	,SM.SharesOutstanding
	,0 AS IsCurrencyFuture
	,FixedIncomeData.CollateralTypeID AS CollateralTypeID
	,SM.ActivSymbol
	,SM.FactSetSymbol
	,SM.BloombergSymbolWithExchangeCode
    ,SM.RoundLot
	FROM [$(SecurityMaster)].dbo.T_SMIndexData AS IndexData
INNER JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable AS SM ON IndexData.Symbol_PK = SM.Symbol_PK
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDAAssetClass AS T_UDAAssetClass ON T_UDAAssetClass.AssetID = SM.UDAAssetClassID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASecurityType AS T_UDASecurityType ON T_UDASecurityType.SecurityTypeID = SM.UDASecurityTypeID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASector AS T_UDASector ON T_UDASector.SectorID = SM.UDASectorID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDASubSector AS T_UDASubSector ON T_UDASubSector.SubSectorID = SM.UDASubSectorID
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_UDACountry AS T_UDACountry ON SM.UDACountryID = T_UDACountry.CountryID -- left outer join  
	--Modified by - omshiv, get UDA from T_SMSymbolLookUpTable as merged UDA to secMaster  
	--LEFT OUTER JOIN V_GetSymbolUDAData AS UDA  
	-- ON UDA.TickerSymbol = SM.TickerSymbol  
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMReuters AS Reuters ON Reuters.Symbol_PK = SM.Symbol_PK
	AND Reuters.ISPrimaryExchange = 'true'
LEFT OUTER JOIN [$(SecurityMaster)].dbo.T_SMFixedIncomeData AS FixedIncomeData ON FixedIncomeData.Symbol_PK = SM.Symbol_PK

