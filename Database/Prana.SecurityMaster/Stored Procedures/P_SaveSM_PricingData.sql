-----------------------------------------------------------------
--Modified BY: Omshiv
--Date: June 2014
--Purpose: save prices based on secondary pricing sources

-----------------------------------------------------------------


-- [dbo].[P_SaveSM_PricingData]'<PricingTable><PricingData><Symbol_PK>81218182031</Symbol_PK><Source>CUSIP</Source>                    
--	<Date>2014-05-16 00:00:00.000</Date>,<NirvanaSymbol>CS</NirvanaSymbol>,<PrimarySymbology>1</PrimarySymbology>,<Fields><ask>11</ask><last>25</last></Fields>
--   </PricingData><PricingData><Symbol_PK>81218182029</Symbol_PK><Source>Source2</Source><Date>2014-02-10 00:00:00.000</Date>,<NirvanaSymbol>GB</NirvanaSymbol>,    
--	<PrimarySymbology>4</PrimarySymbology>,<Fields> <ask>12</ask><last>16</last></Fields></PricingData></PricingTable>','not saved',-1

CREATE PROCEDURE [dbo].[P_SaveSM_PricingData]                                                              
(                          
  @xml varchar(max),                                        
  @ErrorMessage varchar(500) output,                                                                         
  @ErrorNumber int output              
)                                
As                       
  
SET @ErrorNumber = 0                                                        
SET @ErrorMessage = 'Success'                                      
  
BEGIN TRY                                         
  
  DECLARE @handle int                                     
  exec sp_xml_preparedocument @handle OUTPUT,@xml                             
  
CREATE TABLE #TempPricing                                                                                                  
(                                 
	Symbol_PK bigint,                 
	Source nvarchar(50),                                                
	Date datetime, 
    NirvanaSymbol nvarchar(100),
    PrimarySymbology int,   
	PricingXML xml,
	SecondarySource nvarchar(50)
)                                                                                                                    
  
Insert Into #TempPricing                                                                                                                                                   
(                                                                                                                                                  
	Symbol_PK,                 
	Source,                    
	Date,                            
	NirvanaSymbol,    
	PrimarySymbology,
    PricingXML,
    SecondarySource   
)                                                                                                                       
Select                                                                                                                                                   
	Symbol_PK,                 
	Source,                    
	Date,                            
	NirvanaSymbol,    
	PrimarySymbology,
    Fields,
	SecondarySource                                
FROM OPENXML(@handle, '//PricingTable/PricingData', 2)                                                                                                                                                     
WITH                                                                        
(                                
	Symbol_PK bigint,                 
	Source nvarchar(50),                                                
	Date datetime, 
    NirvanaSymbol nvarchar(100),
    PrimarySymbology int,      
	Fields xml,
	SecondarySource nvarchar(50)
)                         
  
--print('table inserted in temp table')
--select * from #TempPricing
Insert into T_SMPricingData 
(
	Symbol_PK,                 
	Source,                    
	Date,                            
	NirvanaSymbol,    
	PrimarySymbology,
    PricingXML,
	SecondarySource
)                 
Select 
 TP.Symbol_PK,                   
 TP.Source,                      
 TP.Date,                              
 TP.NirvanaSymbol,      
 TP.PrimarySymbology,  
    TP.PricingXML,
   TP.SecondarySource  
 From #TempPricing  TP
    Where TP.Symbol_PK not in (Select TSM.Symbol_PK from T_SMPricingData TSM where TP.Symbol_PK=TSM.Symbol_PK
and TP.Source=TSM.Source and TP.SecondarySource=TSM.SecondarySource and year(TSM.Date)=year(TP.Date) AND month(TSM.Date)=month(TP.Date) 
AND day(TSM.Date)=day(TP.Date))                
               
--print('new row inserted')
  
Update T_SMPricingData              
Set 
NirvanaSymbol = TP.NirvanaSymbol, 
PrimarySymbology = TP.PrimarySymbology,
PricingXML = (select isnull(S.N.query('.'),F.N.query('.')) as '*'
              from T_SMPricingData.PricingXML.nodes('/Fields/*') as F(N)
              full outer join TP.PricingXML.nodes('/Fields/*') as S(N)
              on F.N.value('local-name(.)', 'nvarchar(100)') = S.N.value('local-name(.)', 'nvarchar(100)')
              for xml path(''), root('Fields')
              )
,SecondarySource=   TP.SecondarySource  
From #TempPricing TP
inner JOIN T_SMPricingData on T_SMPricingData.Symbol_PK = TP.Symbol_PK
and  T_SMPricingData.Source = TP.Source
and  T_SMPricingData.SecondarySource = TP.SecondarySource
and year(T_SMPricingData.Date)=year(TP.Date) AND month(T_SMPricingData.Date)=month(TP.Date) 
and day(T_SMPricingData.Date)=day(TP.Date)            
             
--print('row updated') 
 
Drop Table #TempPricing              
exec sp_xml_removedocument @handle
END TRY                                                                                        
BEGIN CATCH                                                 
 SET @ErrorMessage = ERROR_MESSAGE()                                                       
 SET @ErrorNumber = Error_number()                                  
      Drop Table #TempPricing                         
END CATCH     
