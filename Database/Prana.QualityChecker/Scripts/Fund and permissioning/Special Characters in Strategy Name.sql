Declare	@From_Date Datetime 
Declare @To_Date Datetime
Declare @ErrorMsg Varchar(Max)
 
set @To_Date=''
set @From_Date=''
Set @ErrorMsg=''

-- column #CompanyFunds_CounterParty will give all the places where there is special characters in the FundShortName.
-- column DoubleSpace is equal to 1 if there is double space in FundShortName, otherwise 0.


BEGIN TRY
create table #TempStartegyName
(	StrategyID INT primary key identity(1,1),
	StrategyShortName varchar(300),
	[Special Characters] varchar(300),
	[Position of Special Characters] varchar(300),
	[Ascii Code of Special Characters] varchar(300),
	DoubleSpace bit,
	[Position of Double Space] varchar(200)
	
)

DECLARE @len INT
select  @len=count(*) from T_CompanyStrategy

DECLARE @x INT 
DECLARE @y INT 
DECLARE @strlen INT
DECLARE @s varchar(max)


insert into #TempStartegyName
select StrategyShortName,'','','',0,'' from T_CompanyStrategy 


SET @x=1
select @len=count(*) from #TempStartegyName
while @x<=@len
BEGIN
	select @s=StrategyShortName from #TempStartegyName where StrategyID=@x
	SET @y=1
	SET @strlen=Datalength(@s)
	
	while @y<=@strlen
	BEGIN
		
		
		-- Finding special character in FundShortName and updating in column SpecialCharacter
		IF NOT( ASCII(SubString(@s,@y,1))>=ASCII('A') AND ASCII(SubString(@s,@y,1))<=ASCII('Z')
			 OR ASCII(SubString(@s,@y,1))>=ASCII('a') AND ASCII(SubString(@s,@y,1))<=ASCII('z')
			 OR ASCII(SubString(@s,@y,1))>=ASCII('0') AND ASCII(SubString(@s,@y,1))<=ASCII('9')
		     OR ASCII(SubString(@s,@y,1))=ASCII(' ')
			 )

		   update #TempStartegyName
		   set [Special Characters]=[Special Characters]+' '+SubString(@s,@y,1),
			   [Position of Special Characters]=[Position of Special Characters]+'  '+LTRIM(str(@y)),
			   [Ascii Code of Special Characters]=[Ascii Code of Special Characters]+' '+ASCII(SubString(@s,@y,1))
		   where StrategyID=@x
		
		-- Finding Double Space in FundShortname and updating in column DoubleSpace
		IF ( ASCII(SubString(@s,@y,1))=ASCII(' ') AND ASCII(SubString(@s,@y+1,1))=ASCII(' '))
			
		   update #TempStartegyName
		   set DoubleSpace=1,
		   [Position of Double Space]=[Position of Double Space]+'	    '+LTRIM(str(@y))
		   where StrategyID=@x 

		SET @y=@y+1 
	END

	SET @x=@x+1
END

delete from #TempStartegyName where [Position of Special Characters]='' AND DoubleSpace=0
select StrategyShortName,[Special Characters],[Position of Special Characters],[Ascii Code of Special Characters],DoubleSpace,[Position of Double Space] from #TempStartegyName

select @len=count(*) from #TempStartegyName
IF @len > 0 
	Set @ErrorMsg='Some special characters exist in strategy name'

select @ErrorMsg as ErrorMsg


drop table #TempStartegyName

END TRY
BEGIN CATCH
	drop table #TempStartegyName
END CATCH