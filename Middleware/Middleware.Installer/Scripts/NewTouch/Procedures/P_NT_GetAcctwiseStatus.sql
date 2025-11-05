GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetAcctwiseStatus]    Script Date: 05/13/2015 16:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Exec P_NT_GetAcctwiseStatus
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetAcctwiseStatus] 
-- Add the parameters for the stored procedure here
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
--SET NOCOUNT OFF;
--SET FMTONLY OFF;
-- Insert statements for procedure here
Select Id,AsOfDate,AcctId From T_NT_AcctwiseStatus
END


GO