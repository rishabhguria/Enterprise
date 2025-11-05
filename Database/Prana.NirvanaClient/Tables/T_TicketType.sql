CREATE TABLE [dbo].[T_TicketType] (
    [TicketTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [TicketType]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_TicketType] PRIMARY KEY CLUSTERED ([TicketTypeID] ASC)
);

