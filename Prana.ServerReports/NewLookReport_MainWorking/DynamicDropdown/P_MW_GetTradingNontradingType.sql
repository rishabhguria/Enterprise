Create procedure P_MW_GetTradingNontradingType    
AS    
select     
1 as ID,    
'Trading' As [DisplayName]    
Union     
select     
2 As ID,    
'Non trading' As [DisplayName]    