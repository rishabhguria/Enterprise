/*
Post-Deployment Script Template							
*/


--update Snapshot table for Change Nirvana Field 

UPDATE T_SAPISnapshotRequestField
SET EquityOption = 1, Future = 1, FutureOption = 1, FX = 1, FixedIncome = 1, FXForward = 1 
WHERE NirvanaFields = 'Change';

--update Subscription table for Nirvana Fields

UPDATE T_SAPISubscriptionRequestField
SET EquityOption = 1, Future = 1
WHERE NirvanaFields = 'Change';

UPDATE T_SAPISubscriptionRequestField
SET Future = 1
WHERE NirvanaFields = 'Futures Contract Expiration Date';
