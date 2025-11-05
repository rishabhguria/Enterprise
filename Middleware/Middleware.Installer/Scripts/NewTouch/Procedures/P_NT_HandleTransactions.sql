GO
/****** Object:  StoredProcedure [dbo].[P_NT_HandleTransactions]    Script Date: 05/13/2015 16:36:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_HandleTransactions] 
	-- Add the parameters for the stored procedure here
@FromDate DateTime,
@ToDate DateTime,
@FundIDSymbol xml = Null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Create Table #FundIDSymbol
	(FundID int,Fund varchar(Max),Symbol varchar(Max))
	Insert Into #FundIDSymbol
	(FundID,Fund,Symbol)
	Select 
	ref.value('FundID[1]','int') As FundID,
	ref.value('Fund[1]','varchar(Max)') As Fund,
	ref.value('Symbol[1]','varchar(Max)') As Symbol 
	From @FundIDSymbol.nodes('/FundIDSymbol') xmlData(ref)

	If Not Exists (Select FundID,Fund,Symbol From #FundIDSymbol)
	Begin 
		Insert Into #FundIDSymbol 
		(FundID,Fund) 
		Select CompanyFundID,FundName From T_CompanyFunds 
	End

	Select Distinct Fund 
	Into #Fund From #FundIDSymbol

	Create Table #AcctAssociations 
	(FundId int Not Null,FundName varchar(Max) Not Null,AcctId int Not Null,AcctName varchar(Max) Not Null)
	Insert Into #AcctAssociations 
	(FundId,FundName,AcctId,AcctName)
	Select Min(C.CompanyMasterFundID),Min(C.MasterFundName),A.CompanyFundID,A.FundName
	From T_CompanyFunds A Join T_CompanyMasterFundSubAccountAssociation B Join T_CompanyMasterFunds C 
	On C.CompanyMasterFundID = B.CompanyMasterFundID On B.CompanyFundID = A.CompanyFundID 
	Group By A.CompanyFundID,A.FundName 

	Delete P with(tablock) From T_MW_Transactions P Inner Join #FundIDSymbol FS On FS.Fund = P.Fund And (FS.Symbol = P.Symbol Or FS.Symbol Is Null) And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 Inner Join #AcctAssociations AA On AA.AcctName = P.Fund And AA.FundName <> P.MasterFund 
	Where Not (Open_CloseTag = 'D' Or Open_CloseTag = 'DP' Or Open_CloseTag = 'Cash') 

	Update P Set MasterFund = Null From T_MW_Transactions P Inner Join #FundIDSymbol FS On FS.Fund = P.Fund And (FS.Symbol = P.Symbol Or FS.Symbol Is Null) And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 
	Where Not (Open_CloseTag = 'D' Or Open_CloseTag = 'DP' Or Open_CloseTag = 'Cash') 

	Delete P with(tablock) From T_MW_Transactions P Join #Fund FS On P.Fund = FS.Fund And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 Inner Join #AcctAssociations AA On AA.AcctName = P.Fund And AA.FundName <> P.MasterFund 
	Where (Open_CloseTag = 'D' Or Open_CloseTag = 'DP' Or Open_CloseTag = 'Cash') 

	Update P Set MasterFund = Null From T_MW_Transactions P Join #Fund FS On P.Fund = FS.Fund And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 
	Where (Open_CloseTag = 'D' Or Open_CloseTag = 'DP' Or Open_CloseTag = 'Cash') 

	Drop Table #FundIDSymbol,#AcctAssociations,#Fund
END


GO