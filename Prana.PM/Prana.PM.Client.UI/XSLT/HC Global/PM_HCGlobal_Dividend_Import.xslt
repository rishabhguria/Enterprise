<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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
    <xsl:param name="DateTime"/>
    <!--  converts date time double number to 18/12/2009  -->
    <xsl:variable name="l">
      <xsl:value-of select="$DateTime + 68569 + 2415019"/>
    </xsl:variable>
    <xsl:variable name="n">
      <xsl:value-of select="floor(((4 * $l) div 146097))"/>
    </xsl:variable>
    <xsl:variable name="ll">
      <xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
    </xsl:variable>
    <xsl:variable name="i">
      <xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
    </xsl:variable>
    <xsl:variable name="lll">
      <xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
    </xsl:variable>
    <xsl:variable name="j">
      <xsl:value-of select="floor(((80 * $lll) div 2447))"/>
    </xsl:variable>
    <xsl:variable name="nDay">
      <xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
    </xsl:variable>
    <xsl:variable name="llll">
      <xsl:value-of select="floor(($j div 11))"/>
    </xsl:variable>
    <xsl:variable name="nMonth">
      <xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
    </xsl:variable>
    <xsl:variable name="nYear">
      <xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
    </xsl:variable>
    <xsl:variable name="varMonthUpdated">
      <xsl:choose>
        <xsl:when test="string-length($nMonth) = 1">
          <xsl:value-of select="concat('0',$nMonth)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$nMonth"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="nDayUpdated">
      <xsl:choose>
        <xsl:when test="string-length($nDay) = 1">
          <xsl:value-of select="concat('0',$nDay)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$nDay"/>
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
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL13)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varDividend)">

          <PositionMaster>

            <xsl:variable name="PB_Name">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="normalize-space(COL7)"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:when test ="contains(COL8,'AWAY') or contains(COL8,'BTIG')">
                  <xsl:value-of select ="'BTIG : AT2R'"/>
                </xsl:when>

                <xsl:when test ="contains(COL8,'UBS')">
                  <xsl:value-of select ="'UBS: 631482/75207794'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_Symbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <Amount>
              <xsl:value-of select="$varDividend"/>
            </Amount>

            <ActivityType>
              <xsl:choose>
                <xsl:when test="$varDividend &lt; 0">
                  <xsl:value-of select="'DividendExpense'"/>
                </xsl:when>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
              </xsl:choose>
            </ActivityType>

            <Description>
              <xsl:choose>
                <xsl:when test="$varDividend &lt; 0">
                  <xsl:value-of select="'DividendExpense'"/>
                </xsl:when>
                <xsl:when test="$varDividend &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>
              </xsl:choose>
            </Description>

            <PayoutDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="normalize-space(COL4)"/>
              </xsl:call-template>
            </PayoutDate>

            <ExDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="DateTime" select="normalize-space(COL3)"/>
              </xsl:call-template>
            </ExDate>

            <CurrencyName>
              <xsl:value-of select="normalize-space(COL11)"/>
            </CurrencyName>

          </PositionMaster>
        </xsl:if>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>