GO
/****** Object:  StoredProcedure [dbo].[P_NT_RemoveExistingFund]    Script Date: 05/13/2015 16:36:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
--Exec P_NT_RemoveExistingFund @FundName = 'Happy',@GroupTypeId = 1
--Exec P_NT_RemoveExistingFund @FundName = 'New Year',@GroupTypeName = 'Account Proxy'
-- Select * From T_NT_GroupType
-- Select * From T_CompanyMasterFunds
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_RemoveExistingFund] 
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

Declare @ExistingFundGroupTypeId int 
If @GroupTypeId Is Not Null 
Begin 
	Select @ExistingFundGroupTypeId = Id From T_NT_GroupType Where Id = @GroupTypeId 
End 
Else If @GroupTypeName Is Not Null 
Begin 
	Select @ExistingFundGroupTypeId = Id From T_NT_GroupType Where [Name] = @GroupTypeName 
End 
If @ExistingFundGroupTypeId Is Not Null 
Begin 
	-- Either FundId Is Passed In, Or FundName Is Passed In, If Both Are Passed In, Judge By FundId
	Declare @ExistingFundId int 
	Select @ExistingFundId = CompanyMasterFundID From T_CompanyMasterFunds Where CompanyMasterFundID = @FundId And GroupTypeId = @ExistingFundGroupTypeId
	If @ExistingFundId Is Null 
	Begin 
		Select @ExistingFundId = CompanyMasterFundID From T_CompanyMasterFunds Where MasterFundName = @FundName And GroupTypeId = @ExistingFundGroupTypeId 
	End 
	If @ExistingFundId Is Not Null 
	Begin 
		Delete From T_CompanyMasterFunds Where CompanyMasterFundID = @ExistingFundId And GroupTypeId = @ExistingFundGroupTypeId 
	End
End  

END

GO