<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="DoubleQuote">"</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),$DoubleQuote,''),'+',''),'$',''))"/>
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

  <xsl:template name="ReplaceSpecialCharater">
    <xsl:param name="String"/>
    <xsl:variable name="varStrings">., '^[■◆]\s*'</xsl:variable>
  </xsl:template>

  <xsl:template match="/">


    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varQuantityValue">
          <xsl:choose>
            <xsl:when test="contains(COL2,'Manage')">
              <xsl:value-of select="substring-before(COL2,'Manage')"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="varQuantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="$varQuantityValue"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQuantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL1)"/>
            </xsl:variable>

            <xsl:variable name="varPB_Symbol_Name">
              <xsl:choose>
                <xsl:when test="contains($PB_SYMBOL_NAME,'Quarterly')">
                  <xsl:value-of select="normalize-space(substring-before($PB_SYMBOL_NAME,' 2 Quarterly '))"/>
                </xsl:when>
                <xsl:when test ="contains($PB_SYMBOL_NAME,'Inception')">
                  <xsl:value-of select ="normalize-space(substring-before($PB_SYMBOL_NAME,' Inception'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$varPB_Symbol_Name]/@PranaSymbol"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varPB_Symbol_Name"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

            <SideTagValue>
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
            </SideTagValue>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
            
            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
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
            </CostBasis>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>

            <StampDuty>
              <xsl:choose>
                <xsl:when test="$MarketValue &gt; 0">
                  <xsl:value-of select="$MarketValue"/>
                </xsl:when>
                <xsl:when test="$MarketValue &lt; 0">
                  <xsl:value-of select="$MarketValue * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>

            <PBSymbol>
              <xsl:value-of select ="normalize-space($varPB_Symbol_Name)"/>
            </PBSymbol>


          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>