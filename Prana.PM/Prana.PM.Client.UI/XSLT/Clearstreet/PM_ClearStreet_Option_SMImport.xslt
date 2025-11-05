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
  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
    <xsl:param name="varPutCall"/>
    <xsl:if test="$varPutCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07' ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$varPutCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
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
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($Symbol,' ')"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(normalize-space(translate($var,'0123456789','')),1,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')) div 1000,7,8),'#.00')"/>
    </xsl:variable>
    <xsl:variable name="MonthCode">
      <xsl:call-template name="MonthCodevar">
        <xsl:with-param name="Month" select="$ExpiryMonth"/>
        <xsl:with-param name="varPutCall" select="$PutORCall"/>
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

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varAsset">
          <xsl:value-of select="normalize-space(COL15)"/>
        </xsl:variable>

        <xsl:if test="string-length(normalize-space(COL9)) &gt; 15">

          <PositionMaster>

            <xsl:variable name="varTicker">
              <xsl:value-of select="normalize-space(COL9)"/>
            </xsl:variable>
            <TickerSymbol>
              <xsl:call-template name="Option">
                <xsl:with-param name="Symbol" select="$varTicker"/>
              </xsl:call-template>
            </TickerSymbol>

            <LongName>
              <xsl:value-of select="COL10"/>
            </LongName>

            <AssetID>
              <xsl:value-of select="2"/>
            </AssetID>

            <xsl:variable name="var">
              <xsl:value-of select="substring-after($varTicker,' ')"/>
            </xsl:variable>

            <xsl:variable name="varPutCall">
              <xsl:choose>
                <xsl:when test="substring(normalize-space(translate($var,'0123456789','')),1,1)='P'">
                  <xsl:value-of select="0"/>
                </xsl:when>
                
                <xsl:when test="substring(normalize-space(translate($var,'0123456789','')),1,1)='C'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="-1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <PutOrCall>
              <xsl:value-of select="$varPutCall"/>
            </PutOrCall>

            <UnderLyingSymbol>
              <xsl:value-of select="substring-before($varTicker,' ')"/>
            </UnderLyingSymbol>

            <xsl:variable name="StrikePrice">
              <xsl:value-of select="format-number(substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')) div 1000,7,8),'#.00')"/>
            </xsl:variable>

            <StrikePrice>
              <xsl:choose>
                <xsl:when test="number($StrikePrice)">
                  <xsl:value-of select="$StrikePrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StrikePrice>

            <xsl:variable name="ExpiryDay">
              <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),5,2)"/>
            </xsl:variable>
            <xsl:variable name="ExpiryMonth">
              <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),3,2)"/>
            </xsl:variable>
            <xsl:variable name="ExpiryYear">
              <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789', ''), '')),1,2)"/>
            </xsl:variable>
            
            <ExpirationDate>
              <xsl:value-of select="concat($ExpiryMonth,'/',$ExpiryDay,'/',concat('20',$ExpiryYear))"/>
            </ExpirationDate>

            <Multiplier>
              <xsl:value-of select="100"/>
            </Multiplier>

            <UDASector>
              <xsl:value-of select="'Undefined'"/>
            </UDASector>

            <UDASubSector>
              <xsl:value-of select="'Undefined'"/>
            </UDASubSector>

            <UDASecurityType>
              <xsl:value-of select="'Undefined'"/>
            </UDASecurityType>

            <UDAAssetClass>
              <xsl:value-of select="'Undefined'"/>
            </UDAAssetClass>

            <UDACountry>
              <xsl:value-of select="'Undefined'"/>
            </UDACountry>

            <UnderLyingID>
              <xsl:value-of select="1"/>
            </UnderLyingID>

            <ExchangeID>
              <xsl:value-of select="5"/>
            </ExchangeID>

            <CurrencyID>
              <xsl:value-of select="'1'"/>
            </CurrencyID>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>