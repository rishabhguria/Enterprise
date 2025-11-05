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

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

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
      <xsl:for-each select="//Comparision">

        <xsl:if test="number(COL8) and not(contains(COL5,'BNY MELLON') or contains(COL5,'U.S. DOLLARS') or contains(COL7,'Expire')  )">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'Jefferies'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPositionSettlementDate">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <!--<xsl:variable name="varCurrency">
              <xsl:value-of select="COL2"/>
            </xsl:variable>-->

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varAvgPX">
              <xsl:value-of select="COL26"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:choose>
                <xsl:when test="normalize-space(COL43)='OPTN'">
                  <xsl:value-of select="'OPT'"/>
                </xsl:when>
				  <xsl:when test="normalize-space(COL43)='EQTY'">
					  <xsl:value-of select="'EQT'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL10+COL11+COL12"/>
            </xsl:variable>

            <xsl:variable name="varSecFees">
              <xsl:value-of select="0"/>
            </xsl:variable>


            <xsl:variable name="varNetNotionalValue">
              <xsl:value-of select="COL32"/>
            </xsl:variable>

            <xsl:variable name="varSMRequest">
              <xsl:value-of select="'TRUE'"/>
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

            <xsl:choose>
              <xsl:when test="$varAssetType = 'OPT'">

                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>

                <IDCOOptionSymbol>
                  <xsl:value-of select="concat($varOSISymbol,'U')"/>
                </IDCOOptionSymbol>

                
              </xsl:when>

              <xsl:otherwise>
                <Symbol>
					<xsl:choose>
						<xsl:when test ="$PRANA_Symbol_NAME != ''">
							<xsl:value-of select ="$PRANA_Symbol_NAME"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="COL3"/>
						</xsl:otherwise>
					</xsl:choose>
                </Symbol>

               

                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:otherwise>
            </xsl:choose>


            <!--<CompanyName>
              <xsl:value-of select="$varDescription"/>
            </CompanyName>-->



			  <TotalCommissionandFees>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select='format-number($varCommission,"###.0000")'/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0" >
                  <xsl:value-of select='format-number($varCommission*-1,"###.0000")'/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TotalCommissionandFees>

			  <Commission>
				  <xsl:value-of select="COL10"/>
			  </Commission>


            <Quantity>
              <xsl:choose>
				  <xsl:when test ="number($varNetPosition) &gt; 0">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
				  <xsl:when test ="number($varNetPosition) &lt; 0">
					  <xsl:value-of select="$varNetPosition*(-1)"/>	  
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <AvgPX>
              <xsl:choose>
                <xsl:when test="number($varAvgPX)">
                  <xsl:value-of select="$varAvgPX"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>


            <xsl:choose>
              <xsl:when test ="number($varNetNotionalValue) &gt; 0">
                <NetNotionalValue>
                  <xsl:value-of select="$varNetNotionalValue"/>
                </NetNotionalValue>
              </xsl:when>
				<xsl:when test ="number($varNetNotionalValue) &lt; 0">
					<NetNotionalValue>
						<xsl:value-of select="$varNetNotionalValue*(-1)"/>
					</NetNotionalValue>
				</xsl:when>
              <xsl:otherwise>
                <NetNotionalValue>
                  <xsl:value-of select="0"/>
                </NetNotionalValue>
              </xsl:otherwise>
            </xsl:choose>


			  <Side>
				  <xsl:choose>
					  <xsl:when test="$varAssetType = 'EQT' and $varSide = 'Buy' "> 
						  <xsl:value-of select="'Buy'"/>
					  </xsl:when>
					  <xsl:when test="$varAssetType = 'EQT' and $varSide = 'Sell'">
						  <xsl:value-of select="'Sell'"/>
					  </xsl:when>
					  <xsl:when test="$varAssetType = 'EQT' and $varSide = 'Sell Short' ">
						  <xsl:value-of select="'Sell short'"/>
					  </xsl:when>
					  <xsl:when test="($varAssetType = 'OPT' or $varAssetType = 'EQT') and $varSide = 'Cover Short' ">
						  <xsl:value-of select="'Buy to Close'"/>
					  </xsl:when>
					  <xsl:when test="$varAssetType = 'OPT' and $varSide = 'Buy' ">
						  <xsl:value-of select="'Buy to Open'"/>
					  </xsl:when>
					  <xsl:when test="$varAssetType = 'OPT' and $varSide = 'Buy' ">
						  <xsl:value-of select="'Buy to Open'"/>
					  </xsl:when>
					  <xsl:when test="$varSide = 'Sell Short' and $varAssetType = 'OPT'">
						  <xsl:value-of select="'Sell to Open'"/>
					  </xsl:when>
					  <xsl:when test="$varAssetType = 'OPT' and $varSide = 'Sell'">
						  <xsl:value-of select="'Sell to Close'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Side>

			  <TradeDate>
				  <xsl:value-of select="COL2"/>
			  </TradeDate>

			  <CounterParty>
				  <xsl:value-of select ="COL15"/>
			  </CounterParty>

			  <CompanyName>
				  <xsl:value-of select="COL5"/>
			  </CompanyName>

            <SMRequest>
              <xsl:value-of select="$varSMRequest"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
