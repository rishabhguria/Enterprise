CREATE TABLE [dbo].[T_BTUpLoadedBasketIDS] (
    [UploadedBasketIDs] VARCHAR (200) NULL,
    [userID]            INT           NOT NULL,
    CONSTRAINT [PK_T_BTUpLoadedBasketIDS_1] PRIMARY KEY CLUSTERED ([userID] ASC)
);

