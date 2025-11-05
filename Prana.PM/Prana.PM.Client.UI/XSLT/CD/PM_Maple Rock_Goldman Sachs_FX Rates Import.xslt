<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">

				<xsl:if test="number(COL13) or number(COL14) ">
					<PositionMaster>

						<BaseCurrency>
							<xsl:value-of select ="COL3"/>
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:value-of select="COL9"/>
						</SettlementCurrency>


						<xsl:variable name="varFX">
							<xsl:value-of select="number(COL13) div number(COL14)"/>
						</xsl:variable>
						
						<ForexPrice>
							<xsl:choose>
								<xsl:when test="number($varFX)">
									<xsl:value-of select="$varFX"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ForexPrice>

						<Date>
							<xsl:value-of select="''"/>
						</Date>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
