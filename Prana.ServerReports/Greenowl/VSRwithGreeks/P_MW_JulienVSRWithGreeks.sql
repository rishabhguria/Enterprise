 /*************************************************                              
Author : Sandeep Singh                                
Creation Date : 12 February , 2015                                
Description : Script for Performance part of Portfolio Summary Middleware Report                
                 
Execution Statement:                             
  P_MW_JulienVSRWithGreeks '2015-01-17' ,'GSEC-AFPK1209,GSEC-AFPK1409,GSEC-AFPK5519',0                
*************************************************/                 
                
CREATE Procedure [dbo].[P_MW_JulienVSRWithGreeks]                
(                
@EndDate datetime,                
@Fund Varchar(max),                
@paramNAVbyMWorPM Int               
)                
As      
    
--Declare @EndDate datetime                
--Declare @Fund Varchar(max)                
--Declare @paramNAVbyMWorPM Int             
--    
--    
--Set @EndDate = '03-03-2015'    
--Set @Fund = 'GSEC-AFPK1209,GSEC-AFPK1409,GSEC-AFPK5519'    
--Set @paramNAVbyMWorPM = 0    
          
Select * Into #Funds                                                            
from dbo.Split(@Fund, ',')            
          
Declare @GlobalNAV Float              
If (@paramNAVbyMWorPM = 1)                      
 BEGIN                      
   Select @GlobalNAV = SUM(ISNULL(EndingMarketValueBase,0))           
 From T_MW_GenericPNL                   
 Inner Join  #Funds On #Funds.Items = T_MW_GenericPNL.Fund          
   Where Open_CLoseTag = 'O' And DATEDIFF(d,Rundate,@EndDate)=0                    
   Group By Rundate                      
 END                      
Else                      
 Begin                    
    Select @GlobalNAV = Sum(ISNULL(NAV.NAVValue,0)) From PM_NAVValue NAV                        
 Inner Join T_CompanyFunds CF on CF.CompanyFundID=NAV.FundID            
    Inner Join  #Funds On #Funds.Items = CF.FundName          
   Where datediff(d,@EndDate,Date)= 0                     
   Group By Date                       
 End          
        
-- if NAV is null then set to zero (0)        
If(@GlobalNAV Is Null)        
Begin         
Set @GlobalNAV = 0         
ENd        
              
Select                 
Fund,                
T_MW_GenericPNL.Symbol,                     
Asset,                 
UnderlyingSymbol,                 
BloombergSymbol,                
UDASector,                
Case                 
 When Asset like  '%option%'                 
 Then Securityname + ', ' + convert(varchar(20), Expirationdate, 101)                    
 Else SecurityName            
End As Securityname,                 
Multiplier,                
UnderlyingSymbolPrice,                
EndingPriceLocal As MarkPrice,                 
Delta,                
Gamma,                 
10 * Gamma * beginningquantity * sidemultiplier as [GammaPosition],                
0.01 * Gamma * underlyingsymbolprice * underlyingsymbolprice * beginningQuantity * sidemultiplier*Multiplier as [Dollar Gamma],                 
Rho,                
rho * beginningQuantity * sidemultiplier * multiplier as [Dollar Rho],                
DeltaExposureBase as AdjExposure,                
EndingMarketValueBase,            
Case            
 When Asset Like  '%option%'            
 Then 0            
 Else BeginningQuantity * SideMultiplier            
End As Quantity,            
            
BeginningQuantity * SideMultiplier * Multiplier * Delta * EndingFXRate As DeltaAdjPosition,            
DeltaExposureBase as [DollarDelta], ----Delta * BeginningQuantity * SideMultiplier * Multiplier * UnderlyingSymbolPrice as [DollarDelta],             
-- $Delta / NAV          
Case                  
 When @GlobalNAV <> 0                  
 Then (DeltaExposureBase / @GlobalNAV) * 100              
 Else 0                  
End As [DollarDeltaPercentage],           
----Beta,             
IsNull(PMDB.Beta,0) As Beta,      
BeginningQuantity * SideMultiplier * Multiplier * T_MW_GenericPNL.Beta * EndingFXRate As DollarBeta,              
-- MV/NAV            
Case                  
 When @GlobalNAV <> 0                  
 Then (EndingMarketValueBase / @GlobalNAV) * 100              
 Else 0                  
End As [MVEquityPercentage],               
-- ($Delta * Beta) / NAV            
Case                  
 When @GlobalNAV <> 0                  
 Then ((DeltaExposureBase * IsNull(T_MW_GenericPNL.Beta,0)) / @GlobalNAV) * 100                  
 Else 0                  
End As [DollarDeltaPercentageBetaAdjusted],          
          
Vega,                
Vega * BeginningQuantity * Sidemultiplier * Multiplier * EndingFXRate As [DollarVega],            
Theta,                
Theta * BeginningQuantity * Sidemultiplier * Multiplier * EndingFXRate As [DollarTheta],            
Case            
 When Asset Like  '%option%' And PutOrCall = 'CALL'            
 Then BeginningQuantity * SideMultiplier            
 Else 0            
End As Qty_CALLOPT,            
Case            
 When Asset Like  '%option%' And PutOrCall = 'PUT'            
 Then BeginningQuantity * SideMultiplier            
 Else 0            
End As Qty_PUTOPT,        
@GlobalNAV As NAV      
      
Into #TempVSRwithGreeks      
            
From dbo.T_MW_GenericPNL             
Inner Join #Funds On #Funds.Items = T_MW_GenericPNL.Fund               
Left Outer Join PM_DailyBeta PMDB ON (PMDB.Symbol = T_MW_GenericPNL.UnderlyingSymbol AND DateDiff(Day,@EndDate,PMDB.Date)= 0)                 
Where DateDiff(Day,Rundate,@EndDate) = 0 And Open_CloseTag = 'O'And Asset <> 'Cash'          
Order by DollarDelta  Desc      
      
Update #TempVSRwithGreeks      
Set Qty_CALLOPT = Qty_CALLOPT +  (Quantity /100) ,      
Qty_PUTOPT = Qty_PUTOPT - (Quantity /100)      
             
Select * from #TempVSRwithGreeks      
      
Drop Table #Funds,#TempVSRwithGreeks 