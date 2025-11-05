CREATE TABLE [dbo].[T_CA_RuleUserPermissions]
(
	[Id] int Identity(1,1) not null,
    [RuleId] VARCHAR(50) NOT NULL, 
    [ShowPopup] BIT NULL , 
    [UserId] INT NOT NULL,     
    [RuleOverrideType] INT NULL DEFAULT 1, 
    CONSTRAINT [PK_T_CA_RuleUserPermissions] PRIMARY KEY ([Id])
)
