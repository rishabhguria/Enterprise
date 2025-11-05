CREATE TABLE [dbo].[T_CA_OtherCompliancePermission] (
    [CompanyId]            INT NOT NULL,
    [UserId]               INT NOT NULL,
    [IsPreTradeEnabled]    BIT NOT NULL,
    [IsOverridePermission] BIT NOT NULL,
    [PowerUser]            BIT NOT NULL,
    [IsApplyToManual]      BIT NOT NULL,
    [Trading]              BIT DEFAULT ('False') NOT NULL,
    [Staging]              BIT DEFAULT ('False') NOT NULL, 
    [DefaultPrePopUp] BIT NULL DEFAULT ('True'), 
    [DefaultPostPopUp] BIT NULL DEFAULT ('True'), 
    [DefaultRuleOverrideType] INT NULL DEFAULT 1,
	[EnableBasketComplianceCheck]  BIT DEFAULT ('TRUE') NOT NULL,
);

