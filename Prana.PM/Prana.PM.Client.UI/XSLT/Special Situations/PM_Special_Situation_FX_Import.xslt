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
				
				<xsl:variable name="varFxrate">
					<xsl:value-of select="COL4"/>
				</xsl:variable>
				
				<xsl:if test="number($varFxrate) and normalize-space(COL5)='FXPrice'">
					
					<PositionMaster>
						<xsl:variable name="varBaseCurrency">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>
						
						<BaseCurrency>
							<xsl:value-of select="$varBaseCurrency"/>
						</BaseCurrency>
						
						<xsl:variable name="varSettlementCurrency">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>
						
						<SettlementCurrency>
							<xsl:value-of select="$varSettlementCurrency"/>
						</SettlementCurrency>											
						
						<ForexPrice>
							<xsl:choose>
								<xsl:when test="$varFxrate &gt;0">
									<xsl:value-of select="$varFxrate"/>
								</xsl:when>
								<xsl:when test="$varFxrate &lt;0">
									<xsl:value-of select="$varFxrate * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</ForexPrice>
						
						<xsl:variable name="varDate" select="COL3"/>
						
						<Date>
							<xsl:value-of select="$varDate"/>
						</Date>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>