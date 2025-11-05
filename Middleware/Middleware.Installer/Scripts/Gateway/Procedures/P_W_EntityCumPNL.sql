/****** Object:  StoredProcedure [dbo].[P_W_EntityCumPNL]    Script Date: 02/27/2014 10:05:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[P_W_EntityCumPNL] 
	-- Add the parameters for the stored procedure here
	@GroupBy varchar(Max),
	@Client varchar(Max),
    @Entity varchar(Max),
	@TimeFrame varchar(Max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Create Table #DailyPNL
	(Date datetime,PNL float)

	Create Table #DailyCumPNL
	(Date datetime,DayPNL float,CumPNL float)

	Insert Into #DailyPNL(Date,PNL) 
	Select Date,Sum(IsNull(PNL,0)) From T_W_SymbolDailyPNL Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame =  @TimeFrame Group By Date 

	Insert Into #DailyCumPNL(Date,DayPNL,CumPNL)
	Select CP1.Date,CP1.PNL,Sum(CP2.PNL) From #DailyPNL CP1 Join #DailyPNL CP2 On CP2.Date <= CP1.Date 
	Group By CP1.Date,CP1.PNL

	Select Date,DayPNL,CumPNL From #DailyCumPNL Order By Date Asc

	Drop Table #DailyPNL,#DailyCumPNL
END

GO
