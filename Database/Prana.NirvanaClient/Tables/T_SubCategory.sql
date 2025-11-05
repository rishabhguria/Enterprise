CREATE TABLE [dbo].[T_SubCategory] (
    [SubCategoryID]    INT          NOT NULL,
    [SubCategoryName]  VARCHAR (50) NULL,
    [MasterCategoryID] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([SubCategoryID] ASC)
);

