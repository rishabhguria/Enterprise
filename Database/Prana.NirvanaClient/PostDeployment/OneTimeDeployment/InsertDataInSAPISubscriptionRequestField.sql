/*

Post-Deployment Script Template	

*/


TRUNCATE TABLE T_SAPISubscriptionRequestField;

INSERT INTO T_SAPISubscriptionRequestField(NirvanaFields, BBGMnemonic, Equity, EquityOption, Future, FutureOption, FX, FixedIncome, FXForward)
VALUES
('VWAP', 'VWAP', 0, 0, 0, 0, 0, 0, 0),
('CusipNo', 'CUSIP_NUMBER_REALTIME', 1, 1, 1, 0, 0, 1, 0),
('SedolSymbol', 'ID_SEDOL1', 0, 0, 0, 0, 0, 0, 0),
('ISIN', 'ISIN_REALTIME', 0, 0, 0, 0, 0, 0, 0),
('AverageVolume20Day', '20_DAY_AVERAGE_VOLUME_AT_TIME_RT', 0, 0, 0, 0, 0, 0, 0),
('Volume10DAvg', '10_DAY_AVERAGE_VOLUME_AT_TIME_RT', 0, 0, 0, 0, 0, 0, 0),
('PutOrCall', 'PUT_CALL_INDICATOR_REALTIME', 0, 1, 0, 1, 0, 0, 0),
('OpenInterest', 'RT_OPEN_INTEREST', 0, 1, 0, 1, 0, 0, 0),
('ExpirationDate', 'EXPIRATION_DATE_REALTIME', 0, 1, 1, 1, 0, 0, 0),
('StrikePrice', 'STRIKE_PRICE_REALTIME', 0, 1, 0, 1, 0, 0, 0),
('LastPrice', 'LAST_PRICE', 1, 1, 1, 1, 1, 1, 1),
('TotalVolume', 'VOLUME', 1, 0, 0, 0, 0, 0, 0),
('TradeVolume', 'VOLUME', 0, 0, 0, 0, 0, 0, 0),
('Bid', 'BID', 1, 1, 1, 1, 1, 1, 1),
('BidSize', 'BID_SIZE', 1, 1, 1, 1, 1, 0, 0),
('MarketCapitalization ', 'CUR_MKT_CAP', 0, 0, 0, 0, 0, 0, 0),
('BidExchange', 'EXCH_CODE_BID', 0, 0, 0, 0, 0, 0, 0),
('Ask', 'ASK', 1, 1, 1, 1, 1, 1, 1),
('AskSize', 'ASK_SIZE', 1, 1, 1, 1, 1, 0, 0),
('AskExchange', 'EXCH_CODE_ASK', 0, 0, 0, 0, 0, 0, 0),
('High', 'HIGH', 1, 1, 1, 1, 1, 1, 1),
('Low', 'LOW', 1, 1, 1, 1, 1, 1, 1),
('High52W', 'PRICE_52_WEEK_HIGH_RT', 0, 0, 0, 0, 0, 0, 0),
('Low52W', 'PRICE_52_WEEK_LOW_RT', 0, 0, 0, 0, 0, 0, 0),
('OpenInterestDelta', 'DELTA', 0, 0, 0, 0, 0, 0, 0),
('SharesOutstanding', 'CURRENT_SHARES_OUTSTANDING_RT', 0, 0, 0, 0, 0, 0, 0),
('CountryId', 'COUNTRY_DOMICILE_REALTIME', 1, 1, 1, 1, 1, 1, 1),
('Previous', 'PREV_CLOSE_VALUE_REALTIME', 1, 1, 1, 1, 1, 1, 1),
('FullCompanyName', 'LONG_COMP_NAME', 1, 1, 1, 1, 1, 1, 1),
('OSIOptionSymbol', 'OCC_SYMBOL', 0, 0, 0, 0, 0, 0, 0),
('UpdateTime', 'TIME', 0, 0, 0, 0, 0, 0, 0),
('LastTick', 'LAST_TICK_DIRECTION_RT', 1, 0, 0, 0, 0, 0, 0),
('Change', 'NET_CHANGE_ON_DAY_TODAY_RT', 1, 1, 1, 1, 1, 1, 1),
('DividendYield', 'FUND_DVD_YLD_RT', 1, 0, 0, 0, 0, 0, 0),
('Dividend', 'DPS_LAST_GROSS_REALTIME', 0, 0, 0, 0, 0, 0, 0),
('DividendInsterval', 'DVD_FREQ', 0, 0, 0, 0, 0, 0, 0),
('XDividendDate', 'DIVIDEND_EX_DATE_REALTIME', 0, 0, 0, 0, 0, 0, 0),
('AnnualDividend', 'EQY_DVD_SH_12M_NET', 0, 0, 0, 0, 0, 0, 0),
('CurrencyCode', 'CURRENCY_RT', 1, 1, 1, 1, 1, 1, 1),
('RoundLot', 'ROUND_LOT_SIZE_REALTIME', 0, 0, 0, 0, 0, 0, 0),
('UnderlyingSymbol', 'UNDERLYING_TICKER_REALTIME', 0, 1, 0, 1, 0, 0, 0),
('Exchange', 'PRIMARY_EXCHANGE_CODE_REALTIME', 1, 1, 1, 1, 0, 0, 0),
('Future Contract Size(Multiplier)', 'FUT_CONT_SIZE', 0, 0, 1, 0, 0, 0, 0),
('Futures Contract Expiration Date', 'FUT_CONTRACT_DT', 0, 0, 1, 0, 0, 0, 0),
('Bloomberg Composite Code', 'COMPOSITE_EXCH_CODE', 1, 0, 0, 0, 0, 0, 0),
('Issue Date', 'ISSUE_DATE_REALTIME', 0, 0, 0, 0, 0, 1, 0),
('Coupon', 'COUPON_RT', 0, 0, 0, 0, 0, 1, 0),
('MaturityDate', 'MATURITY_RT', 0, 0, 0, 0, 0, 1, 0),
('OptionMultiplier', 'OPTION_MULTIPLIER_REALTIME', 0, 1, 0, 0, 0, 0, 0);
