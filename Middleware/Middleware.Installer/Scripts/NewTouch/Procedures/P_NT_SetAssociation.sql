GO
/****** Object:  StoredProcedure [dbo].[P_NT_SetAssociation]    Script Date: 05/13/2015 16:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_SetAssociation 'RJ OASIS','Access OASIS 1','Fund'
Exec P_NT_SetAssociation 'RJ OASIS','Access OASIS 2','Fund'
Exec P_NT_SetAssociation 'RJ OASIS','Access OASIS 3','Fund'
Exec P_NT_SetAssociation 'Account Proxy of Account 1','Access OASIS 1','Account Proxy'
Exec P_NT_SetAssociation 'Account Combination of Account 1-2-3','Access OASIS 1','Account Proxy Combination'
Exec P_NT_SetAssociation 'Account Combination of Account 1-2-3','Access OASIS 2','Account Proxy Combination'
Exec P_NT_SetAssociation 'Account Combination of Account 1-2-3','Access OASIS 3','Account Proxy Combination'

Parameter @FundGroupTypeName refers Acct the one for Fundgroup.
Parameter @InsertFundIfNotExist is optional, pass 1 for shortcut creation of Fund and Acct groups while setting association.
*/
/* Notes:
Account Proxy cannot have associations with multiple accounts.
Fund Combination cannot have associations Acct funds only.

After a batch of inserts, the whole batch can be verified by: 
Exec P_NT_ValidateAssociation 
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_SetAssociation] 
	-- Add the parameters for the sAcctred procedure here
	@FundName varchar(Max),
	@AcctName varchar(Max),
	@FundGroupTypeName varchar(Max),
	@InsertFundIfNotExist bit = 0  
AS
BEGIN
-- SET NOCOUNT ON added Acct prevent extra result sets Fund
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here 
Declare @FundGroupTypeId int 
Select @FundGroupTypeId = Id From T_NT_GroupType Where [Name] = @FundGroupTypeName 

Declare @FundId int 
Declare @AcctId int 
Declare @FundSnapshot Table  
(CompanyMasterFundID int)

-- Define Fund 
If @FundGroupTypeId = 1 
Begin
	Select @FundId = CompanyMasterFundID From T_CompanyMasterFunds Where GroupTypeId = @FundGroupTypeId And MasterFundName = @FundName 
	If @FundId Is Null And @InsertFundIfNotExist = 1 
	Begin 
		Insert Into T_CompanyMasterFunds 
		(MasterFundName,GroupTypeId) 
		Output Inserted.CompanyMasterFundID Into @FundSnapshot
		Select @FundName,@FundGroupTypeId 
		Select @FundId = CompanyMasterFundID From @FundSnapshot
	End 
	Select @AcctId = CompanyFundID From T_CompanyFunds Where FundName = @AcctName
	If (@FundId Is Not Null) And (@AcctId Is Not Null) 
	Begin
		If Not Exists (Select CompanyMasterFundID,CompanyFundID From T_CompanyMasterFundSubAccountAssociation Where CompanyMasterFundID = @FundId And CompanyFundID = @AcctId)
		Begin 
			Insert Into T_CompanyMasterFundSubAccountAssociation 
			(CompanyMasterFundID,CompanyFundID) 
			Select @FundId,@AcctId 
		End
	End 
End
-- Define Account Proxy 
If @FundGroupTypeId = 2 
Begin
	Select @FundId = CompanyMasterFundID From T_CompanyMasterFunds Where GroupTypeId = @FundGroupTypeId And MasterFundName = @FundName 
	If @FundId Is Null And @InsertFundIfNotExist = 1 
	Begin 
		Insert Into T_CompanyMasterFunds 
		(MasterFundName,GroupTypeId) 
		Output Inserted.CompanyMasterFundID Into @FundSnapshot
		Select @FundName,@FundGroupTypeId 
		Select @FundId = CompanyMasterFundID From @FundSnapshot
	End 
	Select @AcctId = CompanyFundID From T_CompanyFunds Where FundName = @AcctName
	If (@FundId Is Not Null) And (@AcctId Is Not Null) 
	Begin
		If Not Exists (Select CompanyMasterFundID,CompanyFundID From T_CompanyMasterFundSubAccountAssociation Where CompanyMasterFundID = @FundId)
		Begin 
			Insert Into T_CompanyMasterFundSubAccountAssociation 
			(CompanyMasterFundID,CompanyFundID) 
			Select @FundId,@AcctId 
		End
	End 
End

END


GO