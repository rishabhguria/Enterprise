-- Assign values for EsignalFormatString
UPDATE T_AUECMapping SET EsignalFormatString ='{Root}-{EsignalExchangeCode}' WHERE ExchangeIdentifier LIKE '%-Equity'
UPDATE T_AUECMapping SET EsignalFormatString ='O:{Root} {Year}{Month}{StrikePrice}D{Day}-{EsignalExchangeCode}' WHERE ExchangeIdentifier LIKE '%-EquityOption%'
UPDATE T_AUECMapping SET EsignalFormatString ='{Root} {Month}{Year}-{EsignalExchangeCode}' WHERE ExchangeIdentifier LIKE '%-Future'
UPDATE T_AUECMapping SET EsignalFormatString ='{Root} {Month}{Year}{OptionType}{StrikePrice}D{Day}-{EsignalExchangeCode}' WHERE ExchangeIdentifier LIKE '%-FutureOption%'
UPDATE T_AUECMapping SET EsignalFormatString ='${Root}' WHERE ExchangeIdentifier LIKE '%-Indices'