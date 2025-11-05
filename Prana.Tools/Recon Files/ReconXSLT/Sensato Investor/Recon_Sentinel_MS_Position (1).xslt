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

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(COL6,'PUT') or contains(COL6,'CALL')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,'1')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8,6),'#.00')"/>
      </xsl:variable>

      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>
      <!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>

				<xsl:otherwise>-->
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
      <!--</xsl:otherwise>-->
      <!--

			</xsl:choose>-->
    </xsl:if>
  </xsl:template>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'FP'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'NA'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'BZ'">
        <xsl:value-of select="'-BSP'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CN'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'LN'">
        <xsl:value-of select="'-LON'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'GR'">
        <xsl:value-of select="'-FRA'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'SM'">
        <xsl:value-of select="'-MAC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varPBName">
          <xsl:value-of select="'MS'"/>
        </xsl:variable>

        <xsl:if test ="number(COL27) and contains(COL50,'CASH')!='true' or contains(COL51,'MONEY')='true'">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name= $varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select ="COL14"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= $varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="PB_CountnerParty" select="COL5"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>


            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL27"/>
            </xsl:variable>

            <!--<xsl:variable name="varCostBasis">
              <xsl:value-of select="COL30"/>
            </xsl:variable>-->

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varMiscFees">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varClearingFee">
              <xsl:value-of select="COL22"/>
            </xsl:variable>

            <xsl:variable name="varStampDuty">
              <xsl:value-of select="COL23"/>
            </xsl:variable>

            <!--<xsl:variable name="varMarketValue">
				<xsl:value-of select="COL32"/>
			</xsl:variable>-->

            <xsl:variable name="varMarketValue">
              <xsl:choose>
                <xsl:when test="COL51 ='EQUITY SWAP'">
                  <xsl:value-of select="COL32"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL34"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:variable>

            <xsl:variable name="varMarketValueBase">
              <xsl:choose>
                <xsl:when test="COL51 ='EQUITY SWAP'">
                  <xsl:value-of select="COL33"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL35"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varSuffix">
              <xsl:call-template name="GetSuffix">
                <xsl:with-param name="Suffix" select="substring-after(COL14, ' ')"/>
              </xsl:call-template>
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


            <AssetCategory>
              <xsl:choose>

                <xsl:when test='substring-before(COL51, " ") = "Call" or substring-before(COL51, " ") = "Put"'>
                  <xsl:value-of select='Option'/>
                </xsl:when>
                <xsl:when test="COL51 ='EQUITY SWAP'">
                  <xsl:value-of select="'EquitySwap'"/>
                </xsl:when>
                <xsl:when test="COL51 ='Convertible Bond'">
                  <xsl:value-of select="'FixedIncome'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </AssetCategory>

            <Symbol>
              <xsl:choose>
                <xsl:when test='$PRANA_SYMBOL_NAME != ""'>
                  <xsl:value-of select='$PRANA_SYMBOL_NAME'/>
                </xsl:when>

                <xsl:when test="contains(COL6,'PUT') or contains(COL6,'CALL')">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL8)"/>
                  </xsl:call-template>

                </xsl:when>

                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test='$varSuffix = "" and substring-after(COL14, " ") != "US"'>
                      <xsl:value-of select='COL14'/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select='concat(substring-before(COL14, " "), $varSuffix)'/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <!--Side-->

            <Side>
              <xsl:choose>
					<xsl:when test="contains(COL6,'CALL') or contains(COL6,'PUT')">
                  <xsl:choose>
                    <xsl:when test="COL29 = 'L'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>
                    <xsl:when test="COL29 = 'S'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
				  
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="COL29 = 'L'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="COL29 = 'S'">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <!--QUANTITY-->

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($varNetPosition)">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>


            <MarkPrice>
              <xsl:value-of select ="COL30"/>
            </MarkPrice>

            <CurrencySymbol>
              <xsl:value-of select ="COL44"/>
            </CurrencySymbol>

            <MarketValue>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarketValue))">
                  <xsl:value-of select="$varMarketValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarketValueBase))">
                  <xsl:value-of select="$varMarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>


            <!--<CUSIP>
             <xsl:value-of select="normalize-space(COL7)"/>
           </CUSIP>-->

            <SEDOL>
              <xsl:value-of select="normalize-space(COL9)"/>
            </SEDOL>





            <xsl:if test="COL51 ='EQUITY SWAP'">

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
                <xsl:value-of select="COL53"/>
              </OrigTransDate>

              <xsl:variable name="varPreviousMonth">
                <xsl:value-of select="substring-before(COL53,'/')"/>
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
                <xsl:value-of select="substring-after(substring-after(COL53,'/'),'/')"/>
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

              <SMRequest>
                <xsl:value-of select ="'true'"/>
              </SMRequest>

            </xsl:if>


          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
