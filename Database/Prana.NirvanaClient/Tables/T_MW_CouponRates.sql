CREATE TABLE [dbo].[T_MW_CouponRates] (
    [Symbol]         VARCHAR (50) NOT NULL,
    [CouponPayments] INT          NULL,
    [Coupon]         FLOAT (53)   NULL,
    [FaceValue]      FLOAT (53)   NULL,
    [DaysToMaturity] INT          NULL,
    [FormulaType]    INT          NOT NULL
);

