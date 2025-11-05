GO
/****** Object:  StoredProcedure [dbo].[P_NT_DisapproveApproveAcctValue]    Script Date: 05/13/2015 16:36:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_DisapproveApproveAcctValue '05/04/2014','Investor 3',1
Exec P_NT_DisapproveApproveAcctValue '05/04/2014','Fund IP 1',1
Exec P_NT_DisapproveApproveAcctValue '05/04/2014','Account IP 3',1
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_DisapproveApproveAcctValue] 
	-- Add the parameters for the stored procedure here
	@UpdateDate smalldatetime,
	@AcctName varchar(Max),
	@DisapproveApprove bit
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here

Declare @AcctId int
Select @AcctId = CompanyFundID 
From T_CompanyFunds 
Where FundName = @AcctName 

If @AcctId Is Not Null
Begin 
	If @DisapproveApprove = 0 
	Begin 
		Update T_NT_AcctVal Set Approved = 0 
		Where AcctId = @AcctId And UpdateDate = @UpdateDate 
	End 
	Else If @DisapproveApprove = 1 
	Begin 
		Update T_NT_AcctVal Set Approved = 1 
		Where AcctId = @AcctId And UpdateDate = @UpdateDate
	End 
End

END


GO