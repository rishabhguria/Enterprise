<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varFX">
					<xsl:value-of select="number(translate(translate(COL45,',',''),'$','')) div number(translate(translate(COL46,',',''),'$',''))"/>
				</xsl:variable>

				<xsl:if test="number($varFX)">

					<PositionMaster>

						<BaseCurrency>
							<xsl:value-of select="COL31"/>							
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:value-of select ="COL25"/>
						</SettlementCurrency>

						<ForexPrice>
							<xsl:choose>
								<xsl:when test ="number($varFX)">
									<xsl:value-of select="$varFX"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose >
						</ForexPrice>

						<Date>
							<xsl:value-of select="COL4"/>
						</Date>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
