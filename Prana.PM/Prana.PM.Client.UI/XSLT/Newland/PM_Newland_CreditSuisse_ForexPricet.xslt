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
						<xsl:value-of select="'USD'"/>
						<!--<xsl:value-of select="translate(COL4,'&quot;','')"/>-->
					</BaseCurrency>

					<SettlementCurrency>
						<!--<xsl:value-of select="'USD'"/>-->
						<xsl:value-of select="translate(COL4,'&quot;','')"/>
					</SettlementCurrency>

					<xsl:choose>
						<xsl:when test ="boolean(number(COL13))">
							<ForexPrice>
								<xsl:value-of select="COL13"/>
							</ForexPrice>
						</xsl:when >
						<xsl:otherwise>
							<ForexPrice>
								<xsl:value-of select="0"/>
							</ForexPrice>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:variable name ="varYear">
						<xsl:value-of select ="substring(COL1,1,4)"/>
					</xsl:variable>

					<xsl:variable name ="varMonth">
						<xsl:value-of select ="substring(COL1,5,2)"/>
					</xsl:variable>

					<xsl:variable name ="varDay">
						<xsl:value-of select ="substring(COL1,7,2)"/>
					</xsl:variable>

					<Date>
						<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
					</Date>

					<!--<FXConversionMethodOperator>
							<xsl:value-of select ="COL47"/>
						</FXConversionMethodOperator>-->
				</PositionMaster>
				
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
