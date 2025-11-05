/****** Object:  StoredProcedure [dbo].[P_NT_GetAcctCashTransactions]    Script Date: 05/13/2015 16:36:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage: 
Declare 
@StartDate datetime,
@EndDate datetime
Select 
@StartDate = '02/26/2015',
@EndDate = '02/26/2015'
Exec P_NT_GetAcctCashTransactions @StartDate,@EndDate
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetAcctCashTransactions] 
@StartDate datetime,
@EndDate datetime
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here

Declare @BaseCurrencyID int
Select @BaseCurrencyID = BaseCurrencyID From T_Company

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

Create Table #AcctValues
(AcctId int Not Null,AcctName varchar(Max) Not Null,Date datetime Not Null,CashTransactionsLocal float Not Null,LocalCurrencyID int Not Null,BaseCurrencyID int Not Null,FXConversionRate float,FXConversionMethod int,FXRate float) 
Insert Into #AcctValues 
(AcctId,AcctName,Date,CashTransactionsLocal,LocalCurrencyID,BaseCurrencyID,FXConversionRate,FXConversionMethod,FXRate) 
Select F.AcctId,F.Acctname,F.Date,(E.DR-E.CR) As CashFlowLocal,E.CurrencyID As LocalCurrencyID,@BaseCurrencyID As BaseCurrencyID,Case When E.CurrencyID = @BaseCurrencyID Then 1 Else G.RateValue End As FXConversionRate,Case When E.CurrencyID = @BaseCurrencyID Then 1 Else G.ConversionMethod End As FXConversionMethod,E.FXRate 
From T_MasterCategory A 
Join T_SubCategory B On B.MasterCategoryID = A.MasterCategoryID 
Join T_SubAccounts C On C.SubCategoryID = B.SubCategoryID 
Join T_TransactionType D On D.TransactionTypeID = C.TransactionTypeID 
Join T_Journal E On C.SubAccountID = E.SubAccountID And E.SubAccountID = 17 And E.TaxlotID = '' And CharIndex('Spot',E.PBDesc) <> 0 
Join #AcctDates F On E.FundID = F.AcctId And E.TransactionDate = F.Date 
Left Outer Join #FXConversionRates G On E.CurrencyID = G.FromCurrencyID And @BaseCurrencyID = G.ToCurrencyID And E.TransactionDate = G.Date 

Select AcctId,AcctName,Date,CashTransactionsLocal,LocalCurrencyID,BaseCurrencyID,FXConversionRate,FXConversionMethod,FXRate
From #AcctValues Order By Date,AcctId,LocalCurrencyID

Drop Table #Dates,#AcctDates,#FXConversionRates,#AcctValues

END
GO