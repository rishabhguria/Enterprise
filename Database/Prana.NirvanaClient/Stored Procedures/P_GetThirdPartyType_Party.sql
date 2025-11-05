

--Modified By: Pooja Porwal   
--Date: NOV 26,2014    
--Desc: Sorted by Third Party Name

CREATE PROCEDURE [dbo].[P_GetThirdPartyType_Party]           
(                  
  @companyID int,        
  @thirdPartyTypeID int                    
 )                  
AS             
SELECT          
  @companyID,         
  TP.ThirdPartyID,         
  TP.ThirdPartyName,         
  TP.Description,         
  TP.ThirdPartyID as CompanyThirdPartyID ,                
  TP.SymbolConvention,        
  TP.SecurityIdentifierTypeID          
 FROM         
 T_ThirdParty TP          
 Inner Join T_ThirdPartyType TPT on TP.ThirdPartyTypeID=TPT.ThirdPartyTypeID               
 Where TPT.ThirdPartyTypeID=@thirdPartyTypeID  
  
order BY TP.ThirdPartyName


