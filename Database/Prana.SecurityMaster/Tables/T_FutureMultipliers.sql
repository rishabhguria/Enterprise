CREATE TABLE [dbo].[T_FutureMultipliers] (
    [Symbol]            NVARCHAR (100) NOT NULL,
    [Multiplier]        FLOAT (53)     NOT NULL,
    [PSSymbol]          VARCHAR (20)   NULL,
    [UnderlyingSymbol]  NVARCHAR (100) NULL,
    [CutOffTime]        VARCHAR (50)   NULL,
    [Exchange]          VARCHAR (50)   NULL,
    [ProxyRoot]         VARCHAR (50)   NULL,
    [UDAAssetClassID]   INT            CONSTRAINT [DF_FutureMultipliers_UDAAssetClassID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDASecurityTypeID] INT            CONSTRAINT [DF_FutureMultipliers_UDASecurityTypeID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDASectorID]       INT            CONSTRAINT [DF_FutureMultipliers_UDASectorID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDASubSectorID]    INT            CONSTRAINT [DF_FutureMultipliers_UDASubSectorID] DEFAULT ((-2147483648.)) NOT NULL,
    [UDACountryID]      INT            CONSTRAINT [DF_FutureMultipliers_UDACountryID] DEFAULT ((-2147483648.)) NOT NULL,
    [IsCurrencyFuture]  BIT            CONSTRAINT [DF_T_FutureMultipliers_IsCurrencyFuture] DEFAULT ((0)) NOT NULL,
    [DynamicUDA]        XML            NULL,
    [BBGYellowKey]      VARCHAR (100)  NULL,
    [BBGRoot]           VARCHAR(100)   NULL
);

