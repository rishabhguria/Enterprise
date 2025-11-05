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

					<xsl:variable name="PB_NAME" select="'BNP'"/>

					<!--<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="FundName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>-->

					<trd_id>
						<xsl:value-of select="concat('A',EntityID)"/>
					</trd_id>

					<txn_code>
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<xsl:value-of select="'BY'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'CS'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Close' or Side='Sell'">
								<xsl:value-of select="'SL'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</txn_code>					

					<qty_filled>
						<xsl:value-of select="AllocatedQty"/>
					</qty_filled>

					<symbol>
						<xsl:choose>							
							<xsl:when test="contains(Asset,'Option')">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</symbol>

					<price>
						<xsl:choose>
							<xsl:when test="AveragePrice &gt; 0">
								<xsl:value-of select="AveragePrice"/>
							</xsl:when>
							<xsl:when test="AveragePrice &lt; 0">
								<xsl:value-of select="AveragePrice * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</price>

					<cl_id>
						<xsl:value-of select="''"/>
					</cl_id>

					<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_BROKER_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_BROKER_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<exec_brk>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
								<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_BROKER_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</exec_brk>
					
					<trade_date>
						<xsl:value-of select ="TradeDate"/>
					</trade_date>

					<commis_type>
						<xsl:value-of select="'T'"/>
					</commis_type>

					<commision>
						<xsl:choose>
							<xsl:when test="CommissionCharged &gt; 0">
								<xsl:value-of select="CommissionCharged"/>
							</xsl:when>
							<xsl:when test="CommissionCharged &lt; 0">
								<xsl:value-of select="CommissionCharged * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</commision>

					<settle_date>
						<xsl:value-of select="SettlementDate"/>
					</settle_date>



					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>