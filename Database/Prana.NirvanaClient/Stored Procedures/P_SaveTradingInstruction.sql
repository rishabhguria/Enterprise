



-- =============================================
-- Author:		Ashish Poddar
-- Create date: 22 Feb, 2007
-- Description:	To Save trading instructions to the database.
-- =============================================
CREATE PROCEDURE [dbo].[P_SaveTradingInstruction]
	-- Add the parameters for the stored procedure here
	@ClOrderID varchar(50),
	@Symbol varchar(50),
	@Quantity	float,
	@Text varchar(200),
	@ClientOrderID varchar(50),
	@UserID int,
	@TradingAccID int,
	@Side varchar(50),
	@Status int,
	@MsgType varchar(10),
	@OnBehalfOfCompID varchar(50) -- refers to Client Name in the object TradingInstruction
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
Insert into T_TradingInstructions
(
	ClOrderID ,
	Symbol ,
	Quantity	,
	Instructions ,
	ClientOrderID ,
	CompanyUserID ,
	TradingAccountID ,
	SideTagValue,
	IsAccepted,
	MsgType,
	OnBehalfOfCompID

)
values
(
	@ClOrderID ,
	@Symbol ,
	@Quantity	,
	@Text ,
	@ClientOrderID ,
	@UserID ,
	@TradingAccID ,
	@Side ,
	@Status,
	@MsgType,
	@OnBehalfOfCompID
)
END




