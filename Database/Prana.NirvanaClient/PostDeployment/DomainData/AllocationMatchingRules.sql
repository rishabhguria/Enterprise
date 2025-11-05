EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";
delete from T_AL_MatchingRule;

	
INSERT INTO T_AL_MatchingRule(Id, MatchingRule, Description)	VALUES(1,	'None',				'Allocation will be done without considering any historical allocation');
INSERT INTO T_AL_MatchingRule(Id, MatchingRule, Description)	VALUES(2,	'SinceLastChange',	'Allocation will be done considering historical alllocation only since last change in pref');
INSERT INTO T_AL_MatchingRule(Id, MatchingRule, Description)	VALUES(3,	'SinceInception',	'Allocation will be done considering all historical alllocation done');
INSERT INTO T_AL_MatchingRule(Id, MatchingRule, Description)	VALUES(4,	'Prorata',			'Allocation will be done considering state percentage');
INSERT INTO T_AL_MatchingRule(Id, MatchingRule, Description)	VALUES(5,	'Leveling',			'Allocation will be done considering fund NAV');
INSERT INTO T_AL_MatchingRule(Id, MatchingRule, Description)	VALUES(6,	'ProrataByNAV',		'Allocation will be done considering the NAV value of each fund');


EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";