CREATE TABLE [dbo].[T_VenuType] (
    [VenueTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [VenuType]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_VenuType] PRIMARY KEY CLUSTERED ([VenueTypeID] ASC)
);

