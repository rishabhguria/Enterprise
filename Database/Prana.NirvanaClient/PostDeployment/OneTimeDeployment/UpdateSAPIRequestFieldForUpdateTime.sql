/*
Post-Deployment Script Template							
*/
Update T_SAPISubscriptionRequestField
SET Equity=1,EquityOption=1,Future=1,FutureOption=1,FX=1,FixedIncome=1,FXForward=1
Where NirvanaFields = 'UpdateTime'