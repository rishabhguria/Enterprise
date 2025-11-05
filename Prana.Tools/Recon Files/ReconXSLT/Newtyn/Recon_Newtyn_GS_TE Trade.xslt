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
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>

      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>

      <xsl:when test="$Suffix = 'GBP'">
        <xsl:value-of select="'-LON'"/>
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
        <xsl:if test ="number(COL17) and COL15!= 'CASH AND FX-CONTRACT' and COL15 != 'FUTURE'">

          <xsl:variable name="varCostBasis">
            <xsl:value-of select="COL28"/>
          </xsl:variable>

			<PositionMaster>


				<xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL10"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL17"/>
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
                <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="COL15 = 'BOND'">
                  <xsl:value-of select="COL34"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL36"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

				<ISINSymbol>
					<xsl:value-of select="normalize-space(COL34)"/>
				</ISINSymbol>

            <AvgPx>
				<xsl:choose>
					<xsl:when test ="contains(COL12,'RON') or contains(COL12,'KRW') ">
						<xsl:value-of select="$varCostBasis"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="COL19"/>
					</xsl:otherwise>
				</xsl:choose>
            </AvgPx>


            <!--QUANTITY-->

            <xsl:choose>
              <xsl:when test="number($varNetPosition)">
                <Quantity>
                  <xsl:value-of select="$varNetPosition"/>
                </Quantity>
              </xsl:when>
              <xsl:otherwise>
                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>
              </xsl:otherwise>
            </xsl:choose>

            <Side>
              <xsl:choose>
                <xsl:when test="COL14 = 'SHORT TAXLOTS'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:when test="COL14 = 'LONG TAXLOTS'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>
				<SEDOL>
					<xsl:choose>
						<xsl:when test="COL35='null'">
							<xsl:value-of select="''"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="COL35"/>
						</xsl:otherwise>
					</xsl:choose>

				</SEDOL>

            <!--<NetNotionalValue>
              <xsl:choose>
                <xsl:when test="COL16 &gt; 0">
                  <xsl:value-of select="COL16"/>
                </xsl:when>
                <xsl:when test="COL16 &lt; 0">
                  <xsl:value-of select="COL16*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>-->
          
          </PositionMaster>
				</xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>