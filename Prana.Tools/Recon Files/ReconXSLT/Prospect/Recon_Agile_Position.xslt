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
    <xsl:param name="Exchange"/>
    <xsl:choose>
      <xsl:when test="$Exchange = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Exchange = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$Exchange = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Exchange = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:when test="$Exchange = 'GBP'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
      <xsl:when test="$Exchange = 'SEK'">
        <xsl:value-of select="'-OMX'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test ="$varMonth = 'JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 'DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varPBName">
          <xsl:value-of select="'Jefferies'"/>
        </xsl:variable>

        <xsl:variable name="varMarkPrice">
          <xsl:value-of select="COL6"/>
        </xsl:variable>


        <xsl:if test ="number(COL5) ">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
			  <xsl:variable name="varPBCode">
				  <xsl:value-of select="normalize-space(substring(COL2,1,2))"/>
			  </xsl:variable>

			  <xsl:variable name="varPrana_Root">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name = 'IB']/SymbolData[@InstrmentCode = $varPBCode ]/@UnderlyingCode"/>
			  </xsl:variable>

			  <xsl:variable name="varSuffix">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name = 'IB']/SymbolData[@InstrmentCode = $varPBCode ]/@ExchangeCode"/>
			  </xsl:variable>

			  <xsl:variable name="varRoot">
				  <xsl:choose>
					  <xsl:when test="$varPrana_Root = ''">
						  <xsl:value-of select="$varPBCode"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$varPrana_Root"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL3"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'IB']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name = "PB_SymbolCurrency_NAME" >
				  <xsl:value-of select="COL3"/>
			  </xsl:variable>

			  <xsl:variable name = "PB_Currency_NAME" >
				  <xsl:value-of select="COL8"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SymbolCurrency_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='IB']/SymbolData[@PBCompanyName=$PB_SymbolCurrency_NAME and @Currency=$PB_Currency_NAME]/@PranaSymbol"/>
			  </xsl:variable>


            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="varMarketValue">
              <xsl:value-of select="number(COL7)"/>
            </xsl:variable>

			  <xsl:variable name ="varMonthCode">
				  <xsl:call-template name ="MonthCode">
					  <xsl:with-param name ="varMonth" select ="substring(COL3, string-length(COL3)-4, 3)"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name ="varYear">
				  <xsl:value-of select ="substring(COL3, string-length(COL3), 1)"/>
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
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>
					  <xsl:when test="COL9='OPT'">
						  <xsl:value-of select ="''"/>
					  </xsl:when>
					  <xsl:when test="COL9='FUT'">
						  <xsl:value-of select ="normalize-space(concat($varRoot,' ',$varMonthCode, $varYear, $varSuffix))"/>
					  </xsl:when>
					  <xsl:when test="COL9='FOP'">
						  <xsl:value-of select ="normalize-space(concat($varRoot,' ',$varMonthCode, $varYear, substring(COL2,6), $varSuffix))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$PRANA_SymbolCurrency_NAME=''">
								  <xsl:value-of select="COL2"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="$PRANA_SymbolCurrency_NAME"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>

			  </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="COL9 = 'OPT'">
                  <xsl:value-of select="concat(COL2,'U')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>
            
            <!--Side-->

            <Side>
              <xsl:choose>
                <xsl:when test="COL16 = 'Long'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="COL16 = 'Short'">
                  <xsl:value-of select="'Sell short'"/>
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
