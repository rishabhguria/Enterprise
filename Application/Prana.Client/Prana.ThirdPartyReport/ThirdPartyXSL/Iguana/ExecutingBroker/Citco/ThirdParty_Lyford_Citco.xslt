<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>


				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<Product>
					<xsl:value-of select="'Product'"/>
				</Product>

				<RecordAction>
					<xsl:value-of select="'Record Action'"/>
				</RecordAction>

				<TradeType>
					<xsl:value-of select="'Trade Type'"/>
				</TradeType>

				<Seqno>
					<xsl:value-of select="'Seqno'"/>
				</Seqno>

				<Traderid>
					<xsl:value-of select="'Traderid'"/>
				</Traderid>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select="'Settle/On/Value Date'"/>
				</SettleDate>

				<EffectiveDate>
					<xsl:value-of select="'Off/Effective Date'"/>
				</EffectiveDate>

				<Clearer>
					<xsl:value-of select="'Clearer'"/>
				</Clearer>

				<Counterparty>
					<xsl:value-of select="'Counterparty'"/>
				</Counterparty>

				<InstrumentCodeType>
					<xsl:value-of select="'Instrument Code Type'"/>
				</InstrumentCodeType>

				<InstrumentCode>
					<xsl:value-of select="'Instrument Code'"/>
				</InstrumentCode>

				<InstrumentDescription>
					<xsl:value-of select="'Instrument Description'"/>
				</InstrumentDescription>

				<StrikePrice>
					<xsl:value-of select="'Strike Price'"/>
				</StrikePrice>

				<PutCall>
					<xsl:value-of select="'Put/Call Indicator'"/>
				</PutCall>

				<Action>
					<xsl:value-of select="'Action'"/>
				</Action>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Price>
					<xsl:value-of select="'Trade/All-In-Price'"/>
				</Price>

				<YieldRate>
					<xsl:value-of select="'Yield/Rate'"/>
				</YieldRate>

				<Principal>
					<xsl:value-of select="'Principal'"/>
				</Principal>

				<SettlementCurrency>
					<xsl:value-of select="'Settlement CCY'"/>
				</SettlementCurrency>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset != 'FX' and Asset != 'FXForward']">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>

					<!--for system use only-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

					<Product>
						<xsl:choose>
							<xsl:when test="Asset = 'Equity'">
								<xsl:value-of select="'EQ'"/>
							</xsl:when>
							<xsl:when test="Asset = 'Future'">
								<xsl:value-of select="'FU'"/>
							</xsl:when>
							<xsl:when test="Asset = 'FixedIncome'">
								<xsl:value-of select="'FI'"/>
							</xsl:when>
							<xsl:when test="Asset = 'EquityOption' or Asset = 'FutureOption'">
								<xsl:value-of select="'OP'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Product>

					<RecordAction>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated' or TaxLotState='Sent'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amemded'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select="'D'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring(TaxLotState,1,1)"/>
							</xsl:otherwise>
						</xsl:choose>
					</RecordAction>

					<TradeType>
						<xsl:value-of select="'A'"/>
					</TradeType>

					<Seqno>
						<xsl:value-of select="TradeRefID"/>
					</Seqno>

					<Traderid>
						<xsl:value-of select="AccountName"/>
					</Traderid>

					<TradeDate>
						<xsl:value-of select="concat(substring(TradeDate,7,4),substring(TradeDate,1,2),substring(TradeDate,4,2))"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="concat(substring(SettlementDate,7,4),substring(SettlementDate,1,2),substring(SettlementDate,4,2))"/>
					</SettleDate>

					<EffectiveDate>
						<!--<xsl:choose>
							<xsl:when test="Asset = 'FixedIncome'">
								<xsl:value-of select="concat(substring(SettlementDate,7,4),substring(SettlementDate,1,2),substring(SettlementDate,4,2))"/>
							</xsl:when>
							<xsl:otherwise>-->
						<xsl:value-of select="''"/>
						<!--</xsl:otherwise>
						</xsl:choose>-->
					</EffectiveDate>

					<!-- Need to ask-->
					<Clearer>
						<xsl:value-of select="'C Z205H 70KV3'"/>
					</Clearer>

					<Counterparty>
						<xsl:value-of select="CounterParty"/>
					</Counterparty>

					<InstrumentCodeType>
						<xsl:value-of select="'B'"/>
					</InstrumentCodeType>

					<InstrumentCode>
						<xsl:value-of select="BBCode"/>
					</InstrumentCode>

					<InstrumentDescription>
						<xsl:value-of select="FullSecurityName"/>
					</InstrumentDescription>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test="Asset = 'EquityOption' or Asset = 'FutureOption'">
								<xsl:value-of select="format-number(StrikePrice,'#.00000000')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

					<PutCall>
						<xsl:value-of select="substring(PutOrCall,1,1)"/>
					</PutCall>

					<Action>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</Action>

					<Quantity>
						<xsl:value-of select="format-number(AllocatedQty,'#.000000')"/>
					</Quantity>

					<Price>
						<xsl:value-of select="format-number(AveragePrice,'#.00000000')"/>
					</Price>

					<YieldRate>
						<!--<xsl:choose>
							<xsl:when test="Asset = 'FixedIncome'">
								<xsl:value-of select="format-number(Coupon,'#.000000')"/>
							</xsl:when>
							<xsl:otherwise>-->
								<xsl:value-of select="''"/>
							<!--</xsl:otherwise>
						</xsl:choose>-->
					</YieldRate>

					<Principal>
						<xsl:value-of select="format-number(GrossAmount,'#.000000')"/>
					</Principal>

					<SettlementCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SettlementCurrency>

					<Commission>
						<xsl:value-of select="CommissionCharged div AllocatedQty"/>
					</Commission>


					<FileFooter>
						<xsl:value-of select="'true'"/>
					</FileFooter>

					<!-- system use only-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
