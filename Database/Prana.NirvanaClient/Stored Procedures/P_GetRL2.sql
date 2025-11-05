


CREATE PROCEDURE [dbo].[P_GetRL2]
(
		@RLID int		
)
AS
if(@RLID > 0 )
begin
Update T_RoutingLogicMemoryRL
SET RLID = @RLID, 
RLName= (SELECT     Name
         FROM         T_RoutingLogic
         WHERE     (RLID = @RLID) ),
AUECID = (SELECT     AUECID_FK
          FROM         T_RoutingLogic
          WHERE     (RLID = @RLID)),
AssetID = (SELECT     T_Asset.AssetID
          FROM         T_RoutingLogic INNER JOIN
                                T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID INNER JOIN
                                T_Asset ON T_AUEC.AssetID = T_Asset.AssetID
          WHERE     (T_RoutingLogic.RLID = @RLID)),
AssetName = (SELECT     T_Asset.AssetName
             FROM         T_RoutingLogic INNER JOIN
                                   T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID INNER JOIN
                                   T_Asset ON T_AUEC.AssetID = T_Asset.AssetID
             WHERE     (T_RoutingLogic.RLID = @RLID)),
UnderLyingID  = (SELECT     T_UnderLying.UnderLyingID
               FROM         T_RoutingLogic INNER JOIN
                                     T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID INNER JOIN
                                     T_UnderLying ON T_AUEC.UnderLyingID = T_UnderLying.UnderLyingID
               WHERE     (T_RoutingLogic.RLID = @RLID)),
UnderLyingName  = (SELECT     T_UnderLying.UnderLyingName
               FROM         T_RoutingLogic INNER JOIN
                                     T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID INNER JOIN
                                     T_UnderLying ON T_AUEC.UnderLyingID = T_UnderLying.UnderLyingID
               WHERE     (T_RoutingLogic.RLID = @RLID)),
ExchangeName  = (SELECT     T_AUEC.DisplayName
                 FROM         T_RoutingLogic INNER JOIN
                                       T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID
                 WHERE     (T_RoutingLogic.RLID = @RLID)),
TradingAccountID0  = (SELECT     CompanyTradingAccountID_FK
                      FROM         T_RoutingLogicVenues
                      WHERE     (Rank = 1) AND (RLID_FK = @RLID)),               
TradingAccountName0  = (SELECT     T_CompanyTradingAccounts.TradingAccountName
                        FROM         T_RoutingLogicVenues INNER JOIN
                                              T_CompanyTradingAccounts ON 
                                              T_RoutingLogicVenues.CompanyTradingAccountID_FK = T_CompanyTradingAccounts.CompanyTradingAccountsID
                        WHERE     (T_RoutingLogicVenues.Rank = 1) AND (T_RoutingLogicVenues.RLID_FK = @RLID)), 
CounterPartyID0  = (SELECT     T_CounterPartyVenue.CounterPartyVenueID
                    FROM         T_RoutingLogicVenues INNER JOIN
                                          T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID
                    WHERE     (T_RoutingLogicVenues.Rank = 1) AND (T_RoutingLogicVenues.RLID_FK = @RLID)), 
CounterPartyName0  = (SELECT     T_CounterParty.FullName
                      FROM         T_RoutingLogicVenues INNER JOIN
                                            T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                                            T_CounterParty ON T_CounterPartyVenue.CounterPartyID = T_CounterParty.CounterPartyID
                      WHERE     (T_RoutingLogicVenues.Rank = 1) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),
VenueID0 = (SELECT     T_CounterPartyVenue.VenueID
             FROM         T_RoutingLogicVenues INNER JOIN
                                   T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID
             WHERE     (T_RoutingLogicVenues.Rank = 1) AND (T_RoutingLogicVenues.RLID_FK = @RLID)), 
VenueName0  = (SELECT     T_Venue.VenueName
               FROM         T_RoutingLogicVenues INNER JOIN
                                     T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                                     T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID
               WHERE     (T_RoutingLogicVenues.Rank = 1) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),                                                  
TradingAccountID1  = (SELECT     CompanyTradingAccountID_FK
                      FROM         T_RoutingLogicVenues
                      WHERE     (Rank = 2) AND (RLID_FK = @RLID)),
