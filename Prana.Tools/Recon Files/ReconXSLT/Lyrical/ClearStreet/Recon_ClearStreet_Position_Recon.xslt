<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''),'$',''))"/>
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


  <xsl:template name="FormatDate">
    <xsl:param name="varFullDate" />
    <xsl:variable name="varYear">
      <xsl:value-of select="substring($varFullDate, string-length($varFullDate) - 3 , 4)"/>
    </xsl:variable>
    <xsl:variable name="varWithoutYear">
      <xsl:value-of select="substring($varFullDate, 1, string-length($varFullDate) - 4)"/>
    </xsl:variable>
    <xsl:variable name="varDay">
      <xsl:value-of select="substring-after($varWithoutYear,'/')"/>
    </xsl:variable>
    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="$varWithoutYear &lt; 999">
          <xsl:value-of select="concat('0',substring($varWithoutYear, 1, 1))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($varWithoutYear, 1, 2)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="concat($varMonth,$varDay,$varYear)"/>
  </xsl:template>
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">     

        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL16"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'JP Morgan'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="Symbol" >
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test ="$Symbol !=''">
                  <xsl:value-of select ="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select ="COL4"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

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

            <Side>
              <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            

            <xsl:variable name="varMarketValueLocal">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="$varMarketValueLocal &gt; 0">
                  <xsl:value-of select="$varMarketValueLocal"/>
                </xsl:when>
                <xsl:when test="$varMarketValueLocal &lt; 0">
                  <xsl:value-of select="$varMarketValueLocal"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>



            <xsl:variable name="varMarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
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


            <ISINSymbol>
              <xsl:value-of select="COL9"/>
            </ISINSymbol>

            <CUSIP>
              <xsl:value-of select="COL10"/>
            </CUSIP>

            <SEDOL>
              <xsl:value-of select="COL11"/>
            </SEDOL>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL1,7,2)"/>
            </xsl:variable>
            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL1,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL1,1,4)"/>
            </xsl:variable>
            <TradeDate>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </TradeDate>

       

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


