CREATE TABLE [dbo].[T_CompanyCompliance] (
    [CompanyComplianceID] INT IDENTITY (1, 1) NOT NULL,
    [FixVersionID]        INT NOT NULL,
    [FixCapabilityID]     INT NOT NULL,
    [CompanyID]           INT NOT NULL,
    CONSTRAINT [PK_T_CompanyCompliance] PRIMARY KEY CLUSTERED ([CompanyComplianceID] ASC),
    CONSTRAINT [FK_T_CompanyCompliance_T_FIX] FOREIGN KEY ([FixVersionID]) REFERENCES [dbo].[T_FIX] ([FIXID]),
    CONSTRAINT [FK_T_CompanyCompliance_T_FIX1] FOREIGN KEY ([FixVersionID]) REFERENCES [dbo].[T_FIX] ([FIXID]),
    CONSTRAINT [FK_T_CompanyCompliance_T_FIXCapability] FOREIGN KEY ([FixCapabilityID]) REFERENCES [dbo].[T_FIXCapability] ([FixCapabilityID])
);

