CREATE TABLE [dbo].[T_UDA_DynamicUDAData] (
    [IndexID]          INT            IDENTITY (1, 1) NOT NULL,
    [Symbol_PK]        BIGINT         NOT NULL,
    [UDAData]          XML            NULL,
    [FundID]           INT            NOT NULL,
    [Analyst]          NVARCHAR (200) NULL,
    [CountryOfRisk]    NVARCHAR (200) NULL,
    [CustomUDA1]       NVARCHAR (200) NULL,
    [CustomUDA2]       NVARCHAR (200) NULL,
    [CustomUDA3]       NVARCHAR (200) NULL,
    [CustomUDA4]       NVARCHAR (200) NULL,
    [CustomUDA5]       NVARCHAR (200) NULL,
    [CustomUDA6]       NVARCHAR (200) NULL,
    [CustomUDA7]       NVARCHAR (200) NULL,
    [Issuer]           NVARCHAR (200) NULL,
    [LiquidTag]        NVARCHAR (200) NULL,
    [MarketCap]        NVARCHAR (200) NULL,
    [Region]           NVARCHAR (200) NULL,
    [RiskCurrency]     NVARCHAR (200) NULL,
    [UCITSEligibleTag] NVARCHAR (200) NULL,
    [CustomUDA8]       NVARCHAR (200) NULL,
    [CustomUDA9]       NVARCHAR (200) NULL,
    [CustomUDA10]       NVARCHAR (200) NULL,
    [CustomUDA11]       NVARCHAR (200) NULL,
    [CustomUDA12]       NVARCHAR (200) NULL,
    CONSTRAINT [pk_DynamicUDAData] PRIMARY KEY CLUSTERED ([Symbol_PK] ASC, [FundID] ASC)
);


GO
CREATE PRIMARY XML INDEX [PXML_T_UDA_DynamicUDAData_UDAData]
    ON [dbo].[T_UDA_DynamicUDAData]([UDAData])
    WITH (PAD_INDEX = OFF);

