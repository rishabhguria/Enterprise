     
          
/**********************************************                  
Author : Rahul Gupta              
Creation date: Nov 12 , 2012              
Description : Fetch Data for Middleware TSR Report within two date range                      
                
Modified By : Nikita Wadhwa    
Description : Added Trade date and process Date check     
    
Execution Method:                  
P_MW_GetTSR_Symbol '2012-1-1','2013-8-31' ,'TradeDate' ,'' ,'underlyingSymbol'            
             
      
**********************************************/                  
         
                 
CREATE PROCEDURE [dbo].[P_MW_GetTSR_Symbol]                  
(                  
@StartDate datetime,                  
@EndDate datetime   ,          
@Method Varchar(100) ,    
@IncludeCommission bit ,     
@SearchString Varchar(100) ,    
@SearchBy Varchar(100)    
)                  
AS     
    
    
--Declare @StartDate datetime                  
--Declare @EndDate datetime             
--Declare @Method Varchar(100)     
--Declare @SearchString Varchar(100)              
--    
--Set @StartDate = '2012-1-1'    
--Set @EndDate = '2013-8-31'    
--Set @Method = 'TradeDate'    
--Set @SearchString = 'VR'    
    
                 
BEGIN               
            
Select              
 Symbol ,                                          
 UnderlyingSymbol,                                          
 Strategy,                                          
 Fund,       
 MasterFund,                                         
 Asset,                                          
 Underlyer,                                          
 Exchange,                                            
 UDASector ,                                          
 UDACountry ,                                          
 UDASecurityType ,                                          
 UDAAssetClass ,                                          
 UDASubSector ,                                          
 TradeCurrency ,                                          
 Side ,                       
 CounterParty ,                       
 PrimeBroker ,                      
 Trader ,                                  
 SecurityName ,                       
 TradeDate ,                          
 SettleDate as SettlementDate ,                      
 Quantity ,                  
 Multiplier,                   
 SideMultiplier,                      
 AvgPrice ,                    
 PutOrCall,                      
    
 CASE     
 When (@IncludeCommission = 1)               
 Then CommissionLocal      
    Else    
 0      
  END as CommissionLocal,    
      
 CASE     
 When (@IncludeCommission = 1)               
 Then CommissionBase      
    Else    
 0             
 END as CommissionBase,    
    
 CASE     
 When (@IncludeCommission = 1)               
 Then FeesLocal      
    Else    
 0             
END as FeesLocal,    
    
 CASE     
 When (@IncludeCommission = 1)               
 Then FeesBase      
    Else    
 0             
END as FeesBase,    
    
 CASE     
 When (@IncludeCommission = 1)               
 Then OtherFeesLocal      
    Else    
 0             
 END as OtherFeesLocal,    
    
 CASE     
 When (@IncludeCommission = 1)               
 Then OtherFeesBase      
    Else    
 0             
 END as OtherFeesBase,    
    
 FXRate_TradeDate as OpeningFXRate ,                      
 MarkFXRate_TradeDate as TradeDateFXRate ,                       
 MarkFXRate_SettleDate as SettlementDateFXRate,                      
    
 CASE     
    When (@IncludeCommission = 1)               
 Then NetAmountBase      
    Else    
 PrincipalAmountBase    
 END as NetAmountBase,      
    
 CASE     
 When (@IncludeCommission = 1)               
 Then PrincipalAmountLocal      
    Else    
 PrincipalAmountBase    
 END as NetAmountLocal,      
             
 PrincipalAmountBase ,                      
 PrincipalAmountLocal ,                      
 TradeOrigin ,                      
 Open_CloseTag ,                      
 DividendLocal,                  
 Dividend as DividendBase,                  
 BloomBergSymbol,                  
 SedolSymbol,                  
 OSISymbol,                  
 IDCOSymbol,                  
 ISINSymbol    ,          
 SecurityName as Descriptions ,            
 ProcessDate ,    
 TradeAttribute1,    
 TradeAttribute2,    
 TradeAttribute3,    
 TradeAttribute4,    
 TradeAttribute5,    
 TradeAttribute6       
into #Transaction    
             
from               
T_MW_Transactions            
                 
where @StartDate <=         
Case         
when (@method = 'Tradedate')        
then Tradedate         
Else        
ProcessDate        
End        
        
and         
        
@EndDate >=         
Case         
when (@method = 'Tradedate')        
then Tradedate         
Else        
ProcessDate        
End        
        
and (Open_closeTag = 'O' or Open_closeTag = 'C')        
    
declare @text Varchar(1000)    
set @text = 'Select * from #Transaction Where '+ @SearchBy  +' like ''%' + @SearchString +  '%'''    
--Select @text    
--Print @text    
    
    
If(@SearchString <> '' or @SearchString is null)              
 Begin              
  --Exec  ('Select * from #Transaction Where Symbol in (' + '''' + @SearchString +  '''' + ')' )               
Exec  (@text)               
 End              
Else              
 Begin              
  Select * from #Transaction Order By Symbol        
 End    
    
Drop table #transaction    
           
END 