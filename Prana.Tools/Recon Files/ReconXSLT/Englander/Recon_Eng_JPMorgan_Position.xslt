<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision[substring(COL1,40,1) = 'O' or substring(COL1,40,1) = 'C']">

        <xsl:variable name ="varPrice">
          <xsl:value-of select ="substring(COL1,235,16)"/>
        </xsl:variable>

        <xsl:variable name ="varPriceInt">
          <xsl:value-of select ="substring($varPrice,1,8)"/>
        </xsl:variable>

        <xsl:variable name ="varPriceFrac">
          <xsl:value-of select ="substring($varPrice,9,8)"/>
        </xsl:variable>

        <xsl:variable name ="varFormatPrice">
          <xsl:value-of select="concat($varPriceInt,'.',$varPriceFrac)"/>
        </xsl:variable>

        <xsl:variable name ="varNetPosition">
          <xsl:value-of select ="substring(COL1,91,18)"/>
        </xsl:variable>

        <xsl:variable name ="varQtyInt">
          <xsl:value-of select ="substring($varNetPosition,1,13)"/>
        </xsl:variable>

        <xsl:variable name ="varQtyFrac">
          <xsl:value-of select ="substring($varNetPosition,14,5)"/>
        </xsl:variable>

        <xsl:variable name ="varFormatQty">
          <xsl:value-of select="concat($varQtyInt,'.',$varQtyFrac)"/>
        </xsl:variable>

        <xsl:if test="$varFormatPrice > 0 and $varFormatQty > 0">
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

            <PBAssetName>
              <xsl:value-of select="''"/>
            </PBAssetName>

            <!--Side-->
            <xsl:variable name ="varSide">
              <xsl:value-of select="substring(COL1,109,1)"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$varSide='L'">
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
              </xsl:when>
              <xsl:when test="$varSide='S'">
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
              </xsl:when>
              <xsl:otherwise>
                <Side>
                  <xsl:value-of select="''"/>
                </Side>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number($varFormatQty))">
                <Quantity>
                  <xsl:value-of select='format-number($varFormatQty, "###,###,###.#####")'/>
                </Quantity>
              </xsl:when>
              <xsl:otherwise>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number($varFormatPrice))">
                <AvgPX>
                  <xsl:value-of select='format-number($varFormatPrice, "###,###,###.########")'/>
                </AvgPX>
              </xsl:when>
              <xsl:otherwise>
                <AvgPX>
                  <xsl:value-of select="0"/>
                </AvgPX>
              </xsl:otherwise>
            </xsl:choose>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
