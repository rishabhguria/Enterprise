<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <PositionMaster>

          <!--   Fund -->
          <FundName>
            <xsl:value-of select="''"/>
          </FundName>

          <xsl:variable name = "PB_COMPANY" >
            <xsl:value-of select="translate(COL1,'&quot;','')"/>
          </xsl:variable>
          <xsl:variable name="PRANA_SYMBOL">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Eng']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test ="$PRANA_SYMBOL != ''">
              <Symbol>
                <xsl:value-of select="$PRANA_SYMBOL"/>
              </Symbol>
            </xsl:when>
            <xsl:otherwise>
              <Symbol>
                <xsl:value-of select="COL3"/>
              </Symbol>
            </xsl:otherwise>
          </xsl:choose >

          <PBSymbol>
            <xsl:value-of select="COL3"/>
          </PBSymbol>

          <xsl:variable name ="varAssetType">
            <xsl:value-of select ="substring-after(COL3,' ')"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test ="COL4 &lt; 0 and $varAssetType=''">
              <NetPosition>
                <xsl:value-of select="COL4*(-1)"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'5'"/>
              </SideTagValue>
            </xsl:when >
            <xsl:when test ="COL4 &gt; 0 and $varAssetType=''">
              <NetPosition>
                <xsl:value-of select="COL4"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'1'"/>
              </SideTagValue>
            </xsl:when >
            <xsl:when test ="COL4 &lt; 0 and $varAssetType !=''">
              <NetPosition>
                <xsl:value-of select="COL4*(-1)"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'C'"/>
              </SideTagValue>
            </xsl:when >
            <xsl:when test ="COL4 &gt; 0 and $varAssetType !=''">
              <NetPosition>
                <xsl:value-of select="COL4"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'A'"/>
              </SideTagValue>
            </xsl:when >
            <xsl:otherwise>
              <NetPosition>
                <xsl:value-of select="0"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="''"/>
              </SideTagValue>
            </xsl:otherwise>
          </xsl:choose >

          <xsl:choose>
            <xsl:when test ="boolean(number(COL7))">
              <CostBasis>
                <xsl:value-of select="COL7"/>
              </CostBasis>
            </xsl:when>
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose>

          <CounterPartyID>
            <xsl:value-of select="'13'"/>
          </CounterPartyID>

          <PositionStartDate>
            <xsl:value-of select="''"/>
          </PositionStartDate>

        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
