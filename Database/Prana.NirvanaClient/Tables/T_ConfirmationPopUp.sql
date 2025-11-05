CREATE TABLE [dbo].[T_ConfirmationPopUp] (
    [ISNewOrder]    VARCHAR (6) NULL,
    [ISCXL]         VARCHAR (6) NULL,
    [ISCXLReplace]  VARCHAR (6) NULL,
	[ISManualOrder] VARCHAR (6) NULL,
    [PK]            INT         IDENTITY (1, 1) NOT NULL,
    [CompanyUserID] INT         NULL
);

