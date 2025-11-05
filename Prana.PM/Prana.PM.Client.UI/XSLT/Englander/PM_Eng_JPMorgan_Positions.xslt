<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster[substring(COL1,40,1) = 'O' or substring(COL1,40,1) = 'C']">

        <PositionMaster>
          <!--   Fund -->
          <xsl:variable name = "PB_FUND_NAME" >
            <xsl:if test ="substring(COL1,4,8)!= '-OF-F'">
              <xsl:value-of select="substring(COL1,4,8)"/>
            </xsl:if>
          </xsl:variable>

          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='JPMorgan']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

          <CUSIP>
            <xsl:value-of select="substring(COL1,14,12)"/>
          </CUSIP>


          <xsl:variable name="PB_COMPANY_NAME" select="substring(COL1,61,30)"/>
          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
          </xsl:variable>

          <Description>
            <xsl:value-of select ="$PB_COMPANY_NAME"/>
          </Description>

          <xsl:variable name ="varSymbol">
            <xsl:value-of select ="normalize-space(substring(COL1,26,8))"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="$PRANA_SYMBOL_NAME != ''">
              <Symbol>
                <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
              </Symbol>
            </xsl:when>
            <xsl:when test ="substring(COL1,40,1) = 'O'">
              <xsl:variable name = "varLength" >
                <xsl:value-of select="string-length($varSymbol)"/>
              </xsl:variable>
              <Symbol>
                <xsl:value-of select="concat(substring($varSymbol,1,($varLength - 2)),' ',substring($varSymbol,($varLength - 1),$varLength))"/>
              </Symbol>
            </xsl:when>
            <xsl:otherwise>
              <Symbol>
                <xsl:value-of select="$varSymbol"/>
              </Symbol>
            </xsl:otherwise>
          </xsl:choose >


          <PBSymbol>
            <xsl:value-of select="substring(COL1,26,8)"/>
          </PBSymbol>

          <xsl:variable name ="varNetPosition">
            <xsl:value-of select ="substring(COL1,91,18)"/>
          </xsl:variable>

          <xsl:variable name ="varQtyInt">
            <xsl:value-of select ="substring($varNetPosition,1,13)"/>
          </xsl:variable>

          <xsl:variable name ="varQtyFrac">
            <xsl:value-of select ="substring($varNetPosition,14,5)"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="boolean(number($varNetPosition))">
              <NetPosition>
                <xsl:value-of select="concat($varQtyInt,'.',$varQtyFrac)"/>
              </NetPosition>
            </xsl:when>
            <xsl:otherwise>
              <NetPosition>
                <xsl:value-of select="0"/>
              </NetPosition>
            </xsl:otherwise>
          </xsl:choose>

          <PositionStartDate>
            <xsl:value-of select="''"/>
          </PositionStartDate>

          <!--Side-->
          <xsl:variable name ="varSide">
            <xsl:value-of select="substring(COL1,109,1)"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="$varSide='L'">
              <SideTagValue>
                <xsl:value-of select="'1'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:when test="$varSide='S'">
              <SideTagValue>
                <xsl:value-of select="'2'"/>
              </SideTagValue>
            </xsl:when>
            <xsl:otherwise>
              <SideTagValue>
                <xsl:value-of select="''"/>
              </SideTagValue>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:variable name ="varPrice">
            <xsl:value-of select ="substring(COL1,235,16)"/>
          </xsl:variable>

          <xsl:variable name ="varPriceInt">
            <xsl:value-of select ="substring($varPrice,1,8)"/>
          </xsl:variable>

          <xsl:variable name ="varPriceFrac">
            <xsl:value-of select ="substring($varPrice,9,8)"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="boolean(number($varPrice))">
              <CostBasis>
                <xsl:value-of select="concat($varPriceInt,'.',$varPriceFrac)"/>
              </CostBasis>
            </xsl:when>
            <xsl:otherwise>
              <CostBasis>
                <xsl:value-of select="0"/>
              </CostBasis>
            </xsl:otherwise>
          </xsl:choose>

          <PBAssetType>
            <xsl:value-of select="''"/>
          </PBAssetType>

        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>