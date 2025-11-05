<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  
  <xsl:template match="/DocumentElement">
    
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        
        <xsl:if test ="number(COL6)">
          <PositionMaster>

            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
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
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
            </FundName>


            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL18)"/>
            
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSEC']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="$PB_COMPANY_NAME"/>
            </Description>

            <xsl:variable name ="varCallPut">
              <xsl:choose>
                <xsl:when test="COL19 = '*' and COL5 != ''">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="varUnderlying">
              <xsl:choose>
                <xsl:when test="COL5 != '' and COL5 != '*' and $varCallPut = 1">
                  <xsl:value-of select="substring-before(COL5,'1')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL5)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varUnderlyingLength">
              <xsl:value-of select="string-length($varUnderlying)"/>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:if test ="$varCallPut = 1">
                <xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
              </xsl:if>
            </xsl:variable>


            <xsl:variable name="varExpirationYear">
              <xsl:if test="$varCallPut = 1">
                <xsl:value-of select="substring(COL5,($varUnderlyingLength + 1),2)"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="varStrikePriceString">
              <xsl:value-of select="substring-after(substring-after(COL5,$varUnderlying),$varMonthCode)"/>
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
              <xsl:value-of select="COL5"/>
            </PBSymbol>

            <PBAssetName>
              <xsl:value-of select="''"/>
            </PBAssetName>


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

            <Quantity>
              <xsl:choose>
                <xsl:when test="number(COL4)">
                  <xsl:value-of select="COL4"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            </Quantity>

            <AvgPX>
              <xsl:choose>
              <xsl:when test="number(COL6)">
                  <xsl:value-of select="COL6"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            </AvgPX>


            <!--GROSS NOTIONAL-->
            <GrossNotionalValue>
              <xsl:choose>
              <xsl:when test="COL9 &lt; 0">
                  <xsl:value-of select="COL9*(-1)"/>
              </xsl:when>
              <xsl:when test="COL9 &gt; 0">
                <xsl:value-of select="COL9"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </GrossNotionalValue>

            <NetNotionalValue>
              <xsl:choose>
              <xsl:when test ="COL17 &lt; 0">
                  <xsl:value-of select="COL17*(-1)"/>
              </xsl:when>
              <xsl:when test ="COL17 &gt; 0">
                  <xsl:value-of select="COL17"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </NetNotionalValue>

            <Commission>
            <xsl:choose>
              <xsl:when test ="number(COL15)">
                  <xsl:value-of select="COL15"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            </Commission>


            <xsl:variable name ="varFees">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <Fees>
              <xsl:choose>
              <xsl:when test ="number($varFees)">
                  <xsl:value-of select="$varFees"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            </Fees>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
