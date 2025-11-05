 
CREATE procedure [dbo].[P_SaveCAPreferences]                    
(  
@companyID int ,                
@caPreference varbinary(max)    
)                    
As                    
             
If((Select count(*) from T_CAPreferences where CompanyID = @companyID) > 0 )                 
Begin             
	Update T_CAPreferences            
	Set             
	CAPreference = @caPreference
	Where CompanyID = @companyID
End            
Else            
	Begin            
		Insert Into T_CAPreferences(CompanyID,CAPreference)     
		Values(@companyID,@caPreference)             
	End            
    