TradingAccountName1  = (SELECT     T_CompanyTradingAccounts.TradingAccountName
                        FROM         T_RoutingLogicVenues INNER JOIN
                                              T_CompanyTradingAccounts ON 
                                              T_RoutingLogicVenues.CompanyTradingAccountID_FK = T_CompanyTradingAccounts.CompanyTradingAccountsID
                        WHERE     (T_RoutingLogicVenues.Rank = 2) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),
CounterPartyID1  = (SELECT     T_CounterPartyVenue.CounterPartyVenueID
                    FROM         T_RoutingLogicVenues INNER JOIN
                                          T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID
                    WHERE     (T_RoutingLogicVenues.Rank = 2) AND (T_RoutingLogicVenues.RLID_FK = @RLID)), 
CounterPartyName1  = (SELECT     T_CounterParty.FullName
                      FROM         T_RoutingLogicVenues INNER JOIN
                                            T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                                            T_CounterParty ON T_CounterPartyVenue.CounterPartyID = T_CounterParty.CounterPartyID
                      WHERE     (T_RoutingLogicVenues.Rank = 2) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),                                             
VenueID1  = (SELECT     T_CounterPartyVenue.VenueID
             FROM         T_RoutingLogicVenues INNER JOIN
                                   T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID
             WHERE     (T_RoutingLogicVenues.Rank = 2) AND (T_RoutingLogicVenues.RLID_FK = @RLID)), 
VenueName1  = (SELECT     T_Venue.VenueName
               FROM         T_RoutingLogicVenues INNER JOIN
                                     T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                                     T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID
               WHERE     (T_RoutingLogicVenues.Rank = 2) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),                      
TradingAccountID2  = (SELECT     CompanyTradingAccountID_FK
                      FROM         T_RoutingLogicVenues
                      WHERE     (Rank = 3) AND (RLID_FK = @RLID)),
TradingAccountName2  = (SELECT     T_CompanyTradingAccounts.TradingAccountName
                        FROM         T_RoutingLogicVenues INNER JOIN
                                              T_CompanyTradingAccounts ON 
      T_RoutingLogicVenues.CompanyTradingAccountID_FK = T_CompanyTradingAccounts.CompanyTradingAccountsID
                        WHERE     (T_RoutingLogicVenues.Rank = 3) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),
CounterPartyID2  = (SELECT     T_CounterPartyVenue.CounterPartyVenueID
                    FROM         T_RoutingLogicVenues INNER JOIN
                                          T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID
                    WHERE     (T_RoutingLogicVenues.Rank = 3) AND (T_RoutingLogicVenues.RLID_FK = @RLID)), 
CounterPartyName2  = (SELECT     T_CounterParty.FullName
                      FROM         T_RoutingLogicVenues INNER JOIN
                                            T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                                            T_CounterParty ON T_CounterPartyVenue.CounterPartyID = T_CounterParty.CounterPartyID
                      WHERE     (T_RoutingLogicVenues.Rank = 3) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),
VenueID2  = (SELECT     T_CounterPartyVenue.VenueID
             FROM         T_RoutingLogicVenues INNER JOIN
                                   T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID
             WHERE     (T_RoutingLogicVenues.Rank = 3) AND (T_RoutingLogicVenues.RLID_FK = @RLID)), 
VenueName2  = (SELECT     T_Venue.VenueName
               FROM         T_RoutingLogicVenues INNER JOIN
                                     T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
                                     T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID
               WHERE     (T_RoutingLogicVenues.Rank = 3) AND (T_RoutingLogicVenues.RLID_FK = @RLID)),
TradingAccountDefaultID   = (SELECT     DefaultCompanyTradingAccountID_FK
                             FROM         T_RoutingLogic
                             WHERE     (RLID = @RLID)),                                                                 
TradingAccountNameDefault = (SELECT     T_CompanyTradingAccounts.TradingAccountName
                             FROM         T_CompanyTradingAccounts INNER JOIN
                                                   T_RoutingLogic ON T_CompanyTradingAccounts.CompanyTradingAccountsID = T_RoutingLogic.DefaultCompanyTradingAccountID_FK
                             WHERE     (T_RoutingLogic.RLID = @RLID)),
ParameterID0 =(SELECT     ParameterID_FK
               FROM         T_RoutingLogicCondition
               WHERE     (RLID_FK = @RLID) AND (Sequence = 1)),
ParameterValue0 =(SELECT     ParameterValue
                  FROM         T_RoutingLogicCondition
                  WHERE     (RLID_FK = @RLID) AND (Sequence = 1)),
