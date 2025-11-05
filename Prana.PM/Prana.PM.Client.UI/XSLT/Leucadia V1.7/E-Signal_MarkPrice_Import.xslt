<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL6)">
					<PositionMaster>

						<Symbol>
							<xsl:value-of select ="COL1" />
						</Symbol>

						<PBSymbol>
							<xsl:value-of select ="COL1"/>
						</PBSymbol>

						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(COL6)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<Date>
							<xsl:value-of select ="''"/>
						</Date>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>

		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
