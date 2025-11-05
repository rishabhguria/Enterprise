Create procedure P_SavePranaKeyValuePreferences  
(@xmlDoc nText)   
as  
declare @handle int  
exec sp_xml_preparedocument @handle output, @xmlDoc   
create table #temp  
(  
preferenceKey varchar(50),  
preferencevalue sql_variant,  
)  
insert into #temp  
(preferenceKey, preferencevalue)  
SELECT PreferenceKey, PreferenceValue   
from openxml(@handle,'dsPranaPref/dtPranaPref',2)   
with  
(  
PreferenceKey VARCHAR(50),   
PreferenceValue SQL_VARIANT
)  
UPDATE  
T_PranaKeyValuePreferences  
SET T_PranaKeyValuePreferences.PreferenceValue=#temp.preferencevalue  
FROM #temp  
where T_PranaKeyValuePreferences.PreferenceKey=#temp.preferenceKey  
exec sp_xml_removedocument @handle
