GO
/****** Object:  StoredProcedure [dbo].[P_NT_DeleteGenericPNL]    Script Date: 05/13/2015 16:36:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
--Exec P_NT_DeleteGenericPNL '04/14/2014','09/18/2014' 
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_DeleteGenericPNL]
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

	Delete A From T_NT_GenericPNL A Join #Fund FS On FS.Fund = A.AcctName Where Open_CloseTag = 'D' And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0  
	Delete A From T_NT_GenericPNL A Join #FundIDSymbol FS On FS.Fund = A.AcctName And (FS.Symbol = A.Symbol Or FS.Symbol Is Null) And Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0 

	Drop Table #FundIDSymbol,#Fund
END


GO