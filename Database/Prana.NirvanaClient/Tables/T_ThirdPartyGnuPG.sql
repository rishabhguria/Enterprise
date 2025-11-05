CREATE TABLE [dbo].[T_ThirdPartyGnuPG] (
    [GnuPGId]              INT            IDENTITY (1, 1) NOT NULL,
    [GnuPGName]            NVARCHAR (50)  NOT NULL,
    [HomeDirectory]        NVARCHAR (255) NOT NULL,
    [Command]              INT            NULL,
    [UseCmdBatch]          BIT            NOT NULL,
    [UseCmdYes]            BIT            NOT NULL,
    [UseCmdArmor]          BIT            NOT NULL,
    [VerboseLevel]         INT            NOT NULL,
    [Recipient]            NVARCHAR (255) NOT NULL,
    [Originator]           NVARCHAR (255) NOT NULL,
    [PassPhrase]           NVARCHAR (255) NULL,
    [PassPhraseDescriptor] NVARCHAR (255) NULL,
    [Timeout]              INT            NOT NULL,
    [Enabled]              BIT            NOT NULL,
    [ExtensionToAdd]       VARCHAR (20)   NULL,
    CONSTRAINT [PK_T_ThirdPartyGnuPG] PRIMARY KEY CLUSTERED ([GnuPGId] ASC)
);

