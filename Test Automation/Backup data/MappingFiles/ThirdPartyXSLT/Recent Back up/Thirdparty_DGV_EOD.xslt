<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>				

				<FundName>
					<xsl:value-of select="'Fund Name'"/>

				</FundName>

				<Trader>
					<xsl:value-of select="'Trader'"/>
				</Trader>

				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<SecurityTicker>
					<xsl:value-of select="'Security Ticker'"/>
				</SecurityTicker>

				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>

				<Expiration>
					<xsl:value-of select="'Expiration'"/>
				</Expiration>

				<StrikePrice>
					<xsl:value-of select="'Strike Price'"/>
				</StrikePrice>



				<ExecutingBroker>
					<xsl:value-of select="'Executing Broker'"/>
				</ExecutingBroker>

				<Domicile>
					<xsl:value-of select="'Domicile'"/>
				</Domicile>

				<OpenClose>
					<xsl:value-of select="'Open/Close'"/>
				</OpenClose>

				<BuySell>
					<xsl:value-of select="'Buy/Sell'"/>

				</BuySell>

				<ContractsPrincipalAmount>
					<xsl:value-of select="'Contracts / Principal Amount'"/>
				</ContractsPrincipalAmount>

				<Price>
					<xsl:value-of select="'Price in $'"/>
				</Price>

				<GrossPremiumValue>
					<xsl:value-of select="'Gross Premium / Value'"/>

				</GrossPremiumValue>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<TradeAwayFee>

					<xsl:value-of select="'Trade-Away Fee'"/>

				</TradeAwayFee>

				<ExchangeFee>

					<xsl:value-of select="'Exchange Fee'"/>

				</ExchangeFee>

				<NetPremiumValue>
					<xsl:value-of select="'Net Premium / Value'"/>
				</NetPremiumValue>


				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>



					<xsl:variable name="PB_NAME" select="'NT'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<FundName>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AccountMappedName"/>
							</xsl:otherwise>
						</xsl:choose>

					</FundName>

					<Trader>
						<xsl:value-of select="TradeAttribute1"/>
					</Trader>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettlementDate>
						<xsl:value-of select="SettlementDate"/>
					</SettlementDate>

					<SecurityTicker>
						<xsl:value-of select="BBCode"/>
					</SecurityTicker>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<Expiration>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Expiration>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</StrikePrice>

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
						<xsl:value-of select="$Broker"/>
					</ExecutingBroker>

					<Domicile>
						<xsl:value-of select="'NTSI'"/>
					</Domicile>

					<OpenClose>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'Open'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Close'"/>
							</xsl:otherwise>
						</xsl:choose>
					</OpenClose>

					<BuySell>
						<xsl:value-of select="Side"/>

					</BuySell>

					<ContractsPrincipalAmount>
						<xsl:choose>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="AllocatedQty * -1 "/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="AllocatedQty"/>
							</xsl:otherwise>
						</xsl:choose>
					</ContractsPrincipalAmount>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<GrossPremiumValue>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="AllocatedQty * AveragePrice * 100"/>
							</xsl:when>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="AllocatedQty * AveragePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</GrossPremiumValue>

					<Commission>
						
								<xsl:value-of select="(CommissionCharged + SoftCommissionCharged) * -1 "/>
						
						
					</Commission>

					<TradeAwayFee>
						
								<xsl:value-of select="OtherBrokerFee * -1 "/>
													
					</TradeAwayFee>

					<ExchangeFee>
						
								<xsl:value-of select="OrfFee * -1 "/>
							
					</ExchangeFee>

					<NetPremiumValue>
					<xsl:value-of select="NetAmount"/>
					</NetPremiumValue>

					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>