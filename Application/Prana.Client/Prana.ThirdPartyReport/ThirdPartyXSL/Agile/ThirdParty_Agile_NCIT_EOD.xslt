<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			
			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxlotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxlotState>
				
				<OrderID>
					<xsl:value-of select="'Order ID'"/>
				</OrderID>

				<TradeID>
					<xsl:value-of select="'Trade ID'"/>
				</TradeID>

				<TradeDirection>
					<xsl:value-of select="'Trade Direction'"/>
				</TradeDirection>

				<Status>
					<xsl:value-of select="'Status'"/>
				</Status>


				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<UnderlyingSecurityName>
					<xsl:value-of select="'Underlying Security Name'"/>
				</UnderlyingSecurityName>


				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>


				<RIC>
					<xsl:value-of select="'RIC'"/>
				</RIC>

				<TICKER>
					<xsl:value-of select="'TICKER'"/>
				</TICKER>

				<UnderlyingSecurityCurrency>
					  <xsl:value-of select="'Underlying Security Currency'"/>
				</UnderlyingSecurityCurrency>

				<SwapCurrency>
					<xsl:value-of select="'Swap Currency'"/>
				</SwapCurrency>


				<Tradedate>
					<xsl:value-of select="'Trade date'"/>
				</Tradedate>

				<SettleDate>
					<xsl:value-of select="'Settle Date'"/>
				</SettleDate>


				<GrossPrice>
					<xsl:value-of select="'Gross Price'"/>
				</GrossPrice>

				<SwapGrossPrice>
					<xsl:value-of select="'Swap Gross Price'"/>
				</SwapGrossPrice>

				<NetPrice>
					<xsl:value-of select="'Net Price'"/>
				</NetPrice>

				<SwapNetPrice>
					<xsl:value-of select="'Swap Net Price'"/>
				</SwapNetPrice>

				<GrossNotional>
					<xsl:value-of select="'Gross Notional'"/>
				</GrossNotional>

				<SwapGrossNotional>
					<xsl:value-of select="'Swap Gross Notional'"/>
				</SwapGrossNotional>

				<NetNotional>
					<xsl:value-of select="'Net Notional'"/>
				</NetNotional>

				<SwapNetNotional>
					<xsl:value-of select="'Swap Net Notional'"/>
				</SwapNetNotional>

				<ExecutingBroker>
					<xsl:value-of select="'Executing Broker'"/>
				</ExecutingBroker>

				<AccountNumber>
					<xsl:value-of select="'Account Number'"/>
				</AccountNumber>

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>


			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>


					<OrderID>
						<xsl:value-of select="concat('A',EntityID)"/>
					</OrderID>

					<TradeID>
						<xsl:value-of select="concat('A',EntityID)"/>
					</TradeID>

					<TradeDirection>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select ="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Sell'">
								<xsl:value-of select ="'S'"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</TradeDirection>

					<Status>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select ="'LIVE'"/>
							</xsl:when>
							<xsl:when test ="TaxLotState = 'Amended'">
								<xsl:value-of  select="'CANCEL'"/>
							</xsl:when>
							
							<xsl:when test="TaxLotState='Deleted'">
								<xsl:value-of select ="'CANCEL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Status>


					<Quantity>
						<xsl:choose>
							<xsl:when test ="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>

					<UnderlyingSecurityName>
						<xsl:value-of select="FullSecurityName"/>
					</UnderlyingSecurityName>


					<ISIN>
						<xsl:choose>
							<xsl:when test="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ISIN>

					<SEDOL>
						<xsl:choose>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SEDOL>

					<CUSIP>
						<xsl:choose>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="CUSIP"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CUSIP>

					<RIC>
						<xsl:choose>
							<xsl:when test="RIC!=''">
								<xsl:value-of select="RIC"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</RIC>

					<TICKER>
						<xsl:value-of select="Symbol"/>
					</TICKER>

					<UnderlyingSecurityCurrency>
						<xsl:value-of select="''"/>
					</UnderlyingSecurityCurrency>

					<SwapCurrency>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="CurrencySymbol"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SwapCurrency>

					<xsl:variable name="varYear">
						<xsl:value-of select="substring-after(substring-after(TradeDate,'/'),'/')"/>
					</xsl:variable>

					<xsl:variable name="varMonth">
						<xsl:value-of select="substring-before(TradeDate,'/')"/>
					</xsl:variable>

					<xsl:variable name="varDay">
						<xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
					</xsl:variable>

					<Tradedate>
						<xsl:value-of select="concat($varYear,$varMonth,$varDay)"/>
					</Tradedate>

					<xsl:variable name="varSYear">
						<xsl:value-of select="substring-after(substring-after(SettlementDate,'/'),'/')"/>
					</xsl:variable>

					<xsl:variable name="varSMonth">
						<xsl:value-of select="substring-before(SettlementDate,'/')"/>
					</xsl:variable>

					<xsl:variable name="varSDay">
						<xsl:value-of select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
					</xsl:variable>

					<SettleDate>
						<xsl:value-of select="''"/>
					</SettleDate>


					<GrossPrice>
						<xsl:value-of select="''"/>
					</GrossPrice>

					<SwapGrossPrice>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SwapGrossPrice>

					<NetPrice>
						<xsl:choose>
							<xsl:when test="IsSwapped='true'">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPrice>

					<SwapNetPrice>
						<xsl:value-of select="''"/>
					</SwapNetPrice>

					<GrossNotional>
						<xsl:value-of select="''"/>
					</GrossNotional>

					<SwapGrossNotional>
						<xsl:value-of select="''"/>
					</SwapGrossNotional>

					<NetNotional>
						<xsl:value-of select="''"/>
					</NetNotional>

					<SwapNetNotional>
						<xsl:value-of select="''"/>
					</SwapNetNotional>

					<xsl:variable name="PB_NAME" select="' '"/>
					
					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<ExecutingBroker>
						<xsl:value-of select="''"/>
					</ExecutingBroker>
					
				

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>
					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<xsl:variable name="varAccountName">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<AccountNumber>
						<xsl:value-of select="$varAccountName"/>
					</AccountNumber>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
