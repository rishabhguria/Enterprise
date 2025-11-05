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
							<xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
								<Symbol>
									<xsl:value-of select="SEDOL"/>
								</Symbol>
							</xsl:when>
							<xsl:when test ="Asset ='EquityOption' ">
								<Symbol>
									<xsl:value-of select="translate(Symbol,' ','+')"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="Symbol"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>

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
							<xsl:value-of select ="SettlementDate"/>
						</SettleDate>

						<ContraBroker>
							<xsl:value-of select="CounterParty"/>
						</ContraBroker>

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
									<xsl:value-of select ="'FU'"/>
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

						<ExecutingBroker>
							<xsl:value-of select ="CounterParty"/>
						</ExecutingBroker>

						<xsl:choose>
							<xsl:when test="$varCheckSymbolUnderlying != '' and Asset != 'FX'">
								<Account>
									<xsl:value-of select="'04F367207-067'"/>
								</Account>
							</xsl:when>
							<xsl:otherwise>
								<Account>
									<xsl:value-of select="'038CAAFX9-002'"/>
								</Account>
							</xsl:otherwise>
						</xsl:choose>

						<Crosscurrency>
							<xsl:value-of select="''"/>
						</Crosscurrency>

						<Strategy>
							<xsl:value-of select="''"/>
						</Strategy>

						<!-- below 4 columns,Column O,P,Q and R are optional, there is no name of these colmns, so given default names-->
						<Comment1>
							<xsl:value-of select="''"/>
						</Comment1>

						<Comment2>
							<xsl:value-of select="''"/>
						</Comment2>

						<Comment3>
							<xsl:value-of select="''"/>
						</Comment3>

						<Comment4>
							<xsl:value-of select="''"/>
						</Comment4>

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
