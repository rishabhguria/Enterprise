<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:if test="COL2!='Account' and COL5 != ''">
          <PositionMaster>


            <!--Need To Add Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:choose>
                <xsl:when test ="$PB_FUND_NAME='Sensato AP Master'">
                  <xsl:value-of select ="'002-276293-Cash'"/>
                </xsl:when>
                <xsl:when test ="$PB_FUND_NAME= 'Sensato S1 AP'">
                  <xsl:value-of select ="'S1_002-200079_CASH'"/>
                </xsl:when>
                <xsl:otherwise>
                  <!--<xsl:value-of select="''"/>-->
                  <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME != '' and $PRANA_FUND_NAME != '*'">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL5)"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <!--<xsl:value-of select="''"/>-->
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
                <PBSymbol>
                  <xsl:value-of select="''"/>
                </PBSymbol>
                <SEDOL>
                  <xsl:value-of select="normalize-space(COL5)"/>
                </SEDOL>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <PBSymbol>
                  <xsl:value-of select="''"/>
                </PBSymbol>
                <SEDOL>
                  <xsl:value-of select="normalize-space(COL5)"/>
                </SEDOL>
              </xsl:otherwise>
            </xsl:choose>

            <Side>
              <xsl:choose>
                <xsl:when test="COL3='SHORT'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Buy'"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <Quantity>
              <xsl:choose>
                <xsl:when test="number(COL14)">
                  <xsl:value-of select="COL14"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="boolean(number(COL12)) and  (number(COL12)&lt; 0 )">
                  <xsl:value-of select="COL12 * (-1)"/>
                </xsl:when>
                <xsl:when test ="boolean(number(COL12)) and (number(COL12) &gt; 0 )">
                  <xsl:value-of select="COL12"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>


            <SMRequest>
              <xsl:value-of select ="'TRUE'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
