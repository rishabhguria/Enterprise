CREATE TABLE [dbo].[T_AUECHolidays] (
    [HolidayID]       INT          IDENTITY (1, 1) NOT NULL,
    [AUECID]          INT          NOT NULL,
    [HolidayDate]     DATETIME     NOT NULL,
    [Description]     VARCHAR (50) NOT NULL,
    [IsMarketOff]     BIT          CONSTRAINT [DF_T_AUECHolidays_IsMarketOff] DEFAULT ((0)) NULL,
    [IsSettlementOff] BIT          CONSTRAINT [DF_T_AUECHolidays_IsSettlementOff] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_T_AUECHoliday] PRIMARY KEY CLUSTERED ([HolidayID] ASC)
);

