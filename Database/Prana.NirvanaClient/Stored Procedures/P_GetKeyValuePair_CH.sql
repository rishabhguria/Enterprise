  
/*          
Added By: Ankit Gupta on 10 Oct, 2014  
Description: This stored procedure will be used only for CH release type, to get only those funds from DB, that are currently in Active State.  
*/          
CREATE PROCEDURE [dbo].[P_GetKeyValuePair_CH]      
AS        
BEGIN        
      
 SELECT AssetID, AssetName from T_Asset order BY AssetName      
      
 SELECT CounterPartyID, ShortName from T_CounterParty order BY ShortName      
      
 SELECT VenueID, VenueName from T_Venue order BY VenueName      
      
 SELECT UserID, ShortName from T_CompanyUser where  IsActive =1  order BY ShortName       
      
 SELECT UnderLyingID, UnderLyingName from T_UnderLying order BY UnderLyingName      
       
 SELECT ExchangeID, displayname from T_Exchange order BY displayname      
       
 SELECT AUECID,AssetID,UnderlyingID,ExchangeID,BaseCurrencyID,OtherCurrencyID from T_AUEC      
      
 SELECT CurrencyID, CurrencySymbol from T_Currency order BY CurrencySymbol      
      
 SELECT CompanyTradingAccountsID, TradingShortName from T_CompanyTradingAccounts order BY TradingShortName      
      
 SELECT     CompanyFundID,FundShortName, FundName, CompanyID, LocalCurrency          
 FROM         T_CompanyFunds  where  IsActive =1       
 order by T_CompanyFunds.UIOrder       
      
 SELECT     CompanyStrategyID, StrategyShortName, StrategyName, CompanyID          
 FROM         T_CompanyStrategy where  IsActive =1  order BY StrategyName      
      
 select ThirdPartyID, ThirdPartyName,ShortName       
 from T_ThirdParty where  IsActive =1  order BY ThirdPartyName      
      
select ThirdPartyID, CompanyFundID       
from T_ThirdParty left JOIN T_CompanyFunds      
on CompanyThirdPartyID=ThirdPartyID where  T_ThirdParty.IsActive =1  and T_CompanyFunds.IsActive = 1    
  
select Acronym,ImportTagName     
from T_ImportTag order BY Acronym      
    
exec P_GetAllThirdPartyPermittedFunds
END     
  
