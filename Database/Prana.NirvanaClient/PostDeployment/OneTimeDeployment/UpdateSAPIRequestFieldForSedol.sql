
/*
Post-Deployment Script Template							
*/

--Update BBG Mnemonic for SEDOL in Snapshot Table
UPDATE T_SAPISnapshotRequestField
SET BBGMnemonic = 'ID_SEDOL1'
WHERE NirvanaFields = 'SedolSymbol';


--Update BBG Mnemonic for SEDOL in Subscription Table
UPDATE T_SAPISubscriptionRequestField
SET BBGMnemonic = 'ID_SEDOL1'
WHERE NirvanaFields = 'SedolSymbol';