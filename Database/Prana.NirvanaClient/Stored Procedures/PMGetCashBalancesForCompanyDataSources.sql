Create PROCEDURE PMGetCashBalancesForCompanyDataSources  
                   
 (                  
  @ThirdPartyIDList varchar(500),                  
  @CompanyID int,    
  @InputDate datetime                   
 )                  
                   
AS     
select   
   TCF.FundShortName  
 , ISNULL(A.CurrentBalance,0) AS [CurrentBalance]  
 , ISNULL(C.PreviousBalance, 0) as [PreviousBalance]  
from   
 T_ThirdParty TTP    
 LEFT OUTER JOIN T_CompanyFunds As TCF ON TCF.CompanyThirdpartyID = TTP.ThirdPartyID     
 left outer join    
    (   
     Select   
       CompanyFundID,  
       CashBalance as [CurrentBalance]  
     FROM   
       PM_CompanyDatasourceCompanyFundCashBalance     
     WHERE   
       CashBalanceID in   
     (  
     Select   
       Max(CashBalanceID)              
     from PM_CompanyDatasourceCompanyFundCashBalance     
     GROUP BY CompanyFundID  
     )  
    ) as A   
on TCF.CompanyFundID = A.CompanyFundID   
 left outer join   
    (  
     Select          
       CashBalance as [PreviousBalance]  
       , CompanyFundID  
     FROM   
       PM_CompanyDatasourceCompanyFundCashBalance     
     WHERE   
       CashBalanceID in        
       (  
              
       SELECT CashBalanceID  
       FROM (  
           
        SELECT ROW_NUMBER()   
         OVER   
          (  
           PARTITION BY CompanyFundID   
           ORDER BY CashBalanceID DESC  
          ) rn,  
        CashBalanceID  
        FROM dbo.PM_CompanyDatasourceCompanyFundCashBalance  
        ) x WHERE rn=2  
       )  
    )  
     as C on TCF.CompanyFundID = C.CompanyFundID    
WHERE  
   
 TTP.ThirdPartyID IN     
     (    
      Select                   
       CAST(ITEMS as INT) AS ThirdPartyID                  
         FROM                   
       SPLIT(@ThirdPartyIDList, ',')     
     )    