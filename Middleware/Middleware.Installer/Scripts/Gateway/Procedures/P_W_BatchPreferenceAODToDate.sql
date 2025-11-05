/****** Object:  StoredProcedure [dbo].[P_W_BatchPreferenceAODToDate]    Script Date: 02/27/2014 10:05:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- =============================================
CREATE PROCEDURE [dbo].[P_W_BatchPreferenceAODToDate] 
	-- Add the parameters for the stored procedure here
	@ReportID varchar(100) = 'Touch'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	Declare @DefaultAUECID int

	/* batch cutoff time for daily process in terms of eastern starndard time*/
	/* timezone diff between eastern standard time and user's timezone*/
	--Select @BatchCutoffTime = 6/7.., @TimezoneDiff = 0/12..
	Declare @BatchCutoffTime int, @TimezoneDiff int

	Declare @AsOfDate datetime,@ToDate datetime

	Select @DefaultAUECID = DefaultAUECID From T_Company

	Select @BatchCutoffTime = BatchCutoffTime,@TimezoneDiff = TimezoneDiff From T_W_BatchPreferences Where ReportID = @ReportID

	Select @AsOfDate = DateAdd(hh,@TimezoneDiff,GetDate())
	If DatePart(hh,@AsOfDate) < @BatchCutoffTime
	Select @AsOfDate = DateAdd(d,-1,@AsOfDate)

	Select @ToDate = dbo.AdjustBusinessDays(@AsOfDate,-1,@DefaultAUECID)

	Select @AsOfDate = Cast(Floor(Cast(@AsOfDate As float)) As datetime),@ToDate = Cast(Floor(Cast(@ToDate As float)) As datetime)

	Select @ReportID As ReportID ,@BatchCutoffTime As BatchCutoffTime,@TimezoneDiff As TimezoneDiff,@AsOfDate As AsOfDate,@ToDate As ToDate
END

GO
