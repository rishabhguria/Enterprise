<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml"
			  encoding="UTF-8" indent="yes"/>

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
      <xsl:when test="$Suffix = 'T'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'OS'">
        <xsl:value-of select="'-OSE'"/>
      </xsl:when>
		<xsl:when test="$Suffix = 'CN'">
			<xsl:value-of select="'-TC'"/>
		</xsl:when>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL12)">
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
              <xsl:value-of select="substring(COL3,5,5)"/>
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
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
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

			  <xsl:variable name ="varPositionSettlementDate">
				  <xsl:value-of select ="COL8"/>
			  </xsl:variable>

			  <xsl:variable name="varBuySell">
				  <xsl:value-of select="substring(COL3,11,1)"/>
			  </xsl:variable>

			  <xsl:variable name="varBuySellOpt">
				  <xsl:value-of select="substring(COL10,1,1)"/>
			  </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME='' or $PRANA_FUND_NAME='*'">
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

            <!--<xsl:variable name="PB_Currency_Name">
              <xsl:value-of select="COL14"/>
            </xsl:variable>
            <xsl:variable name="PB_Suffix">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name = $varPBName]/SymbolData[@TickerSuffixCode = $PB_Currency_Name]/@PBSuffixCode"/>
            </xsl:variable>-->

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

            <Symbol>
				<xsl:choose>
					<xsl:when test ="$PRANA_FUND_NAME = '*'">
						<xsl:value-of select ="''"/>
					</xsl:when>
					<xsl:otherwise>

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
					</xsl:otherwise>
				</xsl:choose>
            </Symbol>

            <IDCOOptionSymbol>
				<xsl:choose>
					<xsl:when test ="$PRANA_FUND_NAME = '*'">
						<xsl:value-of select ="''"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$varAssetType = '2'">
								<xsl:value-of select="concat($varOSISymbol,'U')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
            </IDCOOptionSymbol>

			  <PositionSettlementDate>
				  <xsl:value-of select ="$varPositionSettlementDate"/>
			  </PositionSettlementDate>

            <SEDOL>
				<xsl:choose>
					<xsl:when test ="$PRANA_FUND_NAME = '*'">
						<xsl:value-of select ="''"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$varCurrency != 'USD'">
								<xsl:value-of select="$varSEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
            </SEDOL>

			  <PBSymbol>
				<xsl:choose>
					<xsl:when test ="$PRANA_FUND_NAME = '*'">
						<xsl:choose>
							<xsl:when test="$PRANA_Symbol_NAME != ''">
								<xsl:value-of select="$PRANA_Symbol_NAME"/>
							</xsl:when>
							<xsl:when test="$varAssetType = '2'">
								<xsl:value-of select="concat($varOSISymbol,'U')"/>
							</xsl:when>
							<xsl:when test="$varCurrency != 'USD'">
								<xsl:value-of select="$varSEDOL"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="$varEquitySymbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$varPBSymbol"/>
					</xsl:otherwise>
				</xsl:choose>
            </PBSymbol>

            <CUSIP>
              <xsl:value-of select="$varCUSIP"/>
            </CUSIP>

            <RIC>
              <xsl:value-of select="$varRIC"/>
            </RIC>

            <Bloomberg>
              <xsl:value-of select="$varBloomberg"/>
            </Bloomberg>

            <Description>
              <xsl:value-of select="$varDescription"/>
            </Description>

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
				  <xsl:when test ="$varAssetType = '2'">
					  <xsl:choose>
						  <xsl:when test="$varSide = 'B' and ($varBuySellOpt = 'O' ) ">
							  <xsl:value-of select="'A'"/>
						  </xsl:when>
						  <xsl:when test="$varSide = 'S' and ($varBuySellOpt = 'O')">
							  <xsl:value-of select="'C'"/>
						  </xsl:when>
						  <xsl:when test="$varSide = 'B' and  $varBuySellOpt = 'C'">
							  <xsl:value-of select="'B'"/>
						  </xsl:when>
						  <xsl:when test="$varSide = 'S' and $varBuySellOpt = 'C'">
							  <xsl:value-of select="'D'"/>
						  </xsl:when>
					  </xsl:choose>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:choose>
						  <xsl:when test="$varSide = 'B' and ($varBuySell = '1' or $varBuySell = '2') ">
							  <xsl:value-of select="'1'"/>
						  </xsl:when>
						  <xsl:when test="$varSide = 'S' and ($varBuySell = '1' or $varBuySell = '2')">
							  <xsl:value-of select="'2'"/>
						  </xsl:when>
						  <xsl:when test="$varSide = 'B' and  $varBuySell = '5'">
							  <xsl:value-of select="'B'"/>
						  </xsl:when>
						  <xsl:when test="$varSide = 'S' and $varBuySell = '5'">
							  <xsl:value-of select="'5'"/>
						  </xsl:when>
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

            <!--<SecFees>
              <xsl:choose>
                <xsl:when test="$varSecFees &gt; 0">
                  <xsl:value-of select="$varSecFees"/>
                </xsl:when>
                <xsl:when test="$varSecFees &lt; 0">
                  <xsl:value-of select="$varSecFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFees>-->

			  <StampDuty>
				  <xsl:choose>
					  <xsl:when test="$varMiscFee &gt; 0">
						  <xsl:value-of select="$varMiscFee"/>
					  </xsl:when>
					  <xsl:when test="$varMiscFee &lt; 0">
						  <xsl:value-of select="$varMiscFee*(-1)"/>
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
