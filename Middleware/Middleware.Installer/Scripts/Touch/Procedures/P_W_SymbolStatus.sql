/****** Object:  StoredProcedure [dbo].[P_W_SymbolStatus]    Script Date: 02/27/2014 10:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- Declare @GroupBy varchar(Max),@Client varchar(Max),@Entity varchar(Max),@TimeFrame varchar(Max)
-- Select @GroupBy = 'Fund',@Client = 'Monarch',@Entity = 'Monarch Cap LLC. Account Number: 832-46315D8',@TimeFrame = 'YearToDate'
-- Declare @SymbolLevel varchar(Max),@Symbol
-- Select @SymbolLevel = 'Symbol',@Symbol = 'FB'
-- Exec P_W_SymbolStatus @GroupBy,@Client,@Entity,@TimeFrame,@SymbolLevel,@Symbol
-- =============================================
CREATE PROCEDURE [dbo].[P_W_SymbolStatus] 
	-- Add the parameters for the stored procedure here
	@GroupBy varchar(Max),
	@Client varchar(Max),
    @Entity varchar(Max),
	@TimeFrame varchar(Max),
	@SymbolLevel varchar(Max),
    @Symbol varchar(Max)
As
BEGIN
	-- Set NOCOUNT On added to prevent extra result sets From
	-- interfering with Select statements.
	Set NOCOUNT On;

    -- Insert statements for procedure here

Create Table #SymbolInfo([Current Position] float,[Closing Mark] float,[Exposure] float,[Period P&L] float,[P&L Contribution] float)

If @SymbolLevel = 'Symbol' 
Begin
	Insert Into #SymbolInfo([Current Position],[Closing Mark],[Exposure],[Period P&L],[P&L Contribution])
	Select Sum(Position),Avg(ClosingMark),Sum(DeltaExposure),Sum(PeriodTotalPnL),Sum([P&L Contribution]) From T_W_HoldingSymbolGroup Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame =  @TimeFrame And Symbol = @Symbol 
End
Else If @SymbolLevel = 'UnderlyingSymbol' 
Begin
	Insert Into #SymbolInfo([Current Position],[Closing Mark],[Exposure],[Period P&L],[P&L Contribution])
	Select Sum(DeltaAdjPosition),Avg(UnderlyingClosingMark),Sum(DeltaExposure),Sum(PeriodTotalPnL),Sum([P&L Contribution]) From T_W_HoldingSymbolGroup Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame =  @TimeFrame And UnderlyingSymbol = @Symbol
End

Create Table #Status (Stat varchar(Max),Data float)
Insert Into #Status (Stat,Data) 
Select Stat,Data From 
(Select [Current Position],[Closing Mark],[Exposure],[Period P&L],[P&L Contribution] From #SymbolInfo) P 
UnPivot (Data For Stat In ([Current Position],[Closing Mark],[Exposure],[Period P&L],[P&L Contribution])) As UnPvt

Create Table #StatusKeys(StatusKey varchar(Max))
Insert Into #StatusKeys	(StatusKey)
Select 'Current Position' 
Union All Select 'Closing Mark' 
Union All Select 'Exposure' 
Union All Select 'Period P&L' 
Union All Select 'P&L Contribution'
	
Select StatusKey As Stat,NullIf(Data,0) As Data From #StatusKeys Left Outer Join #Status On #StatusKeys.StatusKey = #Status.Stat

Drop Table #SymbolInfo,#Status,#StatusKeys

END
