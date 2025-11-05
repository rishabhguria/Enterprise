/****** Object:  StoredProcedure [dbo].[P_W_IndexVAMI]    Script Date: 02/27/2014 10:05:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
--	Declare @GroupBy varchar(Max),@Client varchar(Max),@Entity varchar(Max),@TimeFrame varchar(Max)
--	Select @GroupBy = 'Fund',@Client = 'Monarch',@Entity = 'Monarch Cap LLC. Account Number: 832-46315D8',@TimeFrame = 'YearToDate'
--
--	Declare @Index varchar(Max)
--	Select @Index = 'CAC40'
-- =============================================
CREATE PROCEDURE [dbo].[P_W_IndexVAMI] 
	-- Add the parameters for the stored procedure here
	@GroupBy varchar(Max),
	@Client varchar(Max),
    @Entity varchar(Max),
	@TimeFrame varchar(Max),
	@Index varchar(Max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	Create Table #Prices(Date datetime,ClosePrice float)
	Insert Into #Prices(Date)
	Select Date From T_W_VAMI Where GroupBy = @GroupBy And Client = @Client And Entity = @Entity And TimeFrame = @TimeFrame

	Create Table #Indices(Date datetime,ClosePrice float)
	Insert Into #Indices(Date,ClosePrice)
	Select Date,ClosePrice From T_W_IndicesPrices Where Symbol = @Index

	Update Prices Set Prices.ClosePrice = Indices.ClosePrice From #Prices Prices Join #Indices Indices On Prices.Date = Indices.Date

	Create Table #Performance(Date datetime,Performance float)
	;With PricesCte(Date,ClosePrice,[Rank]) As 
	(Select Date,ClosePrice,Rank() Over(Order By Date Asc) From #Prices)

	Insert Into #Performance(Date,Performance)
	Select Prices2.Date,IsNull((IsNull(Prices2.ClosePrice,0)-IsNull(Prices1.ClosePrice,0))/NullIf(Prices1.ClosePrice,0),0) From PricesCte Prices1 Join PricesCte Prices2 On Prices1.[Rank]+1 = Prices2.[Rank] 
	Union 
	Select Date,Null From PricesCte Where [Rank] =  1

	Create Table #VAMI
	(Date datetime,Performance float,VAMI float)
	Insert Into #VAMI(Date,Performance,VAMI)
	Select VAMI1.Date,VAMI1.Performance,1000*Exp(Sum(Log(IsNull(VAMI2.Performance,0)+1))) From #Performance VAMI1 Join #Performance VAMI2 On VAMI2.Date <= VAMI1.Date 
	Group By VAMI1.Date,VAMI1.Performance

	Select Date,VAMI From #VAMI Order By Date Asc

	Drop Table #Prices,#Indices,#Performance,#VAMI

END

GO
