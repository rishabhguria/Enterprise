CREATE TABLE [dbo].[T_FundWiseUDAData] (
    [FundID]            INT           NOT NULL,
    [Symbol_pk]         VARCHAR (100) NOT NULL,
    [UDAAssetClassID]   INT           NOT NULL,
    [UDASecurityTypeID] INT           NOT NULL,
    [UDASectorID]       INT           NOT NULL,
    [UDASubSectorID]    INT           NOT NULL,
    [UDACountryID]      INT           NOT NULL,
    [IsApproved]        BIT           NULL,
    [PrimarySymbol]     VARCHAR (100) NULL,
    [ModifiedBy]        VARCHAR (100) NULL,
    [ApprovedBy]        VARCHAR (100) NULL,
    [CreationDate]      DATETIME      NULL,
    [ModifiedDate]      DATETIME      NULL,
    [CreatedBy]         VARCHAR (100) NULL,
    [isActive]          BIT           DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_T_FundWiseUDAData] PRIMARY KEY CLUSTERED ([FundID] ASC, [Symbol_pk] ASC)
);

