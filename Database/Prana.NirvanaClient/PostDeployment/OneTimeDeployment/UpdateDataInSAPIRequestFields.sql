/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This script update the values of mandatory columns from false to true for certain Nirvana Fields				
--------------------------------------------------------------------------------------
*/

-- Updates the the table T_SAPISnapshotRequestField

UPDATE T_SAPISnapshotRequestField SET FixedIncome = 0 WHERE NirvanaFields = 'ExpirationDate';

UPDATE T_SAPISnapshotRequestField SET Future = 1, FutureOption = 1, FixedIncome = 1, FXForward = 1 WHERE NirvanaFields = 'FullCompanyName';

-- Updates the the table T_SAPISubscriptionRequestField

UPDATE T_SAPISubscriptionRequestField SET Equity = 1, Future = 1, FixedIncome = 1 WHERE NirvanaFields = 'CusipNo';

UPDATE T_SAPISubscriptionRequestField SET Future = 1, FixedIncome = 0 WHERE NirvanaFields = 'ExpirationDate';

UPDATE T_SAPISubscriptionRequestField SET Equity = 1, EquityOption = 1, Future = 1, FutureOption = 1,FX = 1, FixedIncome = 1, FXForward = 1 WHERE NirvanaFields = 'CountryId';

UPDATE T_SAPISubscriptionRequestField SET Equity = 1, EquityOption = 1, Future = 1, FutureOption = 1,FX = 1, FixedIncome = 1, FXForward = 1 WHERE NirvanaFields = 'FullCompanyName';

UPDATE T_SAPISubscriptionRequestField SET EquityOption = 1, Future = 1 WHERE NirvanaFields = 'CurrencyCode';

UPDATE T_SAPISubscriptionRequestField SET Equity = 1, EquityOption = 1, Future = 1, FutureOption = 1 WHERE NirvanaFields = 'Exchange';

UPDATE T_SAPISubscriptionRequestField SET Future = 1 WHERE NirvanaFields = 'Future Contract Size(Multiplier)';

UPDATE T_SAPISubscriptionRequestField SET EquityOption = 1, FutureOption = 1 WHERE NirvanaFields = 'UnderlyingSymbol';

