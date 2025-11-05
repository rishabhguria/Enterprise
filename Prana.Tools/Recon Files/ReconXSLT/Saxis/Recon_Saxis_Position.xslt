<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/DocumentElement">
    <DocumentElement>

      <xsl:for-each select="//Comparision">
        <xsl:if test ="number(COL4)">

          <PositionMaster>

            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GSEC']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <FundName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>


            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL6)"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="$PB_COMPANY_NAME"/>
            </Description>

            <xsl:variable name ="varCallPut">
              <xsl:choose>
                <xsl:when test="COL2 = '*' and COL3 != ''">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="varUnderlying">
              <xsl:choose>
                <xsl:when test="COL3 != '' and COL3 != '*' and $varCallPut = 1">
                  <xsl:value-of select="substring-before(COL3,'1')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL3)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varUnderlyingLength">
              <xsl:value-of select="string-length($varUnderlying)"/>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:if test ="$varCallPut = 1">
                <xsl:value-of select="substring(COL3,($varUnderlyingLength + 5),1)"/>
              </xsl:if>
            </xsl:variable>


            <xsl:variable name="varExpirationYear">
              <xsl:if test="$varCallPut = 1">
                <xsl:value-of select="substring(COL3,($varUnderlyingLength + 1),2)"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="varStrikePriceString">
              <xsl:value-of select="substring-after(substring-after(COL3,$varUnderlying),$varMonthCode)"/>
            </xsl:variable>


            <xsl:variable name ="varStrikePrice">
              <xsl:if test="$varCallPut = 1 and number($varStrikePriceString)">
                <xsl:value-of select="format-number($varStrikePriceString,'#.00')"/>
              </xsl:if>
            </xsl:variable>



            <Symbol>
              <xsl:choose>
                <xsl:when test="$varCallPut = 1">
                  <xsl:value-of select="concat('O:',$varUnderlying,' ',$varExpirationYear,$varMonthCode,$varStrikePrice)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varUnderlying"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </PBSymbol>

            <PBAssetName>
              <xsl:choose>
                <xsl:when test ="$varCallPut = 1">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </PBAssetName>

            <Quantity>
              <xsl:choose>
                <xsl:when test="number(COL4) &gt; 0">
                  <xsl:value-of select="COL4"/>
                </xsl:when>
                <xsl:when test="number(COL4) &lt; 0">
                  <xsl:value-of select="COL4*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>


            <Side>
              <xsl:choose>
                <xsl:when test="number(COL4) &gt; 0">
                  <xsl:choose>
                    <xsl:when test="$varCallPut = 1">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="'Buy'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:when test="number(COL4) &lt; 0">
                  <xsl:choose>
                    <xsl:when test="$varCallPut = 1">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="'Sell'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <AvgPX>
              <xsl:choose>
                <xsl:when test="number(COL5)">
                  <xsl:value-of select="COL5"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>


          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
