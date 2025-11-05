GO
/****** Object:  StoredProcedure [dbo].[P_NT_SetInvestorIP]    Script Date: 05/13/2015 16:36:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
--Exec P_NT_SetInvestorIP 'Happy',@GroupTypeId = 4
--Exec P_NT_SetInvestorIP 'New Year',@GroupTypeName = 'Account Proxy Independent Party'
-- Select * From T_NT_GroupType 
-- Select * From T_NT_InvestorIP
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_SetInvestorIP] 
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

Declare @SetInvestorIPGroupTypeId int 
If @GroupTypeId Is Not Null 
Begin 
Select @SetInvestorIPGroupTypeId = Id From T_NT_GroupType Where Id = @GroupTypeId 
End 
Else If @GroupTypeName Is Not Null 
Begin 
Select @SetInvestorIPGroupTypeId = Id From T_NT_GroupType Where [Name] = @GroupTypeName 
End 
If @SetInvestorIPGroupTypeId Is Not Null 
Begin 
	Declare @ExistingInvestorIPIdByInvestorIPName int 
	Select @ExistingInvestorIPIdByInvestorIPName = [Name] From T_NT_InvestorIP Where [Name] = @InvestorIPName And GroupTypeId = @SetInvestorIPGroupTypeId 
	-- We can not rename or insert a fund have a same name with an existing fund
	If @ExistingInvestorIPIdByInvestorIPName Is Null 
	Begin 
		Declare @ExistingInvestorIPIdByInvestorIPId int 
		Select @ExistingInvestorIPIdByInvestorIPId = Id From T_NT_InvestorIP Where Id = @InvestorIPId And GroupTypeId = @SetInvestorIPGroupTypeId 
		-- Found Existing Fund By InvestorIPId 
		-- Insertion and Renaming Requires a non-whitespace name, we will handle this in C# code
		If @ExistingInvestorIPIdByInvestorIPId Is Not Null 
		Begin 
			-- Print 'Update'
			Update T_NT_InvestorIP 
			Set [Name] = @InvestorIPName Where Id = @ExistingInvestorIPIdByInvestorIPId And GroupTypeId = @SetInvestorIPGroupTypeId
		End 
		Else 
		Begin 
			-- Print 'Insert'
			Insert Into T_NT_InvestorIP ([Name],GroupTypeId) 
			Select @InvestorIPName,@SetInvestorIPGroupTypeId
		End 
	End
End  

END

GO