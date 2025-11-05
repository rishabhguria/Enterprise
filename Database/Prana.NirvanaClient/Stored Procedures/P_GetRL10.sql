


CREATE PROCEDURE [dbo].[P_GetRL10]
	(
		@RLID int
	)

AS
		


SELECT     T_RoutingLogic.RLID AS ID, T_RoutingLogic.Name, T_AUEC.AUECID, T_AUEC.AssetID, T_AUEC.UnderLyingID, T_AUEC.ExchangeID AS ExchangeID, 
                      T_RoutingLogic.DefaultCompanyTradingAccountID_FK  AS TradingAccountIDDefault
FROM         T_RoutingLogic INNER JOIN
                      T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID
WHERE     (T_RoutingLogic.RLID = @RLID)
                      
SELECT     ParameterID_FK AS ParameterID, ParameterValue, Sequence AS Rank, JoinCondition AS JoinConditionID, OperatorID_FK AS OperatorID
FROM         T_RoutingLogicCondition
WHERE     (RLID_FK = @RLID) 

SELECT     T_RoutingLogicVenues.CompanyTradingAccountID_FK AS TradingAccountID, T_CounterPartyVenue.CounterPartyVenueID, 
                      T_CounterPartyVenue.CounterPartyID, T_CounterPartyVenue.VenueID, T_RoutingLogicVenues.Rank
FROM         T_RoutingLogicVenues INNER JOIN
                      T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID
WHERE     (T_RoutingLogicVenues.RLID_FK = @RLID)








