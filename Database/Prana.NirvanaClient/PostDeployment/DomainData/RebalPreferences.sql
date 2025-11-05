 IF NOT EXISTS(SELECT * 
              FROM   T_RebalPreferences 
              WHERE  PreferenceKey = 'RebalPricingField' AND AccountId = 0) 
  BEGIN 
      INSERT INTO T_RebalPreferences 
                  (AccountId,PreferenceKey, 
                   PreferenceValue) 
      VALUES     (0,'RebalPricingField','2'); 
  END 
   IF NOT EXISTS(SELECT * 
              FROM   T_RebalPreferences 
              WHERE  PreferenceKey = 'AssetClass' AND AccountId = 0) 
  BEGIN 
      INSERT INTO T_RebalPreferences 
                  (AccountId,PreferenceKey, 
                   PreferenceValue) 
      VALUES     (0,'AssetClass','1,2,3'); 
  END
     IF NOT EXISTS(SELECT * 
              FROM   T_RebalPreferences 
              WHERE  PreferenceKey = 'OtherItemsImpactingNAV' AND AccountId = 0) 
  BEGIN 
      INSERT INTO T_RebalPreferences 
                  (AccountId,PreferenceKey, 
                   PreferenceValue) 
      VALUES     (0,'OtherItemsImpactingNAV','{"IsIncludeOtherAssetsMarketValue":false,"IsIncludeCash":true,"IsIncludeAccruals":true,"IsIncludeSwapNavAdjustment":false,"IsIncludeUnrealizedPnlOfSwaps":false}'); 
  END 
  ELSE
     BEGIN
        UPDATE T_RebalPreferences SET PreferenceValue = '{"IsIncludeOtherAssetsMarketValue":false,"IsIncludeCash":true,"IsIncludeAccruals":true,"IsIncludeSwapNavAdjustment":false,"IsIncludeUnrealizedPnlOfSwaps":false}' WHERE PreferenceKey = 'OtherItemsImpactingNAV' AND AccountId = 0
     END 

	  IF NOT EXISTS(SELECT * 
              FROM   T_RebalPreferences 
              WHERE  PreferenceKey = 'RebalAccountGroupVisibilityPref' AND AccountId = 0) 
  BEGIN 
      INSERT INTO T_RebalPreferences 
                  (AccountId,PreferenceKey, 
                   PreferenceValue) 
      VALUES     (0,'RebalAccountGroupVisibilityPref','{"IsAccountIncluded":true,"IsMasterFundIncluded":true,"IsCustomGroupIncluded":true}'); 
  END

   IF NOT EXISTS(SELECT * 
              FROM   T_RebalPreferences 
              WHERE  PreferenceKey = 'RASPrefrence' AND AccountId = 0) 
  BEGIN 
      INSERT INTO T_RebalPreferences 
                  (AccountId,PreferenceKey, 
                   PreferenceValue) 
      VALUES     (0,'RASPrefrence','2'); 
  END 