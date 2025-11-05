GO
/****** Object:  StoredProcedure [dbo].[P_NT_SetFund]    Script Date: 05/13/2015 16:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
--Exec P_NT_SetSetFund @FundName='Happy',@GroupTypeId = 1
--Exec P_NT_SetSetFund @FundName='New Year',@GroupTypeName = 'Account Proxy'
-- Select * From T_NT_GroupType
-- Select * From T_CompanyMasterFunds
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_SetFund] 
-- Add the parameters for the stored procedure here
@FundId int = Null,@FundName varchar(Max) = Null,@GroupTypeId int = Null,@GroupTypeName varchar(Max) = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here

Declare @SetFundGroupTypeId int 
If @GroupTypeId Is Not Null 
Begin 
Select @SetFundGroupTypeId = Id From T_NT_GroupType Where Id = @GroupTypeId 
End 
Else If @GroupTypeName Is Not Null 
Begin 
Select @SetFundGroupTypeId = Id From T_NT_GroupType Where [Name] = @GroupTypeName 
End 
If @SetFundGroupTypeId Is Not Null 
Begin 
	Declare @ExistingFundIdByFundName int 
	Select @ExistingFundIdByFundName = CompanyMasterFundID From T_CompanyMasterFunds Where MasterFundName = @FundName And GroupTypeId = @SetFundGroupTypeId 
	-- We can not rename or insert a fund have a same name with an existing fund
	If @ExistingFundIdByFundName Is Null 
	Begin 
		Declare @ExistingFundIdByFundId int 
		Select @ExistingFundIdByFundId = CompanyMasterFundID From T_CompanyMasterFunds Where CompanyMasterFundID = @FundId And GroupTypeId = @SetFundGroupTypeId 
		-- Found Existing Fund By FundId 
		-- Insertion and Renaming Requires a non-whitespace name, we will handle this in C# code
		If @ExistingFundIdByFundId Is Not Null 
		Begin 
			-- Print 'Update'
			Update T_CompanyMasterFunds 
			Set MasterFundName = @FundName Where CompanyMasterFundID = @ExistingFundIdByFundId And GroupTypeId = @SetFundGroupTypeId
		End 
		Else 
		Begin 
			-- Print 'Insert'
			Insert Into T_CompanyMasterFunds (MasterFundName,GroupTypeId) 
			Select @FundName,@SetFundGroupTypeId 
		End 
	End
End  

END

GO