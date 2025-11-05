<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name ="varPrice">
					<xsl:value-of select ="translate(COL3,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="number($varPrice)">
					<PositionMaster>
						
						<Symbol>
							<xsl:value-of select ="translate(COL2,'&quot;','')"/>
						</Symbol>
						
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($varPrice)">
									<xsl:value-of select="number($varPrice)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>
						
						<Date>
							<xsl:value-of select ="translate(COL1,'&quot;','')"/>
						</Date>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>