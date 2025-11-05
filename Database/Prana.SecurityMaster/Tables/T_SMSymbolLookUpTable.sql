CREATE TABLE [dbo].[T_SMSymbolLookUpTable] (
    [TickerSymbol]          VARCHAR (100)    NOT NULL,
    [AssetID]               INT              NULL,
    [UnderLyingID]          INT              NULL,
    [AUECID]                INT              NULL,
	[FactSetSymbol]			VARCHAR (100)    NULL,
	[ActivSymbol]			VARCHAR (100)    NULL,
    [BloombergSymbol]       VARCHAR (200)    NULL,
    [ISINSymbol]            VARCHAR (20)     NULL,
    [SEDOLSymbol]           VARCHAR (20)     NULL,
    [CUSIPSymbol]           VARCHAR (20)     NULL,
    [OSISymbol]             VARCHAR (25)     CONSTRAINT [DF_T_SMSymbolLookUpTable_OSISymbol] DEFAULT ('') NULL,
    [IDCOSymbol]            VARCHAR (25)     CONSTRAINT [DF_T_SMSymbolLookUpTable_IDCOSymbol] DEFAULT ('') NULL,
    [OpraSymbol]            VARCHAR (20)     CONSTRAINT [DF_T_SMSymbolLookUpTable_OpraSymbol] DEFAULT ('') NULL,
    [ExchangeID]            INT              NULL,
    [CountryID]             INT              NULL,
    [CreationDate]          DATETIME         NULL,
    [Symbol_PK]             BIGINT           NOT NULL,
    [Sector]                VARCHAR (20)     NULL,
    [UnderLyingSymbol]      VARCHAR (100)    NULL,
    [ModifiedDate]          DATETIME         NULL,
    [CurrencyID]            INT              NULL,
    [EffectiveDate]         DATETIME         NULL,
    [CorpActionID]          UNIQUEIDENTIFIER NULL,
    [DataSource]            INT              NULL,
    [RoundLot]              DECIMAL (28, 10) CONSTRAINT [DF__T_SMSymbo__Round__71346041] DEFAULT ((1)) NOT NULL,
    [ProxySymbol]           VARCHAR (100)    NULL,
    [IsSecApproved]         BIT              CONSTRAINT [DF_T_SMSymbolLookUpTable_IsSecApproved] DEFAULT ((0)) NULL,
    [ApprovalDate]          DATETIME         CONSTRAINT [DF_T_SMSymbolLookUpTable_ApprovalDate] DEFAULT (((1)/(1))/(1800)) NULL,
    [ApprovedBy]            VARCHAR (100)    NULL,
    [Comments]              VARCHAR (500)    NULL,
    [UDAAssetClassID]       INT              CONSTRAINT [DF_SMSymbolLookUpTable_UDAAssetClassID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDASecurityTypeID]     INT              CONSTRAINT [DF_SMSymbolLookUpTable_UDASecurityTypeID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDASectorID]           INT              CONSTRAINT [DF_SMSymbolLookUpTable_UDASectorID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDASubSectorID]        INT              CONSTRAINT [DF_SMSymbolLookUpTable_UDASubSectorID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDACountryID]          INT              CONSTRAINT [DF_SMSymbolLookUpTable_UDACountryID] DEFAULT ((-2147483648.)) NOT NULL,
    [StrikePriceMultiplier] FLOAT (53)       CONSTRAINT [DF_T_SMSymbolLookUpTable_StrikePriceMultiplier] DEFAULT ((1)) NOT NULL,
    [CreatedBy]             VARCHAR (100)    NULL,
    [ModifiedBy]            VARCHAR (100)    NULL,
    [PrimarySymbology]      INT              CONSTRAINT [DF_T_SMSymbolLookUpTable_PrimarySymbology] DEFAULT ((0)) NULL,
    [BBGID]                 VARCHAR (20)     NULL,
    [EsignalOptionRoot]     VARCHAR (100)    NULL,
    [BloombergOptionRoot]   VARCHAR (100)    NULL,
	[SharesOutstanding]     FLOAT (53)       DEFAULT ((0)) NULL,
    [BloombergSymbolWithExchangeCode]       VARCHAR (200)    NULL,
    CONSTRAINT [PK_T_SMSymbolLookUpTable] PRIMARY KEY CLUSTERED ([Symbol_PK] ASC),
    CONSTRAINT [FK_T_SMSymbolLookUpTable_T_Asset] FOREIGN KEY ([AssetID]) REFERENCES [dbo].[T_Asset] ([AssetID]),
    CONSTRAINT [FK_T_SMSymbolLookUpTable_T_AUEC] FOREIGN KEY ([AUECID]) REFERENCES [dbo].[T_AUEC] ([AUECID]),
    CONSTRAINT [FK_T_SMSymbolLookUpTable_T_Currency] FOREIGN KEY ([CurrencyID]) REFERENCES [dbo].[T_Currency] ([CurrencyID]),
    CONSTRAINT [FK_T_SMSymbolLookUpTable_T_Exchange] FOREIGN KEY ([ExchangeID]) REFERENCES [dbo].[T_Exchange] ([ExchangeID]),
    CONSTRAINT [FK_T_SMSymbolLookUpTable_T_UnderLying] FOREIGN KEY ([UnderLyingID]) REFERENCES [dbo].[T_UnderLying] ([UnderLyingID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_TickerSymbol]
    ON [dbo].[T_SMSymbolLookUpTable]([TickerSymbol] ASC) WITH (ALLOW_PAGE_LOCKS = OFF);


GO
CREATE NONCLUSTERED INDEX [IDX_CusipSymbol]
    ON [dbo].[T_SMSymbolLookUpTable]([CUSIPSymbol] ASC) WITH (ALLOW_PAGE_LOCKS = OFF);


GO
CREATE NONCLUSTERED INDEX [IDX_ISINSymbol]
    ON [dbo].[T_SMSymbolLookUpTable]([ISINSymbol] ASC) WITH (ALLOW_PAGE_LOCKS = OFF);


GO
CREATE NONCLUSTERED INDEX [IDX_SedolSymbol]
    ON [dbo].[T_SMSymbolLookUpTable]([SEDOLSymbol] ASC) WITH (ALLOW_PAGE_LOCKS = OFF);


GO
CREATE NONCLUSTERED INDEX [IDX_BlommbergSymbol]
    ON [dbo].[T_SMSymbolLookUpTable]([BloombergSymbol] ASC) WITH (ALLOW_PAGE_LOCKS = OFF);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMSymbolLookUpTable_16_622625261__K12_K4_K14_K5_K6_K7_K8_K1]
    ON [dbo].[T_SMSymbolLookUpTable]([Symbol_PK] ASC, [AUECID] ASC, [UnderLyingSymbol] ASC, [BloombergSymbol] ASC, [ISINSymbol] ASC, [SEDOLSymbol] ASC, [CUSIPSymbol] ASC, [TickerSymbol] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMSymbolLookUpTable_16_622625261__K12_K4_K14_K5_K6_K7_K8]
    ON [dbo].[T_SMSymbolLookUpTable]([Symbol_PK] ASC, [AUECID] ASC, [UnderLyingSymbol] ASC, [BloombergSymbol] ASC, [ISINSymbol] ASC, [SEDOLSymbol] ASC, [CUSIPSymbol] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMSymbolLookUpTable_16_622625261__K12_K4_K14_K5_K6_K7_K8_K2]
    ON [dbo].[T_SMSymbolLookUpTable]([Symbol_PK] ASC, [AUECID] ASC, [UnderLyingSymbol] ASC, [BloombergSymbol] ASC, [ISINSymbol] ASC, [SEDOLSymbol] ASC, [CUSIPSymbol] ASC, [TickerSymbol] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_T_SMSymbolLookUpTable_16_622625261__K12_K4_K14_K5_K6_K7_K9]
    ON [dbo].[T_SMSymbolLookUpTable]([Symbol_PK] ASC, [AUECID] ASC, [UnderLyingSymbol] ASC, [BloombergSymbol] ASC, [ISINSymbol] ASC, [SEDOLSymbol] ASC, [CUSIPSymbol] ASC);

