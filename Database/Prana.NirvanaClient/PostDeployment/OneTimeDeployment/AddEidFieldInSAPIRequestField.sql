INSERT INTO T_SAPISubscriptionRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'EID', 'EID', 1, 1, 1, 1, 1, 1, 1
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISubscriptionRequestField WHERE NirvanaFields = 'EID'
);