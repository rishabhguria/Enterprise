CREATE TABLE [dbo].[T_User] (
    [UserID]         INT          IDENTITY (1, 1) NOT NULL,
    [LastName]       VARCHAR (50) NULL,
    [FirstName]      VARCHAR (50) NOT NULL,
    [ShortName]      VARCHAR (50) NOT NULL,
    [Title]          VARCHAR (50) NULL,
    [EMail]          VARCHAR (50) NOT NULL,
    [TelphoneWork]   VARCHAR (50) NOT NULL,
    [TelphoneHome]   VARCHAR (50) NULL,
    [TelphoneMobile] VARCHAR (50) NULL,
    [Fax]            VARCHAR (50) NULL,
    [Login]          VARCHAR (20) NOT NULL,
    [Password]       VARCHAR (20) NOT NULL,
    [TelphonePager]  VARCHAR (50) NULL,
    [Address1]       VARCHAR (50) NOT NULL,
    [Address2]       VARCHAR (50) NULL,
    [CountryID]      INT          NOT NULL,
    [StateID]        INT          NOT NULL,
    [Zip]            VARCHAR (50) NULL,
    [SuperUser]      INT          NULL,
    [City]           VARCHAR (50) NULL,
    CONSTRAINT [PK_T_User] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

