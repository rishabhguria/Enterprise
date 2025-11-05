CREATE TABLE [dbo].[PM_Reports] (
    [ReportID]   INT            NOT NULL,
    [ReportName] NVARCHAR (100) NOT NULL,
    [ReportLink] NVARCHAR (400) NOT NULL,
    [Flag]       BIT            CONSTRAINT [DF_PM_Reports_Flag] DEFAULT ((1)) NOT NULL,
    [SectionID]  INT            NOT NULL,
    CONSTRAINT [PK_PM_Reports] PRIMARY KEY CLUSTERED ([ReportID] ASC),
    CONSTRAINT [FK_PM_Reports_PM_Reports] FOREIGN KEY ([SectionID]) REFERENCES [dbo].[PM_ReportSections] ([SectionID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'For External or Internal Report ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_Reports', @level2type = N'COLUMN', @level2name = N'Flag';

