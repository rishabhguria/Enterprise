<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
	<xsl:output method="xml" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				<xsl:if test="normalize-space(COL4)='CASH'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varBaseCurrency">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<BaseCurrency>
							<xsl:value-of select="$varBaseCurrency"/>
						</BaseCurrency>

						<SettlementCurrency>
							<xsl:value-of select="'USD'"/>
						</SettlementCurrency>

						<xsl:variable name="varFxrate">
							<xsl:choose>
								<xsl:when test="COL3='EUR'">
									<xsl:value-of select="COL15"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<ForexPrice>
							<xsl:choose>
								<xsl:when test="number($varFxrate)">
									<xsl:value-of select="$varFxrate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ForexPrice>

						<xsl:variable name="varDate" select="''"/>

						<Date>
							<xsl:value-of select="$varDate"/>
						</Date>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>