CREATE  PROCEDURE [dbo].[P_GetUDASymbolsData_XML]                                                                                                                          
(                                                                                                                          
 @Xml nvarchar(max)                                                                                                    
)                                                                                                                          
As          
                                       
DECLARE @handle int                                                                                                       
exec sp_xml_preparedocument @handle OUTPUT,@Xml                                                                      
                                                                                                                    
Create TABLE #XmlItem                                                                                                                    
(                                                                                                                    
                                                                                                   
TickerSymbol varchar(50),
UnderlyingSymbol varchar(50)                                                    
                                               
)                                                                                   
                                                         
INSERT INTO #XmlItem                                                    
(                                                                                                                    
TickerSymbol,                                                                                                  
  UnderlyingSymbol                                                                                                  
)                                                     
Select distinct                                                                                                                   
  TickerSymbol,UnderlyingSymbol                                           
FROM  OPENXML(@handle, '//',3)                                                                                              
 WITH                                                 
(                                                                             
TickerSymbol varchar(50), 
UnderlyingSymbol varchar(50)                                                                    
)                                                                                        
   

    SELECT DISTINCT 
	SecData.TickerSymbol,   
	SecData.UDAAssetClassID,   
	SecData.UDASecurityTypeID,    
	SecData.UDASectorID,   
	SecData.UDASubSectorID,  
	SecData.UDACountryID,
	ENHD.CompanyName, 
    SecData.UnderLyingSymbol 
   
  
FROM  T_SMSymbolLookUpTable as SecData 
inner JOIN  #XmlItem  on SecData.TickerSymbol = #XmlItem.TickerSymbol   or   SecData.TickerSymbol = #XmlItem.UnderlyingSymbol                                    
left outer join T_SMEquityNonHistoryData as ENHD on SecData.Symbol_PK=ENHD.Symbol_PK                                                                                   

Drop table #XmlItem                     
                              
EXEC sp_xml_removedocument @handle                                                                          
                                   
                                                          
 