CREATE TABLE [dbo].[T_ThirdPartyAllocationFile] (
    [ThirdPartyAllocationFileId] INT           IDENTITY (1, 1) NOT NULL,
    [ThirdPartyId]               INT           NOT NULL,
    [FileName]                   VARCHAR (100) NOT NULL,
    [FileId]                     BIGINT        NOT NULL,
    [TimeOfSave]                 DATETIME      NOT NULL
);

