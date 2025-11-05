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

				<xsl:if test ="boolean(number(COL10)) ">
					<PositionMaster>
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
							<!--<xsl:value-of select="translate(COL8,'&quot;','')"/>-->
						</BaseCurrency>

						<SettlementCurrency>
							<!--<xsl:value-of select="'USD'"/>-->
							<xsl:value-of select="COL9"/>
						</SettlementCurrency>

						<ForexPrice>
							<xsl:value-of select="COL10 div COL11"/>
						</ForexPrice>

						<!--<xsl:choose>
							--><!-- For Direct And Indirect Currency--><!--
							<xsl:when test ="(COL9 = 'GBP' or COL9 = 'EUR' or COL9 ='AUD' or COL9 ='NZD' or COL9 ='CAD')">
								<ForexPrice>
									<xsl:value-of select="COL10 div COL11"/>
									--><!--div COL11"/>--><!--
								</ForexPrice>
							</xsl:when >
							<xsl:when test ="(COL9 != 'GBP' or COL9 != 'EUR' or COL9 !='AUD' or COL9 !='NZD' or COL9 !='CAD')">
								<ForexPrice>
									<xsl:value-of select="COL11 div COL10"/>
									
								</ForexPrice>
							</xsl:when >
							<xsl:otherwise>
								<ForexPrice>
									<xsl:value-of select="0"/>
								</ForexPrice>
							</xsl:otherwise>--><!--
						</xsl:choose >-->

						<Date>
							<xsl:value-of select="''"/>
						</Date>
						<!--<FXConversionMethodOperator>
							<xsl:value-of select ="1"/>
						</FXConversionMethodOperator>-->
					</PositionMaster>
				</xsl:if >
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>