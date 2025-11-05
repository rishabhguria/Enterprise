<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:if test ="number(COL8)">
          <PositionMaster>

            <!--   Fund -->
            <!--<xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Nirvana']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>-->

            <!--<xsl:choose>
              <xsl:when test="$prana_fund_name=''">
                <AccountName>
                  <xsl:value-of select="''"/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$prana_fund_name'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>-->

			  <AccountName>
				  <xsl:value-of select="COL1"/>
			  </AccountName>

			  <Symbol>
				  <xsl:value-of select="COL3"/>
			  </Symbol>
			  <Side>
				  <xsl:value-of select="COL5"/>
			  </Side>

						<!--<xsl:variable name="PB_COMPANY_NAME" select="normalize-space(substring(COL1,61,30))"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="COL4"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose >-->

            <!--<PBSymbol>
              <xsl:value-of select="COL5"/>
            </PBSymbol>-->


            <!--<Side>

            </Side>-->

            <Quantity>
              <xsl:choose>
                <xsl:when test="COL5='Buy'">
                  <xsl:value-of select="COL7"/>
                </xsl:when>
                <xsl:when test="COL5='Sell'">
                  <xsl:value-of select="COL7*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

			  <AvgPX>
				  <xsl:value-of select="COL8"/>
			  </AvgPX>

			  <!--<Markprice>
              <xsl:choose>
                <xsl:when test="boolean(number($varFormatPrice))">
                  <xsl:value-of select="format-number($varFormatPrice, '#,###.0000000')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Markprice>-->

            <!--<MarketValueBase>
              <xsl:choose>
                <xsl:when test="COL9 &gt; 0">
                  <xsl:value-of select="COL9"/>
                </xsl:when>
                <xsl:when test="COL9 &lt; 0">
                  <xsl:value-of select="COL9*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>-->

            <!--<TotalCost>
              <xsl:choose>
                <xsl:when  test="number(COL8)">
                  <xsl:value-of select="COL8"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </TotalCost>-->

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
