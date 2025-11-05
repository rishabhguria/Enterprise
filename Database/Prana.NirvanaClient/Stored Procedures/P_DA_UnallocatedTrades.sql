-- =============================================      
-- Modified By: Sumit Kakra
-- Modification Date: July 8 2013
-- Description: Nirvana Data access webservice proedure for webmethod UnallocatedTrades
-- =============================================      
CREATE Procedure [dbo].[P_DA_UnallocatedTrades]
(
@date DateTime,        
@errorMessage varchar(max) output,                                      
@errorNumber int output                               
)                            
As  
SET @ErrorMessage = 'Success'                                      
SET @ErrorNumber = 0                               
                            
BEGIN TRY                        
      -- To ensure no locking, it allows dirty reads, so check for blank symbols and Qty>0
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; 

	SELECT     
	G.GroupID, 
	G.Symbol, 
	dbo.V_SecmasterData.CompanyName SecurityName, 
	T_CounterParty.ShortName BrokerName,
	dbo.T_Side.Side,  
	G.Quantity OrderQuantity, 
	G.CumQty FilledQuantity,
	G.AvgPrice, 
	G.AUECLocalDate TradeDate, 
	G.SettlementDate,
	V_SecmasterData.UnderlyingSymbol,
	V_SecmasterData.PutOrCall,
	V_SecmasterData.StrikePrice,
	V_SecmasterData.Multiplier, 
	T_Currency.CurrencySymbol, 
	T_Asset.AssetName,
	V_SecmasterData.ExpirationDate,
	t_Companyuser.Login UserName, 
	V_SecmasterData.AssetName AS UDAAssetName,
	V_SecmasterData.SecurityTypeName AS UDASecurityTypeName,
	V_SecmasterData.SectorName AS UDASectorName,
	V_SecmasterData.SubSectorName AS UDASubSectorName,
	V_SecmasterData.CountryName AS UDACountryName,
	V_SecmasterData.BloombergSymbol AS BloombergSymbol,
	V_SecmasterData.OSISymbol AS OSISymbol

	FROM dbo.T_Group AS G 
				INNER JOIN dbo.T_Side ON G.OrderSideTagValue = dbo.T_Side.SideTagValue 
				INNER JOIN dbo.V_SecmasterData ON dbo.V_SecmasterData.TickerSymbol = G.Symbol
				INNER JOIN dbo.T_Currency ON T_Currency.CurrencyID = G.CurrencyID
				INNER JOIN dbo.T_Asset ON dbo.T_Asset.AssetID = V_SecmasterData.AssetID 
				INNER JOIN dbo.T_CounterParty ON dbo.T_CounterParty.CounterPartyID = G.CounterPartyID 
				INNER JOIN dbo.T_Companyuser ON dbo.T_Companyuser.UserID = G.UserID 
	WHERE Datediff(d,G.AUECLocalDate,@Date) = 0 
	And Symbol is Not Null 
	And G.CumQty<>0 
	And G.Quantity <> 0 
	And  G.CumQty is not null 
	And G.Quantity is not null

END TRY                                      
BEGIN CATCH                              
                                        
 SET @ErrorMessage = ERROR_MESSAGE();                                      
 SET @ErrorNumber = Error_number();          
                                       
END CATCH;            

