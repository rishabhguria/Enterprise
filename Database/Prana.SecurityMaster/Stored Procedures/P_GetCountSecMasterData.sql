 --P_GetCountSecMasterData  '<SymbolLookupRequestObject><TickerSymbol>%Goog%</TickerSymbol></SymbolLookupRequestObject>'    
CREATE PROCEDURE [dbo].[P_GetCountSecMasterData]                        
(                        
@Xml varchar(max)                      
)                     
 as                                                             
DECLARE @handle int                                           
exec sp_xml_preparedocument @handle OUTPUT,@Xml              
declare @tickerSymbol varchar(100)           
declare @companyName varchar(100)       
declare @bloombergSymbol varchar(100)        
declare  @isinSymbol varchar(100)        
declare  @sedolSymbol varchar(100)         
declare @cusipPSymbol varchar(100)      
declare @reutersSymbol varchar(100)      
    
    
Select                     
@tickerSymbol=TickerSymbol,                    
@companyName=[Name],     
@bloombergSymbol=BloombergSymbol,      
@isinSymbol=ISINSymbol,      
@sedolSymbol=SEDOLSymbol,      
@cusipPSymbol=CUSIPSymbol,    
@reutersSymbol=ReutersSymbol    
    
FROM  OPENXML(@handle, '//SymbolLookupRequestObject',2)                                                                                                                                        
WITH                      
(                    
TickerSymbol varchar(100),                    
[Name]varchar(100),    
BloombergSymbol varchar(100),     
ISINSymbol varchar(100),      
SEDOLSymbol varchar(100),      
CUSIPSymbol varchar(100),    
ReutersSymbol varchar(100)    
           
)                   
    
    
if( @companyName is not null)    
    
begin     
        select count(*)     
  from T_SMSymbolLookUpTable as SM                       
  join T_SMReuters  on SM.Symbol_PK=T_SMReuters.Symbol_PK                   
  left outer join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK                        
  left outer  join T_SMoptionData as OPT on  SM.Symbol_PK=OPT.Symbol_PK                    
  left outer  join T_SMFutureData as FUT on  SM.Symbol_PK=FUT.Symbol_PK                       
  left outer  join T_SMFxData as FxData on  SM.Symbol_PK=FxData.Symbol_PK    
  left outer  join T_SMFXForwardData as FxForwardData on  SM.Symbol_PK=FxForwardData.Symbol_PK    
     where  ISPrimaryExchange ='true'                       
                      
   and                   
   (ENHD.CompanyName like  isnull(@companyName ,ENHD.CompanyName)                  
   or  OPT.ContractName like  isnull(@companyName ,OPT.ContractName)                   
  or  FUT.ContractName like  isnull(@companyName ,FUT.ContractName)       
or  FxData.LongName like  isnull(@companyName ,FxData.LongName)       
or  FxForwardData.LongName like  isnull(@companyName ,FxForwardData.LongName) )       
    
    
    
end    
    
else    
begin    
    
      if(@reutersSymbol is null )    
    
  begin     
  select count(*) from T_SMSymbolLookUpTable as SM                       
   join T_SMReuters  on SM.Symbol_PK=T_SMReuters.Symbol_PK                   
   left outer join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK                        
   left outer  join T_SMoptionData as OPT on  SM.Symbol_PK=OPT.Symbol_PK                    
   left outer  join T_SMFutureData as FUT on  SM.Symbol_PK=FUT.Symbol_PK                       
   left outer  join T_SMFxData as FxData on  SM.Symbol_PK=FxData.Symbol_PK    
   left outer  join T_SMFXForwardData as FxForwardData on  SM.Symbol_PK=FxForwardData.Symbol_PK    
      where  ISPrimaryExchange ='true'                
                       
    and                   
    (ENHD.CompanyName like  isnull(@companyName ,ENHD.CompanyName)                  
    or  OPT.ContractName like  isnull(@companyName ,OPT.ContractName)                   
   or  FUT.ContractName like  isnull(@companyName ,FUT.ContractName)       
   or  FxData.LongName like  isnull(@companyName ,FxData.LongName)        
   or  FxForwardData.LongName like  isnull(@companyName ,FxForwardData.LongName) )       
    
   and  SM.TickerSymbol like  isnull(@tickerSymbol,SM.TickerSymbol)                        
   and  SM.BloombergSymbol like  isnull(@bloombergSymbol,SM.BloombergSymbol)        
   and  SM.ISINSymbol like  isnull(@isinSymbol,SM.ISINSymbol)        
   and  SM.SEDOLSymbol like  isnull(@sedolSymbol,SM.SEDOLSymbol)        
   and  SM.CUSIPSymbol like  isnull(@cusipPSymbol,SM.CUSIPSymbol)      
    
    
  end    
else    
    
  begin    
    
      
 select count(*) from T_SMSymbolLookUpTable as SM                       
   join T_SMReuters  on SM.Symbol_PK=T_SMReuters.Symbol_PK                   
   left outer join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK                        
   left outer  join T_SMoptionData as OPT on  SM.Symbol_PK=OPT.Symbol_PK                    
   left outer  join T_SMFutureData as FUT on  SM.Symbol_PK=FUT.Symbol_PK                       
   left outer  join T_SMFxData as FxData on  SM.Symbol_PK=FxData.Symbol_PK    
   left outer  join T_SMFXForwardData as FxForwardData on  SM.Symbol_PK=FxForwardData.Symbol_PK    
      where                    
    (ENHD.CompanyName like  isnull(@companyName ,ENHD.CompanyName)                  
    or  OPT.ContractName like  isnull(@companyName ,OPT.ContractName)                   
   or  FUT.ContractName like  isnull(@companyName ,FUT.ContractName)       
   or  FxData.LongName like  isnull(@companyName ,FxData.LongName)        
   or  FxForwardData.LongName like  isnull(@companyName ,FxForwardData.LongName))  
           and  T_SMReuters.ReutersSymbol like  isnull(@reutersSymbol,T_SMReuters.ReutersSymbol)       
    
    
   end    
end     
                
EXEC sp_xml_removedocument @handle 