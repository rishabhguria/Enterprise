<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/DocumentElement">
    <DocumentElement>
      <xsl:for-each select="Comparision">

        <xsl:variable name ="varPrice">
          <xsl:value-of select ="substring(COL1,122,16)"/>
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

        <xsl:if test="$varFormatPrice > 0 ">
          <PositionMaster>
            
            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:if test ="substring(COL1,4,5)!= '-OF-F'">
                <xsl:value-of select="substring(COL1,4,5)"/>
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

            <xsl:variable name="PB_COMPANY_NAME" select="substring(COL1,34,30)"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='JPMorgan']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
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
                  <xsl:value-of select="substring(COL1,23,8)"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose >

            <PBSymbol>
              <xsl:value-of select="substring(COL1,23,8)"/>
            </PBSymbol>

            <PBAssetName>
              <xsl:value-of select="''"/>
            </PBAssetName>

            <!--Side-->
            <xsl:variable name ="varSide">
              <xsl:value-of select="substring(COL1,83,1)"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$varSide='B'">
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


            <xsl:variable name ="varNetPosition">
              <xsl:value-of select ="substring(COL1,104,19)"/>
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

            <!--<xsl:choose>
							<xsl:when test ="COL14 &lt; 0">
								<NetNotionalValue>
									<xsl:value-of select="(COL12 * COL14*(-1)) - (COL15 + COL16 + COL17) "/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:when test ="COL15 &gt; 0">
								<NetNotionalValue>
									<xsl:value-of select="(COL12 * COL14) + (COL15 + COL16 + COL17)"/>
								</NetNotionalValue>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValue>
									<xsl:value-of select="0"/>
								</NetNotionalValue>
							</xsl:otherwise>
						</xsl:choose>-->



            <!--COMMISSION-->
            <!--<xsl:choose>
							<xsl:when test ="boolean(number(COL15))">
								<Commission>
									<xsl:value-of select="COL15"/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>-->

            <!--<Fees>
							<xsl:value-of select="COL16 + COL17"/>
						</Fees>-->

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
