<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<!--<xsl:if test ="CounterParty != 'JETS' and CounterParty != 'BUYIN'">-->
				<!--<xsl:if test ="CounterParty != 'BUYIN' and (FundName='HSBC 25393' or CounterParty='JEFF') ">-->
				<!--<xsl:if test ="(FundName='HSBC 25393' or CounterParty='JEFF') ">-->

				<xsl:variable name="varEntryForFundAcount">
					<xsl:choose>
						<xsl:when test="CounterParty = 'BUYIN'">
							<xsl:value-of select="0"/>
						</xsl:when>

						<xsl:when test="FundName='HSBC 25393' or FundName='HSBC 26206'">
							<xsl:choose>
								<xsl:when test="CounterParty='JEFF'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:when test="(FundName='JEFF Offshore'or  FundName='JEFF SCLP' or  FundName='Pershing SCLP')">						
							<xsl:value-of select="1"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
				<xsl:if test ="$varEntryForFundAcount='1' ">
					<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>
					<!-- this field use internal purpose-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<TradeId>
						<xsl:value-of select ="TradeRefID"/>
					</TradeId>
					<Moniker>
						<xsl:value-of select ="'P0002'"/>
					</Moniker>

					<!--Side Identifier-->

					<xsl:choose>
						<xsl:when test="Side='Buy to Open' or Side='Buy' ">
							<TransactionType>
								<xsl:value-of select ="'BY'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
							<TransactionType>
								<xsl:value-of select ="'CS'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close' ">
							<TransactionType>
								<xsl:value-of select ="'SL'"/>
							</TransactionType>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open' ">
							<TransactionType>
								<xsl:value-of select ="'SS'"/>
							</TransactionType>
						</xsl:when>
						<xsl:otherwise>
							<TransactionType>
								<xsl:value-of select="Side"/>
							</TransactionType>
						</xsl:otherwise>
					</xsl:choose>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>
					
					<xsl:variable name ="varCheckSymbolUnderlying">
						<xsl:value-of select ="substring-before(Symbol,'-')"/>
					</xsl:variable>

						
					<xsl:choose>
						<xsl:when test="$varCheckSymbolUnderlying !=''">
							<InstrumentId>
								<xsl:value-of select="SEDOL"/>
							</InstrumentId>

						</xsl:when>
						<xsl:when test="Asset ='Equity' ">
							<InstrumentId>
								<xsl:value-of select ="Symbol"/>
							</InstrumentId>
						</xsl:when>
						<xsl:when test="Asset ='EquityOption' ">
							<InstrumentId>
								<xsl:value-of select ="OSIOptionSymbol"/>
							</InstrumentId>
						</xsl:when>
						<xsl:when test="Asset ='Future' ">
							<InstrumentId>
								<xsl:value-of select ="translate(Symbol, ' ', '')"/>
							</InstrumentId>
						</xsl:when>
						<xsl:otherwise>
							<InstrumentId>
								<xsl:value-of select="Symbol"/>
							</InstrumentId>
						</xsl:otherwise>
						<!--<xsl:value-of select ="Symbol"/>-->
					</xsl:choose>
					

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<AccountId>
						
						<xsl:choose>
							<xsl:when test="FundName='JEFF Offshore'">
								<xsl:value-of select="'43001171'"/>
							</xsl:when>
							<xsl:when test="FundName='JEFF SCLP'">
								<xsl:value-of select="'43000101'"/>
							</xsl:when>
							<xsl:when test="FundName='Pershing SCLP'">
								<xsl:value-of select="'T63001498'"/>
							</xsl:when>
							
							<xsl:when test="FundName='HSBC 26206'">
								<xsl:value-of select="'43200367'"/>
							</xsl:when>
							<xsl:when test="FundName='HSBC 25393'">
								<xsl:value-of select="'43200413'"/>
							</xsl:when>
						</xsl:choose>
						<!--<xsl:value-of select="FundAccountNo"/>-->
					</AccountId>

					<ExecutingBroker>
						<xsl:value-of select ="CounterParty"/>
					</ExecutingBroker>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select ="''"/>
					</SettleDate>

					<CommissionType>
						<xsl:value-of select="'T'"/>
					</CommissionType>

					<!-- only commission and taxes on commission-->
					<Commission>
						<xsl:value-of select="format-number((CommissionCharged  + TaxOnCommissions),'#.####')"/>
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

					<SellingMethod>
						<xsl:value-of select ="''"/>
					</SellingMethod>

					<Vs_purchases_Date>
						<xsl:value-of select="''"/>
					</Vs_purchases_Date>

					<SettlementCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SettlementCurrency>

					<SettlementExchangeRate>
						<xsl:value-of select="''"/>
					</SettlementExchangeRate>

					<Exchange>
						<xsl:value-of select="''"/>
					</Exchange>

					<OtherFee>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption' and CounterParty !='JEFF'">
								<xsl:value-of select="15 + (0.25 * AllocatedQty)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</OtherFee>

					<Strategy>
						<xsl:value-of select="''"/>
					</Strategy>

					<LotNumber>
						<xsl:value-of select="0"/>
					</LotNumber>

					<LotQuantity>
						<xsl:value-of select="0"/>
					</LotQuantity>

					<Trader>
						<xsl:value-of select="''"/>
					</Trader>

					<Interest>
						<xsl:value-of select="0"/>
					</Interest>

					<Custodian>
						<xsl:value-of select="''"/>
					</Custodian>

					<WhenIssued>
						<xsl:value-of select="'N'"/>
					</WhenIssued>

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
