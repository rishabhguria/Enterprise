/****************************************          
Description : To get user wise fund mapping        
  To get user wise fund mapping, earlier a separate table was used to save user wise fund mapping from expnl,
 but now it is centralised with the same table in which permissions are saved from admin                      
Author : Divya                                                             
date : 08 may 2012                                                          
*********************************************/ 


CREATE Procedure P_GetUserFundMapping  
As 

 Select CUF.CompanyUserID as UserID,CUF.CompanyFundID as  FundID 
 From T_CompanyFunds CF, T_CompanyUserFunds CUF    
 Where CF.CompanyFundID = CUF.CompanyFundID   
 AND CF.IsActive=1








-------**********--------------------
--Commented by : Divya Bansal
-- Instead of usin a separate table for saving user fund mapping , we will use the same table to get user fund mapping i.e T_companyUserFund
------**********-------------------


--create procedure P_GetUserFundMapping  
--As  
--Select * from t_userfundmapping