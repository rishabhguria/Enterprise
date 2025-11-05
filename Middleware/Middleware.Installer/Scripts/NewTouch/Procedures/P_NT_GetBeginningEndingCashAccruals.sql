GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetBeginningEndingCashAccruals]    Script Date: 05/13/2015 16:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage: 
Declare 
@StartDate datetime,
@EndDate datetime,
@BeginningOrEnding bit,
@CashOrAccruals bit,
@EquivalentBusinessDateOrRecentNonZeroCashDate bit,
@RawOrBatch bit 
Select 
@StartDate = '05/01/2015',
@EndDate = '05/01/2015',
@BeginningOrEnding = 0,
@CashOrAccruals = 1,
@EquivalentBusinessDateOrRecentNonZeroCashDate = 1,
@RawOrBatch = 1
Exec [P_NT_GetBeginningEndingCashAccruals] @StartDate,@EndDate,@BeginningOrEnding,@CashOrAccruals,@EquivalentBusinessDateOrRecentNonZeroCashDate,@RawOrBatch

Declare 
@StartDate datetime,
@EndDate datetime,
@BeginningOrEnding bit,
@CashOrAccruals bit,
@EquivalentBusinessDateOrRecentNonZeroCashDate bit,
@RawOrBatch bit 
Select 
@StartDate = '05/01/2015',
@EndDate = '05/01/2015',
@BeginningOrEnding = 0,
@CashOrAccruals = 0,
@EquivalentBusinessDateOrRecentNonZeroCashDate = 1,
@RawOrBatch = 0
Exec [P_NT_GetBeginningEndingCashAccruals] @StartDate,@EndDate,@BeginningOrEnding,@CashOrAccruals,@EquivalentBusinessDateOrRecentNonZeroCashDate,@RawOrBatch
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetBeginningEndingCashAccruals] 
@StartDate datetime,
@EndDate datetime,
@BeginningOrEnding bit,
@CashOrAccruals bit,
@EquivalentBusinessDateOrRecentNonZeroCashDate bit,
@RawOrBatch bit   
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here 

Create Table #AcctValues 
(AcctId int Not Null,AcctName varchar(Max) Not Null,RunDate datetime Not Null,ValueLocal float Not Null,BeginningFXRate float,EndingFXRate float) 

