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
    </xsl:choose>
  </xsl:template>


	<xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="varCostBasis">
          <xsl:value-of select="COL12"/>
        </xsl:variable>

		  <xsl:if test ="number(COL5) and COL2 != 'Cash and Equivalents'">

          <xsl:variable name="varPBName">
            <xsl:value-of select="'GS'"/>
          </xsl:variable>

          <PositionMaster>

			  <xsl:variable name = "PB_FUND_NAME">
				  <xsl:value-of select="COL3"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='LCM']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

			  <xsl:variable name="PB_Symbol">
				  <xsl:value-of select = "COL7"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='LCM']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
			  </xsl:variable>

			  <xsl:variable name="varNetPosition">
				  <xsl:value-of select="COL5"/>
			  </xsl:variable>

			  <!--<xsl:variable name="PB_CountnerParty" select="COL7"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'LCM']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>-->


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
					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_Symbol"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

			  <!--<Bloomberg>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
						  <xsl:value-of select ="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="concat(COL130, ' EQUITY')"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Bloomberg>-->

			  <!--Side-->

			  <Side>
				  <xsl:choose>
					  <xsl:when test="$varNetPosition &gt; 0">
						  <xsl:value-of select="'Buy'"/>
					  </xsl:when>
					  <xsl:when test="$varNetPosition &lt; 0">
						  <xsl:value-of select="'Sell'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Side>

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

			  <!--<TradeDate>
				  <xsl:value-of select ="COL69"/>
			  </TradeDate>-->


			  <NetNotionalValue>
				  <xsl:choose>
					  <xsl:when test="number(COL14)">
						  <xsl:value-of select="COL14"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValue>

			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test="number(COL15)">
						  <xsl:value-of select="COL15"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValueBase>


			  <!--<SMRequest>
				  <xsl:value-of select="'TRUE'"/>
			  </SMRequest>-->


		  </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>