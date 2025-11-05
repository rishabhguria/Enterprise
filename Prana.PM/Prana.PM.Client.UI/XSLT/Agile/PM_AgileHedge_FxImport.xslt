<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				<xsl:if test="number(COL3)">
					<PositionMaster>

						<BaseCurrency>
							<xsl:value-of select ="substring-before(COL1,' ')"/>
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:value-of select="substring-after(COL1,' ')"/>
						</SettlementCurrency>

						<xsl:variable name ="varFx" select ="number(COL3)"/>

						<ForexPrice>
							<xsl:choose>

								<xsl:when test ="$varFx &lt;0">
									<xsl:value-of select ="$varFx* -1"/>
								</xsl:when>

								<xsl:when test ="$varFx &gt;0">
									<xsl:value-of select ="$varFx"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ForexPrice>


						<Date>
							<xsl:value-of select="COL2"/>
						</Date>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
