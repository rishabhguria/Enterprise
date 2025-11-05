-- P_SaveInterestRate 2, 1
-- select * from T_InterestRate
Create PROCEDURE P_DeleteInterestRate
(
		@period int
)
AS 

Delete From T_InterestRate where Period = @period