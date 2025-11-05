CREATE TABLE [dbo].[PM_MarkPricePreferences] (
    [PM_MarkPricePreferenceID] INT          IDENTITY (1, 1) NOT NULL,
    [PriceToBeUsed]            VARCHAR (50) CONSTRAINT [DF__PM_MarkPr__Price__0861DE5C] DEFAULT ('Last') NOT NULL,
    [UseAppMark]               BIT          CONSTRAINT [DF__PM_MarkPr__UseAp__09560295] DEFAULT ((1)) NOT NULL
);

