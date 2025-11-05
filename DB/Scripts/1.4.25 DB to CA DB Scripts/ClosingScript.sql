	Declare @TaxlotClosingID uniqueidentifier
	Declare @PositionalTaxlotID varchar(max) 
	Declare @ClosingTaxlotID varchar(max)
	Declare @ClosedQty float
		--TaxLotOpenQty, 
	Declare @ClosingMode int
	Declare @SettlementPrice float
	Declare @TimeOfSaveUTC datetime 
		--Taxlots.AUECModifiedDate TradeDate, 
	Declare @AUECLocalDate datetime
	Declare @CorpActionID uniqueidentifier  

DECLARE tmp_ClosingData CURSOR FAST_FORWARD FOR                                                              
	Select NewID() TaxlotClosingID, 
	PositionalTaxlotID, 
	ClosingTaxlotID, 
	ClosingQty ClosedQty, 
	--TaxLotOpenQty, 
	ClosingMode, 
	0 SettlementPrice,
	ClosingDatetime TimeOfSaveUTC, 
	--Taxlots.AUECModifiedDate TradeDate, 
	tmp_PM_TaxlotClosing.AUECLocalDate AUECLocalDate,
	NULL CorpActionID
	from 
	tmp_PM_TaxlotClosing 
	INNER JOIN PM_Taxlots AS Taxlots ON Taxlots.TaxlotID = tmp_PM_TaxlotClosing.PositionalTaxlotID
	--Where closingmode <>0
	Order By taxlotclosingid, PositionalTaxlotID;
          
Open tmp_ClosingData;  
FETCH NEXT FROM tmp_ClosingData INTO          
	@TaxlotClosingID, 
	@PositionalTaxlotID, 
	@ClosingTaxlotID, 
	@ClosedQty, 
		--TaxLotOpenQty, 
	@ClosingMode, 
	@SettlementPrice,
	@TimeOfSaveUTC, 
		--Taxlots.AUECModifiedDate TradeDate, 
	@AUECLocalDate,
	@CorpActionID ;          
