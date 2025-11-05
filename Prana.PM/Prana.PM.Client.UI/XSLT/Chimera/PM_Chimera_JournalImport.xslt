<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varMarketValue">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL14)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varMarketValue) and normalize-space(COL1)!='Grand Total:'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "varClientNav">
              <xsl:choose>

                <xsl:when test="normalize-space(COL1)='Booth Bay DA (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('28,800,000')"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="normalize-space(COL1)='Booth Bay SMA (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('48,750,000')"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="normalize-space(COL1)='Stevens Capital (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('20,000,000')"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="normalize-space(COL1)='Walleye Concentrated (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('50,000,000')"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="normalize-space(COL1)='Walleye WIF (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('57,000,000')"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="normalize-space(COL1)='Walleye WIF Systematic (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('3,000,000')"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="normalize-space(COL1)='Walleye WOF (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('118,750,000')"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test="normalize-space(COL1)='Walleye WOF Systematic (USD)'">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="normalize-space('6,250,000')"/>
                  </xsl:call-template>
                </xsl:when>
                
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="PB_FUND_NAME">
              <xsl:choose>
                <xsl:when test ="contains(COL1,'(USD)')">
                  <xsl:value-of select ="substring-before(COL1,'(USD)')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="normalize-space(COL1)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

            <xsl:variable name="varCurrency" select="normalize-space(COL8)"/>
            <CurrencyName>
              <xsl:value-of select ="$varCurrency"/>
            </CurrencyName>

            <xsl:variable name="AbsCash">
              <xsl:choose>
                <xsl:when test="($varMarketValue - $varClientNav) &gt; 0">
                  <xsl:value-of select="($varMarketValue - $varClientNav)"/>
                </xsl:when>
                <xsl:when test="($varMarketValue - $varClientNav) &lt; 0">
                  <xsl:value-of select="($varMarketValue - $varClientNav) * -1"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <JournalEntries>
              <xsl:choose>
                <xsl:when test="($varMarketValue - $varClientNav) &gt; 0">
                  <xsl:value-of select="concat('CashTransferIn', ':' , $AbsCash , '|' , 'Margin A/C', ':' , $AbsCash)"/>
                </xsl:when>
                <xsl:when test="($varMarketValue - $varClientNav) &lt; 0">
                  <xsl:value-of select="concat('Margin A/C', ':' , $AbsCash , '|' , 'CashTransferIn', ':' , $AbsCash)"/>
                </xsl:when>
              </xsl:choose>
            </JournalEntries>

            <Date>
              <xsl:value-of select="''"/>
            </Date>
            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>