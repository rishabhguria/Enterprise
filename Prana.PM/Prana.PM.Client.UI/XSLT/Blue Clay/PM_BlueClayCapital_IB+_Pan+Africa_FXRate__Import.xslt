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
				<xsl:variable name="varMarketValueLocal_R">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL18"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="varMarketValueBase_S">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL19"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:variable name="varFxrate_CNY">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$varMarketValueLocal_R div $varMarketValueBase_S"/>
					</xsl:call-template>
				</xsl:variable>
						
				<xsl:variable name="varFxrate_GBP">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$varMarketValueBase_S div $varMarketValueLocal_R"/>
					</xsl:call-template>
				</xsl:variable>
      
		<xsl:if test="number($varFxrate_GBP)">					
		<PositionMaster>
            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>
            <BaseCurrency>
              <xsl:choose>
                <xsl:when test="$PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD'">
                  <xsl:value-of select="$PB_CURRENCY_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'USD'"/>
                </xsl:otherwise>
              </xsl:choose>
            </BaseCurrency>

            <SettlementCurrency>
              <xsl:choose>
                <xsl:when test="$PB_CURRENCY_NAME='EUR' or $PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='AUD' or $PB_CURRENCY_NAME='NZD'">
                  <xsl:value-of select="'USD'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_CURRENCY_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </SettlementCurrency>
            
						
            
            <xsl:variable name="varForexPrice">
              <xsl:choose>
                <xsl:when test="normalize-space(COL10) = 'GBP'">
                  <xsl:value-of select="$varFxrate_GBP" />
                </xsl:when>
				
				<xsl:when test="normalize-space(COL10) = 'CNH'">
                  <xsl:value-of select="$varFxrate_CNY" />
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="''" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
						<ForexPrice>
							<xsl:choose>
								<xsl:when test="$varForexPrice &gt;0">
									<xsl:value-of select="$varForexPrice"/>
								</xsl:when>
								<xsl:when test="$varForexPrice &lt;0">
									<xsl:value-of select="$varForexPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
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