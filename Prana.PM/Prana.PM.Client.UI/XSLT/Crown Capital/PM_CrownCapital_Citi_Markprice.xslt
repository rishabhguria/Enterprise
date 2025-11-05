<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="COL1 !='cobDateMM/DD/YYYY'">
					<PositionMaster>
						<xsl:choose>
							<xsl:when test="COL16 != ''">
								<Symbol>
									<xsl:value-of select="COL16"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when  test="boolean(number(COL29))">
								<MarkPrice>
									<xsl:value-of select="COL29"/>
								</MarkPrice>
							</xsl:when >
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose >

						<Date>
							<xsl:value-of select="COL1"/>
						</Date>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>


