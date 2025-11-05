GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetPortfolioReportsMasterFundIds]    Script Date: 05/13/2015 16:36:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Exec P_NT_GetPortfolioReportsMasterFundIds '1317,1318,1318'
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetPortfolioReportsMasterFundIds] 
	@AcctIds varchar(Max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Create Table #AcctIds (AcctId varchar(Max))
	Insert Into #AcctIds (AcctId)
	Select Items From dbo.Split(@AcctIds,',')

	Create Table #FundMasterFund (CompanyFundID int,FundName varchar(Max),CompanyMasterFundID int,MasterFundName varchar(Max))
	Insert Into #FundMasterFund (CompanyFundID,FundName,CompanyMasterFundID,MasterFundName)
	Select A.CompanyFundID,A.FundName,B.CompanyMasterFundID,B.MasterFundName
	From T_CompanyFunds A
	Left Outer Join (Select CMF.CompanyMasterFundID,CMF.MasterFundName,CMFSAA.CompanyFundID From T_CompanyMasterFunds CMF 
	Left Outer Join T_CompanyMasterFundSubAccountAssociation CMFSAA On CMFSAA.CompanyMasterFundID = CMF.CompanyMasterFundID 
	Where CMF.GroupTypeId = 2) B On A.CompanyFundID = B.CompanyFundID
	Where A.CompanyFundID In (Select AcctId From #AcctIds) 

	Declare @CompanyFundID int,@FundName varchar(Max)
	Declare MissingMappingCursor Cursor For
	Select CompanyFundID,FundName From #FundMasterFund Where CompanyMasterFundID Is Null 
	Open MissingMappingCursor 
	Fetch Next From MissingMappingCursor 
	Into @CompanyFundID,@FundName
	While @@Fetch_Status = 0 
	Begin 
	Exec P_NT_SetAssociation @FundName,@FundName,'Account Proxy',1

	Fetch Next From MissingMappingCursor 
	Into @CompanyFundID,@FundName
	End 
	Close MissingMappingCursor
	Deallocate MissingMappingCursor

	Declare @FundIds varchar(Max) 
	Select @FundIds = (Select ',' + Cast(B.CompanyMasterFundID As varchar(Max))
	From T_CompanyFunds A 
	Join #AcctIds C On A.CompanyFundID = C.AcctId 
	Left Outer Join (Select CMF.CompanyMasterFundID,CMF.MasterFundName,CMFSAA.CompanyFundID From T_CompanyMasterFunds CMF 
	Left Outer Join T_CompanyMasterFundSubAccountAssociation CMFSAA On CMFSAA.CompanyMasterFundID = CMF.CompanyMasterFundID 
	Where CMF.GroupTypeId = 2) B On A.CompanyFundID = B.CompanyFundID For Xml Path(''))
	Select SubString(@FundIds,2,Len(@FundIds)-1) 

	Drop Table #FundMasterFund

END

GO