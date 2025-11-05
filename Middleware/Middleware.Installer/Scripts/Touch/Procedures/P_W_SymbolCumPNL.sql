/****** Object:  StoredProcedure [dbo].[P_W_SymbolCumPNL]    Script Date: 02/27/2014 10:05:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Declare @GroupBy varchar(Max),@Client varchar(Max),@Entity varchar(Max),@TimeFrame varchar(Max)
-- Select @GroupBy = 'Fund',@Client = 'Monarch',@Entity = 'Monarch Cap LLC. Account Number: 832-46315D8',@TimeFrame = 'YearToDate'
-- Declare @SymbolLevel varchar(Max),@Symbol varchar(Max) 
-- Select @SymbolLevel = 'Symbol',@Symbol = 'FB'
-- Exec P_W_SymbolCumPNL @GroupBy,@Client,@Entity,@TimeFrame,@SymbolLevel,@Symbol
-- =============================================
CREATE PROCEDURE [dbo].[P_W_SymbolCumPNL] 
	-- Add the parameters for the stored procedure here
	@GroupBy varchar(Max),
	@Client varchar(Max),
    @Entity varchar(Max),
	@TimeFrame varchar(Max),
	@SymbolLevel varchar(Max),
	@Symbol varchar(Max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	------Daily/Cumulative P&L 
	------(Be Aware Null Symbol Will Be Excluded Here, Entity With Non Open Positions In This TimeFrame Will Be Excluded)
	------(Be Aware That Symbol Cumulative P&L Is Not Counted After That Symbl Stop Existing In The Entity, Could Not Sum Up Directly To Get Entity Cumulative P&L)
	Create Table #DailyPNL
	(Date datetime,PNL float)

	Create Table #DailyCumPNL
	(Date datetime,DayPNL float,CumPNL float)

	If @SymbolLevel = 'Symbol' 
	Begin
		Insert Into #DailyPNL(Date,PNL) 
		Select Date,PNL From T_W_SymbolDailyPNL Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame =  @TimeFrame And Symbol = @Symbol 
	End
	Else If @SymbolLevel = 'UnderlyingSymbol' 
	Begin 
		Insert Into #DailyPNL(Date,PNL) 
		Select Date,Sum(IsNull(PNL,0)) From T_W_SymbolDailyPNL Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame =  @TimeFrame And UnderlyingSymbol = @Symbol Group By Date
	End

	Insert Into #DailyCumPNL(Date,DayPNL,CumPNL)
	Select CP1.Date,CP1.PNL,Sum(CP2.PNL) From #DailyPNL CP1 Join #DailyPNL CP2 On CP2.Date <= CP1.Date 
	Group By CP1.Date,CP1.PNL

	Select Date,DayPNL,CumPNL From #DailyCumPNL Order By Date Asc

	Drop Table #DailyPNL,#DailyCumPNL

END
GO
