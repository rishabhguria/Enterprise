<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:if test ="number(COL17)">

          <xsl:variable name="varPBName">
            <xsl:value-of select="'SG'"/>
          </xsl:variable>

          <PositionMaster>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL14"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>


            <FundName>
              <xsl:value-of select="'038Q51NH0 - SGSW'"/>
            </FundName>


            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL13"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="varQuantity">
              <xsl:value-of select="COL17"/>
            </xsl:variable>
            <Quantity>
              <xsl:value-of select="$varQuantity"/>
            </Quantity>

            <xsl:variable name="varSettlCurrency">
              <xsl:value-of select="normalize-space(COL16)"/>
            </xsl:variable>
            <SettlCurrency>
              <xsl:value-of select="$varSettlCurrency"/>
            </SettlCurrency>

            
            <xsl:variable name="varBaseCurrency">
              <xsl:value-of select="normalize-space(COL21)"/>
            </xsl:variable>
            <BaseCurrency>
              <xsl:value-of select="$varBaseCurrency"/>
            </BaseCurrency>


            <xsl:variable name="varAvgPx">
              <xsl:value-of select="COL18"/>
            </xsl:variable>
            <AvgPx>
              <xsl:value-of select="$varAvgPx"/>>
            </AvgPx>

            <xsl:variable name="varNetNotionalValue">
              <xsl:value-of select="COL20"/>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValue &gt; 0">
                  <xsl:value-of select="$varNetNotionalValue"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValue &lt; 0">
                  <xsl:value-of select="$varNetNotionalValue*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="varNetNotionalValueBase">
              <xsl:value-of select="COL25"/>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>


            <xsl:variable name="varFXRate">
              <xsl:value-of select="COL23"/>
            </xsl:variable>
            <FXRate>
              <xsl:value-of select="$varFXRate"/>
            </FXRate>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>