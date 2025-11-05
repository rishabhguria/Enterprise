create procedure [dbo].[P_GetAllAccountTables]            
    
As            
    
Begin            
    
SELECT * FROM T_MasterCategory  order by mastercategoryname
    
SELECT * FROM T_SubCategory  order by subcategoryname
    
SELECT * FROM T_Transactiontype   
    
SELECT * FROM T_SubAccounts order by name      
    
SELECT * FROM T_MasterAccountSide       
    
End 