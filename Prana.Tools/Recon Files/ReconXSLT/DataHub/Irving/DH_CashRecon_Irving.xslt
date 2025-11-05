<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL13"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($Cash) and COL4='Cash'">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Triad'"/>
              </xsl:variable>

              <xsl:variable name="PB_FUND_NAME">
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME !=''">
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:when>

                  
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PortfolioAccount>

				<xsl:variable name="varCurrency">
					<xsl:choose>
						<xsl:when test="COL5='EURO CURRENCY'">
							<xsl:value-of select="'EUR'"/>
						</xsl:when>
						<xsl:when test="COL5='U.S. DOLLAR'">
							<xsl:value-of select="'USD'"/>
						</xsl:when>
						<xsl:when test="COL5='SOUTH AFRICAN RAND CURRENCY'">
							<xsl:value-of select="'ZAR'"/>
						</xsl:when>
						<xsl:when test="COL5='TURKISH LIRA_NEW CURRENCY'">
							<xsl:value-of select="'TRY'"/>
						</xsl:when>
						<xsl:when test="COL5='SWISS FRANC CURRENCY'">
							<xsl:value-of select="'CHF'"/>
						</xsl:when>
						<xsl:when test="COL5='CANADIAN DOLLAR CURRENCY'">
							<xsl:value-of select="'CAD'"/>
						</xsl:when>
						<xsl:when test="COL5='SOUTH KOREAN WON CURRENCY'">
							<xsl:value-of select="'KRW'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL5"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				
              <Currency>
                <xsl:value-of select="$varCurrency"/>
              </Currency>
              
              <OpeningBalanceDR>
                <xsl:choose>
                  <xsl:when test="number($Cash)">
                    <xsl:value-of select="$Cash"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceDR>
            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>
              <PortfolioAccount>
                <xsl:value-of select="''"/>
              </PortfolioAccount>
              <Currency>
                <xsl:value-of select="''"/>
              </Currency>
              <OpeningBalanceDR>
                <xsl:value-of select="0"/>
              </OpeningBalanceDR>
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>