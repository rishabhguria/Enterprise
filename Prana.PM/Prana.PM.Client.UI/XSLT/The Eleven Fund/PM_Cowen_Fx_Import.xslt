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

				<xsl:variable name="PB_CURRENCY_NAME">
					<xsl:value-of select="COL6"/>
				</xsl:variable>
				
				<xsl:variable name="varFXRate">					
					<xsl:choose>
						<xsl:when test="$PB_CURRENCY_NAME = 'JPY'">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL11) div 1"/>
							</xsl:call-template>
						</xsl:when>						
						<xsl:otherwise>
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL11)"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
				<xsl:if test="number($varFXRate) ">
					
					<PositionMaster>
						
						<BaseCurrency>
							<xsl:value-of select="$PB_CURRENCY_NAME"/>
						</BaseCurrency>
												
						
						<SettlementCurrency>
							<xsl:value-of select="'USD'"/>
						</SettlementCurrency>
						
						<ForexPrice>
							<xsl:choose>
								<xsl:when test="$varFXRate &gt;0">
									<xsl:value-of select="$varFXRate"/>
								</xsl:when>
								<xsl:when test="$varFXRate &lt;0">
									<xsl:value-of select="$varFXRate * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</ForexPrice>
						
						<!-- <FXConversionMethodOperator>
              <xsl:choose>
                <xsl:when test="$PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD' ">
                  <xsl:value-of select="'M'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'D'"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXConversionMethodOperator> -->
						<xsl:variable name="Date" select="COL1"/>
						<Date>
							<xsl:value-of select="$Date"/>
						</Date>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>