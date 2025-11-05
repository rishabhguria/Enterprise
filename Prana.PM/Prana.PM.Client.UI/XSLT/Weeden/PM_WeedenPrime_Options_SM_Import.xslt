<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

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
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
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
    <xsl:if test="COL39='OPTIONS'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space(COL5),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),1,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),7,2)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),'#.00')"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),1,1)"/>
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
      <xsl:for-each select ="//PositionMaster">


        <xsl:if test="COL39='OPTIONS'">
          <PositionMaster>
            <xsl:variable name="varOption">
              <xsl:call-template name="Option">
                <xsl:with-param name="Symbol" select="COL39"/>
              </xsl:call-template>
            </xsl:variable>
            <TickerSymbol>
              <xsl:value-of select="$varOption"/>
            </TickerSymbol>

            <PutOrCall>
              <xsl:choose>
                <xsl:when test="substring(substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),1,1)='P'">
                  <xsl:value-of select="'0'"/>
                </xsl:when>
                <xsl:when test="substring(substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),1,1)='C'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'-1'"/>
                </xsl:otherwise>
              </xsl:choose>
            </PutOrCall>

            <Multiplier>
              <xsl:value-of select="100"/>
            </Multiplier>

            <xsl:variable name="ExpiryDay">
              <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),3,2)"/>
            </xsl:variable>
            <xsl:variable name="ExpiryMonth">
              <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),1,2)"/>
            </xsl:variable>
            <xsl:variable name="ExpiryYear">
              <xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),7,2)"/>
            </xsl:variable>
            <ExpirationDate>
              <xsl:value-of select="concat($ExpiryMonth,'/',$ExpiryDay,'/','20',$ExpiryYear)"/>
            </ExpirationDate>

            <xsl:variable name="UnderlyingSymbol">
              <xsl:value-of select="substring-before(normalize-space(COL5),' ')"/>
            </xsl:variable>
            <UnderLyingSymbol>
              <xsl:value-of select="$UnderlyingSymbol"/>
            </UnderLyingSymbol>

            <xsl:variable name="StrikePrice">
              <xsl:value-of select="format-number(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),'#.00')"/>
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

            <xsl:variable name="PutORCall">
              <xsl:value-of select="substring(substring-before(COL9,' '),1,1)"/>
            </xsl:variable>

            <xsl:variable name="varStrikePrice1">
              <xsl:value-of select="substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' ')*1000"/>
            </xsl:variable>
            <xsl:variable name="varStrikePrice">
              <xsl:choose>
                <xsl:when test="string-length($varStrikePrice1)=7">
                  <xsl:value-of select="concat('0',$varStrikePrice1)"/>
                </xsl:when>
                <xsl:when test="string-length($varStrikePrice1)=6">
                  <xsl:value-of select="concat('00',$varStrikePrice1)"/>
                </xsl:when>
                <xsl:when test="string-length($varStrikePrice1)=5">
                  <xsl:value-of select="concat('000',$varStrikePrice1)"/>
                </xsl:when>
                <xsl:when test="string-length($varStrikePrice1)=4">
                  <xsl:value-of select="concat('0000',$varStrikePrice1)"/>
                </xsl:when>
                <xsl:when test="string-length($varStrikePrice1)=3">
                  <xsl:value-of select="concat('00000',$varStrikePrice1)"/>
                </xsl:when>
                <xsl:when test="string-length($varStrikePrice1)=2">
                  <xsl:value-of select="concat('000000',$varStrikePrice1)"/>
                </xsl:when>
                <xsl:when test="string-length($varStrikePrice1)=1">
                  <xsl:value-of select="concat('0000000',$varStrikePrice1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varStrikePrice1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="OsiOption">
              <xsl:choose>
                <xsl:when test="string-length($UnderlyingSymbol)=1">
                  <xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',' ',' ',$ExpiryMonth,$ExpiryDay,$ExpiryYear,$PutORCall,$varStrikePrice)"/>
                </xsl:when>
                <xsl:when test="string-length($UnderlyingSymbol)=2">
                  <xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',' ',$ExpiryMonth,$ExpiryDay,$ExpiryYear,$PutORCall,$varStrikePrice)"/>
                </xsl:when>
                <xsl:when test="string-length($UnderlyingSymbol)=3">
                  <xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',$ExpiryMonth,$ExpiryDay,$ExpiryYear,$PutORCall,$varStrikePrice)"/>
                </xsl:when>
                <xsl:when test="string-length($UnderlyingSymbol)=4">
                  <xsl:value-of select="concat($UnderlyingSymbol,' ',' ',$ExpiryMonth,$ExpiryDay,$ExpiryYear,$PutORCall,$varStrikePrice)"/>
                </xsl:when>
                <xsl:when test="string-length($UnderlyingSymbol)=5">
                  <xsl:value-of select="concat($UnderlyingSymbol,' ',$ExpiryMonth,$ExpiryDay,$ExpiryYear,$PutORCall,$varStrikePrice)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <OSIOptionSymbol>
              <xsl:value-of select="$OsiOption"/>
            </OSIOptionSymbol>

            <IDCOOptionSymbol>
              <xsl:value-of select="concat($OsiOption,'U')"/>
            </IDCOOptionSymbol>
            <AUECID>
              <xsl:value-of select="12"/>
            </AUECID>

            <CurrencyID>
              <xsl:value-of select="1"/>
            </CurrencyID>

            <LongName>
              <xsl:value-of select="normalize-space(COL20)"/>
            </LongName>

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
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
