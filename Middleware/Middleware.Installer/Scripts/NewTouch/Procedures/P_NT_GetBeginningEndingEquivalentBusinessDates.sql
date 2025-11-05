GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetBeginningEndingEquivalentBusinessDates]    Script Date: 05/13/2015 16:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage: 
-- Get Non-Zero Cash Before An EndDate Excluding Weekends 
Declare 
@StartDate datetime,
@EndDate datetime 
Select 
@StartDate = '01/01/2014',
@EndDate = '09/18/2014' 
Exec P_NT_GetBeginningEndingEquivalentBusinessDates @StartDate,@EndDate
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetBeginningEndingEquivalentBusinessDates] 
@StartDate datetime,
@EndDate datetime
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT OFF;
SET FMTONLY OFF;
--SET NOCOUNT ON;
-- Insert statements for procedure here 
Declare @DefaultAUECID int 
Select @DefaultAUECID = DefaultAUECID From T_Company 
Create Table #Dates 
(Date datetime Not Null,BeginningEquivalentBusinessDate datetime Not Null,EndingEquivalentBusinessDate datetime Not Null) 
Declare @CurrentDate datetime 
Select @CurrentDate = @StartDate 
While @CurrentDate <= @EndDate
Begin 
Insert Into #Dates (Date,BeginningEquivalentBusinessDate,EndingEquivalentBusinessDate) 
Select @CurrentDate,dbo.AdjustBusinessDays(@CurrentDate,-1, @DefaultAUECID),dbo.AdjustBusinessDays(DateAdd(d,1,@CurrentDate),-1,@DefaultAUECID) 
Select @CurrentDate = DateAdd(d,1,@CurrentDate) 
End 

Select Date,BeginningEquivalentBusinessDate,EndingEquivalentBusinessDate From #Dates
Drop Table #Dates 

END

GO