/*
Post-Deployment Script Template							
*/

--Insert new fields in SAPI Sanpshot Request Field

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Bloomberg Composite Code', 'COMPOSITE_EXCH_CODE', 1, 0, 0, 0, 0, 0, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'Bloomberg Composite Code'
);

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Issue Date', 'ISSUE_DT', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'Issue Date'
);

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Coupon', 'CPN', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'Coupon'
);

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'MaturityDate', 'MATURITY', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'MaturityDate'
);

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Accrual Basis', 'DAY_CNT_DES', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'Accrual Basis'
);

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Coupon Frequency', 'CPN_FREQ', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'Coupon Frequency'
);

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Bond type', 'MARKET_SECTOR_DES', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'Bond type'
);

INSERT INTO T_SAPISnapshotRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'First Coupon Date', 'REAL_FIRST_CPN_DT', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISnapshotRequestField WHERE NirvanaFields = 'First Coupon Date'
);


--Insert new fields in SAPI Subscription Request Field

INSERT INTO T_SAPISubscriptionRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Bloomberg Composite Code', 'COMPOSITE_EXCH_CODE', 1, 0, 0, 0, 0, 0, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISubscriptionRequestField WHERE NirvanaFields = 'Bloomberg Composite Code'
);

INSERT INTO T_SAPISubscriptionRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Issue Date', 'ISSUE_DATE_REALTIME', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISubscriptionRequestField WHERE NirvanaFields = 'Issue Date'
);

INSERT INTO T_SAPISubscriptionRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'Coupon', 'COUPON_RT', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISubscriptionRequestField WHERE NirvanaFields = 'Coupon'
);

INSERT INTO T_SAPISubscriptionRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
SELECT 'MaturityDate', 'MATURITY_RT', 0, 0, 0, 0, 0, 1, 0
WHERE NOT EXISTS (
    SELECT 1 FROM T_SAPISubscriptionRequestField WHERE NirvanaFields = 'MaturityDate'
);


--Update Existing fields for SAPI Snapshot Request field

UPDATE T_SAPISnapshotRequestField SET Future = 1, FixedIncome = 0 WHERE NirvanaFields = 'ExpirationDate';

UPDATE T_SAPISnapshotRequestField SET EquityOPTION = 1 WHERE NirvanaFields = 'CusipNo';

--Update Existing fields for SAPI Subscription Request field

UPDATE T_SAPISubscriptionRequestField SET FixedIncome = 0 WHERE NirvanaFields = 'ExpirationDate';

UPDATE T_SAPISubscriptionRequestField SET EquityOPTION = 1 WHERE NirvanaFields = 'CusipNo';
