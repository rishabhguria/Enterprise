<?xml version="1.0" encoding="utf-8"?>

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
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
		  <xsl:if test="COL1 != 'Report_Date'">
			  <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name="varPBName">
              <xsl:value-of select="'Lazard'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="COL17"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
				<xsl:choose>
					<xsl:when test ="COL17 = '7' or COL17 = '9' or COL17 = '11'">
						<xsl:value-of select ="(COL18 div COL6)*(100)"/>
					</xsl:when>
					<xsl:when test ="COL17 = '2'">
						<xsl:value-of select ="(COL18 div COL6) div 100"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="COL18 div COL6"/>
					</xsl:otherwise>
				</xsl:choose>
            </xsl:variable>
            <!--<xsl:variable name="varSMRequest">
              <xsl:value-of select="'TRUE'"/>
            </xsl:variable>-->
			  
			  <xsl:variable name ="varCount">
				  <xsl:value-of select ="position()"/>
			  </xsl:variable>

            <xsl:variable name="varSuffix">
              <xsl:call-template name="GetSuffix">
                <xsl:with-param name="Suffix" select="$varCurrency"/>
              </xsl:call-template>
            </xsl:variable>

				  <Symbol>
					  <xsl:choose>
						  <xsl:when test ='$varAssetType != "2"'>
							  <xsl:choose>
								  <xsl:when test ="$PRANA_Symbol_NAME = ''">
									  <xsl:choose>
										  <xsl:when test =" ((normalize-space($varEquitySymbol) = '' or $varEquitySymbol = '*') ) and $varCUSIP = ' ' and $varSEDOL = ' '">
											  <xsl:value-of select ="$varCount"/>
										  </xsl:when>
										  <xsl:otherwise>
											  <xsl:choose>
												  <xsl:when test ="$varCurrency != 'USD' or ($varAssetType = '6' or $varAssetType = '7' or $varAssetType = '9' or $varAssetType = '11')">
													  <xsl:value-of select="''"/>
												  </xsl:when>
												  <xsl:otherwise>
													  <xsl:value-of select ="$varEquitySymbol"/>
												  </xsl:otherwise>
											  </xsl:choose>
										  </xsl:otherwise>
									  </xsl:choose>
								  </xsl:when>
								  <xsl:otherwise>
									  <xsl:value-of select ="$PRANA_Symbol_NAME"/>
								  </xsl:otherwise>
							  </xsl:choose>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </Symbol>

				  <IDCOOptionSymbol>
					  <xsl:choose>
						  <xsl:when test="$varAssetType = '2'">
							  <xsl:value-of select="concat($varOSISymbol,'U')"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </IDCOOptionSymbol>



				  <CUSIP>
					  <xsl:choose>
						  <xsl:when test ="($varCurrency != 'USD' or ($varAssetType = '6' or $varAssetType = '7' or $varAssetType = '9' or $varAssetType = '11')) and $varCUSIP != ' '">
							  <xsl:value-of select="$varCUSIP"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </CUSIP>

				  <SEDOL>
					  <xsl:choose>
						  <xsl:when test ="($varCurrency != 'USD' or ($varAssetType = '6' or $varAssetType = '7' or $varAssetType = '9' or $varAssetType = '11')) and $varCUSIP = ' ' and $varSEDOL != ' '">
							  <xsl:value-of select="$varSEDOL"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </SEDOL>

            <Date>
              <xsl:value-of select="$varPositionStartDate"/>
            </Date>

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarkPrice))">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
