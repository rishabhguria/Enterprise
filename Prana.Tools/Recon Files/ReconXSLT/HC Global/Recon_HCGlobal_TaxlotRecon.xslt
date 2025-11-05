<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

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

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL6)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Quantity) and COL3!='Cash and Equivalents'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Geneva'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!='' or $varSymbol!='*'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
                <xsl:when test="number($Quantity)">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test="$varSide='Short Positions'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:when test="$varSide='Long Positions'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
              </xsl:choose>
            </Side>
            
            
            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
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
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
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

            <xsl:variable name="varNetNotionalBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL11)"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalBase"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalBase * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>

            <xsl:variable name="varUnitCost">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL9)"/>
              </xsl:call-template>
            </xsl:variable>
            <UnitCost>
              <xsl:choose>
                <xsl:when test="number($varUnitCost)">
                  <xsl:value-of select="$varUnitCost"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </UnitCost>

            <xsl:variable name="varODate">
              <xsl:choose>
                <xsl:when test="contains(COL8,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(substring-after(normalize-space(COL8),'/'),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(substring-after(COL8,'/'),'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(substring-after(COL8,'/'),'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varOMonth">
              <xsl:choose>
                <xsl:when test="contains(COL8,'/')">
                  <xsl:choose>
                    <xsl:when test="string-length(substring-before(normalize-space(COL8),'/'))=1">
                      <xsl:value-of select="concat(0,substring-before(COL8,'/'))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-before(COL8,'/')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varOyear">
              <xsl:choose>
                <xsl:when test="contains(COL8,'/')">
                  <xsl:value-of select="substring-after(substring-after(COL8,'/'),'/')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <OriginalPurchaseDate>
              <xsl:choose>
                <xsl:when test="contains(COL8,'/')">
                  <xsl:value-of select="concat($varOMonth,'/',$varODate,'/',$varOyear)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </OriginalPurchaseDate>


            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>