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
				<PositionMaster>



					<BaseCurrency>
						<xsl:value-of select="translate(COL2,'&quot;','')"/>
					</BaseCurrency>

					<SettlementCurrency>
						<xsl:value-of select="translate(COL3,'&quot;','')"/>
					</SettlementCurrency>

					<xsl:choose>
						<xsl:when test ="COL4 &lt; 0 or COL4 &gt; 0 or COL4 = 0">
							<ForexPrice>
								<xsl:value-of select="COL4"/>
							</ForexPrice>
						</xsl:when >
						<xsl:otherwise>
							<ForexPrice>
								<xsl:value-of select="0"/>
							</ForexPrice>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:choose>
						<xsl:when test ="COL1 = 'Trade Date' or COL1 = '*'">
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
						<xsl:value-of select ="COL5"/>
					</FXConversionMethodOperator>
				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
