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

	<xsl:template name="MonthNo">
		<xsl:param name="varMonth"/>

		<xsl:choose>
			<xsl:when test ="$varMonth= 'A' or $varMonth= 'M'">
				<xsl:value-of select ="1"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'B' or $varMonth= 'N'">
				<xsl:value-of select ="2"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'C' or $varMonth= 'O'">
				<xsl:value-of select ="3"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'D' or $varMonth= 'P'">
				<xsl:value-of select ="4"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'E' or $varMonth= 'Q'">
				<xsl:value-of select ="5"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'F' or $varMonth= 'R'">
				<xsl:value-of select ="6"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'G' or $varMonth= 'S'">
				<xsl:value-of select ="7"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'H' or $varMonth= 'T'">
				<xsl:value-of select ="8"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'I' or $varMonth= 'U'">
				<xsl:value-of select ="9"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'J' or $varMonth= 'V'">
				<xsl:value-of select ="10"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'K' or $varMonth= 'W'">
				<xsl:value-of select ="11"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'L' or $varMonth= 'X'">
				<xsl:value-of select ="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>

		<xsl:variable name="varRoot">
			<xsl:value-of select="substring-before($varBBCode,'1')"/>
		</xsl:variable>

		<xsl:variable name="varUnderlyingLength">
			<xsl:value-of select="string-length($varRoot)"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">
			<xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +6)), '#.00')"/>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +3),2)"/>
		</xsl:variable>

		<xsl:variable name="varExpiryDay">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)= '0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring(COL3,($varUnderlyingLength + 5),1)"/>
		</xsl:variable>

		<xsl:variable name="varExMonth">
			<xsl:call-template name="MonthNo">
				<xsl:with-param name="varMonth" select="$varMonthCode"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name='varThirdFriday'>
			<xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:if test="normalize-space(COL2)!='Cusip'">
          <PositionMaster>

            <PositionStartDate>
              <xsl:value-of select="COL10"/>
            </PositionStartDate>

            <PBSymbol>
              <xsl:value-of select="COL3"/>
            </PBSymbol>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <xsl:variable name = "PB_COMPANY" >
				  <xsl:value-of select="COL3"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Newland_GS']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			  </xsl:variable>


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

						<!--<AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>-->

            <xsl:variable name ="varCallPut">
              <xsl:choose>
                <xsl:when test="substring-before(COL6, ' ')= 'CALL' or substring-before(COL6, ' ')= 'PUT'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="varUnderlying">
              <xsl:choose>
                <xsl:when test="COL3 != '' and COL3 != '' and $varCallPut = 1">
                  <xsl:value-of select="substring-before(COL3,'1')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL3)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varUnderlyingLength">
              <xsl:value-of select="string-length($varUnderlying)"/>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:if test ="$varCallPut = 1">
                <xsl:value-of select="substring(COL3,($varUnderlyingLength + 5),1)"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="varDateNo">
              <xsl:if test="$varCallPut = 1">
                <xsl:value-of select="substring(COL3,($varUnderlyingLength + 3),2)"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="varExpirationYear">
              <xsl:if test="$varCallPut = 1">
                <xsl:value-of select="substring(COL3,($varUnderlyingLength + 1),2)"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="varStrikePriceString">
              <xsl:value-of select="substring-after(substring-after(COL3,$varUnderlying),$varMonthCode)"/>
            </xsl:variable>


            <xsl:variable name ="varStrikePrice">
              <xsl:if test="$varCallPut = 1 and number($varStrikePriceString)">
                <xsl:value-of select="format-number($varStrikePriceString,'#.00')"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="varMonthNo">
              <xsl:call-template name="MonthNo">
                <xsl:with-param name="varMonth" select="$varMonthCode"/>
              </xsl:call-template> 
            </xsl:variable>

            <!--<xsl:variable name="varThirdFriday">
              <xsl:value-of select =" my:Now($varExpirationYear,$varMonthNo)"/>
            </xsl:variable>

            <xsl:variable name="varIsFlex">
              <xsl:choose>
                <xsl:when test="substring-before(substring-after($varThirdFriday,'/'),'/')=(($varDateNo)-1)">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->
            <xsl:variable name="Date">
              <xsl:choose>
                <xsl:when test="substring($varDateNo,1,1)='0'">
                  <xsl:value-of select="substring($varDateNo,2,1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varDateNo"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_SYMBOL != ''">
						  <xsl:value-of select ="$PRANA_SYMBOL"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$varCallPut = 1">
								  <xsl:call-template name="ConvertBBCodetoTicker">
									  <xsl:with-param name="varBBCode" select="COL3"/>
								  </xsl:call-template>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="$varUnderlying"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

			  <MarkPrice>

				  <xsl:value-of select="COL5"/>

			  </MarkPrice>

			  <Quantity>
				  <xsl:choose>

					  <xsl:when test="COL4 &gt; 0">
						  <xsl:value-of select="COL4"/>
					  </xsl:when>
					  <xsl:when test="COL4 &lt; 0">
					<xsl:value-of select="COL4"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
               </xsl:choose>
            </Quantity>

			  
            <Side>
              <xsl:choose>
                <xsl:when test="COL4 &gt; 0">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="COL4 &lt; 0">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>