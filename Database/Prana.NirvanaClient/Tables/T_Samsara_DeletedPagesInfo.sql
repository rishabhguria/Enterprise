-- [T_Samsara_DeletedPagesInfo] table for storing information about deleted pages for Company users. 

CREATE TABLE [dbo].[T_Samsara_DeletedPagesInfo] (  
 [UserID] int NOT NULL,  
 [PageId] varchar(100) NOT NULL,  
 [PageName] varchar(100) NOT NULL,  
 [DeleteTime] datetime NOT NULL  
);