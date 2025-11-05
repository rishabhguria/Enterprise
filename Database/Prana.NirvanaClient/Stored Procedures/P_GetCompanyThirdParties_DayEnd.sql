

--Modified By: Pooja Porwal   
--Date: NOV 26,2014    
--Desc: Sorted by Third Party Name        
          
/****** Object:  Stored Procedure dbo.P_GetCompanyThirdParties    Script Date: 11/17/2005 9:50:23 AM ******/              
CREATE PROCEDURE [dbo].[P_GetCompanyThirdParties_DayEnd]       
(              
  @companyID int                
 )              
AS              
 SELECT        
CTP.CompanyID,     
CTP.ThirdPartyID,     
TP.ThirdPartyName,     
TP.Description,     
CTP.CompanyThirdPartyID  ,            
TP.SymbolConvention,    
TP.SecurityIdentifierTypeID--,TP.Delimiter,TP.DelimiterName,TP.FileExtension           
 FROM T_CompanyThirdParty CTP, T_ThirdParty TP              
 Where CTP.CompanyID = @companyID AND CTP.ThirdPartyID = TP.ThirdPartyID AND TP.ThirdPartyTypeID = 1   
order BY TP.ThirdPartyName

