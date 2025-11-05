/*
Post-Deployment Script Template							
*/

--Insert New Field Option Multiplier in Snapshot and subscription Tables

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'OptionMultiplier', 'OPT_MULTIPLIER', 0, 1, 0, 0, 0, 0, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'OptionMultiplier'
);


INSERT INTO T_SAPISubscriptionRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'OptionMultiplier', 'OPTION_MULTIPLIER_REALTIME', 0, 1, 0, 0, 0, 0, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISubscriptionRequestField WHERE NirvanaFields = 'OptionMultiplier'
);

--Update the Snapshot table for RoundLot Nirvana Field

UPDATE T_SAPISnapshotRequestField
SET EquityOption = 0
WHERE NirvanaFields = 'RoundLot';
