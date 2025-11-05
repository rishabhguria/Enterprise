CREATE TABLE [dbo].[T_CompanyVenue] (
    [CompanyVenueID]         INT          IDENTITY (1, 1) NOT NULL,
    [FullName]               VARCHAR (50) NOT NULL,
    [ShortName]              VARCHAR (50) NOT NULL,
    [VenueTypeID]            INT          NOT NULL,
    [TimeZone]               FLOAT (53)   NOT NULL,
    [PreMarketTime]          INT          NULL,
    [PreMarketStartTime]     DATETIME     NULL,
    [PreMarketEndTime]       DATETIME     NULL,
    [RegularMarketTime]      INT          NULL,
    [RegularMarketStartTime] DATETIME     NULL,
    [LunchTime]              INT          NULL,
    [LunchStartTime]         DATETIME     NULL,
    [LunchEndTime]           DATETIME     NULL,
    [RegularMarketEndTime]   DATETIME     NULL,
    [PostMarketTime]         INT          NULL,
    [PostMarketStartTime]    DATETIME     NULL,
    [PostMarketEndTime]      DATETIME     NULL,
    [CompanyID]              INT          NOT NULL,
    CONSTRAINT [PK_T_CompanyVenue] PRIMARY KEY CLUSTERED ([CompanyVenueID] ASC)
);