If @RawOrBatch = 0 
Begin 
	Declare @BaseCurrencyID int
	Select @BaseCurrencyID = BaseCurrencyID From T_Company

	Create Table #Dates 
	(Date datetime,BeginningEquivalentBusinessDate datetime,EndingEquivalentBusinessDate datetime,BeginningRecentNonZeroCashDate datetime,EndingRecentNonZeroCashDate datetime) 
	Insert Into #Dates 
	(Date,BeginningEquivalentBusinessDate,EndingEquivalentBusinessDate) 
	Exec P_NT_GetBeginningEndingEquivalentBusinessDates @StartDate,@EndDate 
	Update #Dates Set 
	BeginningRecentNonZeroCashDate = dbo.GetRecentDateForNonZeroCash(BeginningEquivalentBusinessDate),
	EndingRecentNonZeroCashDate = dbo.GetRecentDateForNonZeroCash(EndingEquivalentBusinessDate) 
	Create Table #AcctDates
	(AcctId int Not Null,AcctName varchar(Max) Not Null,Date datetime Not Null,BeginningEquivalentBusinessDate datetime Not Null,EndingEquivalentBusinessDate datetime Not Null,BeginningRecentNonZeroCashDate datetime Not Null,EndingRecentNonZeroCashDate datetime Not Null) 
	Insert Into #AcctDates 
	(AcctId,AcctName,Date,BeginningEquivalentBusinessDate,EndingEquivalentBusinessDate,BeginningRecentNonZeroCashDate,EndingRecentNonZeroCashDate) 
	Select CompanyFundID,FundName,Date,BeginningEquivalentBusinessDate,EndingEquivalentBusinessDate,BeginningRecentNonZeroCashDate,EndingRecentNonZeroCashDate 
	From #Dates,T_CompanyFunds 
	Create Table #FXConversionRates
	(FromCurrencyID int Not Null,
	ToCurrencyID int Not Null,
	RateValue float Not Null,
	ConversionMethod int Not Null,
	Date DateTime Not Null) 
	Insert Into #FXConversionRates 
	(FromCurrencyID,ToCurrencyID,RateValue,ConversionMethod,Date) 
	Select B.FromCurrencyID,B.ToCurrencyID,IsNull(A.ConversionRate,0),0,A.Date 
	From T_CurrencyConversionRate A Join T_CurrencyStandardPairs B On B.CurrencyPairID = A.CurrencyPairID_FK 
	Insert Into #FXConversionRates 
	(FromCurrencyID,ToCurrencyID,RateValue,ConversionMethod,Date) 
	Select B.ToCurrencyID,B.FromCurrencyID,IsNull(A.ConversionRate,0),1,A.Date 
	From T_CurrencyConversionRate A Join T_CurrencyStandardPairs B On B.CurrencyPairID = A.CurrencyPairID_FK 

	Create Table #AcctValues_1 
	(AcctId int Not Null,AcctName varchar(Max) Not Null,Date datetime Not Null,BeginningEquivalentBusinessDate datetime Not Null,EndingEquivalentBusinessDate datetime Not Null,BeginningRecentNonZeroCashDate datetime Not Null,EndingRecentNonZeroCashDate datetime Not Null,	CashValueLocal float Not Null,LocalCurrencyID int Not Null,BaseCurrencyID int Not Null,BeginningFXConversionRate float,EndingFXConversionRate float,BeginningFXConversionMethod int,EndingFXConversionMethod int) 

	If @CashOrAccruals = 0 
	Begin 
		Insert Into #AcctValues_1 
		(AcctId,AcctName,Date,BeginningEquivalentBusinessDate,EndingEquivalentBusinessDate,BeginningRecentNonZeroCashDate,EndingRecentNonZeroCashDate,CashValueLocal,LocalCurrencyID,BaseCurrencyID,BeginningFXConversionRate,EndingFXConversionRate,BeginningFXConversionMethod,EndingFXConversionMethod) 
		Select A.AcctId,A.AcctName,A.Date,A.BeginningEquivalentBusinessDate,A.EndingEquivalentBusinessDate,A.BeginningRecentNonZeroCashDate,A.EndingRecentNonZeroCashDate,Cash.CashValueLocal,Cash.LocalCurrencyID,Cash.BaseCurrencyID,Case When Cash.LocalCurrencyID = Cash.BaseCurrencyID Then 1 Else Bfx.RateValue End,Case When Cash.LocalCurrencyID = Cash.BaseCurrencyID Then 1 Else Efx.RateValue End,Case When Cash.LocalCurrencyID = Cash.BaseCurrencyID Then 0 Else Bfx.ConversionMethod End,Case When Cash.LocalCurrencyID = Cash.BaseCurrencyID Then 0 Else Efx.ConversionMethod End  
		From #AcctDates A 
		Join PM_CompanyFundCashCurrencyValue Cash On (Case @BeginningOrEnding When 0 Then A.BeginningRecentNonZeroCashDate Else A.EndingRecentNonZeroCashDate End) = Cash.Date And A.AcctId = Cash.FundID 
		Left Outer Join #FXConversionRates Bfx On (Case @EquivalentBusinessDateOrRecentNonZeroCashDate When 0 Then A.BeginningEquivalentBusinessDate Else A.BeginningRecentNonZeroCashDate End) = Bfx.Date And Cash.LocalCurrencyID = Bfx.FromCurrencyID And Cash.BaseCurrencyID = Bfx.ToCurrencyID 
		Left Outer Join #FXConversionRates Efx On (Case @EquivalentBusinessDateOrRecentNonZeroCashDate When 0 Then A.EndingEquivalentBusinessDate Else A.EndingRecentNonZeroCashDate End) = Efx.Date And Cash.LocalCurrencyID = Efx.FromCurrencyID And Cash.BaseCurrencyID = Efx.ToCurrencyID 
	End 
	Else 
	Begin 
		Insert Into #AcctValues_1 
		(AcctId,AcctName,Date,BeginningEquivalentBusinessDate,EndingEquivalentBusinessDate,BeginningRecentNonZeroCashDate,EndingRecentNonZeroCashDate,CashValueLocal,LocalCurrencyID,BaseCurrencyID,BeginningFXConversionRate,EndingFXConversionRate,BeginningFXConversionMethod,EndingFXConversionMethod) 
		Select A.AcctId,A.AcctName,A.Date,A.BeginningEquivalentBusinessDate,A.EndingEquivalentBusinessDate,A.BeginningRecentNonZeroCashDate,A.EndingRecentNonZeroCashDate,IsNull(Cash.CloseDRBal-Cash.CloseCRBal,0),Cash.CurrencyID,@BaseCurrencyID,Case When Cash.CurrencyID = @BaseCurrencyID Then 1 Else Bfx.RateValue End,Case When Cash.CurrencyID = @BaseCurrencyID Then 1 Else Efx.RateValue End,Case When Cash.CurrencyID = @BaseCurrencyID Then 0 Else Bfx.ConversionMethod End,Case When Cash.CurrencyID = @BaseCurrencyID Then 0 Else Efx.ConversionMethod End  
		From #AcctDates A 
		Join T_SubAccountBalances Cash On (Case @BeginningOrEnding When 0 Then A.BeginningRecentNonZeroCashDate Else A.EndingRecentNonZeroCashDate End) = Cash.TransactionDate And A.AcctId = Cash.FundID 
		Join T_SubAccounts SubAccounts On SubAccounts.SubAccountID = Cash.SubAccountID    
		Join T_TransactionType TransType On SubAccounts.TransactionTypeID = TransType.TransactionTypeID And TransType.TransactionType = 'Accrued Balance'    
		Left Outer Join #FXConversionRates Bfx On (Case @EquivalentBusinessDateOrRecentNonZeroCashDate When 0 Then A.BeginningEquivalentBusinessDate Else A.BeginningRecentNonZeroCashDate End) = Bfx.Date And Cash.CurrencyID = Bfx.FromCurrencyID And @BaseCurrencyID = Bfx.ToCurrencyID 
		Left Outer Join #FXConversionRates Efx On (Case @EquivalentBusinessDateOrRecentNonZeroCashDate When 0 Then A.EndingEquivalentBusinessDate Else A.EndingRecentNonZeroCashDate End) = Efx.Date And Cash.CurrencyID = Efx.FromCurrencyID And @BaseCurrencyID = Efx.ToCurrencyID 
	End

	Insert Into #AcctValues 
	(AcctId,AcctName,RunDate,ValueLocal,BeginningFXRate,EndingFXRate) 
	Select AcctId,AcctName,Date,CashValueLocal,Case BeginningFXConversionMethod When 0 Then BeginningFXConversionRate Else 1/BeginningFXConversionRate End,Case EndingFXConversionMethod When 0 Then EndingFXConversionRate Else 1/EndingFXConversionRate End From #AcctValues_1 
	
	Drop Table #Dates,#AcctDates,#FXConversionRates,#AcctValues_1	
End 
Else If @RawOrBatch = 1
Begin 
	Insert Into #AcctValues 
	(AcctId,AcctName,RunDate,ValueLocal,BeginningFXRate,EndingFXRate) 
	Select AcctId,AcctName,RunDate,(Case @BeginningOrEnding When 0 Then BeginningMarketValueLocal Else EndingMarketValueLocal End),BeginningFXRate,EndingFXRate From T_NT_CashAccruals Where (CashOrAccruals = @CashOrAccruals) And (RunDate Between @StartDate And @EndDate)
End 

Select AcctId,AcctName,RunDate,ValueLocal,BeginningFXRate,EndingFXRate 
From #AcctValues Order By RunDate,AcctId

Drop Table #AcctValues

END

GO