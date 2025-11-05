
Declare @errormsg varchar(max)

Set @errormsg=''


IF  EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_W_Funds')
BEGIN

Select FundName,CompanyFundID into #temp from T_companyfunds where Companyfundid  not in (select distinct PranaFundID from T_W_Funds )

IF  EXISTS( select * from #temp )
BEGIN
	SET @errormsg='Need to Mapped PranaFundID with TouchFundID'
	SELECT * FROM #temp
	END
Drop table #temp
END

SELECT @errormsg AS ErrorMsg



