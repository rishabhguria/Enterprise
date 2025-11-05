CREATE TABLE [dbo].[T_CompanyHandlingInstructions]
(
    [CompanyHandlingInstructionsID] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [CompanyID] INT NOT NULL, 
    [HandlingInstructionsID] INT NOT NULL,
    Foreign Key (CompanyID) REFERENCES T_Company(CompanyID),
    Foreign Key (HandlingInstructionsID) REFERENCES T_HandlingInstructions(HandlingInstructionsID)
)
