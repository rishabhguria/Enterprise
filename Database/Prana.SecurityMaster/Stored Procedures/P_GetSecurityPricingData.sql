--DECLARE @xml xml
--SET @xml='<Rowset><Row><Symbol_PK>81218182029</Symbol_PK> 
--    	 <Date>20140515</Date> 
--    	 <Source>sfsd</Source> 
--  	</Row> 
--  	<Row> 
--    	 <Symbol_PK>81218182031</Symbol_PK> 
--    	 <Date>20140515</Date> 
--    	 <Source>abc</Source> 
--  	</Row></Rowset>'
-- EXEC P_GetSecurityPricingData @xml,'Field1,Field2'
--http://stackoverflow.com/questions/23675356/pivot-and-merge-columns-in-a-sql-server-stored-procedure
--http://stackoverflow.com/questions/23684316/using-a-variable-as-xquery-sql-server-2005
--http://stackoverflow.com/questions/23681510/fetch-queried-xml-nodes-from-a-column-in-sql-server-2005
CREATE PROCEDURE [dbo].[P_GetSecurityPricingData]                                                                
(                            
  @xml xml,                
  @fields nvarchar(max)                             
)                                  
As                         
DECLARE @handle int 
exec sp_xml_preparedocument @handle OUTPUT,@Xml
IF OBJECT_ID('tempdb..#tempPricing') IS NOT NULL begin DROP TABLE #tempPricing end
CREATE TABLE #TempPricing
(
 Symbol_PK bigint,
 Source nvarchar(50),
 Date datetime,
 SecondarySource nvarchar(50)
 )
INSERT INTO #TempPricing                     
 (                                                                              
    Symbol_PK,
    Source,
    Date,
    SecondarySource                                                 
 )                                                                              
SELECT                                                                               
    Symbol_PK,
    Source,
    Date ,
    SecondarySource                             
    FROM OPENXML(@handle, '/Rowset/Row', 2)                                                                                 
 WITH                                                                               
 (                                                         
    Symbol_PK bigint,
    Source varchar(50),
    Date     DATETIME,
	SecondarySource nvarchar(50)  
 )   

--select * from #TempPricing 

declare @SecondarySource nvarchar(50) 
select @SecondarySource = SecondarySource from #TempPricing 



IF(@SecondarySource is null or @SecondarySource ='')
begin

select T_SMPricingData.Symbol_PK,T_SMPricingData.Source,CONVERT(varchar, T_SMPricingData.Date, 121) as Date,  
(select F.X.query('.') from T_SMPricingData.PricingXML.nodes('/Fields/*') as F(X) inner join 
dbo.Split(@fields,',') as N ON F.X.value('local-name(.)','nvarchar(200)')=N.Items FOR XML PATH(''),ROOT('Fields'),TYPE) as PricingXML,T_SMPricingData.SecondarySource 
FROM T_SMPricingData 
inner JOIN #TempPricing on T_SMPricingData.Symbol_PK = #TempPricing.Symbol_PK
and  T_SMPricingData.Source = #TempPricing.Source
and  (T_SMPricingData.SecondarySource = '' or T_SMPricingData.SecondarySource is NULL)
and year(T_SMPricingData.Date)=year(#TempPricing.Date) AND month(T_SMPricingData.Date)=month(#TempPricing.Date) and day(T_SMPricingData.Date)=day(#TempPricing.Date)
end 

else

begin
select T_SMPricingData.Symbol_PK,T_SMPricingData.Source,CONVERT(varchar, T_SMPricingData.Date, 121)as Date,  
(select F.X.query('.') from T_SMPricingData.PricingXML.nodes('/Fields/*') as F(X) inner join 
dbo.Split(@fields,',') as N ON F.X.value('local-name(.)','nvarchar(200)')=N.Items FOR XML PATH(''),ROOT('Fields'),TYPE) as PricingXML,T_SMPricingData.SecondarySource 
FROM T_SMPricingData 
inner JOIN #TempPricing on T_SMPricingData.Symbol_PK = #TempPricing.Symbol_PK
and  T_SMPricingData.Source = #TempPricing.Source
and    T_SMPricingData.SecondarySource = #TempPricing.SecondarySource
and year(T_SMPricingData.Date)=year(#TempPricing.Date) AND month(T_SMPricingData.Date)=month(#TempPricing.Date) and day(T_SMPricingData.Date)=day(#TempPricing.Date)

exec sp_xml_removedocument @handle
end

