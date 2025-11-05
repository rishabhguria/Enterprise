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

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime"/>

    <xsl:variable name="varDate">
      <xsl:choose>
        <xsl:when test="contains($DateTime,';')">
          <xsl:value-of select="substring-before($DateTime,';')"/>
        </xsl:when>
        
        <xsl:otherwise>
          <xsl:value-of select="$DateTime"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <xsl:value-of select="concat(substring($varDate,5,2),'/',substring($varDate,7,2),'/',substring($varDate,1,4))"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL26)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varPosition) and normalize-space(COL6)='STK'">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="normalize-space(COL12)"/>
            </xsl:variable>

            <xsl:variable name="varISIN">
              <xsl:value-of select="normalize-space(COL13)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
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

            <ISIN>
              <xsl:choose>
                <xsl:when test="$varISIN!='' and $varISIN!='*'">
                  <xsl:value-of select="$varISIN"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISIN>

            <xsl:variable name="PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL34)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='Long'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$varSide='Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL30)"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varPosition &gt; 0">
                  <xsl:value-of select="$varPosition"/>
                </xsl:when>
                <xsl:when test="$varPosition &lt; 0">
                  <xsl:value-of select="$varPosition * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <FXRate>
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL5)"/>
              </xsl:call-template>
            </FXRate>

            <xsl:variable name="varOriginalPurchaseDate">
              <xsl:choose>
                <xsl:when test="COL36!='' and COL36!='*'">
                  <xsl:call-template name="FormatDate">
                    <xsl:with-param name="DateTime" select="normalize-space(COL36)"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <OriginalPurchaseDate>
              <xsl:value-of select="$varOriginalPurchaseDate"/>
            </OriginalPurchaseDate>

            <PositionStartDate>
              <xsl:value-of select="$varOriginalPurchaseDate"/>
            </PositionStartDate>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>