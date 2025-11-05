


CREATE PROCEDURE [dbo].[P_GetRL]
(
		@RLID int,
		@MemoryID varchar(50)		
)
AS

/*DECLARE @MemoryID varchar(50) 
SET @MemoryID =  @TabID*/

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
               WHERE     (T_RoutingLogic.RLID = @RLID))
,ExchangeName  = (SELECT     T_AUEC.DisplayName
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
               WHERE  (RLID_FK = @RLID) AND (Sequence = 3)),               
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
                                            


         
         
         
WHERE (MemoryID = @MemoryID) ;     


end
ELSE
begin

Update T_RoutingLogicMemoryRL
SET RLID = @RLID, 
RLName= (SELECT     RLName
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
AUECID = (SELECT     AUECID
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
AssetID = (SELECT     AssetID
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
AssetName = (SELECT     AssetName
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
UnderLyingID  = (SELECT     UnderLyingID
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
UnderLyingName  = (SELECT     UnderLyingName
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
ExchangeName  = (SELECT     ExchangeName
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
TradingAccountID0  = (SELECT     TradingAccountID0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),            
TradingAccountName0  = (SELECT     TradingAccountName0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ), 
CounterPartyID0  = (SELECT     CounterPartyID0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ), 
CounterPartyName0  = (SELECT     CounterPartyName0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
VenueID0 = (SELECT     VenueID0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
VenueName0  = (SELECT     VenueName0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),                                                 
TradingAccountID1  = (SELECT     TradingAccountID1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
TradingAccountName1  = (SELECT     TradingAccountName1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
CounterPartyID1  = (SELECT     CounterPartyID1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ), 
CounterPartyName1  = (SELECT     CounterPartyName1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),                                             
VenueID1  = (SELECT     VenueID1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
VenueName1  = (SELECT     VenueName1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),                      
TradingAccountID2  = (SELECT     TradingAccountID2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
TradingAccountName2  = (SELECT     TradingAccountName2
         FROM         T_RoutingLogicMemoryRL
  WHERE     (MemoryID = 'Default') ),
CounterPartyID2  = (SELECT     CounterPartyID2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ), 
CounterPartyName2  = (SELECT     CounterPartyName2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
VenueID2  = (SELECT     VenueID2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
VenueName2  = (SELECT     VenueName2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
TradingAccountDefaultID   = (SELECT     TradingAccountDefaultID
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),                                                              
TradingAccountNameDefault = (SELECT     TradingAccountNameDefault
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
ParameterID0 =(SELECT     ParameterID0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
ParameterValue0 =(SELECT     ParameterValue0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
JoinCondition0 =(SELECT     JoinCondition0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
OperatorID0  =(SELECT     OperatorID0
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),                                
ParameterID1 =(SELECT     ParameterID1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
ParameterValue1 =(SELECT     ParameterValue1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
JoinCondition1 =(SELECT     JoinCondition1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
OperatorID1  =(SELECT     OperatorID1
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),              
ParameterID2 =(SELECT     ParameterID2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
ParameterValue2 =(SELECT     ParameterValue2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
JoinCondition2 =(SELECT     JoinCondition2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ),
OperatorID2  =(SELECT     OperatorID2
         FROM         T_RoutingLogicMemoryRL
         WHERE     (MemoryID = 'Default') ) 
         
                  
WHERE (MemoryID = @MemoryID) ;     


--Update T_RoutingLogicMemoryRL
--SELECT T_RoutingLogicMemoryRL.RLID = T_RoutingLogicMemoryRL.RLID FROM  T_RoutingLogicMemoryRL
end

SELECT     MemoryID , RLID, RLName, AUECID, AssetID, AssetName, UnderLyingID, UnderLyingName, ExchangeName, 
                      TradingAccountID0, TradingAccountName0, CounterPartyID0, CounterPartyName0, VenueID0, VenueName0, TradingAccountID1, TradingAccountName1, 
                      CounterPartyID1, CounterPartyName1, VenueID1, VenueName1, TradingAccountID2, TradingAccountName2, CounterPartyID2, CounterPartyName2, 
                      VenueID2, VenueName2, TradingAccountDefaultID, TradingAccountNameDefault, ParameterID0, ParameterValue0, JoinCondition0, OperatorID0, 
                      ParameterID1, ParameterValue1, JoinCondition1, OperatorID1, ParameterID2, ParameterValue2, JoinCondition2, OperatorID2
FROM         T_RoutingLogicMemoryRL





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


