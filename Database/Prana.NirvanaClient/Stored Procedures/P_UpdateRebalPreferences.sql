CREATE PROCEDURE [dbo].[P_UpdateRebalPreferences]
    @accountId int,
	@rebalPreferenceKey varchar(200),
	@rebalPreferenceValue nvarchar(Max)
AS
BEGIN
 IF NOT EXISTS(SELECT * 
              FROM   T_RebalPreferences 
              WHERE  PreferenceKey = @rebalPreferenceKey AND AccountId = @accountId) 
  BEGIN 
      INSERT INTO T_RebalPreferences 
                  (AccountId,PreferenceKey, 
                   PreferenceValue) 
      VALUES     (@accountId,@rebalPreferenceKey,@rebalPreferenceValue); 
  END 
 ELSE
  BEGIN
     UPDATE T_RebalPreferences SET PreferenceValue = @rebalPreferenceValue WHERE PreferenceKey IN (@rebalPreferenceKey) AND AccountId IN (@accountId)
  END
END
