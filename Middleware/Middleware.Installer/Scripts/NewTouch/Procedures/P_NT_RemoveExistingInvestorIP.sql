GO
/****** Object:  StoredProcedure [dbo].[P_NT_RemoveExistingInvestorIP]    Script Date: 05/13/2015 16:36:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
--Exec P_NT_RemoveExistingInvestorIP @InvestorIPName = 'Happy',@GroupTypeId = 4
--Exec P_NT_RemoveExistingInvestorIP @InvestorIPName = 'New Year',@GroupTypeName = 'Account Proxy Independent Party'
-- Select * From T_NT_GroupType
-- Select * From T_NT_InvestorIP
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_RemoveExistingInvestorIP] 
-- Add the parameters for the stored procedure here
@InvestorIPId bigint = Null,@InvestorIPName varchar(Max) = Null,@GroupTypeId int = Null,@GroupTypeName varchar(Max) = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here 

Declare @ExistingInvestorIPGroupTypeId int 
If @GroupTypeId Is Not Null 
Begin 
	Select @ExistingInvestorIPGroupTypeId = Id From T_NT_GroupType Where Id = @GroupTypeId 
End 
Else If @GroupTypeName Is Not Null 
Begin 
	Select @ExistingInvestorIPGroupTypeId = Id From T_NT_GroupType Where [Name] = @GroupTypeName 
End 
If @ExistingInvestorIPGroupTypeId Is Not Null 
Begin 
	-- Either InvestorIPId Is Passed In, Or InvestorIPName Is Passed In, If Both Are Passed In, Judge By InvestorIPId
	Declare @ExistingInvestorIPId int 
	Select @ExistingInvestorIPId = Id From T_NT_InvestorIP Where Id = @InvestorIPId And GroupTypeId = @ExistingInvestorIPGroupTypeId
	If @ExistingInvestorIPId Is Null 
	Begin 
		Select @ExistingInvestorIPId = Id From T_NT_InvestorIP Where [Name] = @InvestorIPName And GroupTypeId = @ExistingInvestorIPId
	End 
	If @ExistingInvestorIPId Is Not Null 
	Begin 
		Delete From T_NT_InvestorIP Where Id = @ExistingInvestorIPId
	End
End  

END

GO