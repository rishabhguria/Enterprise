<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

					<xsl:variable name="PB_NAME" select="'Wells'"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="FundName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

					<TRADEDATE>
						<xsl:value-of select="TradeDate"/>
					</TRADEDATE>

					<ACCOUNT>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</ACCOUNT>

					<ACTION>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Close' or Side='Sell'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ACTION>

					<SHARES>
						<xsl:value-of select="AllocatedQty"/>
					</SHARES>

					<TICKER>
						<xsl:choose>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:choose>
									<xsl:when test="BBCode!=''">
										<xsl:value-of select="substring-before(BBCode,' ')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="normalize-space(OSIOptionSymbol)"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'FixedIncome')">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</TICKER>

					<ASSET_TYPE>
						<!--<xsl:choose>
							<xsl:when test="contains(Asset,'Future')">
								<xsl:value-of select="'FUTR'"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="'OPT'"/>
							</xsl:when>
							<xsl:when test="contains(Asset,'FixedIncome')">
								<xsl:value-of select="'FIXINC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'EQ'"/>
							</xsl:otherwise>
						</xsl:choose>-->
						<xsl:choose>
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="'Futures'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>

					</ASSET_TYPE>

					<SECURITY_SUB_TYPE>
						<xsl:value-of select="UDASecurityTypeName"/>
					</SECURITY_SUB_TYPE>

					<PRICE_FACTOR>
						<xsl:value-of select ="AssetMultiplier"/>
					</PRICE_FACTOR>

					<!--<Blank>
						<xsl:value-of select="''"/>
					</Blank>-->

					<PRICE>
						<xsl:value-of select="AveragePrice"/>
					</PRICE>

					<COMMISSION_METHOD>
						<xsl:value-of select ="'PER SHARE'"/>
					</COMMISSION_METHOD>

					<ENTRY_COMMISSION>
						<xsl:value-of select="CommissionCharged div (AllocatedQty * AssetMultiplier)"/>
					</ENTRY_COMMISSION>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<BROKER>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
								<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</BROKER>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>