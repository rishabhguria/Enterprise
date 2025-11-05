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


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL11)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Scotia'"/>
              </xsl:variable>
              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL23)"/>
              </xsl:variable>

              <xsl:variable name="varSymbol" select="normalize-space(COL8)"/>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="string-length(COL8) &gt; 20">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <IDCOOptionSymbol>
                <xsl:choose>
                  <xsl:when test="string-length(COL8) &gt; 20">
                    <xsl:value-of select="concat(COL8, 'U')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </IDCOOptionSymbol>

              <SEDOL>
                <xsl:choose>

                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$varSymbol!=''">
                    <xsl:choose>

                      <xsl:when test="string-length(COL8) &gt; 16">
                        <xsl:value-of select="''"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="$varSymbol"/>
                      </xsl:otherwise>

                    </xsl:choose>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SEDOL>

              <xsl:variable name="PB_FUND_NAME" select="COL6"/>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

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
                  <xsl:with-param name="Number" select="normalize-space(COL12)"/>
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

              <xsl:variable name="NetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL18)"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValue &gt; 0">
                    <xsl:value-of select="$NetNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValue &lt; 0">
                    <xsl:value-of select="$NetNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>

              <TradeDate>
                <xsl:value-of select="COL19"/>
              </TradeDate>

              <xsl:variable name="Side" select="normalize-space(COL10)"/>

              <Side>
                <xsl:choose>
                  <xsl:when test="string-length(COL8) &gt; 16">
                    <xsl:choose>
                      <xsl:when test="$Side='BL'">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>
                      <xsl:when test="$Side='SS'">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>

                      <xsl:when test="$Side='CS'">
                        <xsl:value-of select="'Buy To Close'"/>
                      </xsl:when>
                      <xsl:when test="$Side='STC'">
                        <xsl:value-of select="'Sell to Close'"/>
                      </xsl:when>
                      <xsl:when test="$Side='BTO'">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>
                      <xsl:when test="$Side='SL'">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>
                      <xsl:when test="$Side='STO'">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$Side='BL'">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                      <xsl:when test="$Side='SS'">
                        <xsl:value-of select="'Sell short'"/>
                      </xsl:when>

                      <xsl:when test="$Side='CS'">
                        <xsl:value-of select="'Buy To Close'"/>
                      </xsl:when>
                      <xsl:when test="$Side='STC'">
                        <xsl:value-of select="'Sell to Close'"/>
                      </xsl:when>
                      <xsl:when test="$Side='BTO'">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>
                      <xsl:when test="$Side='SL'">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>
                      <xsl:when test="$Side='STO'">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>

                </xsl:choose>
              </Side>

              <xsl:variable name="Commission">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL16)"/>
                </xsl:call-template>
              </xsl:variable>

              <Commission>
                <xsl:choose>

                  <xsl:when test="$Commission &gt; 0">
                    <xsl:value-of select="$Commission"/>
                  </xsl:when>

                  <xsl:when test="$Commission &lt; 0">
                    <xsl:value-of select="$Commission * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Commission>

              <xsl:variable name="GrossNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL14)"/>
                </xsl:call-template>
              </xsl:variable>

              <GrossNotionalValue>
                <xsl:choose>

                  <xsl:when test="$GrossNotionalValue &gt; 0">
                    <xsl:value-of select="$GrossNotionalValue"/>
                  </xsl:when>

                  <xsl:when test="$GrossNotionalValue &lt; 0">
                    <xsl:value-of select="$GrossNotionalValue * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </GrossNotionalValue>

              <CurrencySymbol>
                <xsl:value-of select="COL9"/>
              </CurrencySymbol>

              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>
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
              <IDCOOptionSymbol>
                <xsl:value-of select="''"/>
              </IDCOOptionSymbol>
              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>
              <FundName>
                <xsl:value-of select="''"/>
              </FundName>
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>
              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>
              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>
              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>
              <Side>
                <xsl:value-of select="''"/>
              </Side>
              <Commission>
                <xsl:value-of select="''"/>
              </Commission>
              <GrossNotionalValue>
                <xsl:value-of select="''"/>
              </GrossNotionalValue>
              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>
              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>
              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
              <SMRequest>
                <xsl:value-of select="''"/>
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