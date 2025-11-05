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
            <xsl:with-param name="Number" select="normalize-space(COL20)"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'JP Morgan'"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="varSEDOL">
                <xsl:value-of select="normalize-space(COL32)"/>
              </xsl:variable>

              <xsl:variable name="varCUSIP">
                <xsl:value-of select="normalize-space(COL31)"/>
              </xsl:variable>

              <xsl:variable name="varISIN">
                <xsl:value-of select="normalize-space(COL33)"/>
              </xsl:variable>

              <xsl:variable name="varSymbol">
                <xsl:value-of select="normalize-space(COL34)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$varSEDOL!='' and $varSEDOL!='*'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                    <xsl:value-of select="''"/>
                  </xsl:when>

                  <xsl:when test="$varISIN!='' and $varISIN!='*'">
                    <xsl:value-of select="''"/>
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
                  <xsl:when test="$varSEDOL!='' and $varSEDOL!='*'">
                    <xsl:value-of select="$varSEDOL"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SEDOL>

              <CUSIP>
                <xsl:choose>
                  <xsl:when test="$varCUSIP!='' and $varCUSIP!='*'">
                    <xsl:value-of select="$varCUSIP"/>
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

              <xsl:variable name="PB_FUND_NAME" select="''"/>

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

              <xsl:variable name="varAvgPX">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL21)"/>
                </xsl:call-template>
              </xsl:variable>
              <AvgPX>
                <xsl:choose>
                  <xsl:when test="$varAvgPX &gt; 0">
                    <xsl:value-of select="$varAvgPX"/>
                  </xsl:when>
                  <xsl:when test="$varAvgPX &lt; 0">
                    <xsl:value-of select="$varAvgPX * -1"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </AvgPX>

              <xsl:variable name="varNetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL29)"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$varAvgPX &gt; 0">
                    <xsl:value-of select="$varAvgPX"/>
                  </xsl:when>
                  <xsl:when test="$varAvgPX &lt; 0">
                    <xsl:value-of select="$varAvgPX * -1"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>

				<xsl:variable name="NetNotionalValueBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL30"/>
					</xsl:call-template>
				</xsl:variable>
				<NetNotionalValueBase>
					<xsl:choose>
						<xsl:when test="$NetNotionalValueBase &gt; 0">
							<xsl:value-of select="$NetNotionalValueBase"/>
						</xsl:when>
						<xsl:when test="$NetNotionalValueBase &lt; 0">
							<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</NetNotionalValueBase>

              <xsl:variable name="varCOL22">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL22)"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varCOL23">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL23)"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varCOL24">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL24)"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varCOL25">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL25)"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varCOL26">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL26)"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varCOL27">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL27)"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varCOL28">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="normalize-space(COL28)"/>
                </xsl:call-template>
              </xsl:variable>

              <xsl:variable name="varTotalCommissionandFees">
                <xsl:value-of select="$varCOL22 + $varCOL23 + $varCOL24 + $varCOL25 + $varCOL26 + $varCOL27 + $varCOL28"/>
              </xsl:variable>
              <TotalCommissionandFees>
                <xsl:choose>
                  <xsl:when test="$varTotalCommissionandFees &gt; 0">
                    <xsl:value-of select="$varTotalCommissionandFees"/>
                  </xsl:when>
                  <xsl:when test="$varTotalCommissionandFees &lt; 0">
                    <xsl:value-of select="$varTotalCommissionandFees * -1"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TotalCommissionandFees>

              <xsl:variable name="varSide" select="normalize-space(COL16)"/>
              <Side>
                <xsl:value-of select="$varSide"/>
              </Side>

              <CurrencySymbol>
                <xsl:value-of select="normalize-space(COL18)"/>
              </CurrencySymbol>

              <TradeDate>
                <xsl:value-of select="COL12"/>
              </TradeDate>

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
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>
              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>
              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>
				<NetNotionalValueBase>
					<xsl:value-of select="0"/>
				</NetNotionalValueBase>
				
              <TotalCommissionandFees>
                <xsl:value-of select="0"/>
              </TotalCommissionandFees>
              <Side>
                <xsl:value-of select="''"/>
              </Side>
              <CurrencySymbol>
                <xsl:value-of select="''"/>
              </CurrencySymbol>
              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>
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