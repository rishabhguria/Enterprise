<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
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
    <xsl:if test="$PutOrCall='Call'">
      <xsl:choose>
        <xsl:when test="$Month='Jan'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='Feb'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='Mar'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='Apr'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='May'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='Jun'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='Jul'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='Aug'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='Sep'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='Oct'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='Nov'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='Dec'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='Put'">
      <xsl:choose>
        <xsl:when test="$Month='Jan'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='Feb'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='Mar'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='Apr'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='May'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='Jun'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='Jul'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='Aug'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='Sep'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='Oct'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='Nov'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='Dec'">
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
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space(COL1),' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(substring-after(substring-after(COL1,' '),' '),1,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(substring-after(COL1,' '),1,3)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(substring-after(substring-after(substring-after(COL1,' '),' '),' '),3,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(COL1,' '),' '),' '),' '),' ')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring-after(substring-after(substring-after(substring-after(substring-before(COL1,'.'),' '),' '),' '),' '),'#.00')"/>

    </xsl:variable>
    <xsl:variable name="MonthCodVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$ExpiryMonth"/>
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
    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>

  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varMarkPrice">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL13"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varMarkPrice)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$varMarkPrice &gt; 0">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:when test="$varMarkPrice &lt; 0">
                  <xsl:value-of select="$varMarkPrice * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="varDate">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <Date>
              <xsl:value-of select="$varDate"/>
            </Date>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>