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
  <xsl:template name="FormatDate">
    <xsl:param name="Date"/>
    <xsl:variable name="Day">
      <xsl:choose>
        <xsl:when test="string-length(substring-before(substring-after($Date,'-'),'-'))=1">
          <xsl:value-of select="concat('0',substring-before(substring-after($Date,'-'),'-'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-before(substring-after($Date,'-'),'-')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="Month">
      <xsl:choose>
        <xsl:when test="string-length(substring-before($Date,'-'))=1">
          <xsl:value-of select="concat('0',substring-before($Date,'-'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-before($Date,'-')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="Year">
      <xsl:value-of select="substring-after(substring-after($Date,'-'),'-')"/>
    </xsl:variable>

    <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
  </xsl:template>

  <xsl:template name="FormatDate1">
    <xsl:param name="Date"/>
    <xsl:variable name="Day">
      <xsl:choose>
        <xsl:when test="string-length(substring-before(substring-after($Date,'/'),'/'))=1">
          <xsl:value-of select="concat('0',substring-before(substring-after($Date,'/'),'/'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-before(substring-after($Date,'/'),'/')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="Month">
      <xsl:choose>
        <xsl:when test="string-length(substring-before($Date,'/'))=1">
          <xsl:value-of select="concat('0',substring-before($Date,'/'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-before($Date,'/')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="Year">
      <xsl:value-of select="substring-after(substring-after($Date,'/'),'/')"/>
    </xsl:variable>

    <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL90)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Morgan Stanley'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL67"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>


              <xsl:variable name = "varSymbol" >
                <xsl:value-of select="normalize-space(COL66)"/>
              </xsl:variable>

              <xsl:variable name = "varSedol" >
                <xsl:value-of select="COL70"/>
              </xsl:variable>

              <xsl:variable name = "varCusip" >
                <xsl:value-of select="COL68"/>
              </xsl:variable>

              <xsl:variable name = "varISINSymbol" >
                <xsl:value-of select="COL69"/>
              </xsl:variable>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varSedol!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$varCusip!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$varISINSymbol!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>
              <SEDOL>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varSedol!=''">
                    <xsl:value-of select="$varSedol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>

              <CUSIP>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varCusip!=''">
                    <xsl:value-of select="$varCusip"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CUSIP>

              <ISINSymbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$varISINSymbol!=''">
                    <xsl:value-of select="$varISINSymbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ISINSymbol>

              <xsl:variable name="PB_FUND_NAME" select="'Resonate Core'"/>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <xsl:variable name="Side">
                <xsl:value-of select="''"/>
              </xsl:variable>
              <Side>
                <xsl:choose>
                  <xsl:when test ="$Quantity &gt; 0">
                    <xsl:value-of select ="'Buy'"/>
                  </xsl:when>
                  <xsl:when test ="$Quantity &lt; 0">
                    <xsl:value-of select ="'Sell short'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>
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

              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL110"/>
                </xsl:call-template>
              </xsl:variable>


              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="$MarkPrice &gt; 0">
                    <xsl:value-of select="$MarkPrice"/>

                  </xsl:when>
                  <xsl:when test="$MarkPrice &lt; 0">
                    <xsl:value-of select="$MarkPrice"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </MarkPrice>

                <xsl:variable name="MarketValue">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="COL114"/>
                  </xsl:call-template>
                </xsl:variable>

                <MarketValue>
                  <xsl:choose>
                    <xsl:when test="number($MarketValue)">
                      <xsl:value-of select="$MarketValue"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </MarketValue>

                <xsl:variable name="MarketValueBase">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="COL115"/>
                  </xsl:call-template>
                </xsl:variable>

                <MarketValueBase>
                  <xsl:choose>
                    <xsl:when test="number($MarketValueBase)">
                      <xsl:value-of select="$MarketValueBase"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </MarketValueBase>
                <xsl:variable name="AvgPrice">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="COL102"/>
                  </xsl:call-template>
                </xsl:variable>


                <AvgPX>
                  <xsl:choose>
                    <xsl:when test="$AvgPrice &gt; 0">
                      <xsl:value-of select="$AvgPrice"/>

                    </xsl:when>
                    <xsl:when test="$AvgPrice &lt; 0">
                      <xsl:value-of select="$AvgPrice"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </AvgPX>


                <xsl:variable name="varNetNotionalValue">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="COL104"/>
                  </xsl:call-template>
                </xsl:variable>
                <NetNotionalValue>
                  <xsl:choose>
                    <xsl:when test="$varNetNotionalValue &gt; 0">
                      <xsl:value-of select="$varNetNotionalValue"/>

                    </xsl:when>
                    <xsl:when test="$varNetNotionalValue &lt; 0">
                      <xsl:value-of select="$varNetNotionalValue"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </NetNotionalValue>

                <xsl:variable name="varNetNotionalValueBase">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="COL105"/>
                  </xsl:call-template>
                </xsl:variable>
                <NetNotionalValueBase>
                  <xsl:choose>
                    <xsl:when test="$varNetNotionalValueBase &gt; 0">
                      <xsl:value-of select="$varNetNotionalValueBase"/>

                    </xsl:when>
                    <xsl:when test="$varNetNotionalValueBase &lt; 0">
                      <xsl:value-of select="$varNetNotionalValueBase"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </NetNotionalValueBase>

                <CurrencySymbol>
                  <xsl:value-of select="normalize-space(COL80)"/>
                </CurrencySymbol>

                <xsl:variable name="DateOfTrade">
                  <xsl:choose>
                    <xsl:when test="COL86!=''">
                      <xsl:value-of select="substring-before(normalize-space(COL86),' ')"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(normalize-space(COL87),' ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <OriginalPurchaseDate>
                  <xsl:choose>
                    <xsl:when test="contains($DateOfTrade,'/')">
                      <xsl:call-template name="FormatDate1">
                        <xsl:with-param name="Date" select="$DateOfTrade"/>
                      </xsl:call-template>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:call-template name="FormatDate">
                        <xsl:with-param name="Date" select="$DateOfTrade"/>
                      </xsl:call-template>
                    </xsl:otherwise>
                  </xsl:choose>
                </OriginalPurchaseDate>

                <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>

              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>
              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>

              <MasterFund>
                <xsl:value-of select ="''"/>
              </MasterFund>


              <FundName>
                <xsl:value-of select ="''"/>
              </FundName>

              <Side>
                <xsl:value-of select="''"/>
              </Side>
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>
              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>
              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>
              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>
              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>

              <OriginalPurchaseDate>
                <xsl:value-of select ="''"/>
              </OriginalPurchaseDate>
              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

              <SMRequest>
                <xsl:value-of select="'false'"/>
              </SMRequest>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


