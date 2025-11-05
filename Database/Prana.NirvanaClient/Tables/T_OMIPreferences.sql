CREATE TABLE [dbo].[T_OMIPreferences] (
    [SelectedFeedPrice]           INT NULL,
    [OptionSelectedFeedPrice]     INT NULL,
    [UseClosingMark]              BIT NULL,
    [FeedPxCheckOptions]          INT NULL,
    [FeedPxCheckOthers]           INT NULL,
    [UseDefaultDelta]             BIT DEFAULT ((0)) NOT NULL,
    [FeedPxConditionOptions]      INT NULL,
    [FeedPxConditionOthers]		  INT NULL,
    [PriceBarOptions]			  DECIMAL(32,19) NULL,
    [PriceBarOthers]			  DECIMAL(32,19) NULL,
	[OverrideCheckOptions]		  INT NULL,
	[OverrideCheckOthers]		  INT NULL
);

