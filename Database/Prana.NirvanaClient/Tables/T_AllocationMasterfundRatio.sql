CREATE TABLE [dbo].[T_AllocationMasterfundRatio] (
    [pk]             INT        IDENTITY (1, 1) NOT NULL,
    [MasterFundID]   INT        NULL,
    [TargetRatioPct] FLOAT (53) NULL,
    CONSTRAINT [PK_T_AllocationMsterfndRatio] PRIMARY KEY CLUSTERED ([pk] ASC)
);

