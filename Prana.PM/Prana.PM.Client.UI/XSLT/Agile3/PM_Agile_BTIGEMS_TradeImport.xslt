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

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varCostBasis">
          <xsl:value-of select="COL6"/>
        </xsl:variable>

        <xsl:if test ="number(COL5)">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Agile']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL11"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Agile']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="PB_CountnerParty" select="COL17"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'Agile']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>
            
            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varFees">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

			  <xsl:variable name="varAssetType">
				  <xsl:choose>
					  <xsl:when test ="COL13 != '' and COL13 != '*' and string-length(COL13) = 21">
						  <xsl:value-of select="'OPT'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'EQT'"/>
					  </xsl:otherwise>
				  </xsl:choose>
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
              <xsl:value-of select="COL4"/>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:value-of select="COL14"/>
            </PositionSettlementDate>

            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test ="string-length(COL3) = 21">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL3"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test ="string-length(COL3) = 21">
                  <xsl:value-of select="concat(COL3, 'U')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>

            <PBSymbol>
              <xsl:value-of select="COL3"/>
            </PBSymbol>

			  <PBAssetType>
				  <xsl:value-of select="$varAssetType"/>
			  </PBAssetType>


            <!--QUANTITY-->

            <xsl:choose>
              <xsl:when test="$varNetPosition &lt; 0">
                <NetPosition>
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </NetPosition>
              </xsl:when>
              <xsl:when test="$varNetPosition &gt; 0">
                <NetPosition>
                  <xsl:value-of select="$varNetPosition"/>
                </NetPosition>
              </xsl:when>
              <xsl:otherwise>
                <NetPosition>
                  <xsl:value-of select="0"/>
                </NetPosition>
              </xsl:otherwise>
            </xsl:choose>

            <!--Side-->

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAssetType = 'EQT' and COL2 = 'Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
				  <xsl:when test="$varAssetType = 'OPT' and COL2 = 'Buy'">
					  <xsl:value-of select="'A'"/>
				  </xsl:when>
                <xsl:when test="$varAssetType = 'EQT' and COL2 = 'Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
				  <xsl:when test="$varAssetType = 'OPT' and COL2 = 'Sell'">
					  <xsl:value-of select="'D'"/>
				  </xsl:when>
				  <xsl:when test="$varAssetType = 'OPT' and COL2 = 'Sell Short'">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>
				  <xsl:when test="$varAssetType = 'EQT' and COL2 = 'Sell Short'">
					  <xsl:value-of select="'5'"/>
				  </xsl:when>
                <xsl:when test="$varAssetType = 'EQT' and COL2 = 'Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
				  <xsl:when test="$varAssetType = 'OPT' and COL2 = 'Short'">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>

				  <xsl:when test="$varAssetType = 'EQT' and COL2 = 'Cover Short'">
					  <xsl:value-of select="'B'"/>
				  </xsl:when>
				  <xsl:when test="$varAssetType = 'OPT' and COL2 = ' Cover Short'">
					  <xsl:value-of select="'B'"/>
				  </xsl:when>
                <xsl:when test="($varAssetType = 'EQT' or $varAssetType = 'OPT') and COL2 = 'Cover'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <CostBasis>
              <xsl:choose>
                <xsl:when test ="number($varCostBasis) &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test ="number($varCostBasis) &lt; 0">
                  <xsl:value-of select="$varCostBasis*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

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

            <StampDuty>
              <xsl:choose>
                <xsl:when test="$varFees &gt; 0">
                  <xsl:value-of select="$varFees"/>
                </xsl:when>
                <xsl:when test="$varFees &lt; 0">
                  <xsl:value-of select="$varFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>

			  <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_CounterPartyID)">
                  <xsl:value-of select="$PRANA_CounterPartyID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>

            <!--<StampDuty>
              <xsl:choose>
                <xsl:when test="$varStampDuty &gt; 0">
                  <xsl:value-of select="$varStampDuty"/>
                </xsl:when>
                <xsl:when test="$varStampDuty &lt; 0">
                  <xsl:value-of select="$varStampDuty*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </StampDuty>-->
			  
			  
			  <FXRate>
				  <xsl:value-of select="COL15"/>
			  </FXRate>
			  
			  <FXConversionMethodOperator>
              <xsl:value-of select="'M'"/>
            </FXConversionMethodOperator>
           

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
