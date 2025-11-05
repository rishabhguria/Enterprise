CREATE TABLE [dbo].[T_BatchSchedulers] (
    [BatchSchedulerID]     INT            NOT NULL,
    [PriceTolerance]       DECIMAL (5, 2) NULL,
    [ScheduleTypeID]       INT            NULL,
    [CronExpression]       VARCHAR (MAX)  NULL,
    [ThirdPartyID]         INT            NOT NULL,
    [EnablePriceTolerance] BIT            DEFAULT ((0)) NOT NULL,
    [FormatName]           VARCHAR (200)  NULL,
    [AutoExecution]        BIT            DEFAULT ((0)) NOT NULL,
    [ExecutionTime]        VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_T_BatchSchedulers] PRIMARY KEY CLUSTERED ([BatchSchedulerID] ASC)
);

