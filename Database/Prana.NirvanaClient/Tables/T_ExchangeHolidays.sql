CREATE TABLE [dbo].[T_ExchangeHolidays] (
    [ExchangeHolidayID] INT          IDENTITY (1, 1) NOT NULL,
    [ExchangeID]        INT          NOT NULL,
    [HolidayDate]       DATETIME     NOT NULL,
    [Description]       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_ExchangeHolidays] PRIMARY KEY CLUSTERED ([ExchangeHolidayID] ASC)
);

