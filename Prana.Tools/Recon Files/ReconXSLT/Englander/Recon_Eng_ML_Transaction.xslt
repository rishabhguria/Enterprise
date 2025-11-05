<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/DocumentElement">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:variable name ="varInstrument">
          <xsl:value-of select="normalize-space(COL7)"/>
        </xsl:variable>

        <xsl:if test ="$varInstrument='0' or $varInstrument='B'">
          <PositionMaster>

            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL6"/>
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

            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL12)"/>
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
                  <xsl:value-of select="COL11"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose >

            <PBSymbol>
              <xsl:value-of select="COL11"/>
            </PBSymbol>

            <xsl:choose>
              <xsl:when test ="$varInstrument='0'">
                <PBAssetName>
                  <xsl:value-of select="'Equity'"/>
                </PBAssetName>
              </xsl:when>
              <xsl:when test ="$varInstrument='B'">
                <PBAssetName>
                  <xsl:value-of select="'EquityOption'"/>
                </PBAssetName>
              </xsl:when>
              <xsl:otherwise>
                <PBAssetName>
                  <xsl:value-of select="''"/>
                </PBAssetName>
              </xsl:otherwise>
            </xsl:choose>


            <xsl:choose>
              <xsl:when test="COL13='B'">
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
              </xsl:when>
              <xsl:when test="COL13='S'">
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
              </xsl:when>
              <xsl:when test="COL13='SS'">
                <Side>
                  <xsl:value-of select="'Sell short'"/>
                </Side>
              </xsl:when>
              <xsl:when test="COL13='CS'">
                <Side>
                  <xsl:value-of select="'Buy to Close'"/>
                </Side>
              </xsl:when>
              <xsl:otherwise>
                <Side>
                  <xsl:value-of select="''"/>
                </Side>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL15))">
                <Quantity>
                  <xsl:value-of select="COL15"/>
                </Quantity>
              </xsl:when>
              <xsl:otherwise>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL14))">
                <AvgPX>
                  <xsl:value-of select="COL14"/>
                </AvgPX>
              </xsl:when>
              <xsl:otherwise>
                <AvgPX>
                  <xsl:value-of select="0"/>
                </AvgPX>
              </xsl:otherwise>
            </xsl:choose>

            <!--GROSS NOTIONAL-->
            <xsl:choose>
              <xsl:when test="COL16 &lt; 0">
                <GrossNotionalValue>
                  <xsl:value-of select="COL16*(-1)"/>
                </GrossNotionalValue>
              </xsl:when>
              <xsl:when test="COL16 &gt; 0">
                <GrossNotionalValue>
                  <xsl:value-of select="COL16"/>
                </GrossNotionalValue>
              </xsl:when>
              <xsl:otherwise>
                <GrossNotionalValue>
                  <xsl:value-of select="0"/>
                </GrossNotionalValue>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test ="COL17 &lt; 0">
                <NetNotionalValue>
                  <xsl:value-of select="COL17*(-1)"/>
                </NetNotionalValue>
              </xsl:when>
              <xsl:when test ="COL17 &gt; 0">
                <NetNotionalValue>
                  <xsl:value-of select="COL17"/>
                </NetNotionalValue>
              </xsl:when>
              <xsl:otherwise>
                <NetNotionalValue>
                  <xsl:value-of select="0"/>
                </NetNotionalValue>
              </xsl:otherwise>
            </xsl:choose>

            <!--COMMISSION-->
            <xsl:choose>
              <xsl:when test ="boolean(number(COL18))">
                <Commission>
                  <xsl:value-of select="COL18"/>
                </Commission>
              </xsl:when>
              <xsl:otherwise>
                <Commission>
                  <xsl:value-of select="0"/>
                </Commission>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:variable name ="varFees">
              <xsl:value-of select="COL21"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test ="boolean(number($varFees))">
                <Fees>
                  <xsl:value-of select="$varFees"/>
                </Fees>
              </xsl:when>
              <xsl:otherwise>
                <Fees>
                  <xsl:value-of select="0"/>
                </Fees>
              </xsl:otherwise>
            </xsl:choose>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
