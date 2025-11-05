CREATE TABLE [dbo].[T_GroupOrder] (
    [GroupOrderID] INT          IDENTITY (1, 1) NOT NULL,
    [GroupID]      VARCHAR (50) NULL,
    [ClOrderID]    VARCHAR (50) NULL,
    CONSTRAINT [PK_T_GroupOrder] PRIMARY KEY CLUSTERED ([GroupOrderID] ASC),
    CONSTRAINT [FK__T_GroupOr__Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[T_Group] ([GroupID]) ON DELETE CASCADE
);


GO

CREATE TRIGGER [dbo].[GroupOrderTrigger] 
   ON  [dbo].[T_GroupOrder]
   AFTER INSERT
AS 
BEGIN
	
	SET NOCOUNT ON;

   UPDATE T_TradedOrders set GroupID=(Select GroupID from inserted) where CLOrderID=(Select ClOrderID from inserted)

END
