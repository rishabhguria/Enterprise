CREATE TABLE [dbo].[T_Ticket] (
    [TicketID]    INT          IDENTITY (1, 1) NOT NULL,
    [TicketName]  VARCHAR (50) NOT NULL,
    [DisplayName] VARCHAR (50) NOT NULL,
    [TicketType]  INT          NOT NULL,
    [UserID]      INT          NOT NULL,
    CONSTRAINT [PK_T_Ticket] PRIMARY KEY CLUSTERED ([TicketID] ASC)
);

