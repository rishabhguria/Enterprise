
CREATE proc P_DeleteInterestRates
(
@userID int
)
as
delete from T_InterestRates
where UserID = @userID
