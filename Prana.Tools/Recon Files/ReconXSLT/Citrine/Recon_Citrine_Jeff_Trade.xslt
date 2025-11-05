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

	<msxsl:script language="C#" implements-prefix="my">
		public string Now1(int year, int month)
		{
		DateTime firstWednesday= new DateTime(year, month, 1);
		while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
		{
		firstWednesday = firstWednesday.AddDays(1);
		}
		return firstWednesday.ToString();
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

  <xsl:template name="GetUnderlying">
    <xsl:param name="Description"/>
    <xsl:choose>
		<xsl:when test="$Description='COMPASS SAV, LLC' or $Description='COMPASS OFFSHORE SAV PCC LIMITED' or $Description='CITRINE CAPITAL LTD'">
			<xsl:value-of select="'ES'"/>
		</xsl:when>
      <xsl:when test="$Description = 'PALLADIUM' or $Description = 'NYM PALLADIUM'">
        <xsl:value-of select="'PA'"/>
      </xsl:when>
      <xsl:when test="$Description = 'PLATIMUN'">
        <xsl:value-of select="'PL'"/>
      </xsl:when>
		<xsl:when test="$Description = 'NYM PLATINUM'">
			<xsl:value-of select="'PL'"/>
		</xsl:when>
      <xsl:when test="$Description = 'GOLD' or $Description ='GOLD COMX' or $Description ='CMX GOLD'">
        <xsl:value-of select="'GC'"/>
      </xsl:when>
      <xsl:when test="$Description = 'COPPER'">
        <xsl:value-of select="'HG'"/>
      </xsl:when>
      <xsl:when test="$Description = 'SILVER'">
        <xsl:value-of select="'SI'"/>
      </xsl:when>
		
      <xsl:when test="contains($Description, 'MINI') != false">
        <xsl:value-of select="'ES'"/>
      </xsl:when>
		
		<xsl:when test="$Description = 'LME ALUM US'">
			<xsl:value-of select="'AHD'"/>
		</xsl:when>
		<xsl:when test="$Description = 'LME COPPER US'">
			<xsl:value-of select="'CAD'"/>
		</xsl:when>
		<xsl:when test="$Description = 'LME ZINC US'">
			<xsl:value-of select="'ZSD'"/>
		</xsl:when>
		<xsl:when test="$Description = 'LME NICKEL US'">
			<xsl:value-of select="'NID'"/>
		</xsl:when>
		<xsl:when test="$Description = 'LME LEAD US'">
			<xsl:value-of select="'PBD'"/>
		</xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='01' ">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='02' ">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='03'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='04' ">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='05' ">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='06'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='07' ">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='08' ">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='09' ">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' ">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12'">
        <xsl:value-of select ="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:if test="number(COL5)">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'JPM'"/>
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
              <xsl:value-of select="COL2"/>
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
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varExchange">
              <xsl:value-of select="COL8"/>
            </xsl:variable>


            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL22"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varPositionSettlementDate">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varExpiry">
              <xsl:value-of select="COL11"/>
            </xsl:variable>



            <!--<xsl:variable name="varCurrency">
              <xsl:value-of select="COL2"/>
            </xsl:variable>-->

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL23"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="normalize-space(COL23)"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL24"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

			  <xsl:variable name ="varBBCode">
				  <xsl:value-of select ="substring(COL14,1,2)"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_Multiplier">
				  <xsl:value-of select="document('../ReconMappingXml/PriceMulMapping.xml')/PriceMulMapping/PB[@Name='JPM']/MultiplierData[@PranaRoot=$varBBCode]/@Multiplier"/>
			  </xsl:variable>

			  <xsl:variable name="varAvgPX">
				  <xsl:choose>
					  <xsl:when test ="normalize-space($PRANA_Multiplier) != ''">
						  <xsl:value-of select ="COL22*$PRANA_Multiplier"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="COL22"/>
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
				  <xsl:choose>
					  <xsl:when test="number(COL19)">
						  <xsl:value-of select="'FutureOption'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Future'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			 

			  

            <xsl:variable name="varCommission">
              <xsl:value-of select="COL33"/>
            </xsl:variable>

            <xsl:variable name="varFees">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varMiscFee">
              <xsl:value-of select="COL35"/>
            </xsl:variable>


            <xsl:variable name="varPutCall">
              <xsl:value-of select="substring(COL23,1,1)"/>
            </xsl:variable>

            <xsl:variable name="varSideFlag">
              <xsl:value-of select="COL18"/>
            </xsl:variable>

            <xsl:variable name="varNetNotionalValue">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="varNetNotionalValueLocal">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varSMRequest">
              <xsl:value-of select="'TRUE'"/>
            </xsl:variable>

            <xsl:variable name="varGrossNotionalValueBase">
              <xsl:value-of select="COL49"/>
            </xsl:variable>

            <xsl:variable name="varNetNotionalValueBase">
              <xsl:value-of select="COL34"/>
            </xsl:variable>

            <xsl:variable name="varExpiryDay">
				<xsl:choose>
					<xsl:when test ="string-length(COL12) = 1">
						<xsl:value-of select ="concat('0',COL12)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="COL12"/>
					</xsl:otherwise>
				</xsl:choose>
            </xsl:variable>

            <xsl:variable name="varSymbol">
				<xsl:call-template name ="GetUnderlying">
					<xsl:with-param name ="Description" select ="normalize-space(COL16)"/>
				</xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="varCallPut">
              <xsl:value-of select="COL21"/>
            </xsl:variable>

            <xsl:variable name="varStrike">
              <xsl:value-of select="format-number(COL19,'#')"/>
            </xsl:variable>

            <xsl:variable name="varUnderlying">
              <xsl:call-template name="GetUnderlying">
				  <xsl:with-param name ="Description" select ="normalize-space(COL17)"/>
                <!--<xsl:with-param name="Description" select="translate(normalize-space(COL17),$varSmall,$varCapital)"/>-->
              </xsl:call-template>
            </xsl:variable>
            
            <xsl:variable name="varMonthCode">
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="substring($varExpiry,5,2)"/>
              </xsl:call-template>
            </xsl:variable>

			  <xsl:variable name="varExpiryDate">
				  <xsl:value-of select ="my:Now1(number(substring(COL11,1,4)),number(substring(COL11,5,2)))"/>
			  </xsl:variable>

			  <xsl:variable name="varOptExDay">
				  <xsl:value-of select="substring-before(substring-after(normalize-space($varExpiryDate),'/'),'/')"/>
			  </xsl:variable>

			  <xsl:variable name="varOptExDayMod">
				  <xsl:choose>
					  <xsl:when test="string-length($varOptExDay) = 1">
						  <xsl:value-of select="concat('0',$varOptExDay)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$varOptExDay"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_Symbol_NAME != ''">
                  <xsl:value-of select="$PRANA_Symbol_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varAssetType = 'Future' and $varExchange = 'LME'">
                      <xsl:value-of select="concat($varSymbol,' ',substring($varExpiry,4,1),$varMonthCode,$varExpiryDay,'-LME')"/>
                    </xsl:when>
                    <xsl:when test="$varAssetType = 'FutureOption' and $varExchange = 'LME'">
                      <xsl:value-of select="concat($varSymbol,' ',substring($varExpiry,4,1),$varMonthCode,$varOptExDayMod, $varCallPut, $varStrike, '-LME')"/>
                    </xsl:when>
                    <xsl:when test="$varAssetType = 'Future' and $varExchange != 'LME'">
                      <xsl:value-of select="concat(substring($varFutureSymbol,1,2),' ',substring($varFutureSymbol,3,2))"/>
                    </xsl:when>
                    <xsl:when test="$varAssetType = 'FutureOption' and $varExchange != 'LME'">
                      <xsl:value-of select="concat($varUnderlying,' ',$varMonthCode,substring($varExpiry,4,1),$varExpiryDay, $varCallPut, $varStrike)"/>
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


            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

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

			  <Side>
				  <xsl:choose>
					  <xsl:when test="number($varNetPosition) &gt; 0">
						  <xsl:value-of select="'Buy'"/>
					  </xsl:when>
					  <xsl:when test="number($varNetPosition) &lt; 0">
						  <xsl:value-of select="'Sell'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Side>

			  <ExpirationDate>
				  <xsl:value-of select="COL9"/>
			  </ExpirationDate>

            <!--<Side>
              <xsl:choose>
                <xsl:when test="$varSideFlag = '5-SHORT'">
                  <xsl:choose>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'Buy to Close'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'Buy to Close'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'EQUITIES'">
                      <xsl:value-of select="'Sell'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'BUY' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>
                    <xsl:when test="$varSide = 'SELL' and $varAssetType = 'OPTIONS'">
                      <xsl:value-of select="'Sell to Close'"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Side>-->

            <!--<Commission>
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

            <MiscFees>
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
            </MiscFees>


            <TradeDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </TradeDate>

            <SettlementDate>
              <xsl:value-of select="$varPositionSettlementDate"/>
            </SettlementDate>


            <GrossNotionalValueBase>
              <xsl:choose>
                <xsl:when test="number($varGrossNotionalValueBase)">
                  <xsl:value-of select="$varGrossNotionalValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </GrossNotionalValueBase>

            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="number($varNetNotionalValueBase)">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>

            <SMRequest>
              <xsl:value-of select="$varSMRequest"/>
            </SMRequest>-->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  
</xsl:stylesheet>
