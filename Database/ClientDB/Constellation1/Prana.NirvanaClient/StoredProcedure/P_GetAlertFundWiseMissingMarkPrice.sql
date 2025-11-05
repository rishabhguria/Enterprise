/*
declare @msg nvarchar(max)

EXEC P_GetAlertFundWiseMissingMarkPrice 10, @msg output

Select @msg 
*/
CREATE Procedure [dbo].[P_GetAlertFundWiseMissingMarkPrice]
(
	@Days int,
	@Result nvarchar(max) Output
)
AS
BEGIN

Set NoCount On

Declare @EmailXML nvarchar(max)

	Declare @EmailBody nvarchar(max)

	SET @EmailBody = 

	'<html><body> 
	 <table border = 1> 
	  <tr> 
		   <th> Date </th>
		   <th> Account Name</th>
		   <th> Ticker Symbol</th>
	  </tr>'    

Declare @FromDate datetime,
	    @ToDate datetime, 
        @Date Datetime,
		@TMinusOneDate Datetime

--Declare @Days int
--Set @Days = 25

Set @TMinusOneDate = DateAdd(Day,-1,GetDate())

Set @FromDate = DateDiff(Day,(@Days-1),@TMinusOneDate)
Set @ToDate= @TMinusOneDate

Set @Date = @FromDate

Declare @CompanyFunds Table
( 
	CompanyFundID int,
	FundName Varchar(50)
)
Insert Into @CompanyFunds
	Select
	CompanyFundID,
	FundName
	From T_CompanyFunds
	
Create Table #OpenSymbolsFundwise
(
	Date Datetime,
	Symbol varchar(200),
	FundID Int
)

WHILE(@Date <= @ToDate)
BEGIN

	INSERT INTO #OpenSymbolsFundwise
	Select Distinct @Date,Symbol,FundID
	From PM_Taxlots With(NoLock)
	Where Taxlot_PK in
		(
		  Select Max(Taxlot_PK) From PM_Taxlots With(NoLock)
		  Where DateDiff(Day,PM_Taxlots.AUECModifiedDate,@Date) >=0
		  group by TaxLotID
		)
	And TaxLotOpenQty > 0

Set @Date = DateAdd(DAY,1,@Date)
END



SELECT @EmailXML = CAST(( SELECT Convert(VARCHAR(10),OP.Date,101) AS 'td','',CF.FundName  AS 'td','',OP.Symbol AS 'td'  

	From #OpenSymbolsFundwise OP
	Inner Join T_CompanyFunds CF With(NoLock) On CF.CompanyFundID = OP.FundID
	Left Outer Join PM_DayMarkPrice DMP With(NoLock) On DateDiff(Day,OP.Date,DMP.Date) = 0 And 
	OP.Symbol = DMP.Symbol And OP.FundID = DMP.FundID
	Where (DMP.FinalMarkPrice Is Null Or DMP.FinalMarkPrice = 0)
	Order By OP.Date Desc, FundName Asc, OP.Symbol Asc

	FOR XML PATH('tr'), ELEMENTS ) AS NVARCHAR(MAX))

	SET @Result = @EmailBody + @EmailXML +'</table></body></html>'

Drop Table #OpenSymbolsFundwise

END 