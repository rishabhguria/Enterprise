-- Author : Ishant
-- Description : It fetches the sum of CloseCrBal/CloseCrBal for FundId , CurrencyId 
-- and the supplied Transactiontype for certain date
-- Date : 6 feb 12
CREATE PROCEDURE P_GetBalancesByTransactionAcronymForDate
(
	@date DateTime,
	@acronym varchar(50)

)
As

Select SAB.FundId , SAB.CurrencyId, Sum(SAB.CloseDrBal) as CloseDrBal,Sum(SAB.CloseCrBal) as CloseCrBal
from T_SubAccountBalances SAB 
inner join T_SubAccounts SA on SA.SubAccountId = SAB.SubAccountId
inner join T_TransactionType Transac on SA.TransactionTypeID = Transac.TransactionTypeID
Where Transac.TransactionTypeAcronym = @acronym and SAB.TransactionDate = @date
Group by SAB.FundId , SAB.CurrencyId, Transac.TransactionTypeID

