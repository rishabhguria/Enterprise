/****** Object:  StoredProcedure [dbo].[P_W_NaturalHistoricalDates]    Script Date: 02/27/2014 10:05:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE Procedure [dbo].[P_W_NaturalHistoricalDates] 
	-- Add the parameters for the stored Procedure here
	@ToDate datetime
AS
Begin

-- Set NOCOUNT On added to prevent extra result sets from
-- interfering with Select statements.
Set NOCOUNT On;

-- Insert statements for Procedure here
Create Table #NaturalToDate(TimeFrame varchar(Max),FromDate datetime,ToDate datetime)
Insert Into #NaturalToDate(TimeFrame,FromDate,ToDate)
Select 'Day',@ToDate,@ToDate
Insert Into #NaturalToDate(TimeFrame,FromDate,ToDate)
Select 'WeekToDate',DateAdd(dd,1-DatePart(dw,@ToDate),@ToDate),@ToDate
Insert Into #NaturalToDate(TimeFrame,FromDate,ToDate)
Select 'MonthToDate',DateAdd(dd,1-DatePart(dd,@ToDate),@ToDate),@ToDate
Insert Into #NaturalToDate(TimeFrame,FromDate,ToDate)
Select 'QuarterToDate',DateAdd(qq,DateDiff(qq,0,@ToDate),0),@ToDate
Insert Into #NaturalToDate(TimeFrame,FromDate,ToDate)
Select 'YearToDate',DateAdd(yy,DateDiff(yy,0,@ToDate),0),@ToDate

Select TimeFrame,FromDate,ToDate From #NaturalToDate

Drop Table #NaturalToDate

End

GO
