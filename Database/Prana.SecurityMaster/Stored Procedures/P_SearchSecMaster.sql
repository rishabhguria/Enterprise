CREATE PROCEDURE [dbo].[P_SearchSecMaster]                                      
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
declare  @SedolSymbol varchar(100)                       
declare @cusipPSymbol varchar(100)                    
declare @reutersSymbol varchar(100)                    
declare @osiOptionSymbol varchar(100)           
declare @idcoOptionSymbol varchar(100)           
declare @opraOptionSymbol varchar(100)           
declare @startIndex int                  
declare @endIndex int                  
                  
Select                                   
@tickerSymbol=TickerSymbol,                                  
@companyName=[Name],                   
@bloombergSymbol=BloombergSymbol,                    
@isinSymbol=ISINSymbol,                    
@SedolSymbol=SedolSymbol,                    
@cusipPSymbol=CusipSymbol,                  
@reutersSymbol=ReutersSymbol,          
@osiOptionSymbol = OSIOptionSymbol,          
@idcoOptionSymbol = IDCOOptionSymbol,          
@opraOptionSymbol = OPRAOptionSymbol,          
@startIndex  =StartIndex,                  
@endIndex=   EndIndex                    
FROM  OPENXML(@handle, '//SymbolLookupRequestObject',2)                                                                                                                                                      
WITH                                    
(                                  
TickerSymbol varchar(100),                                  
[Name]varchar(100),                  
BloombergSymbol varchar(100),                   
ISINSymbol varchar(100),                    
SEDOLSymbol varchar(100),                    
CUSIPSymbol varchar(100),            
OSIOptionSymbol varchar(100),           
IDCOOptionSymbol varchar(100),           
OPRAOptionSymbol varchar(100),           
ReutersSymbol varchar(100),                  
StartIndex int ,                  
EndIndex int                           
)                                
        
         
                  
if( @companyName is not null)    -- Search By Company Name              
                  
begin                   
                    
     select AssetID,UnderLyingID,ExchangeID ,CurrencyID,TickerSymbol,UnderLyingSymbol,ReutersSymbol,                                      
     ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol,OSISymbol, IDCOSymbol, OPRASymbol,          
  LongName,Delta,Sector,                                
     Symbol_PK,OPTMultiplier,[Type],StrikePrice,OptionName,                                        
     FUTMultiplier,FutureName,AUECID,OPTExpiration,FUTExpiration,LeadCurrencyID,VsCurrencyID,FxContractName,            
              FXForwardMultiplier,IndexLongName, Multiplier from                 
                     
       (                  
     select ROW_NUMBER() OVER (ORDER BY SM.Symbol_PK)AS Row,SM.AssetID,SM.UnderLyingID,T_SMReuters.ExchangeID ,SM.CurrencyID,SM.TickerSymbol,SM.UnderLyingSymbol,T_SMReuters.ReutersSymbol,                                      
     ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol,OSISymbol, IDCOSymbol, OPRASymbol,          
     ENHD.CompanyName as LongName,Delta,Sector,                                
     SM.Symbol_PK,OPT.Multiplier as OPTMultiplier,OPT.[Type],OPT.Strike as StrikePrice,OPT.ContractName as OptionName,                                        
     FUT.Multiplier as FUTMultiplier,FUT.ContractName as FutureName,SM.AUECID,OPT.ExpirationDate as OPTExpiration,            
              IsNull(FUT.ExpirationDate,FxForwardData.ExpirationDate) as FUTExpiration,IsNull(FxData.LeadCurrencyID,FxForwardData.LeadCurrencyID) as LeadCurrencyID,            
        IsNull(FxData.VsCurrencyID,FxForwardData.VsCurrencyID) as VsCurrencyID,            
             IsNull(FxData.LongName,FxForwardData.LongName) as FxContractName,FxForwardData.Multiplier as FXForwardMultiplier,IndexData.LongName as   IndexLongName, ENHD.Multiplier   
                                    from T_SMSymbolLookUpTable as SM                                     
     join T_SMReuters  on SM.Symbol_PK=T_SMReuters.Symbol_PK                                 
     left outer join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK                                      
     left outer  join T_SMoptionData as OPT on  SM.Symbol_PK=OPT.Symbol_PK                                  
     left outer  join T_SMFutureData as FUT on  SM.Symbol_PK=FUT.Symbol_PK                                     
     left outer  join T_SMFxData as FxData on  SM.Symbol_PK=FxData.Symbol_PK                  
    left outer  join T_SMFxForwardData as FxForwardData on  SM.Symbol_PK=FxForwardData.Symbol_PK               
    left outer  join T_SMIndexData as IndexData on  SM.Symbol_PK=IndexData.Symbol_PK               
                  
     where  ISPrimaryExchange ='true'                      
                                       
      and                                 
      (ENHD.CompanyName like  isnull(@companyName ,ENHD.CompanyName)                                
      or  OPT.ContractName like  isnull(@companyName ,OPT.ContractName)                                 
     or  FUT.ContractName like  isnull(@companyName ,FUT.ContractName)                     
   or  FxData.LongName like  isnull(@companyName ,FxData.LongName)                 
   or  FxForwardData.LongName like  isnull(@companyName ,FxForwardData.LongName)                      
         or  IndexData.LongName like  isnull(@companyName ,IndexData.LongName))             
   )                  
    as TempSM                   
   where Row >= @startIndex AND Row <= @endIndex                  
                  
