CREATE TABLE [dbo].[T_MailSetting] (
    [MailSettingID]   INT           IDENTITY (1, 1) NOT NULL,
    [From]            VARCHAR (50)  NOT NULL,
    [To]              VARCHAR (200) NOT NULL,
    [CarbonCopy]      VARCHAR (200) NULL,
    [BlankCarbonCopy] VARCHAR (200) NULL,
    [Subject]         VARCHAR (50)  NOT NULL,
    [Body]            TEXT          NOT NULL,
    [SMTPServer]      VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_T_MailSetting] PRIMARY KEY CLUSTERED ([MailSettingID] ASC)
);

