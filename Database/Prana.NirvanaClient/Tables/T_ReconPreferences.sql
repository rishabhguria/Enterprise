CREATE TABLE [dbo].[T_ReconPreferences]
(
	[ReconPreferenceId] INT NOT NULL IDENTITY, 
    [ClientID] INT NOT NULL, 
    [ReconTypeID] INT NOT NULL , 
    [TemplateName] VARCHAR(100) NOT NULL, 
    [TemplateKey] VARCHAR(100) NOT NULL, 
    [IsShowCAGeneratedTrades] BIT NOT NULL DEFAULT 1, 
    PRIMARY KEY ([ReconPreferenceId]), 
    CONSTRAINT [FK_T_ReconPreferences_ToTable] FOREIGN KEY ([ReconTypeID]) REFERENCES [T_ReconType]([ReconTypeID])
)
