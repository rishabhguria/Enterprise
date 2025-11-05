<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:variable name="FundName" select="substring-after(//Comparision[contains(COL1,'Account Number')]/COL1, ':')"/>

			<xsl:variable name="Date" select="substring-after(//Comparision[contains(COL1,'As of Date')]/COL1, ':')"/>

			<xsl:for-each select="//Comparision">
				<xsl:if test ="number(COL6) and (COL1='FRACTIONAL CUSIP FOR KINDER MORGAN CUSIP 49455U100' or COL1='GOLDMAN SACHS FIN SQ TREAS OBLIGATIONS FD #469 - ADMIN SHS')">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'SalemTrust'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="normalize-space($FundName)"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<Symbol>
							<xsl:value-of select="'USD'"/>
						</Symbol>

						<xsl:variable name="varCashValueLocal" select="number(COL6)"/>

						<EndingQuantity>
							<xsl:choose>
								<xsl:when  test="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</EndingQuantity>

						<TradeDate>
							<xsl:value-of select ="normalize-space($Date)"/>
						</TradeDate>					

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
