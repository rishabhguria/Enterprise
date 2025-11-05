CREATE PROCEDURE [dbo].[P_OTC_GetCustomFields]
@InstrumentType int =0,
@CustomFieldId int=-1
AS
if(@CustomFieldId !=-1)
begin
     select 
    [ID],
	[InstrumentType],
	[Name],
	[DefaultValue],
	SQL_VARIANT_PROPERTY([DefaultValue],'BaseType') as 'BaseType',
	SQL_VARIANT_PROPERTY([DefaultValue],'Precision') AS 'Precision',  
    SQL_VARIANT_PROPERTY([DefaultValue],'Scale') AS 'Scale' , 
	[DataType],
	[UIOrder]
	from T_OTC_CustomFields
	where [ID] = @CustomFieldId 
end
Else 
begin
   select 
    [ID],
	[InstrumentType],
	[Name],
	[DefaultValue],
	SQL_VARIANT_PROPERTY([DefaultValue],'BaseType') as 'BaseType',
	SQL_VARIANT_PROPERTY([DefaultValue],'Precision') AS 'Precision',  
    SQL_VARIANT_PROPERTY([DefaultValue],'Scale') AS 'Scale' , 
	[DataType],
	[UIOrder]
	from T_OTC_CustomFields
	where InstrumentType = @InstrumentType OR @InstrumentType =0
end