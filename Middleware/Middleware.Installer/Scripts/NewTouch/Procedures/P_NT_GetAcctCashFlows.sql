GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetAcctCashFlows]    Script Date: 05/13/2015 16:36:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage: 
Declare 
@StartDate datetime,
@EndDate datetime,
@Equity bit,
@Income bit,
@Expenses bit
Select 
@StartDate = '02/26/2015',
@EndDate = '02/26/2015',
@Equity = 0,
@Income = 0,
@Expenses = 0
Exec P_NT_GetAcctCashFlows @StartDate,@EndDate,@Equity,@Income,@Expenses
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetAcctCashFlows] 
@StartDate datetime,
@EndDate datetime,
@Equity bit = 1,
@Income bit = 0,
@Expenses bit = 0
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

Create Table #MasterCategories (MasterCategoryID int) 
If @Equity = 1  
Begin 
	Insert Into #MasterCategories (MasterCategoryID) Select 3 
End
If @Income = 1  
Begin 
	Insert Into #MasterCategories (MasterCategoryID) Select 4 
End
If @Expenses = 1  
Begin 
	Insert Into #MasterCategories (MasterCategoryID) Select 5 
End

Create Table #JournalBins (MasterCategoryID int,MasterCategoryName varchar(Max),SubCategoryID int,SubCategoryName varchar(Max),SubAccountID int,SubAccountName varchar(Max),TransactionTypeID int,TransactionTypeName varchar(Max))
Insert Into #JournalBins (MasterCategoryID,MasterCategoryName,SubCategoryID,SubCategoryName,SubAccountID,SubAccountName,TransactionTypeID,TransactionTypeName) 
Select A.MasterCategoryID,A.MasterCategoryName,B.SubCategoryID,B.SubCategoryName,C.SubAccountID,C.Name As SubAccountName,D.TransactionTypeID,D.TransactionType As TransactionTypeName From T_MasterCategory A Join T_SubCategory B Join T_SubAccounts C Join T_TransactionType D 
On D.TransactionTypeID = C.TransactionTypeID On C.SubCategoryID = B.SubCategoryID On B.MasterCategoryID = A.MasterCategoryID 
Where A.MasterCategoryID In (Select MasterCategoryID From #MasterCategories) And D.TransactionTypeID <> 3 

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
(AcctId int Not Null,AcctName varchar(Max) Not Null,Date datetime Not Null,CashFlowLocal float Not Null,LocalCurrencyID int Not Null,BaseCurrencyID int Not Null,FXConversionRate float,FXConversionMethod int,MasterCategoryID int Not Null,MasterCategoryName varchar(Max) Not Null,SubCategoryID int Not Null,SubCategoryName varchar(Max) Not Null,SubAccountID int Not Null,SubAccountName varchar(Max) Not Null,TransactionTypeID int Not Null,TransactionTypeName varchar(Max) Not Null) 
Insert Into #AcctValues 
(AcctId,AcctName,Date,CashFlowLocal,LocalCurrencyID,BaseCurrencyID,FXConversionRate,FXConversionMethod,MasterCategoryID,MasterCategoryName,SubCategoryID,SubCategoryName,SubAccountID,SubAccountName,TransactionTypeID,TransactionTypeName) 
Select A.AcctId,A.AcctName,A.Date,(B.CR-B.DR) As CashFlowLocal,B.CurrencyID As LocalCurrencyID,@BaseCurrencyID As BaseCurrencyID,Case When B.CurrencyID = @BaseCurrencyID Then 1 Else D.RateValue End As FXConversionRate,Case When B.CurrencyID = @BaseCurrencyID Then 1 Else D.ConversionMethod End As FXConversionMethod,C.MasterCategoryID,C.MasterCategoryName,C.SubCategoryID,C.SubCategoryName,C.SubAccountID,C.SubAccountName,C.TransactionTypeID,C.TransactionTypeName
From #AcctDates A 
Join T_Journal B On A.AcctId = B.FundID And A.Date = B.TransactionDate 
Join #JournalBins C On B.SubAccountID = C.SubAccountID 
Left Outer Join #FXConversionRates D On B.CurrencyID = D.FromCurrencyID And @BaseCurrencyID = D.ToCurrencyID And B.TransactionDate = D.Date 

Select AcctId,AcctName,Date,CashFlowLocal,LocalCurrencyID,BaseCurrencyID,FXConversionRate,FXConversionMethod,MasterCategoryID,MasterCategoryName,SubCategoryID,SubCategoryName,SubAccountID,SubAccountName,TransactionTypeID,TransactionTypeName 
From #AcctValues Order By Date,AcctId,LocalCurrencyID

Drop Table #Dates,#AcctDates,#FXConversionRates,#AcctValues,#MasterCategories,#JournalBins

END
GO