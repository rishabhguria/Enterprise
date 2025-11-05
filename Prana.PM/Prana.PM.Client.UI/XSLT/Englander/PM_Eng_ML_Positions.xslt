<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

        <xsl:variable name ="varInstrument">
          <xsl:value-of select="normalize-space(COL5)"/>
        </xsl:variable>

        <xsl:if test ="$varInstrument='0' or $varInstrument='B'">
          <PositionMaster>
            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <FundName>
                  <xsl:value-of select="''"/>
                </FundName>
              </xsl:when>
              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </FundName>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL10)"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="$PB_COMPANY_NAME"/>
            </Description>

            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="COL9"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose >


            <PBSymbol>
              <xsl:value-of select="COL9"/>
            </PBSymbol>

            <xsl:choose>
              <xsl:when test ="$varInstrument='0'">
                <PBAssetType>
                  <xsl:value-of select="'Equity'"/>
                </PBAssetType>
              </xsl:when>
              <xsl:when test ="$varInstrument='B'">
                <PBAssetType>
                  <xsl:value-of select="'EquityOption'"/>
                </PBAssetType>
              </xsl:when>
              <xsl:otherwise>
                <PBAssetType>
                  <xsl:value-of select="''"/>
                </PBAssetType>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="COL14 &gt; 0">
                <NetPosition>
                  <xsl:value-of select="COL14"/>
                </NetPosition>
              </xsl:when>
              <xsl:when test="COL14 &lt; 0">
                <NetPosition>
                  <xsl:value-of select="COL14*(-1)"/>
                </NetPosition>
              </xsl:when>
              <xsl:otherwise>
                <NetPosition>
                  <xsl:value-of select="0"/>
                </NetPosition>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="COL14 &gt; 0 and $varInstrument='0'">
                <SideTagValue>
                  <xsl:value-of select="'1'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL14 &lt; 0 and $varInstrument='0'">
                <SideTagValue>
                  <xsl:value-of select="'5'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL14 &gt; 0 and $varInstrument='B'">
                <SideTagValue>
                  <xsl:value-of select="'A'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL14 &lt; 0 and $varInstrument='B'">
                <SideTagValue>
                  <xsl:value-of select="'C'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:otherwise>
                <SideTagValue>
                  <xsl:value-of select="''"/>
                </SideTagValue>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:choose>
              <xsl:when test ="COL3 = 'Trade Date' or COL3='*'">
                <PositionStartDate>
                  <xsl:value-of select="''"/>
                </PositionStartDate>
              </xsl:when>
              <xsl:otherwise>
                <PositionStartDate>
                  <xsl:value-of select="COL3"/>
                </PositionStartDate>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL13))">
                <CostBasis>
                  <xsl:value-of select="COL13"/>
                </CostBasis>
              </xsl:when>
              <xsl:otherwise>
                <CostBasis>
                  <xsl:value-of select="0"/>
                </CostBasis>
              </xsl:otherwise>
            </xsl:choose>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>