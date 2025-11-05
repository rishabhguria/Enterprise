CREATE TABLE [dbo].[T_MultiDayClOrderIDMapping]
(
	[latestClOrderID] VARCHAR (50)  primary key,
	[parentClOrderID] VARCHAR (50) NOT NULL
)
