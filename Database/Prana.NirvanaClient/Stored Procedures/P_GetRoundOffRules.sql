
Create Proc [dbo].[P_GetRoundOffRules]    
(  
 @AppliedOrNonZeroRoundOff bit    
)    
AS    
BEGIN    
    
If(@AppliedOrNonZeroRoundOff = 0)  
  
Begin    
 Select AUECID, RoundOff from T_AUECRoundOffRules    
 Where IsApplied = 1    
End    
  
Else   
  
Begin    
 Select * from T_AUECRoundOffRules    
 Where IsApplied = 1 or RoundOff != 0    
End   
   
END
