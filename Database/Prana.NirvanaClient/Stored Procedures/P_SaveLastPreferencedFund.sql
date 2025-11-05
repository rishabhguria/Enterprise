

CREATE PROCEDURE P_SaveLastPreferencedFund
(
  @fundID int
)

AS
BEGIN
SET NOCOUNT ON
DELETE FROM T_ResidualQtyFund
INSERT INTO T_ResidualQtyFund
(
   PreferencedFundID
)
VALUES (@fundID)

END