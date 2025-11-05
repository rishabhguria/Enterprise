<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
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
  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07 ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08 ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="string-length($Symbol) &gt; 8">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space($Symbol),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space($Symbol),' '),' '),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),'/'),'/'),' ')"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),' '),2),'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

    </xsl:if>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varOption">
              <xsl:call-template name="Option">
                <xsl:with-param name="Symbol" select="COL5"/>
              </xsl:call-template>
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varOption!=''">
                  <xsl:value-of select="$varOption"/>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>

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

            <Quantity>
              <xsl:choose>
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>

            <AvgPX>
              <xsl:choose>
                <xsl:when test="$AvgPrice &gt; 0">
                  <xsl:value-of select="$AvgPrice"/>
                </xsl:when>
                <xsl:when test="$AvgPrice &lt; 0">
                  <xsl:value-of select="$AvgPrice * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>

            <xsl:variable name="varNetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValue &gt; 0">
                  <xsl:value-of select="$varNetNotionalValue"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValue &lt; 0">
                  <xsl:value-of select="$varNetNotionalValue * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name ="Side" select="COL8"/>
            <Side>
              <xsl:choose>
                <xsl:when test="$Side='BY'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$Side='SL'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
                <xsl:when test="$Side='SS'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:when test="$Side='BS'">
                  <xsl:value-of select="'Buy to Close'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>
            
            <CurrencySymbol>
              <xsl:value-of select="COL3"/>
            </CurrencySymbol>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <TradeDate>
              <xsl:value-of select="COL6"/>
            </TradeDate>

            <SettlementDate>
              <xsl:value-of select="COL7"/>
            </SettlementDate>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL14)"/>
            </xsl:variable>

            <CounterParty>
                  <xsl:value-of select="$PB_BROKER_NAME"/>
            </CounterParty>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


