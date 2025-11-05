CREATE TABLE [dbo].[T_Expire_Settlement] (
    [PK_Settlement_expirationID] NVARCHAR (50) NOT NULL,
    [Expire_SettleTaxlotID]      NVARCHAR (50) NOT NULL,
    [GeneratedTaxlotID]          INT           NULL,
    [SettlementMode]             INT           NOT NULL,
    [SettlementPrice]            FLOAT (53)    NULL,
    [SettlementQty]              INT           NOT NULL,
    [TimeofSave]                 DATETIME      NOT NULL,
    [AUECLocalDate]              DATETIME      NOT NULL,
    CONSTRAINT [PK__T_Expire_Settlem__500876A0] PRIMARY KEY CLUSTERED ([PK_Settlement_expirationID] ASC) WITH (FILLFACTOR = 100)
);

