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

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>

    <xsl:variable name="var">
      <xsl:value-of select="translate($Symbol,'0123456789','')"/>
    </xsl:variable>

    <xsl:variable name="var2">
      <xsl:value-of select="translate($Symbol,$vUppercaseChars_CONST,'')"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring($var,1,string-length($var)-1)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring($var2,1,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring($var,string-length($var))"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring($var2,3,2)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring($var2,5) div 100,'##.00')"/>
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
      <xsl:for-each select="//Comparision">
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL34)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="normalize-space(COL7)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@SourceSymbol=$PB_SYMBOL_NAME]/@TargetSymbol"/>
              </xsl:variable>

              <xsl:variable name="varSymbol">
				<xsl:choose>
				  <xsl:when test="contains(normalize-space(COL6),' US')">
					  <xsl:value-of select="substring-before(normalize-space(COL6),' US')"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="normalize-space(COL6)"/>
				  </xsl:otherwise>
			    </xsl:choose>
			  </xsl:variable>
              <xsl:variable name="varSedol" select="normalize-space(COL107)"/>
              <xsl:variable name="varCusip" select="normalize-space(COL108)"/>
              <xsl:variable name="varISIN" select="normalize-space(COL109)"/>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="contains(COL10,'Equity Put Option') or contains(COL10,'Equity Call Option')">
                    <xsl:call-template name="Option">
                      <xsl:with-param name="Symbol" select="substring-before(normalize-space(COL110),'.U')"/>
                    </xsl:call-template>
                  </xsl:when>

                  <xsl:when test="$varSymbol!='' and $varSymbol!='*'">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <SEDOL>
                <xsl:choose>
                  <xsl:when test="$varSedol!='' and $varSedol!='*'">
                    <xsl:value-of select="$varSedol"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>

              <CUSIP>
                <xsl:choose>
                  <xsl:when test="$varCusip!='' and $varCusip!='*'">
                    <xsl:value-of select="$varCusip"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CUSIP>

              <ISINSymbol>
                <xsl:choose>
                  <xsl:when test="$varISIN!='' and $varISIN!='*'">
                    <xsl:value-of select="$varISIN"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ISINSymbol>

              <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL66)"/>
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

              <xsl:variable name="varNetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL39)"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$varNetNotionalValue &gt; 0">
                    <xsl:value-of select="$varNetNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$varNetNotionalValue &lt; 0">
                    <xsl:value-of select="$varNetNotionalValue "/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>

              <xsl:variable name="varNetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL40)"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$varNetNotionalValueBase &gt; 0">
                    <xsl:value-of select="$varNetNotionalValueBase"/>
                  </xsl:when>

                  <xsl:when test="$varNetNotionalValueBase &lt; 0">
                    <xsl:value-of select="$varNetNotionalValueBase "/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>

              <xsl:variable name="varAvgPX">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL55)"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$varAvgPX &gt; 0">
                    <xsl:value-of select="$varAvgPX"/>
                  </xsl:when>

                  <xsl:when test="$varAvgPX &lt; 0">
                    <xsl:value-of select="$varAvgPX "/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AvgPX>

              <xsl:variable name="varMarketValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL41)"/>
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

              <xsl:variable name="varMarketValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL42)"/>
                </xsl:call-template>
              </xsl:variable>
              <MarketValueBase>
                <xsl:choose>
                  <xsl:when test="$varMarketValueBase &gt; 0">
                    <xsl:value-of select="$varMarketValueBase"/>
                  </xsl:when>
                  <xsl:when test="$varMarketValueBase &lt; 0">
                    <xsl:value-of select="$varMarketValueBase "/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </MarketValueBase>

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

              <CurrencySymbol>
                <xsl:value-of select="normalize-space(COL25)"/>
              </CurrencySymbol>

              <Side>
                <xsl:choose>
                  <xsl:when test="contains(COL10,'Equity Put Option') or contains(COL10,'Equity Call Option')">
                    <xsl:choose>
                      <xsl:when test="normalize-space(COL19)= 0">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>
                      <xsl:when test="normalize-space(COL19)= -1">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="normalize-space(COL19)= 0">
                        <xsl:value-of select="'Sell short'"/>
                      </xsl:when>
                      <xsl:when test="normalize-space(COL19)= -1">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <xsl:variable name="varOrgDate">
                <xsl:choose>
                  <xsl:when test="contains(COL17,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL17),'/'),'/'))=1">
                        <xsl:value-of select="concat(0,substring-before(substring-after(COL17,'/'),'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after(COL17,'/'),'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="contains(COL17,'-')">
                        <xsl:choose>
                          <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL17),'-'),'-'))=1">
                            <xsl:value-of select="concat(0,substring-before(substring-after(COL17,'-'),'-'))"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="substring-before(substring-after(COL17,'-'),'-')"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>



              <xsl:variable name="varOrgMonth">
                <xsl:choose>
                  <xsl:when test="contains(COL17,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(substring-before(normalize-space(COL17),'/'))=1">
                        <xsl:value-of select="concat(0,substring-before(COL17,'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(COL17,'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="contains(COL17,'-')">
                        <xsl:choose>
                          <xsl:when test="string-length(substring-before(normalize-space(COL17),'-'))=1">
                            <xsl:value-of select="concat(0,substring-before(COL17,'-'))"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="substring-before(COL17,'-')"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varOrgYear">
                <xsl:choose>
                  <xsl:when test="contains(COL17,'/')">
                    <xsl:value-of select="substring-after(substring-after(COL17,'/'),'/')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="contains(COL17,'-')">
                        <xsl:value-of select="substring-after(substring-after(COL17,'-'),'-')"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <OriginalPurchaseDate>
                <xsl:value-of select="concat($varOrgMonth,'/',$varOrgDate,'/',$varOrgYear)"/>
              </OriginalPurchaseDate>

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

              <SEDOL>
                <xsl:value-of select="''"/>
              </SEDOL>

              <CUSIP>
                <xsl:value-of select="''"/>
              </CUSIP>

              <ISINSymbol>
                <xsl:value-of select="''"/>
              </ISINSymbol>

              <FundName>
                <xsl:value-of select="''"/>
              </FundName>

              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>

              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>

              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

              <MarketValue>
                <xsl:value-of select="0"/>
              </MarketValue>

              <MarketValueBase>
                <xsl:value-of select="0"/>
              </MarketValueBase>

              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>

              <OriginalPurchaseDate>
                <xsl:value-of select="''"/>
              </OriginalPurchaseDate>

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