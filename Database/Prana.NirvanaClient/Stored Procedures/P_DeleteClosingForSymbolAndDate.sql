

create Procedure [dbo].[P_DeleteClosingForSymbolAndDate]       
(      
 @xml nText      
)       
as       
      
DECLARE @handle int                                                                                                                           
                                                                                                                                              
exec sp_xml_preparedocument @handle OUTPUT, @xml      
      
create table #Temp_Symbols                                                                                                                                              
(                                                                                                                                              
  Symbol varchar(50),      
  FromDate datetime                                                                                                                                       
)                                                 
          
      
insert into #Temp_Symbols                                       
 select            
Symbol,      
FromDate      
FROM   OPENXML(@handle, '/ArrayOfDateSymbol/DateSymbol',2)         
      
WITH                                                                                                                                                  
(                                                                                                                                                      
Symbol varchar(50),      
  FromDate datetime                                                                                                                              
)          
      
          
select PM_taxlots.taxlotclosingID_fk,PM_taxlots.TaxlotID  from PM_taxlots inner join #Temp_Symbols on PM_taxlots.Symbol =#Temp_Symbols.Symbol where  Datediff(d,PM_taxlots.AUECModifiedDate,#Temp_Symbols.FromDate ) <= 0  and PM_taxlots.taxlotclosingID_fk is not
 null  

 exec sp_xml_removedocument @handle
