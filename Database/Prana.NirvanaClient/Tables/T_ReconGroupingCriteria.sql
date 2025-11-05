CREATE TABLE [dbo].[T_ReconGroupingCriteria] (
    [ID]              INT           NOT NULL,
    [ReconTypeID_FK]  INT           NOT NULL,
    [GroupingColumns] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_T_ReconGroupingCriteria] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_T_ReconGroupingCriteria_T_ReconType] FOREIGN KEY ([ReconTypeID_FK]) REFERENCES [dbo].[T_ReconType] ([ReconTypeID]),
    CONSTRAINT [constraintname3] UNIQUE NONCLUSTERED ([ReconTypeID_FK] ASC)
);

