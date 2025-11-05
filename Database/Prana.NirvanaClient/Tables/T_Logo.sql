CREATE TABLE [dbo].[T_Logo] (
    [LogoID]    INT          IDENTITY (1, 1) NOT NULL,
    [LogoName]  VARCHAR (50) NOT NULL,
    [LogoImage] IMAGE        NOT NULL,
    CONSTRAINT [PK_T_Logo] PRIMARY KEY CLUSTERED ([LogoID] ASC)
);

