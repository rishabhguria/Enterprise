CREATE TABLE [dbo].[T_CalendarAUEC] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [AUECID]     INT NOT NULL,
    [CalendarID] INT NOT NULL,
    CONSTRAINT [PK_T_CalendarAUEC] PRIMARY KEY CLUSTERED ([ID] ASC)
);

