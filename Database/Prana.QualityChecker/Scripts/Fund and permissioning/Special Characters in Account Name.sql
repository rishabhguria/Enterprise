Declare	@From_Date Datetime 
Declare @To_Date Datetime
Declare @ErrorMsg Varchar(Max)
 
set @To_Date=''
set @From_Date=''
Set @ErrorMsg=''

-- column #CompanyFunds_CounterParty will give all the places where there is special characters in the FundShortName.
-- column DoubleSpace is equal to 1 if there is double space in FundShortName, otherwise 0.


BEGIN TRY
create table #TempFundName
(	FundID INT primary key identity(1,1),
	FundShortName varchar(300),
	[Special Character] varchar(300),
	[Position of Special Character] varchar(300),
	DoubleSpace bit
)

DECLARE @len INT
select  @len=count(*) from T_CompanyFunds

DECLARE @x INT 
DECLARE @y INT 
DECLARE @strlen INT
DECLARE @s varchar(max)


insert into #TempFundName
select FundShortName,'','',0 from T_CompanyFunds 


SET @x=1
select @len=count(*) from #TempFundName
while @x<=@len
BEGIN
	select @s=FundShortName from #TempFundName where FundID=@x
	SET @y=1
	SET @strlen=Datalength(@s)

	while @y<=@strlen
	BEGIN
		
		
		-- Finding special character in FundShortName and updating in column SpecialCharacter
		IF NOT( ASCII(SubString(@s,@y,1))>=ASCII('A') AND ASCII(SubString(@s,@y,1))<=ASCII('Z')
			 OR ASCII(SubString(@s,@y,1))>=ASCII('a') AND ASCII(SubString(@s,@y,1))<=ASCII('z')
			 OR ASCII(SubString(@s,@y,1))>=ASCII('0') AND ASCII(SubString(@s,@y,1))<=ASCII('9')
		     OR ASCII(SubString(@s,@y,1))=ASCII(' ') 
		     OR ASCII(SubString(@s,@y,1))=ASCII('.')
		     OR ASCII(SubString(@s,@y,1))=ASCII('-')
			 OR ASCII(SubString(@s,@y,1))=ASCII(':'))

		   update #TempFundName
		   set [Special Character]=[Special Character]+' '+SubString(@s,@y,1),
			   [Position of Special Character]=[Position of Special Character]+'  '+LTRIM(str(@y))
		   where FundID=@x
		
		-- Finding Double Space in FundShortname and updating in column DoubleSpace
		IF ( ASCII(SubString(@s,@y,1))=ASCII(' ') AND ASCII(SubString(@s,@y+1,1))=ASCII(' '))
			
		   update #TempFundName
		   set DoubleSpace=1
		   where FundID=@x 

		SET @y=@y+1 
	END

	SET @x=@x+1
END

delete from #TempFundName where [Position of Special Character]='' AND DoubleSpace=0
select FundShortName,[Special Character],[Position of Special Character],DoubleSpace from #TempFundName

select @len=count(*) from #TempFundName
IF @len > 0 
	Set @ErrorMsg='Some special characters exist in accounts name'

select @ErrorMsg as ErrorMsg


drop table #TempFundName

END TRY
BEGIN CATCH
	drop table #tempFundName
END CATCH