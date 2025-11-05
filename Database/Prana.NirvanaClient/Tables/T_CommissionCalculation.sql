CREATE TABLE [dbo].[T_CommissionCalculation] (
    [CommissionCalculationID] INT          NOT NULL,
    [CalculationType]         VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_T_CommissionCalculation] PRIMARY KEY CLUSTERED ([CommissionCalculationID] ASC)
);

