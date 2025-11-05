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
			<xsl:for-each select="//PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(COL50,'&quot;','')"/>
				</xsl:variable>
				<xsl:variable name = "varInstrumentTypeDesc" >
					<xsl:value-of select="translate(COL51,'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="($varInstrumentType='CASH' and $varInstrumentTypeDesc='CASH')">
					<PositionMaster>
						<BaseCurrency>
							<!--<xsl:value-of select="'USD'"/>-->
							<xsl:value-of select="translate(COL44,'&quot;','')"/>
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:value-of select="'USD'"/>
							<!--<xsl:value-of select="translate(COL44,'&quot;','')"/>-->
						</SettlementCurrency>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL46))">
								<ForexPrice>
									<xsl:value-of select="COL46"/>
								</ForexPrice>
							</xsl:when >
							<xsl:otherwise>
								<ForexPrice>
									<xsl:value-of select="0"/>
								</ForexPrice>
							</xsl:otherwise>
						</xsl:choose >

						<xsl:choose>
							<xsl:when test ="COL1 = '038C54502' or COL1 = '*' or COL1 = 'MAC001RX - Normalized Rollup Extract'">
								<Date>
									<xsl:value-of select="''"/>
								</Date>
							</xsl:when >
							<xsl:otherwise>
								<Date>
									<xsl:value-of select="translate(COL1,'&quot;','')"/>
								</Date>
							</xsl:otherwise>
						</xsl:choose>

						<FXConversionMethodOperator>
							<xsl:value-of select ="COL47"/>
						</FXConversionMethodOperator>
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