end                  
                  
else      -- Search By Symbol             
begin                  
                  
      if(@reutersSymbol is null )     -- Search By Non Reuters  Symbol             
                     
     begin                   
                      
                     
   select AssetID,UnderLyingID,ExchangeID ,CurrencyID,TickerSymbol,UnderLyingSymbol,ReutersSymbol,                                      
     ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol, OSISymbol, IDCOSymbol, OPRASymbol,          
  LongName,Delta,Sector,                                
     Symbol_PK,OPTMultiplier,[Type],StrikePrice,OptionName,                                        
     FUTMultiplier,FutureName,AUECID,OPTExpiration,FUTExpiration,LeadCurrencyID,VsCurrencyID,FxContractName,
     FXForwardMultiplier,IndexLongName, Multiplier      
    from (                
      select ROW_NUMBER() OVER (ORDER BY SM.Symbol_PK)AS Row,SM.AssetID,SM.UnderLyingID,T_SMReuters.ExchangeID ,SM.CurrencyID,SM.TickerSymbol,SM.UnderLyingSymbol,T_SMReuters.ReutersSymbol,                                      
      ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol,OSISymbol, IDCOSymbol, OPRASymbol,          
   ENHD.CompanyName as LongName,Delta,Sector,                                
      SM.Symbol_PK,OPT.Multiplier as OPTMultiplier,OPT.[Type],OPT.Strike as StrikePrice,OPT.ContractName as OptionName,                                        
     FUT.Multiplier as FUTMultiplier,FUT.ContractName as FutureName,SM.AUECID,OPT.ExpirationDate as OPTExpiration,            
             IsNull(FUT.ExpirationDate,FxForwardData.ExpirationDate) as FUTExpiration,IsNull(FxData.LeadCurrencyID,FxForwardData.LeadCurrencyID) as LeadCurrencyID,            
             IsNull(FxData.VsCurrencyID,FxForwardData.VsCurrencyID) as VsCurrencyID,IsNull(FxData.LongName,FxForwardData.LongName) as FxContractName ,            
             FxForwardData.Multiplier as FXForwardMultiplier ,IndexData.LongName as   IndexLongName,  ENHD.Multiplier                            
                                      
      from T_SMSymbolLookUpTable as SM                                     
      join T_SMReuters  on SM.Symbol_PK=T_SMReuters.Symbol_PK                                 
      left outer join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK                                      
      left outer  join T_SMoptionData as OPT on  SM.Symbol_PK=OPT.Symbol_PK                                  
      left outer  join T_SMFutureData as FUT on  SM.Symbol_PK=FUT.Symbol_PK                                     
      left outer  join T_SMFxData as FxData on  SM.Symbol_PK=FxData.Symbol_PK          
      left outer  join T_SMFxForwardData as FxForwardData on  SM.Symbol_PK=FxForwardData.Symbol_PK            
      left outer  join T_SMIndexData as IndexData on  SM.Symbol_PK=IndexData.Symbol_PK                    
            
      where  ISPrimaryExchange ='true'                              
    and                                 
    (ENHD.CompanyName like  isnull(@companyName ,ENHD.CompanyName)                                
    or  OPT.ContractName like  isnull(@companyName ,OPT.ContractName)                                 
      or  FUT.ContractName like  isnull(@companyName ,FUT.ContractName)                     
      or  FxData.LongName like  isnull(@companyName ,FxData.LongName)                 
      or  FxForwardData.LongName like  isnull(@companyName ,FxForwardData.LongName)             
               or  IndexData.LongName like  isnull(@companyName ,IndexData.LongName)             
               )                     
                     
      and  SM.TickerSymbol like  isnull(@tickerSymbol,SM.TickerSymbol)                                      
      and  SM.BloombergSymbol like  isnull(@bloombergSymbol,SM.BloombergSymbol)                      
      and  SM.ISINSymbol like  isnull(@isinSymbol,SM.ISINSymbol)                      
      and  SM.SedolSymbol like  isnull(@SedolSymbol,SM.SedolSymbol)                      
      and  SM.CusipSymbol like  isnull(@cusipPSymbol,SM.CusipSymbol)         
   and  SM.OSISymbol like  isnull(@osiOptionSymbol,SM.OSISymbol)     
   and  SM.IDCOSymbol like  isnull(@idcoOptionSymbol,SM.IDCOSymbol)     
   and  SM.OPRASymbol like  isnull(@opraOptionSymbol,SM.OPRASymbol)     
    
   )                   
   as TempSM                   
   where Row >= @startIndex AND Row <= @endIndex                  
     end                  
       else      -- Search by Reuteres Symbol            
                  
              begin                  
                                
            select AssetID,UnderLyingID,ExchangeID ,CurrencyID,TickerSymbol,UnderLyingSymbol,ReutersSymbol,                                      
              ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol,OSISymbol, IDCOSymbol, OPRASymbol,          
    LongName,Delta,Sector,                                
              Symbol_PK,OPTMultiplier,[Type],StrikePrice,OptionName,                                        
              FUTMultiplier,FutureName,AUECID,OPTExpiration,FUTExpiration,LeadCurrencyID,VsCurrencyID,FxContractName,            
             FXForwardMultiplier,IndexLongName, Multiplier from                
             (                  
                 select ROW_NUMBER() OVER (ORDER BY SM.Symbol_PK)AS Row,SM.AssetID,SM.UnderLyingID,T_SMReuters.ExchangeID ,SM.CurrencyID,SM.TickerSymbol,SM.UnderLyingSymbol,T_SMReuters.ReutersSymbol,                                      
               ISINSymbol,SedolSymbol,CusipSymbol,BloombergSymbol, OSISymbol, IDCOSymbol, OPRASymbol,          
     ENHD.CompanyName as LongName,Delta,Sector,                                
               SM.Symbol_PK,OPT.Multiplier as OPTMultiplier,OPT.[Type],OPT.Strike as StrikePrice,OPT.ContractName as OptionName,                                        
              FUT.Multiplier as FUTMultiplier,FUT.ContractName as FutureName,SM.AUECID,OPT.ExpirationDate as OPTExpiration,            
             IsNull(FUT.ExpirationDate,FxForwardData.ExpirationDate) as FUTExpiration,IsNull(FxData.LeadCurrencyID,FxForwardData.LeadCurrencyID) as LeadCurrencyID,            
             IsNull(FxData.VsCurrencyID,FxForwardData.VsCurrencyID) as VsCurrencyID,IsNull(FxData.LongName,FxForwardData.LongName) as FxContractName ,            
             FxForwardData.Multiplier as FXForwardMultiplier  ,IndexData.LongName as   IndexLongName ,  ENHD.Multiplier                         
                                               
               from T_SMSymbolLookUpTable as SM                                     
               join T_SMReuters  on SM.Symbol_PK=T_SMReuters.Symbol_PK                                 
               left outer join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK                                      
               left outer  join T_SMoptionData as OPT on  SM.Symbol_PK=OPT.Symbol_PK                                  
               left outer  join T_SMFutureData as FUT on  SM.Symbol_PK=FUT.Symbol_PK                                     
               left outer  join T_SMFxData as FxData on  SM.Symbol_PK=FxData.Symbol_PK                  
            left outer  join T_SMFxForwardData as FxForwardData on  SM.Symbol_PK=FxForwardData.Symbol_PK              
            left outer  join T_SMIndexData as IndexData on  SM.Symbol_PK=IndexData.Symbol_PK                 
               where                                  
             (ENHD.CompanyName like  isnull(@companyName ,ENHD.CompanyName)                                
             or  OPT.ContractName like  isnull(@companyName ,OPT.ContractName)                                 
               or  FUT.ContractName like  isnull(@companyName ,FUT.ContractName)                     
               or  FxData.LongName like  isnull(@companyName ,FxData.LongName)                
            or  FxForwardData.LongName like  isnull(@companyName ,FxForwardData.LongName)             
            or  IndexData.LongName like  isnull(@companyName ,IndexData.LongName)             
             )                      
                 and  T_SMReuters.ReutersSymbol like  isnull(@reutersSymbol,T_SMReuters.ReutersSymbol)                     
                              
            )                  
            as TempSM             
            where Row >= @startIndex AND Row <= @endIndex                  
   end                  
end                   
                              
EXEC sp_xml_removedocument @handle 
