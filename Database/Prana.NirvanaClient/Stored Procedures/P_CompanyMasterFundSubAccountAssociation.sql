
--------------*****************************-----------------  
 -- Fectching data based on company ID
 --Author : omshiv 
        --Dated : 31 March 2014  

   --returns a dictionary which contains a master fund id and the corresponding sub account i.e fund ids  
       --Author : Divya Bansal  
        --Dated : 23 May 2012  
----------------------****************************--------------  
  
CREATE Procedure [dbo].[P_CompanyMasterFundSubAccountAssociation]  
(
@companyID int
)
as  
  
select T_CompanyMasterFunds.CompanyMasterFundId,  
CompanyFundId  
 from   
T_CompanyMasterFundSubAccountAssociation 
inner JOIN T_CompanyMasterFunds on T_CompanyMasterFunds.CompanyMasterFundID = T_CompanyMasterFundSubAccountAssociation.CompanyMasterFundID
where companyID = @companyID  
  
