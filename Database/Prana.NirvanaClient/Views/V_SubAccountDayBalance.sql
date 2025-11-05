
/*
Author		: Rajat Tandon
Date		: 29 Jan 2012
Description	: This view is used to see the summation of the Dr/Cr amount for a FundID, SubAccountID, CurrencyID for a given TransactionDate.
				It would help in finding balances for subaccounts. This index view would be updated as the data is inserted in DBO.T_Journal as the 
				schema is binded with the table.
*/					
CREATE VIEW DBO.V_SubAccountDayBalance
--	WITH SCHEMABINDING
AS
	SELECT 
		ROW_NUMBER() OVER(ORDER BY FundID, SubAccountID, CurrencyID, TransactionDate Asc) AS 'SeqNo', 
		FundID, 
		SubAccountID, 
		CurrencyID, 
		TransactionDate, 
		Sum(Dr) as CurrentDr, 
		Sum(Cr) as CurrentCr,
		COUNT_BIG(*) as Count
	FROM DBO.T_Journal 
	GROUP BY FundID, SubAccountID, CurrencyID, TransactionDate

