SET IDENTITY_INSERT T_AuthPrincipalType ON;
Truncate Table T_AuthPrincipalType

INSERT Into T_AuthPrincipalType (TypeId, TypeName, Description) VALUES (1, N'Client',N'Client');
INSERT Into T_AuthPrincipalType (TypeId, TypeName, Description) VALUES (2, N'Role',	N'ClientRole');
INSERT Into T_AuthPrincipalType (TypeId, TypeName, Description) VALUES (3, N'User',	N'User');


SET IDENTITY_INSERT T_AuthPrincipalType OFF;