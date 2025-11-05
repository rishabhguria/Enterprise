CREATE PROCEDURE [dbo].[P_SaveInterestRates]
(
@userID int,
@date varchar(20),
@autointerestRate float,
@manualinterestRate float
)
AS
INSERT INTO T_InterestRates(UserID, Date, AutoInterestRate,ManualInterestRate)
			
Values(@userID, @date,@autointerestRate,@manualinterestRate)

