CREATE TABLE [dbo].[T_Exchange] (
    [ExchangeID]              INT            IDENTITY (1, 1) NOT NULL,
    [FullName]                VARCHAR (50)   NOT NULL,
    [DisplayName]             VARCHAR (50)   NOT NULL,
    [TimeZone]                NVARCHAR (100) NOT NULL,
    [RegularTime]             INT            NULL,
    [LunchTime]               INT            NULL,
    [RegularTradingStartTime] DATETIME       NULL,
    [RegularTradingEndTime]   DATETIME       NULL,
    [LunchTimeStartTime]      DATETIME       NULL,
    [LunchTimeEndTime]        DATETIME       NULL,
    [Country]                 INT            NOT NULL,
    [StateID]                 INT            NOT NULL,
    [CountryFlagID]           INT            NULL,
    [LogoID]                  INT            NULL,
    [TimeZoneOffSet]          FLOAT (53)     NULL,
    [ExchangeIdentifier]      NVARCHAR (10)  NULL,
    CONSTRAINT [PK_T_Exchange] PRIMARY KEY CLUSTERED ([ExchangeID] ASC),
    CONSTRAINT [FK_T_Exchange_T_Country] FOREIGN KEY ([Country]) REFERENCES [dbo].[T_Country] ([CountryID]),
    CONSTRAINT [FK_T_Exchange_T_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[T_State] ([StateID])
);

