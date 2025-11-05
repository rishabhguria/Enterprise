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

      <xsl:when test="$Suffix = 'GBP'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
    
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">



        <xsl:variable name = "PB_FUND_NAME" >
          <xsl:value-of select="COL9"/>
        </xsl:variable>

        <xsl:variable name="PRANA_FUND_NAME">
          <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='EZE']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
        </xsl:variable>
        <xsl:if test="number(COL10)">
          <PositionMaster>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL25"/>
            </xsl:variable>
			  <xsl:variable name="PB_CURRENCY" select="normalize-space(COL26)"/> 
            <!--<xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='EZE']/SymbolData[@PBCompanyName=$PB_Symbol or @Currency=$PB_CURRENCY]/@PranaSymbol"/>
            </xsl:variable>-->

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='EZE']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_WITH_CURRENCY">
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='EZE']/SymbolData[(@PBCompanyName=$PB_Symbol and @Currency=$PB_CURRENCY)]/@PranaSymbol"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="COL27 = 'PUT' or COL27 = 'CALL'">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>
				  <xsl:when test="contains(COL1,'.S') and COL9!='MAGNITUDE'">
					  <xsl:value-of select="'Swap'"/>
				  </xsl:when>
				  <xsl:when test="contains(COL1,'MSTHAMLP') and COL9!='MAGNITUDE'">
					  <xsl:value-of select="'Swap'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Stock'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PB_CountnerParty" select="COL7"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'EZE']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

			  <xsl:variable name="varClearingFee">
				  <xsl:choose>
					  <xsl:when test="COL9='BP_WCS' and string-length(COL1) &gt; 10" >
						  <xsl:value-of select="0.04*COL10"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="COL13"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <xsl:variable name="varSuffix">
              <xsl:call-template name="GetSuffix">
                <xsl:with-param name="Suffix" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>

			  <MiscFees>
				  <xsl:choose>
					  <xsl:when test="COL9='BP_WCS' and string-length(COL1) &gt; 10">

						  <xsl:choose>
							  <xsl:when test="COL10 &gt;0 and COL10 &lt; 501">
								  <xsl:value-of select="0.050*COL10"/>
							  </xsl:when>
							  <xsl:when test="COL10 &gt;500 and COL10 &lt; 1001">
								  <xsl:value-of select="0.040*COL10"/>
							  </xsl:when>
							  <xsl:when test="COL10 &gt;1000 and COL10 &lt; 2001">
								  <xsl:value-of select="0.030*COL10"/>
							  </xsl:when>
							  <xsl:when test="COL10 &gt;2000 ">
								  <xsl:value-of select="55"/>
							  </xsl:when>

						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </MiscFees>


			  <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <Symbol>
              <!--<xsl:choose>
				  <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
					  <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
				  </xsl:when>-->
				<xsl:choose>

					<xsl:when test="$PRANA_SYMBOL_WITH_CURRENCY!=''">
						<xsl:value-of select="$PRANA_SYMBOL_WITH_CURRENCY"/>
					</xsl:when>

					<xsl:when test="$PRANA_SYMBOL_WITH_CURRENCY='' and $PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					</xsl:when>
                <xsl:when test="$varAsset = 'Option'">
                  <xsl:value-of select="''"/>
                </xsl:when>
				 
				  
                <xsl:otherwise>
					<xsl:choose>
						<xsl:when test="contains(COL1,'.S')">
							<xsl:value-of select="substring-before(COL1,'.S')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL1"/>
						</xsl:otherwise>
					</xsl:choose>
                 
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$varAsset = 'Option'">
                  <xsl:value-of select="concat(COL1, 'U')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>


            <PBSymbol>
              <xsl:value-of select="COL25"/>
            </PBSymbol>


            <CostBasis>
              <xsl:choose>
                <xsl:when  test="number(COL11)">
                  <xsl:value-of select="COL11"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CostBasis>

            <PositionStartDate>
              <xsl:value-of select="COL5"/>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:value-of select="COL6"/>
            </PositionSettlementDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when  test="number(normalize-space(COL10)) &gt; 0">
                  <xsl:value-of select="COL10"/>
                </xsl:when>
                <xsl:when test="number(normalize-space(COL10)) &lt; 0">
                  <xsl:value-of select="COL10 * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="COL3= 'Buy' and $varAsset = 'Option'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>
                <xsl:when test="COL3= 'Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="COL3= 'Cover'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="COL3= 'Sell' and $varAsset = 'Option'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>
                <xsl:when test="COL3= 'Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="COL3= 'Short' and $varAsset = 'Option'">
                  <xsl:value-of select="'C'"/>
                </xsl:when>
                <xsl:when test="COL3= 'Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

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
				<xsl:when test="$varAsset = 'Option' and COL8='JPM' and COL3='Sell'">
					<xsl:value-of select="number(COL10)*number(COL11)*100*0.0000184"/>
				</xsl:when>
				<xsl:when test="$varAsset = 'Option' and COL8='JPM' and COL3='Short'">
					<xsl:value-of select="number(COL10)*number(COL11)*100*0.0000184"/>
				</xsl:when>
              <xsl:when test="number(COL12)">
                <xsl:value-of select="COL12"/>
              </xsl:when>          
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </StampDuty>


          <ClearingFee>
            <xsl:choose>
              <xsl:when test="$varClearingFee &gt; 0">
                <xsl:value-of select="$varClearingFee"/>
              </xsl:when>
              <xsl:when test="$varClearingFee &lt; 0">
              <xsl:value-of select="$varClearingFee*(-1)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </ClearingFee>

			 
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



			  <xsl:if test="$varAsset='Swap'">
				  <IsSwapped>
					  <xsl:value-of select ="1"/>
				  </IsSwapped>

				  <SwapDescription>
					  <xsl:value-of select ="'SWAP'"/>
				  </SwapDescription>

				  <DayCount>
					  <xsl:value-of select ="365"/>
				  </DayCount>

				  <ResetFrequency>
					  <xsl:value-of select ="'Monthly'"/>
				  </ResetFrequency>

				  <OrigTransDate>
					  <!--<xsl:call-template name="MonthCode">
