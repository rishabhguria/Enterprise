CREATE TABLE [dbo].[T_PricingDataType] (
    [DataTypeID]   INT          IDENTITY (1, 1) NOT NULL,
    [DataTypeName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_PricingDataType] PRIMARY KEY CLUSTERED ([DataTypeID] ASC)
);

