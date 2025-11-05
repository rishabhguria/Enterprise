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
		<xsl:when test="$Suffix = 'CHF'">
			<xsl:value-of select="'-SWX'"/>
		</xsl:when>
		<xsl:when test="$Suffix = 'EUR'">
			<xsl:value-of select="'-EEB'"/>
		</xsl:when>
		<xsl:when test="$Suffix = 'CAD'">
			<xsl:value-of select="'-TC'"/>
		</xsl:when>
		<xsl:when test="$Suffix = 'ILS'">
			<xsl:value-of select="'-TAE'"/>
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
        <xsl:variable name="varCostBasis">
          <xsl:value-of select="COL14"/>
        </xsl:variable>

        <xsl:if test ="number(COL13)">

          <xsl:variable name="varPBName">
            <xsl:value-of select="'GS'"/>
          </xsl:variable>

          <PositionMaster>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL8"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL13"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_CountnerParty" select="COL7"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>-->

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL16"/>
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
                <xsl:when test="string-length(COL8) = 21">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL8"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="string-length(COL8) = 21">
                  <xsl:value-of select="concat(COL8, 'U')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>


            <AvgPx>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCostBasis))">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPx>


            <!--QUANTITY-->

            <xsl:choose>
              <xsl:when test="$varNetPosition &lt; 0">
                <Quantity>
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </Quantity>
              </xsl:when>
              <xsl:when test="$varNetPosition &gt; 0">
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
                <xsl:when test="COL11 = 'BUY TO COVER'">
                  <xsl:value-of select="'Buy to Close'"/>
                </xsl:when>
                <xsl:when test="COL11 = 'SELL'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
                <xsl:when test="COL11 = 'SELL TO OPEN'">
                  <xsl:value-of select="'Sell to Open'"/>
                </xsl:when>
				  <xsl:when test="COL11 = 'SHORT SELL'">
					  <xsl:value-of select="'Sell Short'"/>
				  </xsl:when>
				  <xsl:when test="COL11 = 'SELL TO CLOSE'">
					  <xsl:value-of select="'Sell to Close'"/>
				  </xsl:when>
                <xsl:when test="COL11 = 'BUY TO OPEN'">
                  <xsl:value-of select="'Buy to Open'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL11"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="COL17 &gt; 0">
                  <xsl:value-of select="COL17"/>
                </xsl:when>
                <xsl:when test="COL17 &lt; 0">
                  <xsl:value-of select="COL17*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

                      
            <SMRequest>
              <xsl:value-of select="'TRUE'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>