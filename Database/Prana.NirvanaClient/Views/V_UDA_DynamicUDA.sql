CREATE VIEW [dbo].[V_UDA_DynamicUDA] 
							AS
SELECT	DISTINCT	DUDA.Symbol_PK, DUDA.FundID , 
		COALESCE(DUDA.[Analyst], 'Undefined') As [Analyst], 
		COALESCE(DUDA.[CountryOfRisk], 'Undefined') As [CountryOfRisk], 
		COALESCE(DUDA.[CustomUDA1], 'Undefined') As [CustomUDA1], 
		COALESCE(DUDA.[CustomUDA2], 'Undefined') As [CustomUDA2], 
		COALESCE(DUDA.[CustomUDA3], 'Undefined') As [CustomUDA3], 
		COALESCE(DUDA.[CustomUDA4], 'Undefined') As [CustomUDA4], 
		COALESCE(DUDA.[CustomUDA5], 'Undefined') As [CustomUDA5], 
		COALESCE(DUDA.[CustomUDA6], 'Undefined') As [CustomUDA6], 
		COALESCE(DUDA.[CustomUDA7], 'Undefined') As [CustomUDA7], 
		COALESCE(DUDA.[LiquidTag], 'Yes') As [LiquidTag], 
		COALESCE(DUDA.[MarketCap], 'Undefined') As [MarketCap], 
		COALESCE(DUDA.[Region], 'Undefined') As [Region], 
		COALESCE(DUDA.[UCITSEligibleTag], 'Yes') As [UCITSEligibleTag], 
		COALESCE(DUDA.[CustomUDA8], 'Undefined') As [CustomUDA8],
		COALESCE(DUDA.[CustomUDA9], 'Undefined') As [CustomUDA9],
		COALESCE(DUDA.[CustomUDA10], 'Undefined') As [CustomUDA10],
		COALESCE(DUDA.[CustomUDA11], 'Undefined') As [CustomUDA11],
		COALESCE(DUDA.[CustomUDA12], 'Undefined') As [CustomUDA12],
		(COALESCE(DUDA.[Issuer], NULLIF(ENHD.CompanyName,''), NULLIF(OD.ContractName,''), NULLIF(FD.ContractName,''), NULLIF(FXD.LongName,''), NULLIF(FXFD.LongName,''), NULLIF(FID.BondDescription,''), 'Undefined') )
			As [Issuer],  
			CASE 
				WHEN DUDA.[RiskCurrency] IS NOT NULL
				THEN DUDA.[RiskCurrency]
				WHEN SM.AssetID = 5 
				THEN CASE 
					WHEN FXDRC.VsCurrencyID = 1 
				THEN (SELECT [CurrencySymbol] FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXDRC.LeadCurrencyID) 
				ELSE (SELECT [CurrencySymbol] FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXDRC.VsCurrencyID) 
					END
				WHEN SM.AssetID = 11 
				THEN CASE 
					WHEN FXFDRC.VsCurrencyID = 1 
				THEN (SELECT [CurrencySymbol] FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXFDRC.LeadCurrencyID) 
				ELSE (SELECT [CurrencySymbol] FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXFDRC.VsCurrencyID) 
					END
				WHEN SM.AssetID NOT IN (5,11) AND SM.CurrencyID IS NOT NULL 
			THEN (SELECT [CurrencySymbol] FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = SM.CurrencyID)
				ELSE 'Undefined'  
		END As [RiskCurrency] 
FROM		[$(SecurityMaster)].dbo.T_UDA_DynamicUDAData DUDA 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM		ON SM.Symbol_PK = DUDA.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxData FXDRC				ON FXDRC.Symbol_PK = SM.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxForwardData FXFDRC		ON FXFDRC.Symbol_PK = SM.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SMU		ON SMU.TickerSymbol = SM.UnderLyingSymbol 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMEquityNonHistoryData ENHD	ON ENHD.Symbol_PK = SMU.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMOptionData OD				ON OD.Symbol_PK = SMU.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMFutureData FD				ON FD.Symbol_PK = SMU.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxData FXD					ON FXD.Symbol_PK = SMU.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxForwardData FXFD			ON FXFD.Symbol_PK = SMU.Symbol_PK 
							LEFT JOIN [$(SecurityMaster)].dbo.T_SMFixedIncomeData FID		ON FID.Symbol_PK = SMU.Symbol_PK 