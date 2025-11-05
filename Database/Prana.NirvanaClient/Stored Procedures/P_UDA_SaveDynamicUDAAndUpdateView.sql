
CREATE PROCEDURE [dbo].[P_UDA_SaveDynamicUDAAndUpdateView] 
	@Tag			VARCHAR(100),		
	@HeaderCaption	VARCHAR(100),
	@DefaultValue	VARCHAR(100),
	@MasterValues	XML,
	@RenamedKeys	VARCHAR(100)

AS

BEGIN TRY

DECLARE @intErrorCode INT
BEGIN TRAN

	EXEC [$(SecurityMaster)].dbo.P_UDA_SaveDynamicUDA
	@Tag = @Tag,
	@HeaderCaption = @HeaderCaption,
	@DefaultValue = @DefaultValue,
	@MasterValues = @MasterValues,
	@RenamedKeys = @RenamedKeys


	DECLARE	@UDAString		VARCHAR(MAX)
	DECLARE	@TempTableStr	NVARCHAR(MAX)
	DECLARE	@BaseCurrencyID	INT
	SET		@BaseCurrencyID	= (SELECT TOP 1 BaseCurrencyID FROM T_Company WHERE CompanyID > 0)
	SET		@UDAString		= ''

	SELECT	@UDAString = @UDAString + ',' + ' COALESCE(DUDA.[' + Tag  + '], ''' + ISNULL(REPLACE(DefaultValue,'''',''''''),'Undefined') + ''') As [' + Tag +']'
	FROM	[$(SecurityMaster)].dbo.T_UDA_DynamicUDA
	WHERE	Tag NOT IN ('Issuer','RiskCurrency')

	SELECT	@UDAString = @UDAString + ',' + ' 
			CASE
				WHEN SM.AssetID IN (5,11)
				THEN 
					(SM.UnderLyingSymbol)
				ELSE
					(COALESCE(DUDA.[' + Tag  + '], NULLIF(ENHD.CompanyName,''''), NULLIF(OD.ContractName,''''), NULLIF(FD.ContractName,''''), NULLIF(FXD.LongName,''''), NULLIF(FXFD.LongName,''''), NULLIF(FID.BondDescription,''''), ''' + ISNULL(REPLACE(DefaultValue,'''',''''''),'Undefined') + '''))
			END As [' + Tag +']'
	FROM	[$(SecurityMaster)].dbo.T_UDA_DynamicUDA
	WHERE	Tag = 'Issuer'

	SELECT	@UDAString = @UDAString + ',' + '  
			CASE 
				WHEN DUDA.[' + Tag  + '] IS NOT NULL
				THEN DUDA.[' + Tag  + ']
				WHEN SM.AssetID = 5 
				THEN CASE 
					WHEN FXDRC.VsCurrencyID = ' + CONVERT(VARCHAR(10), @BaseCurrencyID)  + ' 
					THEN (SELECT CurrencySymbol FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXDRC.LeadCurrencyID) 
					ELSE (SELECT CurrencySymbol FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXDRC.VsCurrencyID) 
					END
				WHEN SM.AssetID = 11 
				THEN CASE 
					WHEN FXFDRC.VsCurrencyID = ' + CONVERT(VARCHAR(10), @BaseCurrencyID) + ' 
					THEN (SELECT CurrencySymbol FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXFDRC.LeadCurrencyID) 
					ELSE (SELECT CurrencySymbol FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = FXFDRC.VsCurrencyID) 
					END
				WHEN SM.AssetID NOT IN (5,11) AND SM.CurrencyID IS NOT NULL 
				THEN (SELECT CurrencySymbol FROM [$(SecurityMaster)].dbo.T_Currency WHERE CurrencyID = SM.CurrencyID)
				ELSE ''' + ISNULL(REPLACE(DefaultValue,'''',''''''),'Undefined') + '''  
			END As [' + Tag +']'
	FROM	[$(SecurityMaster)].dbo.T_UDA_DynamicUDA
	WHERE	Tag = 'RiskCurrency'

	IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[V_UDA_DynamicUDA]') AND OBJECTPROPERTY(id, N'IsView') = 1)
		SET	@TempTableStr = 'ALTER '
	ELSE
		SET	@TempTableStr = 'CREATE '

	SET	@TempTableStr = @TempTableStr + ' VIEW [dbo].[V_UDA_DynamicUDA] 
						AS
						SELECT DISTINCT DUDA.Symbol_PK, DUDA.FundID '+ @UDAString + 
						' FROM [$(SecurityMaster)].dbo.T_UDA_DynamicUDAData DUDA 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SM		ON SM.Symbol_PK = DUDA.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxData FXDRC				ON FXDRC.Symbol_PK = SM.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxForwardData FXFDRC		ON FXFDRC.Symbol_PK = SM.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMSymbolLookUpTable SMU		ON SMU.TickerSymbol = SM.UnderLyingSymbol 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMEquityNonHistoryData ENHD	ON ENHD.Symbol_PK = SMU.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMOptionData OD				ON OD.Symbol_PK = SMU.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMFutureData FD				ON FD.Symbol_PK = SMU.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxData FXD					ON FXD.Symbol_PK = SMU.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMFxForwardData FXFD			ON FXFD.Symbol_PK = SMU.Symbol_PK 
						LEFT JOIN [$(SecurityMaster)].dbo.T_SMFixedIncomeData FID		ON FID.Symbol_PK = SMU.Symbol_PK '
	EXEC(@TempTableStr)


	

COMMIT TRAN

END TRY


BEGIN CATCH
	--print('Error occured rolling back transaction')
	ROLLBACK TRAN
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
           @ErrorSeverity = ERROR_SEVERITY(),
           @ErrorState = ERROR_STATE();
	

    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH

