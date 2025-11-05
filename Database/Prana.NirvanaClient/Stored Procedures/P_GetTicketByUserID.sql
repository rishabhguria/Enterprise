

CREATE PROCEDURE dbo.P_GetTicketByUserID
(
@userID int
--@ticktType int
)
AS
SELECT     TicketID, TicketName, DisplayName, TicketType
FROM         T_Ticket
WHERE     (UserID = @userID)
		-- And (TicketType = @ticktType)

