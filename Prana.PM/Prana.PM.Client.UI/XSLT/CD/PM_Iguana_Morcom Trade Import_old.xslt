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
      <xsl:when test="$Suffix = 'T'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'OS'">
        <xsl:value-of select="'-OSE'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="($varMonth='01' or $varMonth='1') and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='02' or $varMonth='2') and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='03' or $varMonth='3') and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='04' or $varMonth='4') and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='05' or $varMonth='5') and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='06' or $varMonth='6') and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='07' or $varMonth='7') and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='08' or $varMonth='8') and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='09' or $varMonth='9') and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='01' or $varMonth='1') and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='02' or $varMonth='2') and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='03' or $varMonth='3') and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='04' or $varMonth='4') and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='05' or $varMonth='5') and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='06' or $varMonth='6') and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='07' or $varMonth='7') and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='08' or $varMonth='8') and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='09' or $varMonth='9') and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL25) and COL4= 'TRADE' ">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'Lazard'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL23"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL6"/>
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
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>


            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL22"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varPositionSettlementDate">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="COL37"/>
            </xsl:variable>

            <xsl:variable name="varExYear">
              <xsl:value-of select="substring-after(substring-after($varOptionExpiry,'/'),'/')"/>
            </xsl:variable>

            <!--<xsl:variable name="varCurrency">
              <xsl:value-of select="COL2"/>
            </xsl:variable>-->

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL23"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="COL23"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL24"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL25"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varUnderlying">
              <xsl:value-of select="substring-before(COL21,' ')"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="COL20"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL33"/>
            </xsl:variable>

            <xsl:variable name="varFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varMiscFee">
              <xsl:value-of select="COL54"/>
            </xsl:variable>

			  <xsl:variable name ="varStampDuty">
				  <xsl:value-of select ="COL35"/>
			  </xsl:variable>


            <xsl:variable name="varPutCall">
              <xsl:value-of select="substring(COL23,1,1)"/>
            </xsl:variable>

            <xsl:variable name="varStrike">
              <xsl:value-of select="COL29"/>
            </xsl:variable>

            <xsl:variable name="varSideFlag">
              <xsl:value-of select="COL18"/>
            </xsl:variable>


            <Symbol>
                  <xsl:choose>
                    <xsl:when test="$PRANA_Symbol_NAME != ''">
                      <xsl:value-of select="$PRANA_Symbol_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
                        <xsl:when test="$varAssetType = 'EQUITIES'">
                          <xsl:value-of select="$varEquitySymbol"/>
                        </xsl:when>
                        <xsl:when test="$varAssetType = 'OPTIONS'">
                          <xsl:variable name="MonthCode">
                            <xsl:call-template name="MonthCode">
                              <xsl:with-param name="varMonth" select="substring-before($varOptionExpiry,'/')"/>
                              <xsl:with-param name="varPutCall" select="$varPutCall"/>
                            </xsl:call-template>
                          </xsl:variable>
                          <xsl:variable name="varThirdFriday">
                            <xsl:value-of select =" my:Now(number($varExYear),number(substring-before($varOptionExpiry,'/')))"/>
                          </xsl:variable>

                          <xsl:variable name="varIsFlex">
                            <xsl:choose>
                              <xsl:when test="(substring-before(substring-after($varThirdFriday,'/'),'/') + 1) = substring-before(substring-after($varOptionExpiry,'/'),'/')">
                                <xsl:value-of select="0"/>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="1"/>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:variable>

                          <xsl:variable name="varDate">
                            <xsl:choose>
                              <xsl:when test="string-length(substring-before(substring-after($varOptionExpiry,'/'),'/'))=0">
                                <xsl:value-of select="concat('0',substring-before(substring-after($varOptionExpiry,'/'),'/'))"/>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="substring-before(substring-after($varOptionExpiry,'/'),'/')"/>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:variable>
                          <xsl:choose>
                            <xsl:when test="$varIsFlex = 0">
                              <xsl:value-of select="concat('O:',$varUnderlying,' ',substring($varExYear,3,2),$MonthCode,format-number($varStrike,'#.00'))"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="concat('O:',$varUnderlying,' ',substring($varExYear,3,2),$MonthCode,format-number($varStrike,'#.00'),'D',$varDate)"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="''"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </Symbol>
             
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

            <PositionSettlementDate>
              <xsl:value-of select="$varPositionSettlementDate"/>
            </PositionSettlementDate>



            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

            <!--<RIC>
              <xsl:value-of select="$varRIC"/>
            </RIC>

            <Bloomberg>
              <xsl:value-of select="$varBloomberg"/>
            </Bloomberg>-->

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
                <xsl:when test="$varSideFlag = '5-SHORT'">
                  <xsl:choose>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'D'"/>
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

            <Description>
              <xsl:value-of select="$varDescription"/>
            </Description>

			  <MiscFees>
				  <xsl:choose>
					  <xsl:when test ="boolean(number($varMiscFee))">
						  <xsl:value-of select="$varMiscFee"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose> 
			  </MiscFees>

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

            <Fees>
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

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