JoinCondition0 =(SELECT     JoinCondition
                 FROM         T_RoutingLogicCondition
                 WHERE     (RLID_FK = @RLID) AND (Sequence = 1)),
OperatorID0  =(SELECT     OperatorID_FK
               FROM         T_RoutingLogicCondition
               WHERE     (RLID_FK = @RLID) AND (Sequence = 2)),                                
ParameterID1 =(SELECT     ParameterID_FK
               FROM         T_RoutingLogicCondition
               WHERE     (RLID_FK = @RLID) AND (Sequence = 2)),
ParameterValue1 =(SELECT     ParameterValue
                  FROM         T_RoutingLogicCondition
                  WHERE     (RLID_FK = @RLID) AND (Sequence = 2)),
JoinCondition1 =(SELECT     JoinCondition
                 FROM         T_RoutingLogicCondition
                 WHERE     (RLID_FK = @RLID) AND (Sequence = 2)),
OperatorID1  =(SELECT     OperatorID_FK
               FROM         T_RoutingLogicCondition
               WHERE     (RLID_FK = @RLID) AND (Sequence = 3)),               
ParameterID2 =(SELECT     ParameterID_FK
               FROM         T_RoutingLogicCondition
               WHERE     (RLID_FK = @RLID) AND (Sequence = 3)),
ParameterValue2 =(SELECT     ParameterValue
                  FROM         T_RoutingLogicCondition
                  WHERE     (RLID_FK = @RLID) AND (Sequence = 3)),
JoinCondition2 =(SELECT     JoinCondition
                 FROM         T_RoutingLogicCondition
                 WHERE     (RLID_FK = @RLID) AND (Sequence = 3)),
OperatorID2  =(SELECT     OperatorID_FK
               FROM         T_RoutingLogicCondition
               WHERE     (RLID_FK = @RLID) AND (Sequence = 3))               
                                            


         
         
         
WHERE (MemoryID = 'RL') ;     

Select * From T_RoutingLogicMemoryRL where (MemoryID='RL');
end
ELSE
begin
Select * From T_RoutingLogicMemoryRL where (MemoryID='Default');
--Update T_RoutingLogicMemoryRL
--SELECT T_RoutingLogicMemoryRL.RLID = T_RoutingLogicMemoryRL.RLID FROM  T_RoutingLogicMemoryRL
end




-- SELECT DISTINCT T_RoutingLogic.RLID, T_RoutingLogic.Name, T_RoutingLogic.AUECID_FK
--FROM         T_RoutingLogic INNER JOIN
--                      T_RoutingLogicVenues ON T_RoutingLogic.RLID = T_RoutingLogicVenues.RLID_FK INNER JOIN
--                      T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID INNER JOIN
--                      T_AUECExchange ON T_AUEC.AUECExchangeID = T_AUECExchange.AUECExchangeID INNER JOIN
--                      T_Asset ON T_AUEC.AssetID = T_Asset.AssetID INNER JOIN
--                      T_UnderLying ON T_AUEC.UnderLyingID = T_UnderLying.UnderLyingID AND T_Asset.AssetID = T_UnderLying.AssetID INNER JOIN
--                      T_CounterPartyVenue ON T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK = T_CounterPartyVenue.CounterPartyVenueID INNER JOIN
--                      T_CounterParty ON T_CounterPartyVenue.CounterPartyID = T_CounterParty.CounterPartyID INNER JOIN
--                      T_Venue ON T_CounterPartyVenue.VenueID = T_Venue.VenueID INNER JOIN
--                      T_CompanyTradingAccounts ON 
--                      T_RoutingLogicVenues.CompanyTradingAccountID_FK = T_CompanyTradingAccounts.CompanyTradingAccountsID INNER JOIN
--                      T_RoutingLogicCondition ON T_RoutingLogic.RLID = T_RoutingLogicCondition.RLID_FK INNER JOIN
--                      T_RoutingLogicParameter ON T_RoutingLogicCondition.ParameterID_FK = T_RoutingLogicParameter.ParameterID INNER JOIN
--                      T_Operator ON T_RoutingLogicCondition.OperatorID_FK = T_Operator.OperatorID , T_RoutingLogicMemoryRL This
--WHERE     (T_RoutingLogic.RLID = @RLID)
--ORDER BY T_RoutingLogic.Name, T_RoutingLogic.RLID


