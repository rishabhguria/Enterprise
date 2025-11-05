<?xml version="1.0" encoding="utf-8"?>
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
					<xsl:choose>
						<xsl:when test ="COL2 != 'FromCurrency'">
							<BaseCurrency>
								<xsl:value-of select="translate(COL2,'&quot;','')"/>
							</BaseCurrency>
						</xsl:when>
						<xsl:otherwise>
							<BaseCurrency>
								<xsl:value-of select="COL2"/>
							</BaseCurrency>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test ="COL3 != 'ToCurrency'">
							<SettlementCurrency>
								<xsl:value-of select="translate(COL3,'&quot;','')"/>
							</SettlementCurrency>
						</xsl:when>
						<xsl:otherwise>
							<SettlementCurrency>
								<xsl:value-of select="COL3"/>
							</SettlementCurrency>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:choose>
						<xsl:when test ="boolean(number(COL4)) and COL4 != 'Rate'">
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
						<xsl:when test ="COL1 = 'Date/Time'">
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

					<!--<FXConversionMethodOperator>
							<xsl:value-of select ="COL47"/>
						</FXConversionMethodOperator>-->
				</PositionMaster>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>