CREATE TABLE [dbo].[T_CompanyExecutionsInstructions]
(
    [CompanyExecutionInstructionsID] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [CompanyID] INT NOT NULL, 
    [ExecutionInstructionsID] INT NOT NULL,
    Foreign Key (CompanyID) REFERENCES T_Company(CompanyID),
    Foreign Key (ExecutionInstructionsID) REFERENCES T_ExecutionInstructions(ExecutionInstructionsID)
)
