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

  <xsl:template name ="GetCurrencySymbol">
    <xsl:param name ="Currency"/>
    <xsl:choose>
      <xsl:when test ="$Currency = 'U S DOLLAR'">
        <xsl:value-of select ="'USD'"/>
      </xsl:when>
      <xsl:when test ="$Currency = 'EURO'">
        <xsl:value-of select ="'EUR'"/>
      </xsl:when>
      <xsl:when test ="$Currency = 'AUSTRALIAN DOLLAR'">
        <xsl:value-of select ="'AUD'"/>
      </xsl:when>
      <xsl:when test ="$Currency = 'CANADIAN DOLLAR'">
        <xsl:value-of select ="'CAD'"/>
      </xsl:when>
      <xsl:when test ="$Currency = 'MEXICAN PESO'">
        <xsl:value-of select ="'MXN'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:if test="number(COL6)">
        <PositionMaster>
          <xsl:variable name="varPBName">
            <xsl:value-of select="'MapleRock'"/>
          </xsl:variable>
          
          <!--fundname section-->
          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="COL2"/>
          </xsl:variable>

          <xsl:variable name="PRANA_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

          <!--<Symbol>
				<xsl:call-template name ='GetCurrencySymbol'>
					<xsl:with-param name ='Currency' select ='COL4'/>
				</xsl:call-template>
            </Symbol>-->

          <Symbol>
            <xsl:value-of select ="COL3"/>
          </Symbol>

			<CashValueBase>
				<xsl:choose>
					<xsl:when test="number(COL8)">
						<xsl:value-of select="COL8"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="0"/>
					</xsl:otherwise>
				</xsl:choose>
			</CashValueBase>

			<CashValueLocal>
				<xsl:choose>
					<xsl:when test="number(COL6)">
						<xsl:value-of select="COL6"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="0"/>
					</xsl:otherwise>
				</xsl:choose>
			</CashValueLocal>

			<!--<TradeDate>
            <xsl:value-of select ="COL1"/>
          </TradeDate>-->

        </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>