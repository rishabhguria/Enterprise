GO
/****** Object:  StoredProcedure [dbo].[P_NT_RemoveAcctValue]    Script Date: 05/13/2015 16:36:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_RemoveAcctValue '05/04/2014','Investor 3'
Exec P_NT_RemoveAcctValue '05/04/2014','Fund IP 1'
Exec P_NT_RemoveAcctValue '05/04/2014','Account IP 3'
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_RemoveAcctValue] 
	-- Add the parameters for the stored procedure here
	@UpdateDate smalldatetime,
	@AcctName varchar(Max)
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
	Delete From T_NT_AcctVal Where AcctId = @AcctId And UpdateDate = @UpdateDate And IsNull(Approved,0) = 0
End

END


GO