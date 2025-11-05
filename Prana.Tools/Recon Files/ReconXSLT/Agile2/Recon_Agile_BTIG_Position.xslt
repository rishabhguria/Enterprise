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

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varPBName">
          <xsl:value-of select="'BTIG'"/>
        </xsl:variable>

       


        <xsl:if test ="number(COL6)">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL5"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name = "PB_Identifier_NAME">
				  <xsl:value-of select="COL4"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_Identifier_Name">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[@Identifier=$PB_Identifier_NAME]/@PranaSymbol"/>
			  </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:choose>
                <xsl:when test="COL2 = 'OPTION'">
                  <xsl:value-of select="COL7 div (COL6*100)"/>
                </xsl:when>
				  <xsl:when test="COL2 = 'BOND'">
					  <xsl:value-of select="(COL7 div COL6)*100"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL7 div COL6"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varMarketValue">
              <xsl:value-of select="number(COL7)"/>
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
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="COL2 = 'OPTION'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL3"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>-->

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="COL2 = 'OPTION'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:otherwise>

						  <xsl:choose>
							  <xsl:when test="$PRANA_Identifier_Name!=''">
								  <xsl:value-of select="$PRANA_Identifier_Name"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:choose>
									  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
										  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										  </xsl:when>
									  <xsl:when test="COL5='DBV TECHNOLOGIES SA'">
										  <xsl:value-of select="''"/>
									  </xsl:when>
									  <xsl:when test="COL2='BOND' and COL4!=''">
										  <xsl:value-of select="COL4"/>
									  </xsl:when>
									  <xsl:when test="COL3!=''">
										  <xsl:value-of select="COL3"/>
									  </xsl:when>
									  
										  <xsl:otherwise>
											  <xsl:value-of select ="$PB_Symbol"/>
										  </xsl:otherwise>
									  </xsl:choose>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>
				  
								 
							  <!--</xsl:otherwise>
							  <xsl:when test="COL3!='' and $PRANA_SYMBOL_NAME=''">
								  <xsl:choose>

								  </xsl:when>
									  <xsl:otherwise>
										  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									  </xsl:otherwise>
								  </xsl:choose>

							  </xsl:when>

							  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
								  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
							  </xsl:when>

						  </xsl:choose>-->

					
			  </Symbol>

			  <CUSIP>
				  <xsl:choose>
					  <xsl:when test="COL5='DBV TECHNOLOGIES SA' or COL5='CELLECTIS SA'">
						  <xsl:value-of select="normalize-space(COL4)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CUSIP>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="COL2 = 'OPTION'">
                  <xsl:value-of select="concat(COL3,'U')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>
            
            <!--Side-->

            <Side>
              <xsl:choose>
                <xsl:when test="COL6 &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="COL6 &lt; 0">
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
