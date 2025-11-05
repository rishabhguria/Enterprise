<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">

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
						<xsl:value-of select ="'P0716'"/>
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
						<xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
							<InstrumentId>
								<xsl:value-of select="SEDOL"/>
							</InstrumentId>
						</xsl:when>
						<xsl:when test ="Asset ='EquityOption' ">
						<InstrumentId>
								<xsl:value-of select="OSIOptionSymbol"/>
						</InstrumentId>					
							
						</xsl:when>
						<xsl:otherwise>
							<InstrumentId>
								<xsl:value-of select="Symbol"/>
							</InstrumentId>
						</xsl:otherwise>
					</xsl:choose>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<xsl:choose>
						<xsl:when test="FundName ='Clover Street'">
							<AccountId>
								<xsl:value-of select="'43001562'"/>
							</AccountId>
						</xsl:when>
						
						<xsl:when test="FundName= 'Clover SMA: Hatteras' and Asset ='EquityOption' ">
							<AccountId>
								<xsl:value-of select="'43001563'"/>
							</AccountId>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Sell short'">
							<AccountId>
								<xsl:value-of select="'43001563'"/>
							</AccountId>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Sell to Open'">
							<AccountId>
								<xsl:value-of select="'43001563'"/>
							</AccountId>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Buy to Cover'">
							<AccountId>
								<xsl:value-of select="'43001563'"/>
							</AccountId>
						</xsl:when>


						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Buy to Close'">
							<AccountId>
								<xsl:value-of select="'43001563'"/>
							</AccountId>
						</xsl:when>


						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Buy to Open'">
							<AccountId>
								<xsl:value-of select="'43200340'"/>
							</AccountId>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Buy'">
							<AccountId>
								<xsl:value-of select="'43200340'"/>
							</AccountId>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Sell'">
							<AccountId>
								<xsl:value-of select="'43200340'"/>
							</AccountId>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Sell to Close'">
							<AccountId>
								<xsl:value-of select="'43200340'"/>
							</AccountId>
						</xsl:when>
						
						
					</xsl:choose>
					
					<!--<Accoun
						<xsl:value-of select="'61160464'"/>
					</AccountId>-->
					<xsl:choose>
						
						<xsl:when test ="CounterParty='CSFB'">
							<ExecutingBroker>
								<xsl:value-of select ="'FBCO'"/>
							</ExecutingBroker>
						</xsl:when>
						<xsl:when test ="CounterParty='NITE'">
							<ExecutingBroker>
								<xsl:value-of select ="'NITD'"/>
							</ExecutingBroker>
						</xsl:when>
						<xsl:otherwise>
							<ExecutingBroker>
								<xsl:value-of select ="CounterParty"/>
							</ExecutingBroker>
						</xsl:otherwise>
					</xsl:choose>

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
						<xsl:value-of select="0"/>
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

					<!--<Custodian>
						<xsl:value-of select="''"/>
					</Custodian>-->



					<xsl:choose>



						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Buy to Open'">
							<Custodian>
								<xsl:value-of select="'AWAY'"/>
							</Custodian>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Buy'">
							<Custodian>
								<xsl:value-of select="'AWAY'"/>
							</Custodian>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Sell'">
							<Custodian>
								<xsl:value-of select="'AWAY'"/>
							</Custodian>
						</xsl:when>

						<xsl:when test="FundName='Clover SMA: Hatteras' and Asset ='Equity' and Side='Sell to Close'">
							<Custodian>
								<xsl:value-of select="'AWAY'"/>
							</Custodian>
						</xsl:when>

						


					</xsl:choose>
					
					
					
					
					
					
					
					<WhenIssued>
						<xsl:value-of select="'N'"/>
					</WhenIssued>

					<!-- this is also for internal purpose-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
