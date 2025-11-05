CREATE TABLE [dbo].[T_SMFixedIncomeData] (
    [IssueDate]         DATETIME     NULL,
    [Coupon]            FLOAT (53)   NOT NULL,
    [MaturityDate]      DATETIME     NOT NULL,
    [BondTypeID]        INT          NOT NULL,
    [AccrualBasisID]    INT          NOT NULL,
    [Symbol_PK]         BIGINT       NOT NULL,
    [BondDescription]   VARCHAR (200) NOT NULL,
    [FirstCouponDate]   DATETIME     NULL,
    [IsZero]            BIT          NOT NULL,
    [CouponFrequencyID] INT          NOT NULL,
    [DaysToSettlement]  INT          NOT NULL,
    [Multiplier]        FLOAT (53)   NOT NULL,
    [LeveragedFactor]   FLOAT (53)   NULL,
	[CollateralTypeID]	INT          NULL
    CONSTRAINT [FK_T_SMFixedIncomeData_T_SMSymbolLookUpTable] FOREIGN KEY ([Symbol_PK]) REFERENCES [dbo].[T_SMSymbolLookUpTable] ([Symbol_PK])
);

