/****** Object:  StoredProcedure [dbo].[P_NT_AdjustBusinessDays]    Script Date: 05/13/2015 16:36:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
--Exec P_NT_AdjustBusinessDays '01/02/2015',-1
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_AdjustBusinessDays]
-- Add the parameters for the stored procedure here
@StartDate datetime,
@NumDays int,
@AUECID int = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

If @AUECID Is Null 
Begin 
	Select @AUECID = DefaultAUECID From T_Company 
End 

Select dbo.AdjustBusinessDays(@StartDate,@NumDays,@AUECID)

END

GO
