/****** Object:  StoredProcedure [dbo].[P_W_SymbolFootprint]    Script Date: 02/27/2014 10:05:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- exec P_W_SymbolFootprint 'UnderlyingSymbol','SN','01/01/2013','09/17/2013','Monarch Cap LLC. Account Number: 832-46315D8'
-- exec P_W_SymbolFootprint 'Symbol','SN','01/01/2013','09/17/2013','Monarch Cap LLC. Account Number: 832-46315D8'
-- =============================================
CREATE PROCEDURE [dbo].[P_W_SymbolFootprint] 
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

	If @SymbolLevel = 'Symbol' 
	Begin
		Select Date,Symbol,AvgPrice,Quantity,Side From T_W_SymbolTransactions Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame =  @TimeFrame And Symbol = @Symbol 
	End
	Else If @SymbolLevel = 'UnderlyingSymbol' 
	Begin
		Select Date,UnderlyingSymbol As Symbol,AvgPrice,Sum(Quantity) As Quantity,Side From T_W_SymbolTransactions Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame =  @TimeFrame And UnderlyingSymbol = @Symbol Group By Date,UnderlyingSymbol,AvgPrice,Side
	End

END

GO
