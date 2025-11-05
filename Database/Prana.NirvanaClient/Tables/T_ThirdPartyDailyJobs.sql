CREATE TABLE [dbo].[T_ThirdPartyDailyJobs]
(
	[JobId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
    [ThirdPartyBatchId] INT NOT NULL, 
    [BatchRunDate] DATETIME NOT NULL,
    [TransmissionType] INT NOT NULL
)
