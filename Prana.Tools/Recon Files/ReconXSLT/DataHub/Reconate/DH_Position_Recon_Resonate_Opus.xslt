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
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
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
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
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
    <xsl:param name="varSymbol"/>
    <xsl:variable name="varUnderlying">
      <xsl:value-of select="substring-before($varSymbol,' ')"/>
    </xsl:variable>

    <xsl:variable name="varExYear">
      <xsl:value-of select="substring(substring-after($varSymbol,' '),1,2)"/>
    </xsl:variable>

    <xsl:variable name="varStrike">
      <xsl:value-of select="format-number(substring(substring-after($varSymbol,' '),8) div 1000, '#.00')"/>
    </xsl:variable>

    <xsl:variable name="varExDay">
      <xsl:value-of select="substring(substring-after($varSymbol,' '),5,2)"/>
    </xsl:variable>

    <xsl:variable name="varPutOrCall">
      <xsl:value-of select="substring(substring-after($varSymbol,' '),7,1)"/>
    </xsl:variable>

    <xsl:variable name="varMonthCode">
      <xsl:value-of select="substring(substring-after($varSymbol,' '),3,2)"/>
    </xsl:variable>

    <xsl:variable name="varExMonth">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$varMonthCode"/>
        <xsl:with-param name="PutOrCall" select="$varPutOrCall"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="varExpiryDay">
      <xsl:choose>
        <xsl:when test="substring($varExDay,1,1)= '0'">
          <xsl:value-of select="substring($varExDay,2,1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varExDay"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="normalize-space(concat('O:', $varUnderlying, ' ', $varExYear,$varExMonth,$varStrike,'D',$varExpiryDay))"/>

  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL9"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:choose>
          <xsl:when test="number($Quantity) and contains(COL1,'CASH')!='true' ">

            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Morgan Stanley and Co. International plc'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL6)"/>
              </xsl:variable>

				<xsl:variable name="PRANA_SYMBOL_NAME">
					<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
				</xsl:variable>


				<xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="string-length(COL3) &gt; 16">
                    <xsl:value-of select="'Option'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Symbol" >
                <xsl:choose>
                  <xsl:when test="contains(COL3,' ')">
                    <xsl:value-of select="substring-before(COL3,' ')"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="COL3"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varOption">
                <xsl:call-template name="Option">
                  <xsl:with-param name="varSymbol" select="COL3"/>
                </xsl:call-template>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$Asset='Option'">
                    <xsl:value-of select="$varOption"/>
                  </xsl:when>
                  <xsl:when test="$Asset='Equity'">
                    <xsl:value-of select="$Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <xsl:variable name="PB_FUND_NAME" select="COL8"/>
				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

              <xsl:variable name="Side" select="COL2"/>

              <Side>
                <xsl:choose>
                  <xsl:when test="$Asset='Option'">
                    <xsl:choose>
                      <xsl:when test="$Side='Long'">
                        <xsl:value-of select="'Buy to open'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Short'">
                        <xsl:value-of select="'Sell to open'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$Side='Long'">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Short'">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>



              <Quantity>
                <xsl:choose>
                  <xsl:when test="number($Quantity)">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </Quantity>

              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL7"/>
                </xsl:call-template>
              </xsl:variable>


              <MarkPrice>
                <xsl:choose>
                  <xsl:when test="$MarkPrice &gt; 0">
                    <xsl:value-of select="$MarkPrice"/>

                  </xsl:when>
                  <xsl:when test="$MarkPrice &lt; 0">
                    <xsl:value-of select="$MarkPrice * (-1)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>

                </xsl:choose>
              </MarkPrice>

              <xsl:variable name="MarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL12"/>
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
                  <xsl:with-param name="Number" select="COL13"/>
                </xsl:call-template>
              </xsl:variable>

              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test="number(COL13)">
                    <xsl:value-of select="COL13"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>

              <SEDOL>
                <xsl:value-of select="normalize-space(COL4)"/>
              </SEDOL>

              <ISINSymbol>
                <xsl:value-of select="normalize-space(COL5)"/>
              </ISINSymbol>

              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>

                <PBSymbol>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </PBSymbol>
                
            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>


              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>

              <FundName>
                <xsl:value-of select="''"/>
              </FundName>

              <Side>
                <xsl:value-of select="''"/>
              </Side>
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <MarkPrice>
                <xsl:value-of select="0"/>
              </MarkPrice>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>
              
              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>
              
              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>
              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>

              <ISINSymbol>
                <xsl:value-of select="''"/>
              </ISINSymbol>
              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>

            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>


      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>