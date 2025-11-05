<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:sort select="AssetID" />
				<!-- this for equity only-->
				<xsl:choose>
					<xsl:when test ="AssetID=1">
						<ThirdPartyFlatFileDetail>
							<!-- this field is used for internal purpose-->
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>
							<!-- this field is used for internal purpose-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<DealType>
								<xsl:value-of select ="'EquityDeal'"/>
							</DealType>

							<DealId>
								<xsl:value-of select ="TradeRefID"/>
							</DealId>

							<xsl:choose>
								<xsl:when test ="TaxLotState='Amemded'">
									<Action>
										<xsl:value-of select ="'Correct'"/>
									</Action>
								</xsl:when >
								<xsl:when test ="TaxLotState='Deleted'">
									<Action>
										<xsl:value-of select ="'Cancel'"/>
									</Action>
								</xsl:when>
								<xsl:otherwise>
									<Action>
										<xsl:value-of select ="'New'"/>
									</Action>
								</xsl:otherwise>
							</xsl:choose >

							<!-- this id not clear, this 4/D column in the sepc document-->
							<Client>
								<xsl:value-of select="'MILLENNIUM'"/>
							</Client>

							<Reserved>
								<xsl:value-of select="''"/>
							</Reserved>

							<Reserved1>
								<xsl:value-of select="''"/>
							</Reserved1>

							<!-- this id not clear, this 7/G column in the sepc document-->
							<Folder>
								<xsl:value-of select ="'ELVNELVN'"/>
							</Folder>

							<!-- need mapping-->
							<Custodian>
								<xsl:value-of select="'CSFB'"/>
							</Custodian>

							<!-- Fund mapped Name from Admin-Company - Third party mapping -->
							<CashAccount>
								<xsl:value-of select ="'CSNMIL11PB'"/>
							</CashAccount>

							<!-- need mapping for brokers-->
							<CounterParty>
								<xsl:value-of select ="'CSFBNY'"/>
							</CounterParty>

							<Comments>
								<xsl:value-of select ="''"/>
							</Comments>

							<State>
								<xsl:value-of select ="''"/>
							</State>

							<xsl:variable name = "varTradeMth" >
								<xsl:value-of select="substring(TradeDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varTradeDay" >
								<xsl:value-of select="substring(TradeDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varTradeYR" >
								<xsl:value-of select="substring(TradeDate,7,4)"/>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat($varTradeYR,'-',$varTradeMth,'-',$varTradeDay)"/>
							</TradeDate>

							<xsl:variable name = "varSettleMth" >
								<xsl:value-of select="substring(SettlementDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varSettleDay" >
								<xsl:value-of select="substring(SettlementDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varSettleYR" >
								<xsl:value-of select="substring(SettlementDate,7,4)"/>
							</xsl:variable>
							<SettlementDate>
								<xsl:value-of select="concat($varSettleYR,'-',$varSettleMth,'-',$varSettleDay)"/>
							</SettlementDate>

							<Reserved2>
								<xsl:value-of select ="''"/>
							</Reserved2>

							<GlobeOpSecurityIdentifier>
								<xsl:value-of select ="''"/>
							</GlobeOpSecurityIdentifier>

							<CUSIP>
								<xsl:value-of select ="CUSIP"/>
							</CUSIP>
							<ISIN>
								<xsl:value-of select ="ISIN"/>
							</ISIN>
							<SEDOL>
								<xsl:value-of select ="SEDOL"/>
							</SEDOL>
							<BloombergTicker>
								<xsl:value-of select ="BBCode"/>
							</BloombergTicker>
							<RIC>
								<xsl:value-of select ="RIC"/>
							</RIC>

							<SecurityDescription>
								<xsl:value-of select ="FullSecurityName"/>
							</SecurityDescription>

							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
									<TransactionIndicator>
										<xsl:value-of select ="'Buy'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
									<TransactionIndicator>
										<xsl:value-of select ="'Sell'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:otherwise>
									<TransactionIndicator>
										<xsl:value-of select="''"/>
									</TransactionIndicator>
								</xsl:otherwise>
							</xsl:choose>

							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Buy Long'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Buy Cover'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Sell Long'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side='Sell to Open'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Sell Short'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:otherwise>
									<SubTransactionIndicator>
										<xsl:value-of select="''"/>
									</SubTransactionIndicator>
								</xsl:otherwise>
							</xsl:choose>

							<Quantity>
								<xsl:value-of select="AllocatedQty"/>
							</Quantity>
							<Price>
								<xsl:value-of select="AveragePrice"/>
							</Price>

							<Commission>
								<xsl:value-of select ="CommissionCharged"/>
							</Commission>

							<SECFee>
								<xsl:value-of select ="OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>
							</SECFee>

							<VAT>
								<xsl:value-of select="TaxOnCommissions"/>
							</VAT>

							<TradeCurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</TradeCurrency>

							<ExchangeRate>
								<xsl:value-of select="0"/>
							</ExchangeRate>

							<Reserved3>
								<xsl:value-of select="''"/>
							</Reserved3>

							<BrokerShortName>
								<xsl:value-of select="''"/>
							</BrokerShortName>

							<ExpirationDate>
								<xsl:value-of select="''"/>
							</ExpirationDate>

							<Exchange>
								<xsl:value-of select="''"/>
							</Exchange>

							<PutCallIndicator>
								<xsl:value-of select="''"/>
							</PutCallIndicator>

							<Strike>
								<xsl:value-of select="0"/>
							</Strike>

							<SecurityCurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</SecurityCurrency>

							<!-- this is for Future only-->
							<InitialMarginCurrency>
								<xsl:value-of select ="''"/>
							</InitialMarginCurrency>

							<!-- this is also for internal purpose-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>
						</ThirdPartyFlatFileDetail>
					</xsl:when >
					<xsl:when test ="AssetID=2">
						<!-- AssetId=2 means Equiy Options-->
						<ThirdPartyFlatFileDetail>
							<!-- this field is used for internal purpose-->
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<!-- this field is used for internal purpose-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<DealType>
								<xsl:value-of select ="'OptionDeal'"/>
							</DealType>

							<DealId>
								<xsl:value-of select ="TradeRefID"/>
							</DealId>

							<xsl:choose>
								<xsl:when test ="TaxLotState='Amemded'">
									<Action>
										<xsl:value-of select ="'Correct'"/>
									</Action>
								</xsl:when >
								<xsl:when test ="TaxLotState='Deleted'">
									<Action>
										<xsl:value-of select ="'Cancel'"/>
									</Action>
								</xsl:when>
								<xsl:otherwise>
									<Action>
										<xsl:value-of select ="'New'"/>
									</Action>
								</xsl:otherwise>
							</xsl:choose >

							<!-- this id not clear, this 4/D column in the sepc document-->
							<Client>
								<xsl:value-of select="'MILLENNIUM'"/>
							</Client>

							<Reserved>
								<xsl:value-of select="''"/>
							</Reserved>

							<Reserved1>
								<xsl:value-of select="''"/>
							</Reserved1>

							<!-- this id not clear, this 7/G column in the sepc document-->
							<Folder>
								<xsl:value-of select ="'ELVNELVN'"/>
							</Folder>

							<!-- need mapping-->
							<Custodian>
								<xsl:value-of select="'CSFB'"/>
							</Custodian>

							<!-- Fund mapped Name from Admin-Company - Third party mapping -->
							<CashAccount>
								<xsl:value-of select ="'CSNMIL11PB'"/>
							</CashAccount>

							<!-- need mapping for brokers-->
							<CounterParty>
								<xsl:value-of select ="'CSFBNY'"/>
							</CounterParty>

							<Comments>
								<xsl:value-of select ="''"/>
							</Comments>

							<State>
								<xsl:value-of select ="''"/>
							</State>

							<xsl:variable name = "varTradeMth" >
								<xsl:value-of select="substring(TradeDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varTradeDay" >
								<xsl:value-of select="substring(TradeDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varTradeYR" >
								<xsl:value-of select="substring(TradeDate,7,4)"/>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat($varTradeYR,'-',$varTradeMth,'-',$varTradeDay)"/>
							</TradeDate>

							<xsl:variable name = "varSettleMth" >
								<xsl:value-of select="substring(SettlementDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varSettleDay" >
								<xsl:value-of select="substring(SettlementDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varSettleYR" >
								<xsl:value-of select="substring(SettlementDate,7,4)"/>
							</xsl:variable>
							<SettlementDate>
								<xsl:value-of select="concat($varSettleYR,'-',$varSettleMth,'-',$varSettleDay)"/>
							</SettlementDate>

							<Reserved2>
								<xsl:value-of select ="''"/>
							</Reserved2>

							<!-- this field is not clear for equity options-->
							<GlobeOpSecurityIdentifier>
								<xsl:value-of select ="''"/>
							</GlobeOpSecurityIdentifier>

							<!--Column 17 -->
							<CUSIP>
								<xsl:value-of select ="UnderlyingSymbol"/>
							</CUSIP>
							<ISIN>
								<xsl:value-of select ="RIC"/>
							</ISIN>

							<SEDOL>
								<xsl:value-of select ="''"/>
							</SEDOL>
							<BloombergTicker>
								<xsl:value-of select ="BBCode"/>
							</BloombergTicker>

							<RIC>
								<xsl:value-of select ="translate(Symbol, ' ', '+')"/>
							</RIC>

							<SecurityDescription>
								<xsl:value-of select ="FullSecurityName"/>
							</SecurityDescription>

							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
									<TransactionIndicator>
										<xsl:value-of select ="'Buy'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
									<TransactionIndicator>
										<xsl:value-of select ="'Sell'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:otherwise>
									<TransactionIndicator>
										<xsl:value-of select="''"/>
									</TransactionIndicator>
								</xsl:otherwise>
							</xsl:choose>

							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Buy Long'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Buy Cover'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Sell Long'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell short' or Side='Sell to Open'">
									<SubTransactionIndicator>
										<xsl:value-of select ="'Sell Short'"/>
									</SubTransactionIndicator>
								</xsl:when>
								<xsl:otherwise>
									<SubTransactionIndicator>
										<xsl:value-of select="''"/>
									</SubTransactionIndicator>
								</xsl:otherwise>
							</xsl:choose>

							<Quantity>
								<xsl:value-of select="AllocatedQty"/>
							</Quantity>

							<!-- Column 26-->
							<Price>
								<xsl:value-of select="AveragePrice"/>
							</Price>

							<Commission>
								<xsl:value-of select ="CommissionCharged"/>
							</Commission>

							<SECFee>
								<xsl:value-of select ="OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>
							</SECFee>

							<VAT>
								<xsl:value-of select ="TaxOnCommissions"/>
							</VAT>

							<TradeCurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</TradeCurrency>

							<ExchangeRate>
								<xsl:value-of select="0"/>
							</ExchangeRate>

							<Reserved3>
								<xsl:value-of select ="''"/>
							</Reserved3>

							<BrokerShortName>
								<xsl:value-of select ="''"/>
							</BrokerShortName>

							<xsl:variable name = "varExpirationMth" >
								<xsl:value-of select="substring(ExpirationDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varExpirationDay" >
								<xsl:value-of select="substring(ExpirationDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varExpirationYR" >
								<xsl:value-of select="substring(ExpirationDate,7,4)"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test ="$varExpirationYR != ''">
									<ExpirationDate>
										<xsl:value-of select="concat($varExpirationYR,'-',$varExpirationMth,'-',$varExpirationDay)"/>
									</ExpirationDate>
								</xsl:when>
								<xsl:otherwise>
									<ExpirationDate>
										<xsl:value-of select="''"/>
									</ExpirationDate>
								</xsl:otherwise>
							</xsl:choose>

							<Exchange>
								<xsl:value-of select ="''"/>
							</Exchange>

							<xsl:choose>
								<xsl:when test ="PutOrCall = 'CALL'">
									<PutCallIndicator>
										<xsl:value-of select ="'C'"/>
									</PutCallIndicator>
								</xsl:when>
								<xsl:when test ="PutOrCall = 'PUT'">
									<PutCallIndicator>
										<xsl:value-of select ="'P'"/>
									</PutCallIndicator>
								</xsl:when>
								<xsl:otherwise>
									<PutCallIndicator>
										<xsl:value-of select ="PutOrCall"/>
									</PutCallIndicator>
								</xsl:otherwise>
							</xsl:choose>

							<Strike>
								<xsl:value-of select="StrikePrice"/>
							</Strike>

							<SecurityCurrency>
								<xsl:value-of select="''"/>
							</SecurityCurrency>

							<!-- this is for Future only-->
							<InitialMarginCurrency>
								<xsl:value-of select ="''"/>
							</InitialMarginCurrency>

							<!-- this is also for internal purpose-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>


						</ThirdPartyFlatFileDetail>
					</xsl:when>
					<xsl:when test ="AssetID=3">
						<ThirdPartyFlatFileDetail>
							<!-- this field is used for internal purpose-->
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<!-- this field is used for internal purpose-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<DealType>
								<xsl:value-of select ="'FutureDeal'"/>
							</DealType>

							<DealId>
								<xsl:value-of select ="TradeRefID"/>
							</DealId>
							<!-- Action -->
							<xsl:choose>
								<xsl:when test ="TaxLotState='Amemded'">
									<Action>
										<xsl:value-of select ="'Correct'"/>
									</Action>
								</xsl:when >
								<xsl:when test ="TaxLotState='Deleted'">
									<Action>
										<xsl:value-of select ="'Cancel'"/>
									</Action>
								</xsl:when>
								<xsl:otherwise>
									<Action>
										<xsl:value-of select ="'New'"/>
									</Action>
								</xsl:otherwise>
							</xsl:choose >

							<!--this id not clear, this 4/D column in the sepc document-->
							<Client>
								<xsl:value-of select="'MILLENNIUM'"/>
							</Client>

							<Reserved>
								<xsl:value-of select="''"/>
							</Reserved>

							<Reserved1>
								<xsl:value-of select="''"/>
							</Reserved1>

							<!--this id not clear, this 7/G column in the sepc document-->
							<Folder>
								<xsl:value-of select ="'ELVNELVN'"/>
							</Folder>

							<!--need mapping-->
							<Custodian>
								<xsl:value-of select="'CSFB'"/>
							</Custodian>

							<!--Fund mapped Name from Admin-Company - Third party mapping-->
							<CashAccount>
								<xsl:value-of select ="'CSNMIL11PB'"/>
							</CashAccount>

							<!--need mapping for brokers-->
							<CounterParty>
								<xsl:value-of select ="'CSFBNY'"/>
							</CounterParty>

							<Comments>
								<xsl:value-of select ="''"/>
							</Comments>

							<State>
								<xsl:value-of select ="''"/>
							</State>
							<!--TradeDate-->
							<xsl:variable name = "varTradeMth" >
								<xsl:value-of select="substring(TradeDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varTradeDay" >
								<xsl:value-of select="substring(TradeDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varTradeYR" >
								<xsl:value-of select="substring(TradeDate,7,4)"/>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat($varTradeYR,'-',$varTradeMth,'-',$varTradeDay)"/>
							</TradeDate>

							<!--SettlementDate-->
							<xsl:variable name = "varSettleMth" >
								<xsl:value-of select="substring(SettlementDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varSettleDay" >
								<xsl:value-of select="substring(SettlementDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varSettleYR" >
								<xsl:value-of select="substring(SettlementDate,7,4)"/>
							</xsl:variable>
							<SettlementDate>
								<xsl:value-of select="concat($varSettleYR,'-',$varSettleMth,'-',$varSettleDay)"/>
							</SettlementDate>

							<Reserved2>
								<xsl:value-of select ="''"/>
							</Reserved2>

							<GlobeOpSecurityIdentifier>
								<xsl:value-of select ="''"/>
							</GlobeOpSecurityIdentifier>

							<CUSIP>
								<xsl:value-of select ="''"/>
							</CUSIP>
							<ISIN>
								<xsl:value-of select ="''"/>
							</ISIN>
							<SEDOL>
								<xsl:value-of select ="''"/>
							</SEDOL>

							<BloombergTicker>
								<xsl:value-of select ="BBCode"/>
							</BloombergTicker>

							<RIC>
								<xsl:value-of select ="translate(Symbol, ' ', '')"/>
							</RIC>
							<SecurityDescription>
								<xsl:value-of select ="FullSecurityName"/>
							</SecurityDescription>

							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
									<TransactionIndicator>
										<xsl:value-of select ="'Buy'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
									<TransactionIndicator>
										<xsl:value-of select ="'Sell'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:otherwise>
									<TransactionIndicator>
										<xsl:value-of select="'Side'"/>
									</TransactionIndicator>
								</xsl:otherwise>
							</xsl:choose>

							<SubTransactionIndicator>
								<xsl:value-of select ="''"/>
							</SubTransactionIndicator>

							<Quantity>
								<xsl:value-of select="AllocatedQty"/>
							</Quantity>
							<Price>
								<xsl:value-of select="AveragePrice"/>
							</Price>

							<Commission>
								<xsl:value-of select ="CommissionCharged"/>
							</Commission>

							<SECFee>
								<xsl:value-of select ="OtherBrokerFee + StampDuty + TransactionLevy + ClearingFee + MiscFees"/>
							</SECFee>

							<VAT>
								<xsl:value-of select="TaxOnCommissions"/>
							</VAT>

							<TradeCurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</TradeCurrency>

							<ExchangeRate>
								<xsl:value-of select="''"/>
							</ExchangeRate>

							<Reserved3>
								<xsl:value-of select="''"/>
							</Reserved3>

							<BrokerShortName>
								<xsl:value-of select="''"/>
							</BrokerShortName>

							<xsl:variable name = "varExpirationMth" >
								<xsl:value-of select="substring(ExpirationDate,1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varExpirationDay" >
								<xsl:value-of select="substring(ExpirationDate,4,2)"/>
							</xsl:variable>
							<xsl:variable name = "varExpirationYR" >
								<xsl:value-of select="substring(ExpirationDate,7,4)"/>
							</xsl:variable>
							<ExpirationDate>
								<xsl:value-of select="concat($varExpirationYR,'-',$varExpirationMth,'-',$varExpirationDay)"/>
							</ExpirationDate>

							<Exchange>
								<xsl:value-of select="''"/>
							</Exchange>

							<PutCallIndicator>
								<xsl:value-of select="''"/>
							</PutCallIndicator>

							<Strike>
								<xsl:value-of select="''"/>
							</Strike>

							<SecurityCurrency>
								<xsl:value-of select="''"/>
							</SecurityCurrency>

							<InitialMarginCurrency>
								<xsl:value-of select ="''"/>
							</InitialMarginCurrency>

							<!-- this is also for internal purpose-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>
					<xsl:when test ="AssetID=5">
						<ThirdPartyFlatFileDetail>
							<!-- this field is used for internal purpose-->
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<!-- this field is used for internal purpose-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<DealType>
								<xsl:value-of select ="'SpotDeal'"/>
							</DealType>

							<DealId>
								<xsl:value-of select ="TradeRefID"/>
							</DealId>
							<!-- Action -->
							<xsl:choose>
								<xsl:when test ="TaxLotState='Amemded'">
									<Action>
										<xsl:value-of select ="'Correct'"/>
									</Action>
								</xsl:when >
								<xsl:when test ="TaxLotState='Deleted'">
									<Action>
										<xsl:value-of select ="'Cancel'"/>
									</Action>
								</xsl:when>
								<xsl:otherwise>
									<Action>
										<xsl:value-of select ="'New'"/>
									</Action>
								</xsl:otherwise>
							</xsl:choose >

							<!--this id not clear, this 4/D column in the sepc document-->
							<Client>
								<xsl:value-of select="'MILLENNIUM'"/>
							</Client>

							<Reserved>
								<xsl:value-of select="''"/>
							</Reserved>

							<Reserved1>
								<xsl:value-of select="''"/>
							</Reserved1>

							<!--this id not clear, this 7/G column in the sepc document-->
							<Folder>
								<xsl:value-of select ="'ELVNELVN'"/>
							</Folder>

							<!--need mapping-->
							<Custodian>
								<xsl:value-of select="'CSFB'"/>
							</Custodian>

							<!--Fund mapped Name from Admin-Company - Third party mapping-->
							<CashAccount>
								<xsl:value-of select ="'CSNMIL11PB'"/>
							</CashAccount>

							<!--need mapping for brokers-->
							<CounterParty>
								<xsl:value-of select ="'CSFBNY'"/>
							</CounterParty>

							<Comments>
								<xsl:value-of select ="''"/>
							</Comments>

							<State>
								<xsl:value-of select ="''"/>
							</State>
							<!--TradeDate-->							
							<TradeDate>
								<xsl:value-of select="concat(substring(TradeDate,7,4),'-',substring(TradeDate,1,2),'-',substring(TradeDate,4,2))"/>
							</TradeDate>

							<!--SettlementDate-->							
							<SettlementDate>
								<xsl:value-of select="concat(substring(SettlementDate,7,4),'-',substring(SettlementDate,1,2),'-',substring(SettlementDate,4,2))"/>
							</SettlementDate>

							<Reserved2>
								<xsl:value-of select ="''"/>
							</Reserved2>

							<!-- Add Spot Rate-->
							<GlobeOpSecurityIdentifier>
								<xsl:value-of select ="''"/>
							</GlobeOpSecurityIdentifier>

							<!-- Reserved3-->
							<CUSIP>
								<xsl:value-of select ="''"/>
							</CUSIP>
							
							<!-- 18/R Buy Currency-->
							<ISIN>
								<xsl:value-of select ="''"/>
							</ISIN>
							<!-- Buy Amount-->
							<SEDOL>
								<xsl:value-of select ="0"/>
							</SEDOL>
							<!-- sell Amount-->
							<BloombergTicker>
								<xsl:value-of select ="0"/>
							</BloombergTicker>

							<!--20/T Sell Currency-->
							<RIC>
								<xsl:value-of select ="''"/>
							</RIC>

							<!-- Sell Amount-->
							<SecurityDescription>
								<xsl:value-of select ="0"/>
							</SecurityDescription>
							<!-- 22/V Clearing Fees-->
							<TransactionIndicator>
								<xsl:value-of select ="0"/>
							</TransactionIndicator>

							<!--<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
									<TransactionIndicator>
										<xsl:value-of select ="'Buy'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
									<TransactionIndicator>
										<xsl:value-of select ="'Sell'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:otherwise>
									<TransactionIndicator>
										<xsl:value-of select="''"/>
									</TransactionIndicator>
								</xsl:otherwise>
							</xsl:choose>-->

							<!--23 Reserved4-->
							<SubTransactionIndicator>
								<xsl:value-of select ="''"/>
							</SubTransactionIndicator>
							<!-- 24 Reserved5-->
							<Quantity>
								<xsl:value-of select="''"/>
							</Quantity>
							
							<!-- 25 Commission Currency-->
							<Price>
								<xsl:value-of select="''"/>
							</Price>

							<Commission>
								<xsl:value-of select ="CommissionCharged"/>
							</Commission>

							<!-- 27 Reserved6-->
							<SECFee>
								<xsl:value-of select ="''"/>
							</SECFee>

							<!-- 28 Reserved7-->
							<VAT>
								<xsl:value-of select="''"/>
							</VAT>
							<!-- 29 Reserved8-->
							<TradeCurrency>
								<xsl:value-of select="''"/>
							</TradeCurrency>

							<!--30/AD Broker Short Name-->
							<ExchangeRate>
								<xsl:value-of select="''"/>
							</ExchangeRate>

							<!-- 31 Client Reference-->
							<Reserved3>
								<xsl:value-of select="'MILLENNIUM'"/>
							</Reserved3>

							<BrokerShortName>
								<xsl:value-of select="''"/>
							</BrokerShortName>

							
							<ExpirationDate>
								<xsl:value-of select="''"/>
							</ExpirationDate>

							<Exchange>
								<xsl:value-of select="''"/>
							</Exchange>

							<PutCallIndicator>
								<xsl:value-of select="''"/>
							</PutCallIndicator>

							<Strike>
								<xsl:value-of select="''"/>
							</Strike>

							<SecurityCurrency>
								<xsl:value-of select="0"/>
							</SecurityCurrency>

							<InitialMarginCurrency>
								<xsl:value-of select ="''"/>
							</InitialMarginCurrency>

							<!-- this is also for internal purpose-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>
					<xsl:when test ="AssetID=11">
						<ThirdPartyFlatFileDetail>
							<!-- this field is used for internal purpose-->
							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<!-- this field is used for internal purpose-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<DealType>
								<xsl:value-of select ="'ForwardDeal'"/>
							</DealType>

							<DealId>
								<xsl:value-of select ="TradeRefID"/>
							</DealId>
							<!-- Action -->
							<xsl:choose>
								<xsl:when test ="TaxLotState='Amemded'">
									<Action>
										<xsl:value-of select ="'Correct'"/>
									</Action>
								</xsl:when >
								<xsl:when test ="TaxLotState='Deleted'">
									<Action>
										<xsl:value-of select ="'Cancel'"/>
									</Action>
								</xsl:when>
								<xsl:otherwise>
									<Action>
										<xsl:value-of select ="'New'"/>
									</Action>
								</xsl:otherwise>
							</xsl:choose >

							<!--this id not clear, this 4/D column in the sepc document-->
							<Client>
								<xsl:value-of select="'MILLENNIUM'"/>
							</Client>

							<Reserved>
								<xsl:value-of select="''"/>
							</Reserved>

							<Reserved1>
								<xsl:value-of select="''"/>
							</Reserved1>

							<!--this id not clear, this 7/G column in the sepc document-->
							<Folder>
								<xsl:value-of select ="'ELVNELVN'"/>
							</Folder>

							<!--need mapping-->
							<Custodian>
								<xsl:value-of select="'CSFB'"/>
							</Custodian>

							<!--Fund mapped Name from Admin-Company - Third party mapping-->
							<CashAccount>
								<xsl:value-of select ="'CSNMIL11PB'"/>
							</CashAccount>

							<!--need mapping for brokers-->
							<CounterParty>
								<xsl:value-of select ="'CSFBNY'"/>
							</CounterParty>

							<Comments>
								<xsl:value-of select ="''"/>
							</Comments>

							<State>
								<xsl:value-of select ="''"/>
							</State>
							<!--TradeDate-->
							<TradeDate>
								<xsl:value-of select="concat(substring(TradeDate,7,4),'-',substring(TradeDate,1,2),'-',substring(TradeDate,4,2))"/>
							</TradeDate>

							<!--SettlementDate-->
							<SettlementDate>
								<xsl:value-of select="concat(substring(SettlementDate,7,4),'-',substring(SettlementDate,1,2),'-',substring(SettlementDate,4,2))"/>
							</SettlementDate>

							<Reserved2>
								<xsl:value-of select ="''"/>
							</Reserved2>

							<!-- Add Spot Rate-->
							<GlobeOpSecurityIdentifier>
								<xsl:value-of select ="''"/>
							</GlobeOpSecurityIdentifier>

							<!-- 17/R Add Forward Rate-->
							<CUSIP>
								<xsl:value-of select ="''"/>
							</CUSIP>

							<!-- 18/R Buy Currency-->
							<ISIN>
								<xsl:value-of select ="''"/>
							</ISIN>
							<!-- Buy Amount-->
							<SEDOL>
								<xsl:value-of select ="0"/>
							</SEDOL>
							<!-- sell Amount-->
							<BloombergTicker>
								<xsl:value-of select ="0"/>
							</BloombergTicker>

							<!--20/T Sell Currency-->
							<RIC>
								<xsl:value-of select ="''"/>
							</RIC>

							<!-- Sell Amount-->
							<SecurityDescription>
								<xsl:value-of select ="0"/>
							</SecurityDescription>
							<!-- 22/V Clearing Fees-->
							<TransactionIndicator>
								<xsl:value-of select ="0"/>
							</TransactionIndicator>

							<!--<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
									<TransactionIndicator>
										<xsl:value-of select ="'Buy'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
									<TransactionIndicator>
										<xsl:value-of select ="'Sell'"/>
									</TransactionIndicator>
								</xsl:when>
								<xsl:otherwise>
									<TransactionIndicator>
										<xsl:value-of select="''"/>
									</TransactionIndicator>
								</xsl:otherwise>
							</xsl:choose>-->

							<!--23 Fixing Date-->
							<SubTransactionIndicator>
								<xsl:value-of select ="''"/>
							</SubTransactionIndicator>
							<!-- 24 NDF Indicator-->
							<Quantity>
								<xsl:value-of select="''"/>
							</Quantity>

							<!-- 25 Commission Currency-->
							<Price>
								<xsl:value-of select="''"/>
							</Price>

							<Commission>
								<xsl:value-of select ="CommissionCharged"/>
							</Commission>

							<!-- 27 Points Conditionally Mandatory-->
							<SECFee>
								<xsl:value-of select ="''"/>
							</SECFee>

							<!-- 28 Reserved7-->
							<VAT>
								<xsl:value-of select="''"/>
							</VAT>
							<!-- 29 Value Date-->
							<TradeCurrency>
								<xsl:value-of select="''"/>
							</TradeCurrency>

							<!--30/AD Broker Short Name-->
							<ExchangeRate>
								<xsl:value-of select="''"/>
							</ExchangeRate>

							<!-- 31 Client Reference-->
							<Reserved3>
								<xsl:value-of select="''"/>
							</Reserved3>

							<BrokerShortName>
								<xsl:value-of select="''"/>
							</BrokerShortName>


							<ExpirationDate>
								<xsl:value-of select="''"/>
							</ExpirationDate>

							<Exchange>
								<xsl:value-of select="''"/>
							</Exchange>

							<PutCallIndicator>
								<xsl:value-of select="''"/>
							</PutCallIndicator>

							<Strike>
								<xsl:value-of select="''"/>
							</Strike>

							<SecurityCurrency>
								<xsl:value-of select="0"/>
							</SecurityCurrency>

							<InitialMarginCurrency>
								<xsl:value-of select ="''"/>
							</InitialMarginCurrency>

							<!-- this is also for internal purpose-->
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:when>
				</xsl:choose>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
