CREATE PROCEDURE [dbo].[P_GetInterestRates]
(
@userID int
)
AS
select Date,AutoInterestRate,ManualInterestRate from T_InterestRates
where UserID = @userID
