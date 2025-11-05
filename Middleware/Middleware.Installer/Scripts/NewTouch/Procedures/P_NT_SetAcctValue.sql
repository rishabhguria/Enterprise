GO
/****** Object:  StoredProcedure [dbo].[P_NT_SetAcctValue]    Script Date: 05/13/2015 16:36:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_SetAcctValue '05/04/2014','Investor 3',0.22,0.22
Exec P_NT_SetAcctValue '05/04/2014','Fund IP 1',0.22,0.22
Exec P_NT_SetAcctValue '05/04/2014','Account IP 3',0.22,0.22 
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_SetAcctValue] 
	-- Add the parameters for the stored procedure here
	@UpdateDate smalldatetime,
	@AcctName varchar(Max),
	@Val_TradingLevel decimal(38,8),
	@Val_InvestedCash decimal(38,8)
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
	Delete From T_NT_AcctVal Where AcctId = @AcctId And UpdateDate = @UpdateDate
	Insert Into T_NT_AcctVal 
	(AcctId,UpdateDate,Val_TradingLevel,Val_InvestedCash)
	Select @AcctId,@UpdateDate,@Val_TradingLevel,@Val_InvestedCash
End

END

GO