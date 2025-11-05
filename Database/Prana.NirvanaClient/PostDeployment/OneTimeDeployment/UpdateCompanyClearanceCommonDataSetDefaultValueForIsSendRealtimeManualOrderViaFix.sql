
IF EXISTS (
		SELECT *
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_CompanyClearanceCommonData'
			AND COLUMN_NAME = 'IsSendRealtimeManualOrderViaFix'
		)
		begin
		update T_CompanyClearanceCommonData set IsSendRealtimeManualOrderViaFix= 0 where IsSendRealtimeManualOrderViaFix is null

		end