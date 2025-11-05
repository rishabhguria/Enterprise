<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="string-length(normalize-space(COL3))&lt; 4">
				
				<PositionMaster>
						
						<BaseCurrency>
							<xsl:value-of select ="COL3"/>
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:value-of select="COL2"/>
						</SettlementCurrency>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL4))">
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
							<xsl:when test ="COL1!=''">
								<Date>
									<xsl:value-of select="normalize-space(COL1)"/>
								</Date>
							</xsl:when>
							<xsl:otherwise>
								<Date>
									<xsl:value-of select="''"/>
								</Date>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
					</xsl:if>
				</xsl:for-each>
			</DocumentElement>
		</xsl:template>
	</xsl:stylesheet>


	