<?xml version="1.0" encoding="UTF-8"?>
<!-- Object -Trade Recon for GS, Date -01-11-2012(dd/MM/yyyy) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:if test="number(COL9)">
          <PositionMaster>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <!--Need to Add here PB Name From FundMapping.xml-->
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
              </xsl:otherwise>
            </xsl:choose>
            </AccountName>


            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(COL10)"/>


            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </PBSymbol>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@CompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <CompanyName>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </CompanyName>

            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
                <SEDOL>
                  <xsl:value-of select="''"/>
                </SEDOL>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <SEDOL>
                  <xsl:value-of select="COL14"/>
                </SEDOL>
              </xsl:otherwise>
            </xsl:choose>

            <Side>
              <xsl:choose>
              <xsl:when test ="COL7 ='BUY' ">
                  <xsl:value-of select="'Buy'"/>
              </xsl:when>
              <xsl:when test ="COL7 ='BUY TO OPEN' ">
                  <xsl:value-of select="'Buy to Open'"/>
              </xsl:when>
              <xsl:when test ="COL7 ='BCOV' or COL7 ='BUY TO CLOSE'">
                  <xsl:value-of select="'Buy to Close'"/>
              </xsl:when>
              <xsl:when test ="COL7 ='SSEL' ">
                  <xsl:value-of select="'Sell short'"/>
              </xsl:when>
              <xsl:when test ="COL7 ='SELL' ">
                  <xsl:value-of select="'Sell'"/>
              </xsl:when>
              <xsl:when test ="COL7 ='SELL TO OPEN' ">
                  <xsl:value-of select="'Sell to Open'"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="COL7"/>
              </xsl:otherwise>
            </xsl:choose>
            </Side>



            <!--BEGIN FOR NET POSITION ie QUANTITY -->
            <Quantity>
              <xsl:choose>
              <xsl:when  test="number(COL9) and COL9 &lt; 0">
                  <xsl:value-of select="COL9 * (-1)"/>
              </xsl:when>
              <xsl:when  test="number(COL9) and COL9 &gt; 0">
                  <xsl:value-of select="COL9"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="'0'"/>
              </xsl:otherwise>
            </xsl:choose>
            </Quantity>


            <!-- Value in Local Currency-->
            <AvgPX>
              <xsl:choose>
              <xsl:when test="number(COL15) and  number(COL15) &lt; 0">
                  <xsl:value-of select= "COL15 *(-1)"/>
              </xsl:when>
              <xsl:when test="number(COL15) and  number(COL15) &gt; 0">
                  <xsl:value-of select= "COL15"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select= "0"/>
              </xsl:otherwise>
            </xsl:choose>
            </AvgPX>


            <!-- Commission Value in Local Currency-->
            <!--<Commission>
              <xsl:choose>
              <xsl:when test="number(COL35) and COL35 &lt; 0 ">
                  <xsl:value-of select="COL35 *(-1)"/>
              </xsl:when>
              <xsl:when test="number(COL35) and COL35 &gt; 0 ">
                  <xsl:value-of select="COL35"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            </Commission>-->


            <!--<Fees>
              <xsl:choose>
              <xsl:when test="number(COL36) and COL36 &lt; 0 ">
                  <xsl:value-of select="COL36 *(-1)"/>
              </xsl:when>
              <xsl:when test="number(COL36) and COL36 &gt; 0 ">
                  <xsl:value-of select="COL36"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            </Fees>-->


            <!--GROSS NOTIONAL in Local Currency-->

            <GrossNotionalValue>
              <xsl:value-of select="0"/>
            </GrossNotionalValue>


            <!--NET NOTIONAL in Local Currency -->
            <NetNotionalValue>
              <xsl:choose>
              <xsl:when test="number(COL21) &lt; 0 ">
                  <xsl:value-of select="(COL21) *(-1)"/>
              </xsl:when>
              <xsl:when test="number(COL21) &gt; 0 ">
                  <xsl:value-of select="COL21"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
            </NetNotionalValue>


            <!--For SEDOL Search-->
            <SMRequest>
              <xsl:value-of select ="'TRUE'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
