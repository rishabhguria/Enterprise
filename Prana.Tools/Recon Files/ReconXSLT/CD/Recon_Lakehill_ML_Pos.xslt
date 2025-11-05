<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template name="left-trim">
    <xsl:param name="s" />
    <xsl:choose>
      <xsl:when test="substring($s, 1, 1) = ''">
        <xsl:value-of select="$s"/>
      </xsl:when>
      <xsl:when test="normalize-space(substring($s, 1, 1)) = ''">
        <xsl:call-template name="left-trim">
          <xsl:with-param name="s" select="substring($s, 2)" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$s" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="right-trim">
    <xsl:param name="s" />
    <xsl:choose>
      <xsl:when test="substring($s, 1, 1) = ''">
        <xsl:value-of select="$s"/>
      </xsl:when>
      <xsl:when test="normalize-space(substring($s, string-length($s))) = ''">
        <xsl:call-template name="right-trim">
          <xsl:with-param name="s" select="substring($s, 1, string-length($s) - 1)" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$s" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="trim">
    <xsl:param name="s" />
    <xsl:call-template name="right-trim">
      <xsl:with-param name="s">
        <xsl:call-template name="left-trim">
          <xsl:with-param name="s" select="$s" />
        </xsl:call-template>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  
  <xsl:template match="/DocumentElement">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:variable name ="varInstrument">
          <xsl:value-of select="normalize-space(COL5)"/>
        </xsl:variable>

        <xsl:if test ="$varInstrument='0' or $varInstrument='B' or $varInstrument='BL' or $varInstrument='J'">

          <PositionMaster>

            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select="''"/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL10)"/>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="$PB_COMPANY_NAME"/>
            </Description>

            <xsl:variable name = "PB_SYMBOL_TRIM" >
              <xsl:call-template name="trim">
                <xsl:with-param name="s" select="translate(COL24,'&quot;','')" />
              </xsl:call-template>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test ="$varInstrument = 'B' or $varInstrument = 'J' or $varInstrument = 'BL'">
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="concat($PB_SYMBOL_TRIM,'U')"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:when test ="$varInstrument = '0'">
                <Symbol>
                  <xsl:value-of select="$PB_SYMBOL_TRIM"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="$PB_SYMBOL_TRIM"/>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:otherwise>
            </xsl:choose>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_TRIM"/>
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
              <xsl:when test="COL14 &gt; 0">
                <Quantity>
                  <xsl:value-of select="COL14"/>
                </Quantity>
                <Side>
                  <xsl:value-of select="'Buy'"/>
                </Side>
              </xsl:when>
              <xsl:when test="COL14 &lt; 0">
                <Quantity>
                  <xsl:value-of select="COL14"/>
                </Quantity>
                <Side>
                  <xsl:value-of select="'Sell'"/>
                </Side>
              </xsl:when>
              <xsl:otherwise>
                <Side>
                  <xsl:value-of select="''"/>
                </Side>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:choose>
              <xsl:when test="boolean(number(COL13))">
                <AvgPX>
                  <xsl:value-of select="COL13"/>
                </AvgPX>
              </xsl:when>
              <xsl:otherwise>
                <AvgPX>
                  <xsl:value-of select="0"/>
                </AvgPX>
              </xsl:otherwise>
            </xsl:choose>

            <SMRequest>
              <xsl:value-of select ="'TRUE'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
