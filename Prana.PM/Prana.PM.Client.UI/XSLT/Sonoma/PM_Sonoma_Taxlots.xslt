<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

        <xsl:variable name ="varAsset">
          <xsl:value-of select ="COL2"/>
        </xsl:variable>
        <xsl:if test ="$varAsset='Equity' or $varAsset='Equity Option' or $varAsset = 'Fund' or $varAsset ='Bond Convertible' or $varAsset = 'Bond Corporate' or $varAsset = 'warrants'">

          <PositionMaster>
            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL26"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Sonoma']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME != ''">
                <FundName>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="$PB_FUND_NAME='HSBC Custody' and COL27='Sonoma LP'">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_H'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="$PB_FUND_NAME='HSBC Custody' and COL27='Sonoma Ltd'">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_H'"/>
                </FundName>
              </xsl:when>

              <xsl:when test ="$PB_FUND_NAME='Interactive Brokers' and COL27='Sonoma LP'">
                <FundName>
                  <xsl:value-of select ="'SONOMALP_IB'"/>
                </FundName>
              </xsl:when>
              <xsl:when test ="$PB_FUND_NAME='Interactive Brokers' and COL27='Sonoma Ltd'">
                <FundName>
                  <xsl:value-of select ="'SONOMALTD_IB'"/>
                </FundName>
              </xsl:when>
              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select="''"/>
                </FundName>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:variable name="PB_COMPANY_NAME" select="COL5"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Sonoma']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
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
              <xsl:when test ="$varAsset='Equity' or $varAsset='Fund'">
                <Symbol>
                  <xsl:value-of select ="substring-before(COL22,' ')"/>
                </Symbol>
              </xsl:when>
              <xsl:when test ="$varAsset='Equity Option'">
                <Symbol>
                  <xsl:value-of select ="translate(COL22,'+',' ')"/>
                </Symbol>
              </xsl:when>
              <xsl:when test ="$varAsset='Bond Convertible' or $varAsset='Bond Corporate' or $varAsset='warrants'">
                <Symbol>
                  <xsl:value-of select ="COL5"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="COL22"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose >

            <PBSymbol>
              <xsl:value-of select="COL22"/>
            </PBSymbol>

            <PBAssetType>
              <xsl:value-of select="COL2"/>
            </PBAssetType>

            <xsl:choose>
              <xsl:when test="COL3 &gt; 0 and ($varAsset='Equity' or $varAsset='Bond Convertible' or $varAsset='Bond Corporate' or $varAsset='warrants' or $varAsset='Fund')">
                <NetPosition>
                  <xsl:value-of select="COL3"/>
                </NetPosition>
                <SideTagValue>
                  <xsl:value-of select="'1'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL3 &gt; 0 and $varAsset='Equity Option'">
                <NetPosition>
                  <xsl:value-of select="COL3"/>
                </NetPosition>
                <SideTagValue>
                  <xsl:value-of select="'A'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL3 &lt; 0 and ($varAsset='Equity' or $varAsset='Bond Convertible' or $varAsset='Bond Corporate' or $varAsset='warrants' or $varAsset='Fund')">
                <NetPosition>
                  <xsl:value-of select="COL3*(-1)"/>
                </NetPosition>
                <SideTagValue>
                  <xsl:value-of select="'5'"/>
                </SideTagValue>
              </xsl:when>
              <xsl:when test="COL3 &lt; 0 and $varAsset='Equity Option'">
                <NetPosition>
                  <xsl:value-of select="COL3*(-1)"/>
                </NetPosition>
                <SideTagValue>
                  <xsl:value-of select="'C'"/>
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
              <xsl:when test ="COL10 = 'Lot Date' or COL10 = '*'">
                <PositionStartDate>
                  <xsl:value-of select="''"/>
                </PositionStartDate>
              </xsl:when>
              <xsl:otherwise>
                <PositionStartDate>
                  <xsl:value-of select="COL10"/>
                </PositionStartDate>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL11))">
                <CostBasis>
                  <xsl:value-of select="COL11"/>
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