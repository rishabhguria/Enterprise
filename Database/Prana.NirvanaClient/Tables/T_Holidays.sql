CREATE TABLE [dbo].[T_Holidays] (
    [HolidayID]   INT          IDENTITY (1, 1) NOT NULL,
    [HolidayDate] DATETIME     NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_Holidays] PRIMARY KEY CLUSTERED ([HolidayID] ASC)
);

