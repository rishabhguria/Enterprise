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
      <xsl:for-each select="//Comparision">
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL28"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($Quantity) and COL12 != 'Cash'">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Morgan Stanley and Co. International plc'"/>
              </xsl:variable>
              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL14)"/>
              </xsl:variable>
				<xsl:variable name="PRANA_SYMBOL_NAME">
					<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
				</xsl:variable>


				<xsl:variable name="Asset">
                <xsl:choose>
                  <xsl:when test="contains(COL12,'Option')">
                    <xsl:value-of select="'Option'"/>
                  </xsl:when>
                  <xsl:when test="contains(COL12,'FX')">
                    <xsl:value-of select="'FX'"/>
                  </xsl:when>
                  <xsl:when test="COL11='CFD' and COL12='Equity'">
                    <xsl:value-of select="'EquitySwap'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Symbol" select="substring-before(COL13, ' ')"/>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$Asset='Option'">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$Asset='FX'">
                    <xsl:value-of select="concat(translate(substring-before(COL14,' '),'/','-'),' ',translate(substring-after(COL14,' '),'-',''))"/>
                  </xsl:when>
                  <xsl:when test="$Asset='Equity'">
                    <xsl:value-of select="normalize-space($Symbol)"/>
                  </xsl:when>
                  <xsl:when test="$Asset='EquitySwap'">
                    <xsl:value-of select="substring-before(COL5,' ')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <IDCOOptionSymbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$Asset='Option'">
                    <xsl:value-of select="concat(COL16,'U')"/>
                  </xsl:when>
                  <xsl:when test="$Asset='FX'">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$Asset='Equity'">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$Asset='EquitySwap'">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </IDCOOptionSymbol>

				<xsl:variable name="PB_FUND_NAME">
					<xsl:value-of select="COL21"/>
				</xsl:variable>
				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

              <xsl:variable name="Side" select="COL24"/>
              <Side>
                <xsl:choose>
                  <xsl:when test="$Asset='Options'">
                    <xsl:choose>
                      <xsl:when test="$Side='L'">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>
                      <xsl:when test="$Side='S'">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$Side='L'">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                      <xsl:when test="$Side='S'">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <xsl:variable name="varQuantity">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL25"/>
                </xsl:call-template>
              </xsl:variable>
              <Quantity>
                <xsl:choose>
                  <xsl:when test="number($varQuantity)">
                    <xsl:value-of select="$varQuantity"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <!--<xsl:variable name="varAvgPX">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL12 div COL6"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$varAvgPX &gt; 0">
                    <xsl:value-of select="$varAvgPX"/>
                  </xsl:when>
                  <xsl:when test="$varAvgPX &lt; 0">
                    <xsl:value-of select="$varAvgPX * (1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AvgPX>-->

              <xsl:variable name="MarkPrice">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL28"/>
                </xsl:call-template>
              </xsl:variable>
              <xsl:variable name="varMarkPrice">
                <xsl:choose>
                  <xsl:when test="COL8='GBP'">
                    <xsl:value-of select="$MarkPrice div 100"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$MarkPrice"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
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
              <MarkPriceBase>
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
              </MarkPriceBase>

              <xsl:variable name="varMarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL32"/>
                </xsl:call-template>
              </xsl:variable>
              <MarketValue>
                <xsl:choose>
                  <xsl:when test="$varMarketValue &gt; 0">
                    <xsl:value-of select="$varMarketValue"/>
                  </xsl:when>
                  <xsl:when test="$varMarketValue &lt; 0">
                    <xsl:value-of select="$varMarketValue "/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValue>

				<xsl:variable name="MarketValueBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL33"/>
					</xsl:call-template>
				</xsl:variable>
				<MarketValueBase>
					<xsl:choose>
						<xsl:when test="$MarketValueBase &gt; 0">
							<xsl:value-of select="$MarketValueBase"/>
						</xsl:when>
						<xsl:when test="$MarketValueBase &lt; 0">
							<xsl:value-of select="$MarketValueBase "/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>

					</xsl:choose>
				</MarketValueBase>
				
              <xsl:variable name="TradeDate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="COL33"/>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="$TradeDate"/>
              </TradeDate>
              <!--<PrimeBroker>
                <xsl:value-of select="COL1"/>
              </PrimeBroker>-->
              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>
              <!--<PBAssetName>
                <xsl:value-of select="COL5"/>
              </PBAssetName>-->
              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>
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
              <!--<CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>-->
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
              <MarkPriceBase>
                <xsl:value-of select="0"/>
              </MarkPriceBase>
              <!--<AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>-->
              <!--<MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>-->
              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>
              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>
              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>