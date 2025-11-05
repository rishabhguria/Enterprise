CREATE PROCEDURE [dbo].[P_UpdatePBWiseTaxlotState] (
	@xml NTEXT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT
	)
AS
--declare @xml varchar(500)
--declare @ErrorMessage VARCHAR(500)
--declare @ErrorNumber INT
--declare @p2 nvarchar(500)
--set @Xml=N'<TaxLots><TaxLot GroupID ="1906261236460" TaxLotID ="19062612364601184" Level1AllocationID ="1184" Action="UNALLOCATE"/></TaxLots>'
SET @ErrorNumber = 0
SET @ErrorMessage = 'Success'

BEGIN TRY
	DECLARE @handle INT

	EXEC sp_xml_preparedocument @handle OUTPUT
		,@xml

	CREATE TABLE #Temp_Group (
		GroupID VARCHAR(50)
		,TaxlotID VARCHAR(50)
		,Level1AllocationID VARCHAR(50)
		,[Action] VARCHAR(50)
		)

	INSERT INTO #Temp_Group
	SELECT GroupID
		,TaxlotID
		,Level1AllocationID
		,[Action]
	FROM OPENXML(@handle, '/TaxLots/TaxLot', 1) WITH (
			GroupID VARCHAR(50) '@GroupID'
			,TaxLotID VARCHAR(50) '@TaxLotID'
			,Level1AllocationID VARCHAR(50) '@Level1AllocationID'
			,[Action] VARCHAR(50) '@Action'
			)

	--INSERT INTO T_DeletedTaxLots
	SELECT V_TaxLots.TaxLotID
		,V_TaxLots.Symbol
		,V_TaxLots.OrderSideTagValue
		,V_TaxLots.CounterPartyID
		,V_TaxLots.VenueID
		,V_TaxLots.AUECID
		,TaxlotQty
		,V_TaxLots.AvgPrice
		,Level2Percentage
		,V_TaxLots.AUECLocalDate
		,V_TaxLots.AssetID
		,V_TaxLots.UnderlyingID
		,V_TaxLots.ExchangeID
		,V_TaxLots.CurrencyID
		,FUNDID
		,V_TaxLots.SettlementDate
		,V_TaxLots.Commission
		,V_TaxLots.GroupID
		,V_TaxLots.CumQty
		,V_TaxLots.Quantity
		,V_TaxLots.OtherBrokerFees
		,3 as TaxlotState                                                                                                                                                                      
		,V_TaxLots.GroupRefID
		,V_TaxLots.OrderTypeTagValue
		,V_TaxLots.Level1AllocationID
		,V_TaxLots.StampDuty
		,V_TaxLots.TransactionLevy
		,V_TaxLots.ClearingFee
		,V_TaxLots.TaxOnCommissions
		,V_TaxLots.MiscFees
		,T_AUEC.Multiplier
		,T_PBwiseTaxlotState.PBID
		,V_TaxLots.FXRate
		,V_TaxLots.FXConversionMethodOperator
		,V_TaxLots.ProcessDate
		,V_TaxLots.OriginalPurchaseDate
		,V_TaxLots.AccruedInterest
		,V_TaxLots.BenchMarkRate
		,V_TaxLots.Differential
		,V_TaxLots.SwapDescription
		,V_TaxLots.DayCount
		,V_TaxLots.FirstResetDate
		,V_TaxLots.IsSwapped
		,V_TaxLots.FXRate_Taxlot
		,V_TaxLots.FXConversionMethodOperator_Taxlot
		,V_Taxlots.Level2ID
		,V_Taxlots.Description
		,V_Taxlots.LotId
		,V_Taxlots.ExternalTransId
		,V_Taxlots.TradeAttribute1
		,V_Taxlots.TradeAttribute2
		,V_Taxlots.TradeAttribute3
		,V_Taxlots.TradeAttribute4
		,V_Taxlots.TradeAttribute5
		,V_Taxlots.TradeAttribute6
		,T_PBwiseTaxlotState.FileFormatID
		,V_TaxLots.SecFee
		,V_TaxLots.OccFee
		,V_TaxLots.OrfFee
		,V_TaxLots.ClearingBrokerFee
		,V_TaxLots.SoftCommission
		,V_Taxlots.TransactionType
		,V_Taxlots.InternalComments
		,V_Taxlots.SettlCurrency_Taxlot AS SettlCurrency
		,V_Taxlots.OptionPremiumAdjustment AS OptionPremiumAdjustment
		,V_Taxlots.AdditionalTradeAttributes
		into  #T_DeletedTaxLotsFinal
	FROM V_TaxLots
	INNER JOIN #Temp_Group TEMP ON V_TaxLots.GroupID = TEMP.GroupID
		AND V_TaxLots.TaxLotID = TEMP.TaxLotID
		AND V_TaxLots.Level1AllocationID = TEMP.Level1AllocationID
		AND TEMP.[Action] = 'UNALLOCATE'
	INNER JOIN T_AUEC ON T_AUEC.AUECID = V_TaxLots.AUECID
	INNER JOIN T_PBwiseTaxlotState ON T_PBwiseTaxlotState.TaxLotID = TEMP.TaxLotID

	   INSERT INTO T_DeletedTaxLots
	    select TaxLotID
		,Symbol
		,OrderSideTagValue
		,CounterPartyID
		,VenueID
		,AUECID
		,TaxlotQty
		,AvgPrice
		,Level2Percentage
		,AUECLocalDate
		,AssetID
		,UnderlyingID
		,ExchangeID
		,CurrencyID
		,FUNDID
		,SettlementDate
		,Commission
		,GroupID
		,CumQty
		,Quantity
		,OtherBrokerFees
		,3 as TaxlotState                                                                                                                                                                      
		,GroupRefID
		,OrderTypeTagValue
		,Level1AllocationID
		,StampDuty
		,TransactionLevy
		,ClearingFee
		,TaxOnCommissions
		,MiscFees
		,Multiplier
		,PBID
		,FXRate
		,FXConversionMethodOperator
		,ProcessDate
		,OriginalPurchaseDate
		,AccruedInterest
		,BenchMarkRate
		,[Differential]
		,SwapDescription
		,DayCount
		,FirstResetDate
		,IsSwapped
		,FXRate_Taxlot
		,FXConversionMethodOperator_Taxlot
		,Level2ID
		,[Description]
		,LotId
		,ExternalTransId
		,TradeAttribute1
		,TradeAttribute2
		,TradeAttribute3
		,TradeAttribute4
		,TradeAttribute5
		,TradeAttribute6
		,FileFormatID
		,SecFee
		,OccFee
		,OrfFee
		,ClearingBrokerFee
		,SoftCommission
		,TransactionType
		,InternalComments
		,SettlCurrency
		,OptionPremiumAdjustment
		,AdditionalTradeAttributes
		 from #T_DeletedTaxLotsFinal
         where TaxLotID not in (select TaxlotID from T_DeletedTaxLots)

  --- Update TaxLotState and ExternalTransId when record already existes in T_DeletedTaxLots

	Update T_DeletedTaxLots
	set TaxLotState =3,
	ExternalTransId = 
	Case	
		When #T_DeletedTaxLotsFinal.ExternalTransId <>'' 
		Then #T_DeletedTaxLotsFinal.ExternalTransId
		Else T_DeletedTaxLots.ExternalTransId
	END
	From T_DeletedTaxLots
	Inner join #T_DeletedTaxLotsFinal on T_DeletedTaxLots.TaxLotID= #T_DeletedTaxLotsFinal.TaxLotID
    
	DELETE PBT
	FROM T_PBwiseTaxlotState PBT
	INNER JOIN #Temp_Group TEMP ON PBT.TaxLotID = TEMP.TaxLotID AND TEMP.[Action] = 'UNALLOCATE'

	UPDATE PBT
	SET PBT.TaxLotState = 2
	FROM T_PBwiseTaxlotState PBT
	INNER JOIN #Temp_Group TEMP ON PBT.TaxLotID = TEMP.TaxLotID
			AND TEMP.[Action] <> 'UNALLOCATE' 
			AND TEMP.[Action] <> 'CLOSING' 
			AND TEMP.[Action] <> 'Exercise_Assignment' 
			AND TEMP.[Action] <> 'Unwinding_for_Exercised_Option'
			AND TEMP.[Action] <> 'LotId_Changed' 
			AND TEMP.[Action] <> 'ExternalTransId_Changed'

---- While re-allocation a trade, if any taxlot exists in the T_Deletedtaxlots table and it is not yet sent from Third Party, 
---- it should be moved back to T_PBwiseTaxlotState with Amended State
	Insert Into T_PBwiseTaxlotState
	Select 
	TDT.PBID,
	TDT.TaxLotID,
	2 As TaxlotState,
	(Cast(TDT.PBID As Varchar(50)) + TDT.TaxLotID + Cast(TDT.FileFormatID As VARCHAR(50))) As PBTaxlotID,
	TDT.FileFormatID 
	From T_DeletedTaxLots TDT
	Inner Join #Temp_Group Temp On Temp.TaxlotID = TDT.TaxLotID And Temp.Action = 'REALLOCATE'
	Where TDT.TaxLotState = 3
	And TDT.TaxLotID Not In
		(
			Select TaxlotID From T_PBwiseTaxlotState
		)


	DROP TABLE #Temp_Group
END TRY

BEGIN CATCH
	SET @ErrorMessage = ERROR_MESSAGE();
	SET @ErrorNumber = Error_number();

	SELECT @ErrorMessage
END CATCH;
