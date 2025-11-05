

        
CREATE PROCEDURE dbo.P_SaveCompanyThirdMappingDetails        
(        
  @CompanyThirdPartyID_FK int,        
  @InternalFundNameID_FK int,        
  @MappedName varchar(50),        
  @FundAccntNo varchar(50),        
  @FundTypeID_FK int,        
  @companyID int,        
  @result int        
           
)        
AS         
Declare @total int         
Set @total = 0        
        
Select @total = Count(*)        
From T_CompanyThirdPartyMappingDetails         
Where  InternalFundNameID_FK = @InternalFundNameID_FK        
        
if(@total > 0)        
begin         
        
 Update T_CompanyThirdPartyMappingDetails         
 Set MappedName = @MappedName,        
  FundAccntNo = @FundAccntNo,         
  FundTypeID_FK = @FundTypeID_FK,    
  CompanyThirdPartyID_FK = @CompanyThirdPartyID_FK            
           
 Where InternalFundNameID_FK = @InternalFundNameID_FK        
        
 Set @result = -1       
         
end        
else        
        
begin        
 INSERT T_CompanyThirdPartyMappingDetails(CompanyThirdPartyID_FK, InternalFundNameID_FK,MappedName, FundAccntNo, FundTypeID_FK , CompanyID_FK)        
 Values(@CompanyThirdPartyID_FK, @InternalFundNameID_FK, @MappedName, @FundAccntNo, @FundTypeID_FK, @CompanyID)          
             
    Set @result = scope_identity()        
end        
select @result 

