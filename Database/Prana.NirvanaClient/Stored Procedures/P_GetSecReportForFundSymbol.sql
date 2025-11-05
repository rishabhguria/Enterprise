-- =============================================    
-- Created by: Bharat raturi  
-- Date: 01 may 2014  
-- Purpose: get  report details from DB   
-- Usage: P_GetSecReportForFundSymbol  
-- =============================================  
--Exec P_GetSecReportForFundSymbol '',2
CREATE procedure [dbo].[P_GetSecReportForFundSymbol]  
@xmlFund nvarchar(max),  
@reportID int  
as  

declare @whereClause varchar(max)  
select @whereClause= WhereClause from T_ReportTemplate where ReportID=@reportID   
--print @whereClause
declare @columnSet varchar(max)  
declare @selectData varchar(max)  
select @columnSet= [Columns] from T_ReportTemplate where ReportID=@reportID 

if @xmlFund is not null and @xmlFund <> ''
BEGIN
if @whereClause is not null and @whereClause <>''  
set @whereClause = ' and ' + @whereClause  
else  
set @whereClause=''  
declare @handle int  
exec sp_xml_preparedocument @handle output, @xmlFund  

create table #tempFund  
(fundID int)  
insert INTO #tempFund(fundID)  
SELECT  
FundID  
from openXML(@handle,'dsFund/dtFund',2)  
with (FundID int)  
--select * from #tempFund  
create table #tempSymbol  
(  
FundSymbol varchar(100)  
)  
insert INTO #tempSymbol  
select DISTINCT Symbol from PM_Taxlots where FundID in (SELECT FundID from #tempFund)  
--select @groupingcolumns= GroupingBy from T_ReportTemplate where ReportID= @reportID   
set @selectData='select '+@columnSet+' from V_SecMasterData inner join #tempSymbol'+   
' on V_SecMasterData.TickerSymbol=#tempSymbol.FundSymbol' + @whereClause -- group BY ' + @groupingcolumns  
END
ELSE
BEGIN
if @whereClause is not null and @whereClause <>''  
set @whereClause = ' where ' + @whereClause  
else  
set @whereClause=''  
set @selectData='select '+@columnSet+' from V_SecMasterData ' + @whereClause
END
--print @selectData  
exec (@selectData)  

exec sp_xml_removedocument @handle
