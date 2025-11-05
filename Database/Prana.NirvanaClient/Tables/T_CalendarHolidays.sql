CREATE TABLE [dbo].[T_CalendarHolidays] (
    [HolidayId]          INT            IDENTITY (1, 1) NOT NULL,
    [CalendarId_FK]      INT            NOT NULL,
    [HolidayDate]        DATETIME       NOT NULL,
    [HolidayDescription] NVARCHAR (MAX) NOT NULL,
    [IsMarketOff]        BIT            CONSTRAINT [DF_T_CalendarHolidays_IsMarketOff] DEFAULT ((0)) NULL,
    [IsSettlementOff]    BIT            CONSTRAINT [DF_T_CalendarHolidays_IsSettlementOff] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_T_CalendarHolidays] PRIMARY KEY CLUSTERED ([HolidayId] ASC),
    CONSTRAINT [FK_T_CalendarHolidays_T_Calendar] FOREIGN KEY ([CalendarId_FK]) REFERENCES [dbo].[T_Calendar] ([CalendarID])
);

