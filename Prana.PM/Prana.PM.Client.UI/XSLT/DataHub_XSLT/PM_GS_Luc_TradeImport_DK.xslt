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
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="COL1 != 'Advisor' and number(COL13) and not(contains(COL7,'CFD'))">

			
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <xsl:variable name="PRANA_STRATEGY_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$varPBName]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
			  </xsl:variable>


            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL9"/>
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

            <xsl:variable name="varStampDuty">
				<xsl:choose>
					<xsl:when test="string-length(COL8) = 21">
						<xsl:value-of select="format-number(COL13*COL14*100 + COL17 - COL16,'#.00')"/>

					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="format-number(COL13*COL14 + COL17 - COL16,'#.00')"/>

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
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

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

            <!--<Fees>
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
            </Fees>

            <MiscFees>
              <xsl:choose>
                <xsl:when test="$varMiscFees &gt; 0">
                  <xsl:value-of select="$varMiscFees"/>
                </xsl:when>
                <xsl:when test="$varMiscFees &lt; 0">
                  <xsl:value-of select="$varMiscFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>

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

            <TransactionLevy>
              <xsl:choose>
                <xsl:when test="$varTransactionLevy &gt; 0">
                  <xsl:value-of select="$varTransactionLevy"/>
                </xsl:when>
                <xsl:when test="$varTransactionLevy &lt; 0">
                  <xsl:value-of select="$varTransactionLevy*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionLevy>

            -->

			  <StampDuty>
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
			  </StampDuty>
			  
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
