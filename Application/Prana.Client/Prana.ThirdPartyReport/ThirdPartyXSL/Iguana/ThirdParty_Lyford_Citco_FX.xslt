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

				<Seqno>
					<xsl:value-of select="'Seqno'"/>
				</Seqno>

				<Traderid>
					<xsl:value-of select="'Traderid'"/>
				</Traderid>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<ActionOnCurrency>
					<xsl:value-of select="'Action on Currency'"/>
				</ActionOnCurrency>

				<InstrumentCode>
					<xsl:value-of select="'Instrument ISO Code'"/>
				</InstrumentCode>

				<InstrumentDescription>
					<xsl:value-of select="'Instrument Description'"/>
				</InstrumentDescription>

				<DealRate>
					<xsl:value-of select="'Deal Rate'"/>
				</DealRate>

				<Currency1Amt>
					<xsl:value-of select="'Currency1 Amount'"/>
				</Currency1Amt>

				<SettlementCurrency1>
					<xsl:value-of select="'Settlement CCY1'"/>
				</SettlementCurrency1>

				<Currency2Amt>
					<xsl:value-of select="'Currency2 Amount'"/>
				</Currency2Amt>


				<SettlementCurrency2>
					<xsl:value-of select="'Settlement CCY2'"/>
				</SettlementCurrency2>

				<ExpirationDate>
					<xsl:value-of select="'Value Date'"/>
				</ExpirationDate>


				<Counterparty>
					<xsl:value-of select="'Counterparty'"/>
				</Counterparty>
				
				<Clearer>
					<xsl:value-of select="'Clearer'"/>
				</Clearer>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset = 'FX' or Asset = 'FXForward']">
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
							<xsl:when test="Asset = 'FX'">
								<xsl:value-of select="'FX'"/>
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

					<Seqno>
						<xsl:value-of select="TradeRefID"/>
					</Seqno>

					<Traderid>
						<xsl:value-of select="AccountName"/>
					</Traderid>

					<TradeDate>
						<xsl:value-of select="concat(substring(TradeDate,7,4),substring(TradeDate,1,2),substring(TradeDate,4,2))"/>
					</TradeDate>


					<ActionOnCurrency>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Close'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell short'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ActionOnCurrency>

					<InstrumentCode>
						<xsl:value-of select="translate(Symbol,'-',' ')"/>
					</InstrumentCode>

					<InstrumentDescription>
						<xsl:value-of select="FullSecurityName"/>
					</InstrumentDescription>

					<DealRate>
						<xsl:value-of select="AveragePrice"/>
					</DealRate>

					<Currency1Amt>
						<xsl:value-of select="AllocatedQty"/>
					</Currency1Amt>

					<SettlementCurrency1>
						<xsl:value-of select="LeadCurrencyName"/>
					</SettlementCurrency1>

					<Currency2Amt>
						<xsl:value-of select="AllocatedQty * AveragePrice"/>
					</Currency2Amt>

					<SettlementCurrency2>
						<xsl:value-of select="VsCurrencyName"/>
					</SettlementCurrency2>

					<ExpirationDate>
						<xsl:value-of select="concat(substring(ExpirationDate,7,4),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2))"/>
					</ExpirationDate>

					<Counterparty>
						<xsl:value-of select="CounterParty"/>
					</Counterparty>

					<!-- Need to ask-->
					<Clearer>
						<xsl:value-of select="'C Z205H 70KV3'"/>
					</Clearer>

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
