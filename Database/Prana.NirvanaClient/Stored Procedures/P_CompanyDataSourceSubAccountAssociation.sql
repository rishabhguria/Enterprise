--------------*****************************-----------------      
   --returns a dictionary which contains a DataSource id and the corresponding sub account i.e fund ids      
       --Author : Mukul Bhandari    
        --Dated : 23 May 2012      
----------------------****************************--------------      
      
Create Procedure [dbo].[P_CompanyDataSourceSubAccountAssociation]      
      
as      
      
select CompanythirdpartyID,      
CompanyFundID      
 from       
T_CompanyFunds  