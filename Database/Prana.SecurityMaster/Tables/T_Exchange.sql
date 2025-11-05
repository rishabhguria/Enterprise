CREATE TABLE [dbo].[T_Exchange] (
    [ExchangeID]              INT            NOT NULL,
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
    CONSTRAINT [PK__T_Exchange__123EB7A3] PRIMARY KEY CLUSTERED ([ExchangeID] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK__T_Exchang__Count__17036CC0] FOREIGN KEY ([Country]) REFERENCES [dbo].[T_Country] ([CountryID]),
    CONSTRAINT [FK__T_Exchang__State__17F790F9] FOREIGN KEY ([StateID]) REFERENCES [dbo].[T_State] ([StateID])
);

