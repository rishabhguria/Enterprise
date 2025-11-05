/*
This script merges the temp table into the main table WITHOUT modifing the current prefrences
*/


select * into #Temp_T_PranaKeyValuePreferences from T_PranaKeyValuePreferences where 0 = 1;

SET IDENTITY_INSERT #Temp_T_PranaKeyValuePreferences ON;
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(1,	'ReleaseViewType',						0,	'0 for Prana, 1 for Custum House, 2 for common');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(2,	'IsFeederSetupAllowed',					0,	'0 for disallowed and 1 for allow');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(3,	'IsFundManyToManyMappingAllowed',		0,	'0 for disallowed and 1 for allow');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(4,	'IsStrategyManyToManyMappingAllowed',	0,	'0 for disallowed and 1 for allow');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(5,	'PricingSource',						0,	'0 for Esignal, 1 for Bloomberg, 2 for FactSet, 3 for ACTIV');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(6,	'IsFeederFundEnabled',					0,	'0 for disabled and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(7,	'IsNAVLockingEnabled',					0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(8,	'IsFundLockingEnabled',					0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(9,	'IsPermanentDeletion',					1,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(10,	'IsFundPermissionByGroup',				0,	'0 for No and 1 for Yes');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(11,	'SettlementAutoCalculateField',			0,	'0 for SettlementAmount,1 for SettlementFXRate,2 for AveragePrice');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(12,	'IsZeroCommissionAndFeesForSwaps',		0,	'0 for No and 1 for Yes');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(13,	'IsWindowUserReq',						0,	'0 for No and 1 for Yes');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(14,	'AvgPriceRounding',						-1,	'-1 for No Rounding and 0-10 for rounding digits');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(15,	'IsShowMasterFundonTT',			     	0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(16,	'IsShowmasterFundAsClient',		    	0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(17,	'IsEquityOptionManualValidation',		0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(18,	'IsCollateralMarkPriceValidation',		0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(19,	'IsShowmasterFundOnShortLocate',		0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(20,	'IsImportOverrideOnShortLocate',		0,	'0 for disables and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(21,	'IsFilePricingForTouch',				0,	'0 for disabled and 1 to use file prices for touch');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(22,	'Disclaimer',		'All reporting, data management services and software are provided on an "as is" basis without warranty of any kind. Customer''s sole remedy for defective software shall be replacement of the same. Customer''s sole remedy for defective application service shall be to terminate the applicable agreement. Each party hereby expressly disclaims all other warranties, whether express or implied. In no event will either party be liable for any indirect, incidental, special or consequential damages, including without limitation damages for loss of profits, lost business, or lost data, incurred by the other party or any third party, whether in action in contract or tort, even if such party has been advised of the possibility of such damages, including without limitation any such damages resulting from or relating to: (i) A user''s inability to transmit or re-transmit orders, or transmit or re-transmit a message; (ii) Any order executed or not executed or any change in price prior to or after any such execution or non-execution; (iii) Failure, malfunction, delay, or interruption of service or in any hardware or software; (iv) A user''s use of (or inability to use) any "Position Management," "Risk Management," "Portfolio Analytics," or other, similar module or function of the service.',	'Prana Application Disclaimer');
    insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(23,  'IsPriceEnterTillSettlementDateInDailyValuation',0,'0 for disabled and 1 for enabled');
	insert into #Temp_T_PranaKeyValuePreferences(ID, PreferenceKey,PreferenceValue,Description) values(24,  'RevaluationDailyProcessDays',0,'The revaluation process started for back dated enteries for the account in daily process mode will run till the T - {provided days}');
SET IDENTITY_INSERT #Temp_T_PranaKeyValuePreferences OFF


--SET IDENTITY_INSERT T_PranaKeyValuePreferences ON;	
-- if not exist
		insert into T_PranaKeyValuePreferences(PreferenceKey,PreferenceValue,Description)
		select 
		#Temp_T_PranaKeyValuePreferences.PreferenceKey, #Temp_T_PranaKeyValuePreferences.PreferenceValue, #Temp_T_PranaKeyValuePreferences.Description
		from #Temp_T_PranaKeyValuePreferences 
		left outer join  T_PranaKeyValuePreferences 
		on #Temp_T_PranaKeyValuePreferences.PreferenceKey = T_PranaKeyValuePreferences.PreferenceKey
		where 
		T_PranaKeyValuePreferences.PreferenceKey is null 

-- remove if not a part of temp
		delete T_PranaKeyValuePreferences from 
		T_PranaKeyValuePreferences 
		left outer join  #Temp_T_PranaKeyValuePreferences 
		on #Temp_T_PranaKeyValuePreferences.PreferenceKey = T_PranaKeyValuePreferences.PreferenceKey
		where 
		#Temp_T_PranaKeyValuePreferences.PreferenceKey is null

		--update Disclaimer to default if not set
		if exists (select * from T_PranaKeyValuePreferences where PreferenceKey ='Disclaimer' and PreferenceValue ='Disclaimer')
		begin
		update T_PranaKeyValuePreferences
		set PreferenceValue ='All reporting, data management services and software are provided on an "as is" basis without warranty of any kind. Customer''s sole remedy for defective software shall be replacement of the same. Customer''s sole remedy for defective application service shall be to terminate the applicable agreement. Each party hereby expressly disclaims all other warranties, whether express or implied. In no event will either party be liable for any indirect, incidental, special or consequential damages, including without limitation damages for loss of profits, lost business, or lost data, incurred by the other party or any third party, whether in action in contract or tort, even if such party has been advised of the possibility of such damages, including without limitation any such damages resulting from or relating to: (i) A user''s inability to transmit or re-transmit orders, or transmit or re-transmit a message; (ii) Any order executed or not executed or any change in price prior to or after any such execution or non-execution; (iii) Failure, malfunction, delay, or interruption of service or in any hardware or software; (iv) A user''s use of (or inability to use) any "Position Management," "Risk Management," "Portfolio Analytics," or other, similar module or function of the service.'
		where PreferenceKey ='Disclaimer' 
		
		end

--SET IDENTITY_INSERT T_PranaKeyValuePreferences OFF

-- Dropping the temporary table
drop table #Temp_T_PranaKeyValuePreferences
