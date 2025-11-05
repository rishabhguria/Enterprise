<?xml version="1.0" encoding="utf-8" ?>
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
    <xsl:if test="COL15='OPTION'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="normalize-space(COL17)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="normalize-space(COL61)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="normalize-space(COL60)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(COL59,3,2)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number((translate(COL62,'+','')),'#.00')"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="COL63"/>
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
      <xsl:for-each select="//PositionMaster">


        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="translate(COL74,'+','')"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and COL15='OPTION'">

          <PositionMaster>
            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="COL18"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='WedbushS']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="UnderlyingSymbol">
              <xsl:value-of select="normalize-space(COL17)"/>
            </xsl:variable>

            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="normalize-space(COL15)='OPTION'">
                  <xsl:value-of select="'EqutiyOption'"/>
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <UnderLyingSymbol>
              <xsl:choose>
                <xsl:when test ="$UnderlyingSymbol!=''">
                  <xsl:value-of select ="$UnderlyingSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </UnderLyingSymbol>

            <xsl:variable name="varStrikePrice">
              <xsl:value-of select="format-number((translate(COL62,'+','')),'#.00')"/>
            </xsl:variable>
            <StrikePrice>
              <xsl:value-of select ="$varStrikePrice"/>
            </StrikePrice>

            <Multiplier>
              <xsl:value-of select ="100"/>
            </Multiplier>

            <xsl:variable name ="varPutCall">
              <xsl:value-of select ="COL63"/>
            </xsl:variable>

            <PutOrCall>
              <xsl:choose>
                <xsl:when test ="$varPutCall='P'">
                  <xsl:value-of select ="'0'"/>
                </xsl:when >
                <xsl:when test ="$varPutCall='C'">
                  <xsl:value-of select ="'1'"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select ="'-1'"/>
                </xsl:otherwise>
              </xsl:choose>
            </PutOrCall>

            <xsl:variable name="varExpiryDay">
              <xsl:value-of select="normalize-space(COL61)"/>
            </xsl:variable>
            <xsl:variable name="varExpiryMonth">
              <xsl:value-of select="normalize-space(COL60)"/>
            </xsl:variable>
            <xsl:variable name="varExpiryYear">
              <xsl:value-of select="normalize-space(COL59)"/>
            </xsl:variable>

            <ExpirationDate>
              <xsl:value-of select ="concat($varExpiryMonth,'/',$varExpiryDay,'/',$varExpiryYear)"/>
            </ExpirationDate>

            <TickerSymbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:when test="$varAsset='EqutiyOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL17)"/>
                  </xsl:call-template>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </TickerSymbol>

            <xsl:variable name="varOsiOption">
              <xsl:value-of select="COL115"/>
            </xsl:variable>

            <OSIOptionSymbol>
              <xsl:value-of select="$varOsiOption"/>
            </OSIOptionSymbol>


            <IDCOOptionSymbol>
              <xsl:value-of select="concat($varOsiOption,'U')"/>
            </IDCOOptionSymbol>

            <AUECID>
              <xsl:value-of select ="12"/>
            </AUECID>
          
              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="concat(normalize-space(COL66),normalize-space(COL67))"/>
              </xsl:variable>
            
            <LongName>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </LongName>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

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

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


