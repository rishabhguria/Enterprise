CREATE TABLE [dbo].[T_SMEquityNonHistoryData] (
    [CompanyName]   VARCHAR (500)    NULL,
    [ModifiedAt]    DATETIME         NULL,
    [RoundLot]      DECIMAL (28, 10) NULL,
    [Symbol_PK]     BIGINT           NULL,
    [EffectiveDate] DATETIME         CONSTRAINT [DF_T_SMEquityNonHistoryData_EffectiveDate] DEFAULT ('1/1/1800 12:00:00 PM') NULL,
    [CorpActionID]  UNIQUEIDENTIFIER NULL,
    [Delta]         FLOAT (53)       NULL,
    [NonHistory_PK] BIGINT           IDENTITY (1, 1) NOT NULL,
    [Multiplier]    FLOAT (53)       CONSTRAINT [DF_T_SMEquityNonHistoryData_Multiplier] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_SMEquityNonHistoryData] PRIMARY KEY NONCLUSTERED ([NonHistory_PK] ASC),
    CONSTRAINT [FK_T_SMNonHistoryData_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);


GO
CREATE CLUSTERED INDEX [PK_IDX_SMEquityNonHistoryData]
    ON [dbo].[T_SMEquityNonHistoryData]([Symbol_PK] ASC) WITH (ALLOW_PAGE_LOCKS = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_T_SMEquityNonHistoryData]
    ON [dbo].[T_SMEquityNonHistoryData]([CompanyName] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMEquityNonHistoryData_16_830626002__K4_K8]
    ON [dbo].[T_SMEquityNonHistoryData]([Symbol_PK] ASC, [NonHistory_PK] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMEquityNonHistoryData_16_830626002__K4_K9]
    ON [dbo].[T_SMEquityNonHistoryData]([Symbol_PK] ASC, [NonHistory_PK] ASC);

