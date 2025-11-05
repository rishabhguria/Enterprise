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


	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
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
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <!--<xsl:if test="COL1 != 'Advisor' and number(COL13) and not(contains(COL6,'CFD'))and not(contains(COL6,'NON-TRADE ACTIVITY'))">-->
		  <xsl:if test="COL1 != 'Advisor' and number(COL13) and not(contains(COL6,'NON-TRADE ACTIVITY'))">

			
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'Goldman Sachs'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <xsl:variable name="PRANA_STRATEGY_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$varPBName]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
			  </xsl:variable>


            <xsl:variable name="varAssetType">
              <xsl:value-of select="normalize-space(COL31)"/>
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
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL13"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL16"/>
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

			  <xsl:variable name="COL13">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL13"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <xsl:variable name="COL14">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL14"/>
				  </xsl:call-template>
			  </xsl:variable>
			  
			  <xsl:variable name="COL17">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL17"/>
				  </xsl:call-template>
			  </xsl:variable>
			  
			  <xsl:variable name="COL16">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL16"/>
				  </xsl:call-template>
			  </xsl:variable>

            <xsl:variable name="varStampDuty">
				<xsl:choose>
					<xsl:when test="string-length(COL8) = 21">
						<xsl:value-of select="format-number($COL13 * $COL14*100 + $COL17 - $COL16,'#.00')"/>

					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="format-number($COL13 * $COL14 + $COL17 - $COL16,'#.00')"/>

					</xsl:otherwise>
				</xsl:choose>
            </xsl:variable>


            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <FundName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </FundName>
              </xsl:when>
              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </FundName>
              </xsl:otherwise>
            </xsl:choose>

			  <xsl:variable name="Month2">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL9,'/'))='1'">
						  <xsl:value-of select="concat(0,substring-before(COL9,'/'))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="substring-before(COL9,'/')"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <PositionStartDate>
				  <xsl:choose>
					  <xsl:when test="contains(substring-after(substring-after(normalize-space(COL9),'/'),'/'),'20')">
						  <xsl:value-of select="concat($Month2,'/',substring-before(substring-after(normalize-space(COL9),'/'),'/'),'/',substring-after(substring-after(COL9,'/'),'/'))"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="concat($Month2,'/',substring-before(substring-after(normalize-space(COL9),'/'),'/'),'/',20,substring-after(substring-after(COL9,'/'),'/'))"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </PositionStartDate>

			

			  <xsl:variable name="Month3">
				  <xsl:choose>
					  <xsl:when test="string-length(substring-before(COL10,'/'))='1'">
						  <xsl:value-of select="concat(0,substring-before(COL10,'/'))"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="substring-before(COL10,'/')"/>
					  </xsl:otherwise>
				  </xsl:choose>	  
			  </xsl:variable>

			  <PositionSettlementDate>
				  <xsl:choose>
					  <xsl:when test="contains(substring-after(substring-after(normalize-space(COL10),'/'),'/'),'20')">
						  <xsl:value-of select="concat($Month3,'/',substring-before(substring-after(normalize-space(COL10),'/'),'/'),'/',substring-after(substring-after(COL10,'/'),'/'))"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="concat($Month3,'/',substring-before(substring-after(normalize-space(COL10),'/'),'/'),'/',20,substring-after(substring-after(COL10,'/'),'/'))"/>
					  </xsl:otherwise>
				  </xsl:choose>
				  
			  </PositionSettlementDate>
			  <Strategy>
				  <xsl:value-of select ="$PRANA_STRATEGY_NAME"/>
			  </Strategy>

            <xsl:choose>
              <xsl:when test="string-length(COL8) = 21">
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
                    <xsl:with-param name="Suffix" select="COL5"/>
                  </xsl:call-template>
                </xsl:variable>

                <Symbol>
                  <xsl:choose>
                    <xsl:when test="$PRANA_Symbol_NAME != ''">
                      <xsl:value-of select="$PRANA_Symbol_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
						  <xsl:when test ="COL4 = 'SO AFRICAN RAND' or COL4 = 'EURO' or COL4 = 'CANADIAN DOLLAR' or COL4 = 'SWISS FRANC' or COL4 = 'UK POUND STERLING'">
							  <xsl:value-of select ="''"/>
						  </xsl:when>
                        <xsl:when test="COL4 != 'U S DOLLAR'">
                          <xsl:value-of select="concat($varEquitySymbol, $varSuffix)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="$varEquitySymbol"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </Symbol>

				  <SEDOL>
					  <xsl:choose>
						  <xsl:when test ="COL4 = 'SO AFRICAN RAND' or COL4 = 'EURO' or COL4 = 'CANADIAN DOLLAR' or COL4 = 'SWISS FRANC' or COL4 = 'UK POUND STERLING'">
							  <xsl:value-of select ="COL8"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="''"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </SEDOL>
                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>
              </xsl:otherwise>
            </xsl:choose>


            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

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
					<xsl:when test="contains(COL11,'CANCEL')">
						<xsl:choose>
							<xsl:when test="COL11 = 'CANCEL SELL'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'CANCEL SELL TO OPEN'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'CANCEL SHORT SELL'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'CANCEL SELL TO CLOSE'">
								<xsl:value-of select="'A'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'CANCEL BUY TO CLOSE' or COL11 = 'CANCEL BUY TO COVER'">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'CANCEL BUY TO OPEN'">
								<xsl:value-of select="'D'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'CANCEL BUY'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
							
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="COL11 = 'SELL TO OPEN'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'SHORT SELL'">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'SELL TO CLOSE'">
								<xsl:value-of select="'D'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'BUY TO CLOSE' or COL11 = 'BUY TO COVER'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'BUY TO OPEN'">
								<xsl:value-of select="'A'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'BUY'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="COL11 = 'SELL'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
             
            </SideTagValue>

            <CostBasis>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCostBasis))">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <Commission>

				<xsl:choose>
					<xsl:when test="contains(COL11,'CANCEL')">
						<xsl:choose>
							<xsl:when test="$varCommission &gt; 0">
								<xsl:value-of select="$varCommission * (-1)"/>
							</xsl:when>
							<xsl:when test="$varCommission &lt; 0">
								<xsl:value-of select="$varCommission"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
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
					</xsl:otherwise>
				</xsl:choose>

			</Commission>



			  <StampDuty>
				  <xsl:choose>
					  <xsl:when test="contains(COL11,'CANCEL')">
						  <xsl:choose>
							  <xsl:when test="$varStampDuty &gt; 0">
								  <xsl:value-of select="$varStampDuty * (-1)"/>
							  </xsl:when>
							  <xsl:when test="$varStampDuty &lt; 0">
								  <xsl:value-of select="$varStampDuty"/>
							  </xsl:when>
							   <xsl:otherwise>
								  <xsl:value-of select="0"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
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
					  </xsl:otherwise>
				  </xsl:choose>

				  </StampDuty>
			  </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