<xsl:with-param name="varMonth" select="COL6"/>
</xsl:call-template>-->
					  <!--<xsl:value-of select="concat(substring-before(COL13,'/'),'/',substring-after(substring-before(COL13,'/'),'/'),'/',substring-after(substring-after(COL13,'/'),'/'))"/>-->
					  <xsl:value-of select="COL5"/>
				  </OrigTransDate>

				  <xsl:variable name="varPreviousMonth">
					  <xsl:value-of select="substring-before(COL5,'/')"/>
				  </xsl:variable>



				  <xsl:variable name ="varPrevMonth">
					  <!--<xsl:call-template name="PrevMonth">
<xsl:with-param name="varPreviousMonth" select="$varPreviousMonth"/>
</xsl:call-template>-->
					  <xsl:choose>
						  <xsl:when test="number($varPreviousMonth)=1">
							  <xsl:value-of select="12"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="$varPreviousMonth - 1"/>
						  </xsl:otherwise>
					  </xsl:choose>

				  </xsl:variable>

				  <xsl:variable name="varYearNo">
					  <xsl:value-of select="substring-after(substring-after(COL5,'/'),'/')"/>
				  </xsl:variable>

				  <xsl:variable name ="varYear">
					  <xsl:choose>
						  <xsl:when test="number($varPrevMonth)=1">
							  <xsl:value-of select="($varYearNo)-1"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="$varYearNo"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

				  <FirstResetDate>
					  <xsl:value-of select ="concat($varPrevMonth,'/28/',$varYear)"/>
				  </FirstResetDate>


			  </xsl:if>


		  </PositionMaster>
	  </xsl:if>
  </xsl:for-each>
</DocumentElement>
</xsl:template>

<!-- variable declaration for lower to upper case -->

<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


