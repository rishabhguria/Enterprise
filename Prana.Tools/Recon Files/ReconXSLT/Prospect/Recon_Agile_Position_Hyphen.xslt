<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'FP'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varPBName">
          <xsl:value-of select="'GS'"/>
        </xsl:variable>

        <xsl:variable name="varMarkPrice">
          <xsl:value-of select="COL10"/>
        </xsl:variable>


        <xsl:if test ="number(COL8)">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY">
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varMarketValue">
              <xsl:value-of select="number(COL11)"/>
            </xsl:variable>


            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>

          

            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <SEDOL>
              <xsl:value-of select="COL5"/>
            </SEDOL>

          
            <!--QUANTITY-->

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($varNetPosition)">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>
            <!--Side-->

            <Side>
              <xsl:choose>
                <xsl:when test="COL8 &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="COL8 &lt; 0">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="number($varMarkPrice) ">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>


            <MarketValue>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarketValue))">
                  <xsl:value-of select="$varMarketValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>


            <SMRequest>
              <xsl:value-of select="'TRUE'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
