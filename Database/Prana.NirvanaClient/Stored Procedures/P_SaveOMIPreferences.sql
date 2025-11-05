-----------------------------------------------------------------
--Modified BY: Kuldeep Agrawal
--Date: 22-may-14
--Purpose: Added a preference on PI to use Default Delta.
-----------------------------------------------------------------

CREATE PROCEDURE P_SaveOMIPreferences          
(       
@SelectedFeedPrice			INT,          
@OptionSelectedFeedPrice	INT,
@FeedPxCheckOptions			INT,      
@FeedPxCheckOthers			INT,    
@UseClosingMark				BIT,
@UseDefaultDelta			INT,
@FeedPxConditionOptions		INT,  
@FeedPxConditionOthers		INT,	
@PriceBarOptions			DECIMAL(32,19),
@PriceBarOthers				DECIMAL(32,19), 
@OverrideCheckOptions		INT,
@OverrideCheckOthers		INT       
)          
AS          
          
DELETE FROM T_OMIPreferences      
    
INSERT INTO T_OMIPreferences(SelectedFeedPrice,OptionSelectedFeedPrice,FeedPxCheckOptions,FeedPxCheckOthers,UseClosingMark,UseDefaultDelta, FeedPxConditionOptions, FeedPxConditionOthers, PriceBarOptions, PriceBarOthers, OverrideCheckOptions, OverrideCheckOthers)             
VALUES(@SelectedFeedPrice,@OptionSelectedFeedPrice,@FeedPxCheckOptions,@FeedPxCheckOthers,@UseClosingMark,@UseDefaultDelta, @FeedPxConditionOptions, @FeedPxConditionOthers, @PriceBarOptions, @PriceBarOthers, @OverrideCheckOptions, @OverrideCheckOthers)   

