<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="number(COL7)">

					<PositionMaster>

						<SettlementCurrency>
							<xsl:value-of select="COL6"/>
						</SettlementCurrency>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<Date>
							<xsl:value-of select="''"/>
						</Date>

						<xsl:variable name="varFXPrice">
							<xsl:choose>
								<xsl:when test="number(COL11) and number(COL11)" >
									<xsl:value-of select="COL11 div COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</xsl:variable>
						
						<ForexPrice>
							<xsl:value-of select="$varFXPrice"/>
						</ForexPrice>



					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
