-- =============================================    
--Created by: Kuldeep agrawal  
--date: 24 feb, 2014  
-- Description: This procedure gets all key value pairs in one go and thus it saves many DB calls which   
-- we were making to get every key value pair individually.  
  
-- Modified By:  Bharat raturi    
-- Create date: 30 apr,2014.    
--Modified the script so that the third party and the funds for third party could be fetched   
-- =============================================    
CREATE PROCEDURE [dbo].[P_GetKeyValuePair]      
AS        
BEGIN        
      
 SELECT AssetID, AssetName from T_Asset order BY AssetName      
      
 SELECT CounterPartyID, ShortName from T_CounterParty order BY ShortName      
      
 SELECT VenueID, VenueName from T_Venue order BY VenueName      
      
 SELECT UserID, ShortName from T_CompanyUser order BY ShortName      
      
 SELECT UnderLyingID, UnderLyingName from T_UnderLying order BY UnderLyingName      
       
 SELECT ExchangeID, displayname from T_Exchange order BY displayname      
       
 SELECT AUECID,AssetID,UnderlyingID,ExchangeID,BaseCurrencyID,OtherCurrencyID,C.ISOCode as CountryISOCode from T_AUEC T
 LEFT JOIN T_Country C on T.Country=C.CountryID    
      
 SELECT CurrencyID, CurrencySymbol from T_Currency order BY CurrencySymbol      
      
 SELECT CompanyTradingAccountsID, TradingShortName from T_CompanyTradingAccounts order BY TradingShortName      
      
 SELECT     CompanyFundID,FundShortName, FundName, CompanyID, LocalCurrency, IsSwapAccount          
 FROM         T_CompanyFunds        
 where        IsActive = 1
 order by T_CompanyFunds.UIOrder      
      
 SELECT     CompanyStrategyID, StrategyShortName, StrategyName, CompanyID          
 FROM         T_CompanyStrategy order BY StrategyName      
      
 select ThirdPartyID, ThirdPartyName  ,ShortName      
 from T_ThirdParty order BY ThirdPartyName      
      
select ThirdPartyID, CompanyFundID       
from T_ThirdParty left JOIN T_CompanyFunds      
on CompanyThirdPartyID=ThirdPartyID   
where T_CompanyFunds.IsActive = 1   
    
select Acronym,ImportTagName     
from T_ImportTag order BY Acronym      
   
exec P_GetAllThirdPartyPermittedFunds
END     
  

