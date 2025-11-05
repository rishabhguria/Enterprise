GO
/****** Object:  StoredProcedure [dbo].[P_NT_RemoveAssociation]    Script Date: 05/13/2015 16:36:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_RemoveAssociation 'RJ OASIS','Access OASIS 1','Fund'
Exec P_NT_RemoveAssociation 'RJ OASIS','Access OASIS 2','Fund'
Exec P_NT_RemoveAssociation 'RJ OASIS','Access OASIS 3','Fund'
Exec P_NT_RemoveAssociation 'Account Proxy of Account 1','Access OASIS 1','Account Proxy'
Exec P_NT_RemoveAssociation 'Account Combination of Account 1-2-3','Access OASIS 1','Account Proxy Combination'
Exec P_NT_RemoveAssociation 'Account Combination of Account 1-2-3','Access OASIS 2','Account Proxy Combination'
Exec P_NT_RemoveAssociation 'Account Combination of Account 1-2-3','Access OASIS 3','Account Proxy Combination'

Parameter @GroupTypeName refers to the one for fromgroup.
*/
/* Notes:
Account Proxy cannot have associations with multiple accounts.
Fund Combination cannot have associations to funds only.

After a batch of removals, the whole batch can be verified by: 
Exec P_NT_ValidateAssociation 
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_RemoveAssociation] 
	-- Add the parameters for the stored procedure here
	@FundName varchar(Max),
	@AcctName varchar(Max),
	@GroupTypeName varchar(Max)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here 
Declare @GroupTypeId int 
Select @GroupTypeId = Id From T_NT_GroupType Where [Name] = @GroupTypeName 

Declare @FundId int 
Declare @AcctId int 

-- Define Fund 
-- Define Account Proxy 
If @GroupTypeId In (1,2) 
Begin
	Select @FundId = CompanyMasterFundID From T_CompanyMasterFunds Where GroupTypeId = @GroupTypeId And MasterFundName = @FundName 
	Select @AcctId = CompanyFundID From T_CompanyFunds Where FundName = @AcctName
	If (@FundId Is Not Null) And (@AcctId Is Not Null)
	Begin 
		Delete From T_CompanyMasterFundSubAccountAssociation 
		Where CompanyMasterFundID = @FundId And CompanyFundID = @AcctId
	End 
End

END


GO