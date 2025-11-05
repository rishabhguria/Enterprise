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
        <xsl:variable name="CashDr">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($CashDr)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
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
                <xsl:value-of select="normalize-space(COL3)"/>
              </xsl:variable>

              <Currency>
                <xsl:choose>
                  <xsl:when test="$varCurrency='US Dollar'">
                    <xsl:value-of select="'USD'"/>
                  </xsl:when>

                  <xsl:when test="$varCurrency='Canadian Dollar'">
                    <xsl:value-of select="'CAD'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Currency>

              <CompanyName>
                <xsl:value-of select="normalize-space(COL4)"/>
              </CompanyName>

              <OpeningBalanceDR>
                <xsl:choose>
                  <xsl:when test="number($CashDr)">
                    <xsl:value-of select="$CashDr"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceDR>

              <xsl:variable name="CashCr">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL6"/>
                </xsl:call-template>
              </xsl:variable>

              <OpeningBalanceCR>
                <xsl:choose>
                  <xsl:when test="number($CashCr)">
                    <xsl:value-of select="$CashCr"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceCR>
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
              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
              <OpeningBalanceDR>
                <xsl:value-of select="0"/>
              </OpeningBalanceDR>
              <OpeningBalanceCR>
                <xsl:value-of select="0"/>
              </OpeningBalanceCR>
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>