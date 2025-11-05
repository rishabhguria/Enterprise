CREATE TABLE [dbo].[T_Vendor] (
    [VendorID]         INT           IDENTITY (1, 1) NOT NULL,
    [VendorName]       VARCHAR (50)  NULL,
    [ContactLastName]  VARCHAR (50)  NULL,
    [ContactFirstName] VARCHAR (50)  NOT NULL,
    [ShortName]        VARCHAR (50)  NULL,
    [Title]            VARCHAR (50)  NULL,
    [Product]          VARCHAR (50)  NULL,
    [Comment]          VARCHAR (200) NULL,
    [MailingAddress]   VARCHAR (100) NULL,
    [EMail]            VARCHAR (50)  NULL,
    [TelphoneWork]     VARCHAR (50)  NULL,
    [TelphoneHome]     VARCHAR (50)  NULL,
    [TelphoneMobile]   VARCHAR (50)  NULL,
    [Pager]            VARCHAR (50)  NULL,
    [Fax]              VARCHAR (50)  NULL,
    [Address1]         VARCHAR (100) NULL,
    [Address2]         VARCHAR (100) NULL,
    CONSTRAINT [PK_T_Vendor] PRIMARY KEY CLUSTERED ([VendorID] ASC)
);

