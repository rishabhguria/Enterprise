/****** Object:  StoredProcedure [dbo].[P_NT_GetAcctValues]    Script Date: 05/13/2015 16:36:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec [P_NT_GetAcctValues]
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetAcctValues] 
-- Add the parameters for the stored procedure here
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--SET NOCOUNT OFF;
--SET FMTONLY OFF;
SET NOCOUNT ON;
-- Insert statements for procedure here

Select IsNull(A.Approved,0) As Approved,B.CompanyFundID As AcctId,B.FundName As AcctName,A.UpdateDate,A.Val_TradingLevel,A.Val_InvestedCash 
From T_NT_AcctVal A Join T_CompanyFunds B On A.AcctId = B.CompanyFundID

END

GO