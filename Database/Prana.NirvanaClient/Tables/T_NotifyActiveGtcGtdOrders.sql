CREATE TABLE [dbo].[T_NotifyActiveGtcGtdOrders] ( 
    [CompanyID]			   INT			 NULL,
	[IsNotifyActiveGtcGtdOrders] INT      Default 0,
    [TimeToNotify]             DATETIME      NULL
);
