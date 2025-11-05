CREATE TABLE [dbo].[T_ScheduleTypes] (
    [ScheduleID]   INT          IDENTITY (1, 1) NOT NULL,
    [ScheduleName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_ScheduleTypes] PRIMARY KEY CLUSTERED ([ScheduleID] ASC)
);

