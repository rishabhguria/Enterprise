CREATE procedure [dbo].[P_BTSaveExchangeMapping]
(
@exchangeIDList varchar(50),
@exchangeMappingList varchar(100),
@templateID varchar(200)
)
as
if((select count(*) from T_BTExchangeMapping where TemplateID = @templateID) =0)
insert into T_BTExchangeMapping(ExchangeIDList,ExchangeMappingList,TemplateID)
values(@exchangeIDList,@exchangeMappingList,@templateID)
else 
update T_BTExchangeMapping
set
ExchangeIDList = @exchangeIDList,
ExchangeMappingList = @exchangeMappingList
where
TemplateID = @templateID