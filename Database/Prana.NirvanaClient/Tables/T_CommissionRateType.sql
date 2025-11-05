CREATE TABLE [dbo].[T_CommissionRateType] (
    [CommissionRateID]   INT          NOT NULL,
    [CommissionRateType] VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_T_CommissionRateType] PRIMARY KEY CLUSTERED ([CommissionRateID] ASC)
);

