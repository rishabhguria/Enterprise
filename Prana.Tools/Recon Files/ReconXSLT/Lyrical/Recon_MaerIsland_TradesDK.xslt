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

        <xsl:if test="COL1 != 'Account Name' and number(COL8)">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'Jefferies'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL5)"/>
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


            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="normalize-space(COL16)"/>
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
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="''"/>
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

            <xsl:variable name="varCommission">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varMiscFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varClearingFee">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varTransactionLevy">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varSecFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varStampDuty">
              <xsl:value-of select="0"/>
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

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>


            <xsl:choose>
              <xsl:when test="$varAssetType = 'OPTC' or $varAssetType = 'OPTP'">
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>

                <IDCOOptionSymbol>
                  <xsl:value-of select="concat($varOSISymbol,'U')"/>
                </IDCOOptionSymbol>

              </xsl:when>

              <xsl:otherwise>
                <xsl:variable name="varSuffix">
                  <xsl:call-template name="GetSuffix">
                    <xsl:with-param name="Suffix" select="COL24"/>
                  </xsl:call-template>
                </xsl:variable>

                <Symbol>
                  <xsl:choose>
                    <xsl:when test="$PRANA_Symbol_NAME != ''">
                      <xsl:value-of select="$PRANA_Symbol_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
                        <xsl:when test="COL24 != 'USD'">
                          <xsl:value-of select="concat($varEquitySymbol, $varSuffix)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="$varEquitySymbol"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </Symbol>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:otherwise>
            </xsl:choose>

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

            <Side>
              <xsl:choose>
				  <xsl:when test="COL7 = 'Sell Short' and substring(COL16,1,3) = 'OPT'">
					  <xsl:value-of select="'Sell to Open'"/>
				  </xsl:when>
				  <xsl:when test="COL7 = 'Buy' and substring(COL16,1,3) = 'OPT'">
					  <xsl:value-of select="'Buy to Open'"/>
				  </xsl:when>
				  <xsl:when test="COL7 = 'Buy'">
					  <xsl:value-of select="'Buy'"/>
				  </xsl:when>
				  <xsl:when test="COL7 = 'Cover Short'">
					  <xsl:value-of select="'Buy to Close'"/>
				  </xsl:when>
                <xsl:when test="normalize-space(COL7)='Sell'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
                <xsl:when test="COL7 = 'Sell Short'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
				 
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($varNetPosition) &gt; 0">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
				  <xsl:when test="number($varNetPosition) &lt; 0">
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


            <!--GROSS NOTIONAL-->

            <xsl:choose>
              <xsl:when test ="number(COL13)">
                <NetNotionalValue>
                  <xsl:value-of select="COL13"/>
                </NetNotionalValue>
              </xsl:when>
              <xsl:otherwise>
                <NetNotionalValue>
                  <xsl:value-of select="0"/>
                </NetNotionalValue>
              </xsl:otherwise>
            </xsl:choose>

			  <xsl:choose>
				  <xsl:when test ="number(COL32)">
					  <NetNotionalValueLocal>
						  <xsl:value-of select="COL32"/>
					  </NetNotionalValueLocal>
							</xsl:when>
							<xsl:otherwise>
								<NetNotionalValueLocal>
						  <xsl:value-of select="0"/>
					  </NetNotionalValueLocal>
				  </xsl:otherwise>
			  </xsl:choose>

            <!--<PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>-->

            <SMRequest>
              <xsl:value-of select="'TRUE'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
