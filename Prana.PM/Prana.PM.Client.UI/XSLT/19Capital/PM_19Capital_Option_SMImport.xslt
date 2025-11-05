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

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($Symbol,' ')"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),1,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="translate($var, '0123456789.', '')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),3,2)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(number(substring(translate($var,translate($var, '0123456789.', ''), ''),5,8)),'#.00')"/>
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

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$ExpiryMonth,$StrikePrice,'D',$Day)"/>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varQty">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="(contains(COL21,'PUT') or contains(COL21,'CALL')) and number($varQty)">

          <PositionMaster>

            <xsl:variable name="varTicker">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>
            <TickerSymbol>
              <xsl:call-template name="Option">
                <xsl:with-param name="Symbol" select="$varTicker"/>
              </xsl:call-template>
            </TickerSymbol>

            <LongName>
              <xsl:value-of select="COL21"/>
            </LongName>

            <xsl:variable name="varPutCall">
              <xsl:choose>
                <xsl:when test="contains(COL21,'PUT')">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:when test="contains(COL21,'CALL')">
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

            <xsl:variable name="var">
              <xsl:value-of select="substring-after($varTicker,' ')"/>
            </xsl:variable>
            <xsl:variable name="StrikePrice">
              <xsl:value-of select="format-number(number(substring(translate($var,translate($var, '0123456789.', ''), ''),5,8)),'#.00')"/>
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

            <ExpirationDate>
              <xsl:value-of select="normalize-space(substring-after(COL21,'EXP'))"/>
            </ExpirationDate>

            <Multiplier>
              <xsl:value-of select="100"/>
            </Multiplier>

            <AUECID>
              <xsl:value-of select="'12'"/>
            </AUECID>

            <UnderLyingID>
              <xsl:value-of select="1"/>
            </UnderLyingID>

            <ExchangeID>
              <xsl:value-of select="5"/>
            </ExchangeID>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>
            <CurrencyID>
              <xsl:choose>
                <xsl:when test="$varCurrency='USD'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test="$varCurrency='CAD'">
                  <xsl:value-of select="7"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyID>
			
			<AssetID>
				<xsl:value-of select="2"/>
			</AssetID>
			
			<!-- <ExpirationOrSettlementDate> -->
				<!-- <xsl:value-of select="normalize-space(substring-after(COL21,'EXP'))"/> -->
			<!-- </ExpirationOrSettlementDate> -->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>