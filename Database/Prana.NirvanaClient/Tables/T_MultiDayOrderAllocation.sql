CREATE TABLE [dbo].[T_MultiDayOrderAllocation]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[ClOrderID] VARCHAR (50)   NOT NULL,
	[GroupId] VARCHAR (50) NOT NULL
)
