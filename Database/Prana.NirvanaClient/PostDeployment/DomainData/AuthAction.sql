SET IDENTITY_INSERT T_AuthAction ON;
truncate table T_AuthAction ;

INSERT T_AuthAction (AuthActionId, AuthActionName, Description) VALUES (1, N'None',		N'None');
INSERT T_AuthAction (AuthActionId, AuthActionName, Description) VALUES (2, N'Read',		N'Read');
INSERT T_AuthAction (AuthActionId, AuthActionName, Description) VALUES (3, N'Write',	N'Write');
INSERT T_AuthAction (AuthActionId, AuthActionName, Description) VALUES (4, N'Approve',	N'Approve');


SET IDENTITY_INSERT T_AuthAction OFF;