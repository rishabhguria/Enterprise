CREATE Procedure [GetWorkingAUECForCompany] 
(    
@companyID int     
)    
As    
    
select distinct     
G.AUECID,    
A.AssetName + '\' +               
U.UnderLyingName + '\' +               
E.DisplayName + '\' +               
C.CurrencySymbol AS [AUEC] 
From       
T_Group G,
T_AUEC AUEC,
T_ASSET A,
T_UNDERLYING U,
T_EXCHANGE E ,
T_CURRENCY C,
T_CompanyAUEC CA    
Where 
G.AUECID=AUEC.AUECID AND
AUEC.AssetID = A.AssetID AND 
AUEC.UnderlyingID = U.UnderlyingID  AND
AUEC.ExchangeID = E.ExchangeID AND
AUEC.BaseCurrencyID = C.CurrencyID AND   
G.AUECID = CA.AUECID AND  CA.CompanyID = @companyID        
order by [AUEC] asc