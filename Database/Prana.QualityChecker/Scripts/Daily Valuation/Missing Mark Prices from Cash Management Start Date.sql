declare @ToDate datetime
Declare @errormsg varchar(max)
Declare @Date Datetime
Declare @FundId varchar(max)
Declare @iterator int

set @ToDate=''
set @errormsg=''

CREATE TABLE #opensymbols (Symbol VARCHAR(MAX), FundId VARCHAR(MAX) )

Create Table #CompanyFunds
( 
CompanyFundID int,
FundName Varchar(50)
)

Insert Into #CompanyFunds
Select
CompanyFundID,
FundName
From T_CompanyFunds

Create Table #OpenSymbolsWithMarkPrice
(
Date Datetime,
Symbol varchar(200),
[Mark Price] float,
BloombergSymbol Varchar(200),
FundName Varchar(200),
[No Entry For Symbol] Varchar(10)
)

Create Table #OpenSymbolsWithMarkPriceWithoutFundWise
(
Date Datetime,
Symbol varchar(200),
[Mark Price] float,
BloombergSymbol Varchar(200),
FundName Varchar(200),
[No Entry For Symbol] Varchar(10)
)

Create Table #FundInfo
( 
fundid int,
CashMgmtStartDate Datetime,
ID int identity (1,1)
)

insert into #FundInfo select fundid, CashMgmtStartDate
from T_CashPreferences
order by ID

DECLARE @ID INT
SET @ID = 1

WHILE (@ID <= (SELECT COUNT(*) From #FundInfo)) 
BEGIN
	Select @Date = CashMgmtStartDate from #FundInfo where ID = @ID
	select @FundId = fundid from #FundInfo where ID = @ID
	WHILE (@Date <= @ToDate)
	BEGIN
		DELETE #opensymbols
		
		INSERT INTO #opensymbols
		Select distinct Symbol, FundID 
		From PM_Taxlots
		Where TaxLotOpenQty<>0 and FundID = @FundId and 
		Taxlot_PK in
		(
			 Select max(Taxlot_PK) from PM_Taxlots
			 where DateDiff(d,PM_Taxlots.AUECModifiedDate,@Date) >=0
			 group by taxlotid
		)
		Insert into #OpenSymbolsWithMarkPrice
		Select
		@Date,
		Symbol,
		0,
		'',
		FundId,
		'False' from #opensymbols 	

		Set @Date=DateAdd(d,1,@Date)
	END
	SET @ID = @ID + 1
END

Insert into #OpenSymbolsWithMarkPriceWithoutFundWise
Select 
Date,
Symbol,
0,
'',
0,
'False' from #OpenSymbolsWithMarkPrice 
Group By Date,Symbol


if (Select Count(*) From PM_DayMarkPrice where FundID <> 0) > 0
Begin

	UPDATE #OpenSymbolsWithMarkPrice 
	Set [Mark Price]= CASE When FinalMarkPrice IS NOT NULL THEN FinalMarkPrice ELSE NULL END,
	BloombergSymbol= VM.BloombergSymbol,
	FundName = CASE WHEN CF.FundName IS NULL THEN NULL ELSE CF.FundName END,
	[No Entry For Symbol]=Case When FinalMarkPrice IS NULL THEN 'True' Else 'False' End
	FROM #OpenSymbolsWithMarkPrice OMP
	LEFT JOIN PM_DayMarkPrice DMP On OMP.Symbol = DMP.Symbol and OMP.Date=DMP.Date AND DMP.FundID = OMP.FundName
	INNER JOIN #CompanyFunds CF On OMP.FundName = CF.CompanyFundID
	INNER JOIN V_SecMasterDAta VM ON OMP.Symbol = VM.TickerSymbol

	IF EXISTS (Select * from #OpenSymbolsWithMarkPrice Where [Mark Price]=0 OR [Mark Price] IS NULL)
	BEGIN
		set @errormsg ='Mark price are missing for some symbols for fund wise  (From Cash Management Start date)'
		Select * from #OpenSymbolsWithMarkPrice 
		Where [Mark Price]=0 OR [Mark Price] IS NULL 
		Order By Date Desc,Symbol
	END
END
ELSE
BEGIN
	
	UPDATE #OpenSymbolsWithMarkPriceWithoutFundWise 
	Set [Mark Price]= CASE When FinalMarkPrice IS NOT NULL THEN FinalMarkPrice ELSE NULL END,
	BloombergSymbol= VM.BloombergSymbol,
	FundName = NULL,
	[No Entry For Symbol]=Case When FinalMarkPrice IS NULL THEN 'True' Else 'False' End
	FROM #OpenSymbolsWithMarkPriceWithoutFundWise OMP
	LEFT JOIN PM_DayMarkPrice DMP On OMP.Symbol = DMP.Symbol and OMP.Date=DMP.Date 
	INNER JOIN V_SecMasterDAta VM ON OMP.Symbol = VM.TickerSymbol

	
	IF EXISTS (Select * from #OpenSymbolsWithMarkPriceWithoutFundWise Where [Mark Price]=0 OR [Mark Price] IS NULL)
	BEGIN
		set @errormsg ='Mark price are missing for some symbols for without fund wise (From Cash Management Start date)'
		Select * from #OpenSymbolsWithMarkPriceWithoutFundWise
		Where [Mark Price]=0 OR [Mark Price] IS NULL
		Order By Date Desc,Symbol
	END
	
END
select @errormsg as ErrorMsg

Drop table #FundInfo
Drop table #opensymbols,#CompanyFunds
Drop table #OpenSymbolsWithMarkPrice
Drop table #OpenSymbolsWithMarkPriceWithoutFundWise