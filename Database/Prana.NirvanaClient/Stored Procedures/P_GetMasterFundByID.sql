/*      
P_GetMasterFundByID '0'      
Go    
Select * from T_CompanyMasterFunds    
*/      
      
CREATE Procedure P_GetMasterFundByID      
(      
 @FundID varchar(max)      
)      
As      
Declare @Fund Table                                                                            
(                                                                            
 FundID int                                                                        
)                           
                        
Insert into @Fund                          
Select Cast(Items as int) from dbo.Split(@FundID,',')         
    
Declare @count int    
Set @count = (Select Count(*) from @Fund)     
    
If @count = 1    
   Begin     
   Declare @MasterFundID Int    
   Set @MasterFundID = (Select FundID from @Fund)     
    If (@MasterFundID = 0)    
    Begin   
                   Declare @intCounter int  
     Set @intCounter=(Select Count(*) From T_CompanyMasterFunds)  
     If(@intCounter > 0)  
        Begin  
       Select      
       CMF.CompanyMasterFundID as MasterFundID,      
       CMF.MasterFundName as MasterFundName,    
    CMF.MasterFundLogo as MasterFundLogo       
       From T_CompanyMasterFunds CMF   
       End  
     Else  
          Select   
       0 as MasterFundID,  
       '-Select-' as MasterFundName,    
  null as MasterFundLogo        
     End    
    Else    
      Begin    
     Select      
     CMF.CompanyMasterFundID as MasterFundID,      
     CMF.MasterFundName as MasterFundName,    
    CMF.MasterFundLogo as MasterFundLogo         
     From T_CompanyMasterFunds CMF       
     Where CMF.CompanyMasterFundID =@MasterFundID      
      End     
   End    
    
Else    
 Begin    
   Select      
    CMF.CompanyMasterFundID as MasterFundID,      
    CMF.MasterFundName as MasterFundName,    
    CMF.MasterFundLogo as MasterFundLogo        
    From T_CompanyMasterFunds CMF        
    Where CMF.CompanyMasterFundID in (select FundID from @Fund)     
 End    
    
    
      
    