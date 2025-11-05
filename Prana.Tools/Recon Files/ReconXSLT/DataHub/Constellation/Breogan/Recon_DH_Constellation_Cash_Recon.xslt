<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>

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
<xsl:template name="FormatDate">
<xsl:param name="DateTime" />
<!-- converts date time double number to 18/12/2009 -->

<xsl:variable name="l">
<xsl:value-of select="$DateTime + 68569 + 2415019" />
</xsl:variable>

<xsl:variable name="n">
<xsl:value-of select="floor(((4 * $l) div 146097))" />
</xsl:variable>

<xsl:variable name="ll">
<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
</xsl:variable>

<xsl:variable name="i">
<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
</xsl:variable>

<xsl:variable name="lll">
<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
</xsl:variable>

<xsl:variable name="j">
<xsl:value-of select="floor(((80 * $lll) div 2447))" />
</xsl:variable>

<xsl:variable name="nDay">
<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
</xsl:variable>

<xsl:variable name="llll">
<xsl:value-of select="floor(($j div 11))" />
</xsl:variable>

<xsl:variable name="nMonth">
<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
</xsl:variable>

<xsl:variable name="nYear">
<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
</xsl:variable>

<xsl:variable name ="varMonthUpdated">
<xsl:choose>
<xsl:when test ="string-length($nMonth) = 1">
<xsl:value-of select ="concat('0',$nMonth)"/>
</xsl:when>
<xsl:otherwise>
<xsl:value-of select ="$nMonth"/>
</xsl:otherwise>
</xsl:choose>
</xsl:variable>

<xsl:variable name ="nDayUpdated">
<xsl:choose>
<xsl:when test ="string-length($nDay) = 1">
<xsl:value-of select ="concat('0',$nDay)"/>
</xsl:when>
<xsl:otherwise>
<xsl:value-of select ="$nDay"/>
</xsl:otherwise>
</xsl:choose>
</xsl:variable>

<xsl:value-of select="$varMonthUpdated"/>
<xsl:value-of select="'/'"/>
<xsl:value-of select="$nDayUpdated"/>
<xsl:value-of select="'/'"/>
<xsl:value-of select="$nYear"/>

</xsl:template>

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//Comparision">
        
         <xsl:variable name="COL5">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL5"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="COL6">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL6"/>
                </xsl:call-template>
              </xsl:variable>
              
              <xsl:variable name="OpeningBalanceDR">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="$COL5 + $COL6"/>
                </xsl:call-template>
              </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($OpeningBalanceDR)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="PB_FUND_NAME" select="COL2"/>

              <xsl:variable name ="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <PortfolioAccount>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>

                </xsl:choose>
              </PortfolioAccount>

              <xsl:variable name="varCurrency" select="COL4"/>

              <Currency>
                <xsl:value-of select ="$varCurrency"/>
              </Currency>

              <OpeningBalanceDR>
                <xsl:choose>
                  <xsl:when test="number($OpeningBalanceDR)">
                    <xsl:value-of select="$OpeningBalanceDR"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceDR>

              <xsl:variable name="OpeningBalanceCR">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="''"/>
                </xsl:call-template>
              </xsl:variable>
              <OpeningBalanceCR>
                <xsl:choose>
                  <xsl:when test="number($OpeningBalanceCR)">
                    <xsl:value-of select="$OpeningBalanceCR"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OpeningBalanceCR>

              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>

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

              <OpeningBalanceCR>
                <xsl:value-of select="0"/>
              </OpeningBalanceCR>
				
				<xsl:variable name="TradeDate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL1"/>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select ="$TradeDate"/>
              </TradeDate>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>