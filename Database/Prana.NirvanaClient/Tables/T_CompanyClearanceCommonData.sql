CREATE TABLE [dbo].[T_CompanyClearanceCommonData] (
    [TimeZone]             VARCHAR (100) NULL,
    [AutoClearing]         VARCHAR (10)  NULL,
    [BaseTime]             DATETIME      NULL, 
    [CompanyID]			   INT			 NULL, 
    [RolloverPermittedUserID]			   INT			 NULL,
	[IsSendRealtimeManualOrderViaFix] INT Default 0
);