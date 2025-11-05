GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetAccts]    Script Date: 05/13/2015 16:36:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_GetAccts
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetAccts] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--	SET NOCOUNT ON;
SET NOCOUNT OFF;
SET FMTONLY OFF;
-- Insert statements for procedure here 

Select CompanyFundId As AcctId,FundName As AcctName,StartDate From T_CompanyFunds 

END

GO