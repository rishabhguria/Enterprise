<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

        <PositionMaster>
          <!--   Fund -->
          <xsl:variable name = "PB_FUND_NAME" >
            <xsl:value-of select="COL3"/>
          </xsl:variable>

          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

          <xsl:variable name="PB_COMPANY_NAME" select="COL14"/>
          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Jefferies']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
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
                <xsl:value-of select="COL33"/>
              </Symbol>
            </xsl:otherwise>
          </xsl:choose >

          <PBSymbol>
            <xsl:value-of select="COL33"/>
          </PBSymbol>

          <PBAssetType>
            <xsl:value-of select="''"/>
          </PBAssetType>

          <xsl:choose>
            <xsl:when test="COL12 &gt; 0">
              <NetPosition>
                <xsl:value-of select="COL12"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'1'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:when test="COL12 &lt; 0">
              <NetPosition>
                <xsl:value-of select="COL12*(-1)"/>
              </NetPosition>
              <SideTagValue>
                <xsl:value-of select="'5'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:otherwise>             
              <NetPosition>
                <xsl:value-of select="0"/>
              </NetPosition>
               <SideTagValue>
                <xsl:value-of select="''"/>
              </SideTagValue>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when test ="COL1 = 'Report_Date' or COL1 = '*'">
              <PositionStartDate>
                <xsl:value-of select="''"/>
              </PositionStartDate>
            </xsl:when>
            <xsl:otherwise>
              <PositionStartDate>
                <xsl:value-of select="COL1"/>
              </PositionStartDate>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when test="boolean(number(COL39))">
              <CostBasis>
                <xsl:value-of select="COL39"/>
              </CostBasis>
            </xsl:when>
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose>         
         
        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>