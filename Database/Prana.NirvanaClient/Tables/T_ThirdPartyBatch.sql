CREATE TABLE [dbo].[T_ThirdPartyBatch] (
    [ThirdPartyBatchId]   INT           IDENTITY (1, 1) NOT NULL,
    [Description]         NVARCHAR (50) NOT NULL,
    [ThirdPartyTypeId]    INT           NULL,
    [ThirdPartyId]        INT           NULL,
    [ThirdPartyFormatId]  INT           NULL,
    [IsLevel2Data]        BIT           NULL,
    [Active]              BIT           NULL,
    [AllowedFixTransmission] BIT        NULL,
    [ThirdPartyCompanyId] INT           NULL,
    [GnuPGId]             INT           NULL,
    [FtpId]               INT           NULL,
    [EmailDataId]         INT           NULL,
    [EmailLogId]          INT           NULL,
    [IncludeSent]         BIT           NULL,
    CONSTRAINT [PK_T_ThirdPartyBatch] PRIMARY KEY CLUSTERED ([ThirdPartyBatchId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_T_ThirdPartyBatch]
    ON [dbo].[T_ThirdPartyBatch]([ThirdPartyFormatId] ASC, [ThirdPartyCompanyId] ASC, [ThirdPartyId] ASC, [ThirdPartyTypeId] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique Index', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'T_ThirdPartyBatch', @level2type = N'INDEX', @level2name = N'IX_T_ThirdPartyBatch';

