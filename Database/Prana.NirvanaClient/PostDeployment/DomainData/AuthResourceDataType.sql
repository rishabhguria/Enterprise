SET IDENTITY_INSERT T_AuthResourceDataType ON;
Truncate Table T_AuthResourceDataType;

INSERT Into T_AuthResourceDataType (ResourceDataTypeId, ResourceDataTypeName, Description) VALUES (1, N'FundGroup', N'FundGroup');
INSERT Into T_AuthResourceDataType (ResourceDataTypeId, ResourceDataTypeName, Description) VALUES (2, N'Modules', N'Modules');

SET IDENTITY_INSERT T_AuthResourceDataType OFF;