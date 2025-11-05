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
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:if test="COL3 != 'Account' and number(COL12)">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'Lazard'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
              <xsl:value-of select="COL23"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varAvgPX">
				<xsl:choose>
					<xsl:when test ="COL15 = '6M'">
						<xsl:value-of select="COL17 div COL12"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="COL13"/>
					</xsl:otherwise>
				</xsl:choose>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="COL24"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varSecFees">
              <xsl:value-of select="COL19"/>
            </xsl:variable>

            <xsl:variable name="varMiscFee">
              <xsl:value-of select="COL19"/>
            </xsl:variable>

            <xsl:variable name="varNetNotionalValue">
              <xsl:value-of select="COL20"/>
            </xsl:variable>

			  <xsl:variable name="varBuySell">
				  <xsl:value-of select="substring(COL3,11,1)"/>
			  </xsl:variable>

			  <xsl:variable name="varBuySellOpt">
				  <xsl:value-of select="substring(COL10,1,1)"/>
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

            <xsl:variable name="PB_Currency_Name">
              <xsl:value-of select="COL14"/>
            </xsl:variable>
            <xsl:variable name="PB_Suffix">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name = $varPBName]/SymbolData[@TickerSuffixCode = $PB_Currency_Name]/@PBSuffixCode"/>
            </xsl:variable>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$varAssetType = '2'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test ="$PRANA_Symbol_NAME = ''">
								  <xsl:choose>
									  <xsl:when test="$varCurrency != 'USD'">
										  <xsl:value-of select="''"/>
									  </xsl:when>
									  <xsl:otherwise>

										  <xsl:value-of select="$varEquitySymbol"/>

									  </xsl:otherwise>
								  </xsl:choose>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select ="$PRANA_Symbol_NAME"/>
							  </xsl:otherwise>
						  </xsl:choose>
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

            <SEDOL>
              <xsl:choose>
                <xsl:when test="$varCurrency != 'USD'">
                  <xsl:value-of select="$varSEDOL"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>
            
            <Side>
				<xsl:choose>
					<xsl:when test ="$varAssetType = '2'">
						<xsl:choose>
							<xsl:when test="$varSide = 'B' and ($varBuySellOpt = '8') ">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'S' and ($varBuySellOpt = '8')">
								<xsl:value-of select="'Sell to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'B' and  $varBuySellOpt = '9'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'S' and $varBuySellOpt = '9'">
								<xsl:value-of select="'Sell to Open'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$varSide = 'B' and ($varBuySell = '1' or $varBuySell = '2') ">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'S' and ($varBuySell = '1' or $varBuySell = '2')">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'B' and  $varBuySell = '5'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>
							<xsl:when test="$varSide = 'S' and $varBuySell = '5'">
								<xsl:value-of select="'Sell short'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
            </Side>

            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0" >
                  <xsl:value-of select="$varCommission*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

			  <StampDuty>
				  <xsl:choose>
					  <xsl:when test="$varMiscFee &gt; 0">
						  <xsl:value-of select="$varMiscFee"/>
					  </xsl:when>
					  <xsl:when test="$varMiscFee &lt; 0" >
						  <xsl:value-of select="$varMiscFee*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </StampDuty>

            <Quantity>
              <xsl:choose>
                <xsl:when test="$varNetPosition &gt; 0">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
				  <xsl:when test="$varNetPosition &lt; 0">
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

            <NetNotionalValue>
              <xsl:choose>
				  <xsl:when test="$varNetNotionalValue &gt; 0">
					  <xsl:value-of select="$varNetNotionalValue"/>
				  </xsl:when>
				  <xsl:when test="$varNetNotionalValue &lt; 0">
					  <xsl:value-of select="$varNetNotionalValue*(-1)"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>
			  
			  

			  <SettlementDate>
				  <xsl:value-of select ="normalize-space(COL8)"/>
			  </SettlementDate>

            <!--GROSS NOTIONAL-->

            
				  <GrossNotionalValue>
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
			  </GrossNotionalValue>

			  <TradeDate>
				  <xsl:value-of select ="normalize-space(COL7)"/>
			  </TradeDate>

            <SMRequest>
              <xsl:value-of select="$varSMRequest"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
