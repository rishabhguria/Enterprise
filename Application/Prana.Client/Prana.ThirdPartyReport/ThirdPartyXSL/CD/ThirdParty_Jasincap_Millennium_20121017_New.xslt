<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<xsl:if test ="TaxLotState !='Deleted'">

					<ThirdPartyFlatFileDetail>
						<!-- internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>
						<!-- internal use-->
						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<TransType>
							<xsl:value-of select ="'N'"/>
						</TransType>
						<Ref>
							<xsl:value-of select ="TradeRefID"/>
						</Ref>

						<!--Side Identifier-->

						<xsl:choose>
							<xsl:when test="Side='Buy to Open' or Side='Buy' ">
								<Action>
									<xsl:value-of select ="'B'"/>
								</Action>
							</xsl:when>
							<xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
								<Action>
									<xsl:value-of select ="'BC'"/>
								</Action>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close' ">
								<Action>
									<xsl:value-of select ="'S'"/>
								</Action>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
								<Action>
									<xsl:value-of select ="'SS'"/>
								</Action>
							</xsl:when>
							<xsl:otherwise>
								<Action>
									<xsl:value-of select="Side"/>
								</Action>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:variable name ="varCheckSymbolUnderlying">
							<xsl:value-of select ="substring-before(Symbol,'-')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX' and Asset != 'Future' and Asset != 'FutureOption'">
								<Symbol>
									<xsl:value-of select="SEDOL"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="Asset ='EquityOption' ">
								<Symbol>
									<xsl:value-of select="UnderlyingSymbol"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="(Asset ='Future' or Asset ='FutureOption') and RIC != ''">
								<Symbol>
									<xsl:value-of select="RIC"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="(Asset ='Future' or Asset ='FutureOption') and BBCode != ''">
								<Symbol>
									<xsl:value-of select="BBCode"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="Symbol"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

						<!--<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<SYMBOL>
						<xsl:value-of select="substring-before(Symbol,' ')"/>
								</SYMBOL>
								<SYMBOL>
									<xsl:value-of select="UnderlyingSymbol"/>
								</SYMBOL>
							</xsl:when>
							<xsl:otherwise>
								<SYMBOL>
									<xsl:value-of select="Symbol"/>
								</SYMBOL>
							</xsl:otherwise>
						</xsl:choose>-->



						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>

						<Price>
							<xsl:value-of select="AveragePrice"/>
						</Price>

						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>

						<SettleDate>
							<xsl:value-of select ="''"/>
						</SettleDate>

						<xsl:choose>
							<xsl:when test ="CounterParty='CSFB'">
								<ContraBroker>
									<xsl:value-of select ="'FBCO'"/>
								</ContraBroker>
							</xsl:when>
							<xsl:when test ="CounterParty='NITE'">
								<ContraBroker>
									<xsl:value-of select ="'DTTX'"/>
								</ContraBroker>
							</xsl:when>
							<xsl:otherwise>
								<ContraBroker>
									<xsl:value-of select ="CounterParty"/>
								</ContraBroker>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varFUandFOCode">
							<xsl:choose>
								<xsl:when test="Asset ='Future' and RIC != '' ">
									<xsl:value-of select ="'FU_R'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'FU'"/>
								</xsl:otherwise>
							</xsl:choose >
						</xsl:variable>

						<!--define security type-->
						<xsl:choose>
							<xsl:when test="Asset ='Equity' ">
								<SecurityTypeCode>
									<xsl:value-of select ="'EQ'"/>
								</SecurityTypeCode>
							</xsl:when>
							<xsl:when test="Asset ='EquityOption' ">
								<SecurityTypeCode>
									<xsl:value-of select ="'EO'"/>
								</SecurityTypeCode>
							</xsl:when>
							<xsl:when test="Asset ='Future' ">
								<SecurityTypeCode>
									<xsl:value-of select ="$varFUandFOCode"/>
								</SecurityTypeCode>
							</xsl:when>
							<xsl:when test="Asset ='FutureOption' ">
								<SecurityTypeCode>
									<xsl:value-of select ="'FO'"/>
								</SecurityTypeCode>
							</xsl:when>
							<xsl:otherwise>
								<SecurityTypeCode>
									<xsl:value-of select="Asset"/>
								</SecurityTypeCode>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="CounterParty='CSFB'">
								<ExecutingBroker>
									<xsl:value-of select ="'FBCO'"/>
								</ExecutingBroker>
							</xsl:when>
							<xsl:when test ="CounterParty='NITE'">
								<ExecutingBroker>
									<xsl:value-of select ="'DTTX'"/>
								</ExecutingBroker>
							</xsl:when>
							<xsl:otherwise>
								<ExecutingBroker>
									<xsl:value-of select ="CounterParty"/>
								</ExecutingBroker>
							</xsl:otherwise>
						</xsl:choose>

						<!--<xsl:variable name ="varCheckSymbolUnderlying">-->

						<xsl:variable name ="varSymbolExchange">
							<xsl:value-of select ="substring-after(Symbol,'-')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$varCheckSymbolUnderlying != '' and Asset != 'FX' and Asset != 'Future' and Asset != 'FutureOption' and $varSymbolExchange != 'LON'">
								<Account>
									<!--<xsl:value-of select="'04F367207-067'"/>-->
									<xsl:value-of select="'02CU-158'"/>
								</Account>
							</xsl:when>
							<xsl:when test="$varCheckSymbolUnderlying != '' and Asset != 'FX' and Asset != 'Future' and Asset != 'FutureOption' and $varSymbolExchange = 'LON'">
								<Account>
									<!--<xsl:value-of select="'04F367207-067'"/>-->
									<xsl:value-of select="'0JK7-158'"/>
								</Account>
							</xsl:when>
							<xsl:when test="(Asset = 'Future' or Asset = 'FutureOption')">
								<Account>
									<xsl:value-of select="'910C-22'"/>
								</Account>
							</xsl:when>
							<xsl:otherwise>
								<Account>
									<!--<xsl:value-of select="'038CAAFX9-002'"/>-->
									<!-- http://jira.nirvanasolutions.com:8080/browse/JASINKIEWICZ-346-->
									<xsl:value-of select ="'75203931-002'"/>
								</Account>
							</xsl:otherwise>
						</xsl:choose>

						<!--<Account>
							<xsl:value-of select="FundAccountNo"/>
						</Account>-->

						<Crosscurrency>
							<xsl:value-of select="''"/>
						</Crosscurrency>

						<Strategy>
							<xsl:value-of select="''"/>
						</Strategy>

						<!-- below 4 columns,Column O,P,Q and R are optional, there is no name of these colmns, so given default names-->
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<StrikePrice>
									<xsl:value-of select="StrikePrice"/>
								</StrikePrice>
							</xsl:when>
							<xsl:otherwise>
								<StrikePrice>
									<xsl:value-of select="''"/>
								</StrikePrice>
							</xsl:otherwise>
						</xsl:choose>

						<!--<StrikePrice>
							<xsl:value-of select="StrikePrice"/>
						</StrikePrice>-->

						<xsl:choose>
							<xsl:when test ="PutOrCall='CALL'">
								<PutCall>
									<xsl:value-of select ="'Call'"/>
								</PutCall>
							</xsl:when>
							<xsl:when test ="PutOrCall = 'PUT'">
								<PutCall>
									<xsl:value-of select ="'Put'"/>
								</PutCall>
							</xsl:when>
							<xsl:otherwise>
								<PutCall>
									<xsl:value-of select ="''"/>
								</PutCall>
							</xsl:otherwise>
						</xsl:choose>

						<!--<PutCall>
							<xsl:value-of select="''"/>
						</PutCall>-->

						<Comment4>
							<xsl:value-of select="''"/>
						</Comment4>

						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<ExpiryDate>
									<xsl:value-of select="ExpirationDate"/>
								</ExpiryDate>
							</xsl:when>
							<xsl:otherwise>
								<ExpiryDate>
									<xsl:value-of select="''"/>
								</ExpiryDate>
							</xsl:otherwise>
						</xsl:choose>

						<!--<ExpiryDate>
							<xsl:value-of select="ExpirationDate"/>
						</ExpiryDate>-->


						<!-- only commission and taxes on commission-->
						<Commission>
							<xsl:value-of select="CommissionCharged  + TaxOnCommissions"/>
						</Commission>

						<!--<FeesLocal>
							<xsl:value-of select ="OtherBrokerFee + MiscFees"/>
						</FeesLocal>

						<ClearingFee>
							<xsl:value-of select ="ClearingFee "/>
						</ClearingFee>

						<TransactionLevy>
							<xsl:value-of select ="TransactionLevy "/>
						</TransactionLevy>

						<StampDuty>
							<xsl:value-of select ="StampDuty"/>
						</StampDuty>-->

						<Comment5>
							<xsl:value-of select="''"/>
						</Comment5>
						<Comment6>
							<xsl:value-of select="''"/>
						</Comment6>
						<Comment7>
							<xsl:value-of select="''"/>
						</Comment7>


						<xsl:choose>
							<xsl:when test ="Asset = 'EquityOption' and (Side = 'Buy to Open' or Side='Sell to Open')">
								<Open_CloseTag>
									<xsl:value-of select ="'Open'"/>
								</Open_CloseTag>
							</xsl:when>
							<xsl:when test ="Asset = 'EquityOption' and (Side='Sell to Close' or Side = 'Buy to Close')">
								<Open_CloseTag>
									<xsl:value-of select ="'Close'"/>
								</Open_CloseTag>
							</xsl:when>
							<xsl:otherwise>
								<Open_CloseTag>
									<xsl:value-of select ="''"/>
								</Open_CloseTag>
							</xsl:otherwise>
						</xsl:choose>

						<CommissionType>
							<xsl:value-of select="'T'"/>
						</CommissionType>

						<!-- this is also for internal purpose-->
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>

				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
