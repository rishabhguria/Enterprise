declare @FromDate datetime
declare @ToDate datetime
Declare @errormsg varchar(max)
Declare @Smdb Varchar(max)

set @FromDate=''
set @ToDate=''
set @errormsg=''
set @Smdb=''

DECLARE @IndexInputQuery VARCHAR(MAX)

create table #tempUDA
(	ID INT primary key identity(1,1),
	CountryID INT ,
	CountryName varchar(300) ,
	Location varchar(300),
	Position varchar(300) 
)

create table #temp
(
	CountryID INT ,
	CountryName varchar(300),
	Location varchar(300),
	Position varchar(300) 
)



DECLARE @x INT 
DECLARE @y INT 
DECLARE @strlen INT
DECLARE @len INT
DECLARE @s varchar(max)
DECLARE @st varchar(max)

BEGIN
set @IndexInputQuery = 'insert into #tempUDA (CountryID,CountryName) (select * from ['+@Smdb+'].dbo.T_UDACountry)'
exec (@IndexInputQuery)
END

update #tempUDA
set Position='Sides' WHERE ((RIGHT(CountryName,1)=' ' or LEFT(CountryName,1)=' ') and CountryName<>'')

set @st=''
SET @x=1
select @len=count(*) from #tempUDA
while @x<=@len
BEGIN

select @s=CountryName from #tempUDA where @x = #tempUDA.ID
	SET @y=1
	SET @strlen=Datalength(@s)

	while @y<=@strlen
	BEGIN
		
		-- Finding Double Space in FundShortname and updating in column DoubleSpace
		IF (ASCII(SubString(@s,@y-1,1))<>ASCII(' ') AND ASCII(SubString(@s,@y,1))=ASCII(' ') AND ASCII(SubString(@s,@y+1,1))=ASCII(' ')) OR
			(@y=1 AND ASCII(SubString(@s,@y,1))=ASCII(' ') AND ASCII(SubString(@s,@y+1,1))=ASCII(' '))
			
		   
		   set @st = @st+','+LTRIM(str(@y))
		   --set Location = LTRIM(str(@y))+'  '+str(Location) where @x = #tempUDA.ID

		SET @y=@y+1 
	END

	update #tempUDA
	set Location = substring(@st,2,Datalength(@s)) where @x = #tempUDA.ID
	update #tempUDA
	set Position = 'Middle' where Location<>''
	set @st=''
	SET @x = @x+1

END

insert into #temp select CountryID,CountryName,Location,Position from #tempUDA where Position<>''


IF EXISTS (select * from #temp)
BEGIN
	SET @errormsg = 'Unwanted Whitespace present in T_UDACountry'
	select * from #temp
END

SELECT @errormsg AS errormsg

DROP TABLE #temp, #tempUDA