WHILE @@fetch_status = 0                                                              
 BEGIN           
     Insert Into PM_TaxlotClosing Values
	  (  
		@TaxlotClosingID, 
		@PositionalTaxlotID, 
		@ClosingTaxlotID, 
		@ClosedQty, 
			--TaxLotOpenQty, 
		@ClosingMode, 
		@SettlementPrice,
		@TimeOfSaveUTC, 
			--Taxlots.AUECModifiedDate TradeDate, 
		@AUECLocalDate,
		@CorpActionID        
	  )

		 IF (@ClosingMode = 0)
		 BEGIN
			-- Need to insert two rows for each taxlot i.e. positional and closing
			-- take care of open qty from latest state of taxlots

			-- Insert PositionalTaxlot
		   Insert Into PM_Taxlots
			(
				TaxLotID,
				Symbol,
				TaxLotOpenQty,
				AvgPrice,
				TimeOfSaveUTC,
				CorpActionID,
				GroupID,
				AUECModifiedDate,
				FundID,
				Level2ID,
				OpenTotalCommissionandFees,
				ClosedTotalCommissionandFees,
				PositionTag,
				OrderSideTagValue,
				TaxLotClosingId_Fk
			)
			Select 
				@PositionalTaxlotID,
				Symbol,
				TaxLotOpenQty - @ClosedQty,
				AvgPrice,
				@TimeOfSaveUTC,
				NULL,
				GroupID,
				@AUECLocalDate,
				FundID,
				Level2ID,
				((TaxLotOpenQty - @ClosedQty)/TaxLotOpenQty) * OpenTotalCommissionandFees,
				ClosedTotalCommissionandFees + (OpenTotalCommissionandFees 
												- (((TaxLotOpenQty - @ClosedQty)/TaxLotOpenQty) 
														* OpenTotalCommissionandFees)
												),
				PositionTag,
				OrderSideTagValue,
				@TaxlotClosingID
			FROM PM_Taxlots	Where Taxlot_PK = 
				(
					Select MAX(Taxlot_PK) from PM_Taxlots Where TaxlotID  = @PositionalTaxlotID
				)
				

			-- Insert ClosingTaxlot
		   Insert Into PM_Taxlots
			(
				TaxLotID,
				Symbol,
				TaxLotOpenQty,
				AvgPrice,
				TimeOfSaveUTC,
				CorpActionID,
				GroupID,
				AUECModifiedDate,
				FundID,
				Level2ID,
				OpenTotalCommissionandFees,
				ClosedTotalCommissionandFees,
				PositionTag,
				OrderSideTagValue,
				TaxLotClosingId_Fk
			)
			Select 
				@ClosingTaxlotID,
				Symbol,
				TaxLotOpenQty - @ClosedQty,
				AvgPrice,
				@TimeOfSaveUTC,
				NULL,
				GroupID,
				@AUECLocalDate,
				FundID,
				Level2ID,
				((TaxLotOpenQty - @ClosedQty)/TaxLotOpenQty) * OpenTotalCommissionandFees,
				ClosedTotalCommissionandFees + (OpenTotalCommissionandFees 
												- (((TaxLotOpenQty - @ClosedQty)/TaxLotOpenQty) 
														* OpenTotalCommissionandFees)
												),
				PositionTag,
				OrderSideTagValue,
				@TaxlotClosingID
			FROM PM_Taxlots	Where Taxlot_PK = 
				(
					Select MAX(Taxlot_PK) from PM_Taxlots Where TaxlotID  = @ClosingTaxlotID
				)
		 END

		 IF (@ClosingMode <> 0)
		 BEGIN
			-- Need to insert one rows for 
			-- take care of open qty from latest state of taxlots
		   Insert Into PM_Taxlots
			(
				TaxLotID,
				Symbol,
				TaxLotOpenQty,
				AvgPrice,
				TimeOfSaveUTC,
				CorpActionID,
				GroupID,
				AUECModifiedDate,
				FundID,
				Level2ID,
				OpenTotalCommissionandFees,
				ClosedTotalCommissionandFees,
				PositionTag,
				OrderSideTagValue,
				TaxLotClosingId_Fk
			)
			Select 
				@PositionalTaxlotID,
				Symbol,
				TaxLotOpenQty - @ClosedQty,
				AvgPrice,
				@TimeOfSaveUTC,
				NULL,
				GroupID,
				@AUECLocalDate,
				FundID,
				Level2ID,
				((TaxLotOpenQty - @ClosedQty)/TaxLotOpenQty) * OpenTotalCommissionandFees,
				ClosedTotalCommissionandFees + (OpenTotalCommissionandFees 
												- (((TaxLotOpenQty - @ClosedQty)/TaxLotOpenQty) 
														* OpenTotalCommissionandFees)
												),
				PositionTag,
				OrderSideTagValue,
				@TaxlotClosingID
			FROM PM_Taxlots	Where Taxlot_PK = 
				(
					Select MAX(Taxlot_PK) from PM_Taxlots Where TaxlotID  = @PositionalTaxlotID
				)

			IF (@ClosingMode = 2)
			BEGIN
				-- Need to copy @TaxlotClosingID in generated taxlot as a result of excercise
				Update PM_Taxlots	
					Set TaxLotClosingId_Fk = @TaxlotClosingID
				Where Taxlot_PK = 
				(
					Select MIN(Taxlot_PK) from PM_Taxlots Where TaxlotID  = @ClosingTaxlotID
				)

			END
		 END


    
	 FETCH NEXT FROM tmp_ClosingData INTO           
		@TaxlotClosingID, 
		@PositionalTaxlotID, 
		@ClosingTaxlotID, 
		@ClosedQty, 
			--TaxLotOpenQty, 
		@ClosingMode, 
		@SettlementPrice,
		@TimeOfSaveUTC, 
			--Taxlots.AUECModifiedDate TradeDate, 
		@AUECLocalDate,
		@CorpActionID ;         
 END


CLOSE tmp_ClosingData;                                     
DEALLOCATE tmp_ClosingData;                    
