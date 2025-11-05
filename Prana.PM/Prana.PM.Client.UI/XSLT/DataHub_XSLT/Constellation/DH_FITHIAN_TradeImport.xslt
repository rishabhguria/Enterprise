<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL9)"/>
          </xsl:call-template>
        </xsl:variable>
        
        <xsl:if test="number($NetPosition)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>


            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            
            <xsl:variable name="varSedol" select="normalize-space(COL6)"/>
            <xsl:variable name="varCusip" select="normalize-space(COL5)"/>
            <xsl:variable name="varISIN" select="normalize-space(COL4)"/>
            <xsl:variable name="varSymbol" select="normalize-space(COL3)"/>
            
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

                <xsl:when test="$varISIN!=''">
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

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                
                <xsl:when test="$varSedol!=''">
                  <xsl:value-of select="$varSedol"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <CUSIP>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varCusip!=''">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>

            <ISIN>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$varISIN!=''">
                  <xsl:value-of select="$varISIN"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISIN>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

            <xsl:variable name="PB_COUNTER_PARTY" select="normalize-space(COL18)"/>
            
            <xsl:variable name="PRANA_COUNTER_PARTY">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_COUNTER_PARTY]/@PranaBroker"/>
            </xsl:variable>

            <ExecutingBroker>
              <xsl:choose>
                <xsl:when test ="$PRANA_COUNTER_PARTY != ''">
                  <xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_COUNTER_PARTY"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExecutingBroker>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <PositionStartDate>
              <xsl:value-of select="concat(substring(COL7,5,2),'/',substring(COL7,7,2),'/',substring(COL7,1,4))"/>
            </PositionStartDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide" select="normalize-space(COL2)"/>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='Buy to close'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="$varSide='Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL11)"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>
                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL13)"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test="$varFee &gt; 0">
                  <xsl:value-of select="$varFee"/>
                </xsl:when>
                <xsl:when test="$varFee &lt; 0">
                  <xsl:value-of select="$varFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

            <xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL16)"/>
              </xsl:call-template>
            </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test="$varFXRate &gt; 0">
                  <xsl:value-of select="$varFXRate"/>
                </xsl:when>
                <xsl:when test="$varFXRate &lt; 0">
                  <xsl:value-of select="$varFXRate * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_CURRENCY_ID">
              <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
            </xsl:variable>

            <CurrencyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_CURRENCY_ID)">
                  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyID>


            <PBSymbol>
              <xsl:value-of select="normalize-space($PB_SYMBOL_NAME)"/>
            </PBSymbol>
          
        </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>