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
					<xsl:value-of select ="normalize-space(COL2)"/>
				</xsl:variable>
				
				<xsl:variable name="varForexPrice">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL15)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varForexPrice) and ($PB_CURRENCY_NAME='GBP' or $PB_CURRENCY_NAME='JPY')">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>
						
						<BaseCurrency>
							<xsl:choose>
                              <xsl:when test="$PB_CURRENCY_NAME='GBP'">
                                 <xsl:value-of select="$PB_CURRENCY_NAME"/>
                              </xsl:when>
                              <xsl:otherwise>
                                 <xsl:value-of select="'USD'"/>
                              </xsl:otherwise>
                            </xsl:choose>
						</BaseCurrency>
	
						<SettlementCurrency>
							<xsl:choose>
                              <xsl:when test="$PB_CURRENCY_NAME='GBP'">
                                 <xsl:value-of select="'USD'"/>
                              </xsl:when>
                              <xsl:otherwise>
                                 <xsl:value-of select="$PB_CURRENCY_NAME"/>
                              </xsl:otherwise>
                            </xsl:choose>
						</SettlementCurrency>
																				
						<ForexPrice>
							<xsl:choose>
                              <xsl:when test="$PB_CURRENCY_NAME = 'GBP'">
                                <xsl:value-of select="COL16 div COL15"/>
                              </xsl:when>
								 <xsl:when test="$PB_CURRENCY_NAME = 'JPY'">
                                <xsl:value-of select="COL15 div COL16"/>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="$varForexPrice"/>
                              </xsl:otherwise>
                            </xsl:choose>
						</ForexPrice>

						<Date>
							<xsl:value-of select="''"/>
						</Date>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>