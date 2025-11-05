  
/****** Object:  Stored Procedure dbo.P_SaveSetUpContracts    Script Date: 04/18/2007 12:45:22 PM ******/  
CREATE PROCEDURE [dbo].[P_SaveSetUpContracts]  
(  
  @symbol varchar(10),  
  @auecID int,  
  @multiplier float,  
  @contractMonthID int,  
  @companyID int,  
  @companyContractSetUpID int,  
  @description varchar(100),  
  @result int   
    
 )  
AS   
Declare @total int   
Set @total = 0  
  
Select @total = Count(*)  
From T_SetUpContracts   
Where CompanySetUpContractID = @companyContractSetUpID  
  
if(@total > 0)  
begin   
   
 --Update T_SetUpContracts  
 Update T_SetUpContracts   
 Set Symbol = @symbol,   
  AuecID = @auecID,   
  ContractSize = @multiplier,  
  ContractMonthID = @contractMonthID,  
  Description = description  
     
 Where CompanySetUpContractID = @companyContractSetUpID    
      
    
 Set @result = @companyContractSetUpID   
   
end  
else  
--Insert T_SetUpContracts  
begin  
    INSERT T_SetUpContracts(Symbol, AuecID, ContractSize, ContractMonthID, Description, CompanyID)  
      Values(@symbol, @auecID, @multiplier, @contractMonthID, @description, @companyID)    
       
    Set @result = scope_identity()  
end  
select @result  