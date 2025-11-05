GO
/****** Object:  StoredProcedure [dbo].[P_NT_HandleGenericPNL]    Script Date: 05/13/2015 16:36:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_HandleGenericPNL] 
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
	(FundId int Not Null,FundName varchar(Max) Not Null,AssociationCount int Not Null,AcctId int Not Null,AcctName varchar(Max) Not Null)
	Insert Into #AcctAssociations 
	(FundId,FundName,AssociationCount,AcctId,AcctName)
	Select Min(C.CompanyMasterFundID),Min(C.MasterFundName),Count(C.CompanyMasterFundID),A.CompanyFundID,A.FundName
	From T_CompanyFunds A Join T_CompanyMasterFundSubAccountAssociation B Join T_CompanyMasterFunds C 
	On C.CompanyMasterFundID = B.CompanyMasterFundID On B.CompanyFundID = A.CompanyFundID 
	Group By A.CompanyFundID,A.FundName 

	-- For Each Fund, Delete (Non-Cash, Non-Dividend) Items With Master Funds Different From The First Associated Mater Fund; 
    -- Therefore, For Each Fund, Only The First Associated Mater Fund's (Non-Cash, Non-Dividend) Items Remain
	Delete P From T_MW_GenericPNL P Inner Join #FundIDSymbol FS On FS.Fund = P.Fund And (FS.Symbol = P.Symbol Or FS.Symbol Is Null) And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 Inner Join #AcctAssociations AA On AA.AcctName = P.Fund And AA.FundName <> P.MasterFund
	Where Not (Asset = 'Cash' Or Open_CloseTag = 'D') 

	-- For Each Fund, Update (Non-Cash, Non-Dividend) Items By Setting Master Fund Column To Null
	Update P Set MasterFund = Null From T_MW_GenericPNL P Inner Join #FundIDSymbol FS On FS.Fund = P.Fund And (FS.Symbol = P.Symbol Or FS.Symbol Is Null) And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 
	Where Not (Asset = 'Cash' Or Open_CloseTag = 'D') 

	-- For Each Fund, Update (Cash, Dividend) Items By Dividing Their Values Counted Association Times Than Necessary, And Setting The Master Fund Column To Null 
	Update P Set 
	MasterFund = Null,
	DividendLocal = DividendLocal/AssociationCount,
	Dividend = Dividend/AssociationCount,
	TotalCost_Local = TotalCost_Local/AssociationCount,
	TotalCost_Base = TotalCost_Base/AssociationCount,
	TotalCost_BaseD0FX = TotalCost_BaseD0FX/AssociationCount,
	TotalCost_BaseD2FX = TotalCost_BaseD2FX/AssociationCount,
	BeginningMarketValueLocal = BeginningMarketValueLocal/AssociationCount,
	BeginningMarketValueBase = BeginningMarketValueBase/AssociationCount,
	BeginningMarketValue_BaseD2FX = BeginningMarketValue_BaseD2FX/AssociationCount,
	EndingMarketValueLocal = EndingMarketValueLocal/AssociationCount, 
	EndingMarketValueBase = EndingMarketValueBase/AssociationCount 
	From T_MW_GenericPNL P Join #Fund FS On P.Fund = FS.Fund And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 Join #AcctAssociations AA On AA.AcctName = P.Fund 
	Where (Asset = 'Cash' Or Open_CloseTag = 'D') 

	Drop Table #FundIDSymbol,#Fund,#AcctAssociations
END


GO