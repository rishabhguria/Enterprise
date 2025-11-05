CREATE TABLE [dbo].[T_ClosingAlgos] (
    [AlgorithmId]      INT           NOT NULL,
    [AlgorithmAcronym] VARCHAR (20)  NOT NULL,
    [AlgorithmDesc]    VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_T_ClosingAlgos] PRIMARY KEY CLUSTERED ([AlgorithmId] ASC)
);

