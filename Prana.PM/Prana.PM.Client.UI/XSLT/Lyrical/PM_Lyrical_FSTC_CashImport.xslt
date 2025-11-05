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

			<xsl:variable name="FundName" select="//PositionMaster[contains(COL1,'Account Name ')]/COL2"/>
			<!--<xsl:variable name="varCash" select="//PositionMaster[contains(COL3,'Northern Institutional Treasury Portfolio')]/COL8"/>-->

			<xsl:variable name="Date" select="//PositionMaster[contains(COL1,'Processing Date')]/COL2"/>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="COL3 = 'Northern Institutional Treasury Portfolio '">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'FSTC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="$FundName"/>
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

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>

						

						<!--<xsl:variable name="varCashValueLocal" select="number(COL6)"/>-->
						<xsl:variable name="varCashValueLocal">
							<xsl:value-of select="normalize-space(COL8)"/>
						</xsl:variable>
						<CashValueLocal>
							<xsl:choose>
								<xsl:when  test="number($varCashValueLocal)">
									<xsl:value-of select="$varCashValueLocal"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
							<!--<xsl:value-of select="$varCash"/>-->
						</CashValueLocal>

						<Date>
							<xsl:value-of select ="normalize-space($Date)"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
