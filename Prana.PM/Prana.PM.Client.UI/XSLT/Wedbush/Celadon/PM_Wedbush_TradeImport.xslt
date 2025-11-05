<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<!--Third Friday check-->
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
			<xsl:choose>
				<xsl:when test="contains(COL5,'20')">
					<xsl:value-of select="substring-before($varBBCode,'20')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before($varBBCode,'1')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!--<xsl:variable name="varRoot">
			<xsl:value-of select="substring-before($varBBCode,'1')"/>
		</xsl:variable>-->

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
			<xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
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
      <xsl:for-each select="//PositionMaster">


        <!--<xsl:if test="number(COL4) and  (COL2='59730260' or COL2='59730261'or COL2='59730251' or COL2='59730262'or COL2='59730263' or COL2='71240527' or COL2='71240528'or COL2='71240529' or COL2='71240540' or COL2='51394556' or COL2='31405346')">-->
			<xsl:if test="number(COL4)">
		<!--<xsl:if test="number(COL4) and (COL23='0') and (COL2='23484257' or COL2='69191389'or COL2='14003395' or COL2='10111644'or COL2='59792710'or COL2='71240526' or COL2='71240527' or COL2='71240528'or COL2='71240529' or COL2='71240540' or COL2='51394556' or COL2='31405346')">-->
			<!--<xsl:if test="number(COL4) and (COL23='0') and (COL2='14003395' )">-->
		
          <PositionMaster>



			  <xsl:variable name = "PB_COMPANY" >
				  <xsl:value-of select="COL18"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Wedbush']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			  </xsl:variable>

			  <!--<xsl:variable name = "PB_NAME">
				  <xsl:value-of select="'WedbushS'"/>
			  </xsl:variable>

			  <xsl:variable name = "PB_SYMBOL_NAME" >
				  <xsl:value-of select ="COL18"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>-->
			  
			  
			  <xsl:variable name = "PB_FUND_NAME" >
					  <xsl:value-of select="COL2"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

			  <FundName>
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select="$PRANA_FUND_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </FundName>
			  
			  
			  <xsl:variable name ="varCallPut">
				  <xsl:choose>
					  <xsl:when test="COL19 = '*' and COL5 != ''">
						  <xsl:value-of select="1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


				  <xsl:variable name="varUnderlying">
					  <xsl:choose>
						  <xsl:when test="COL5 != '' and COL5 != '*' and $varCallPut = 1">
							  <xsl:value-of select="substring-before(COL5,'1')"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select="normalize-space(COL5)"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:variable>

				  <xsl:variable name="varUnderlyingLength">
					  <xsl:value-of select="string-length($varUnderlying)"/>
				  </xsl:variable>

				  <xsl:variable name="varMonthCode">
					  <xsl:if test ="$varCallPut = 1">
						  <xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
					  </xsl:if>
				  </xsl:variable>


			  <!-- <xsl:variable name="varExpirationYear">
					  <xsl:if test="$varCallPut = 1">
						  <xsl:value-of select="substring(COL5,($varUnderlyingLength + 1),2)"/>
					  </xsl:if>
				  </xsl:variable>

				  <xsl:variable name="varStrikePriceString">
					  <xsl:value-of select="substring-after(substring-after(COL5,$varUnderlying),$varMonthCode)"/>
				  </xsl:variable>

				  <xsl:variable name ="varStrikePrice">
					  <xsl:if test="$varCallPut = 1 and number($varStrikePriceString)">
						  <xsl:value-of select="format-number($varStrikePriceString,'#.00')"/>
					  </xsl:if>
				  </xsl:variable>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$varCallPut = 1">
						  <xsl:value-of select="concat('O:',$varUnderlying,' ',$varExpirationYear,$varMonthCode,$varStrikePrice)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$varUnderlying"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>-->

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME!=''">
						  <xsl:choose>
							  <xsl:when test ="$PRANA_SYMBOL != ''">
								  <xsl:value-of select ="$PRANA_SYMBOL"/>
							  </xsl:when>
							  <xsl:when test="$varCallPut = 1">
								  <xsl:call-template name="ConvertBBCodetoTicker">
									  <xsl:with-param name="varBBCode" select="COL5"/>
								  </xsl:call-template>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="$varUnderlying"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
				  
			  </Symbol>


            <PBSymbol>
              <xsl:if test="COL5 != '' and COL5 != '*'">
                <xsl:value-of select="COL5"/>
              </xsl:if>
            </PBSymbol>


            <CostBasis>
              <xsl:choose>
                <xsl:when  test="number(COL6)">
                  <xsl:value-of select="COL6"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </CostBasis>



            <PositionStartDate>
                <xsl:value-of select="COL28"/>
            </PositionStartDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when  test="number(normalize-space(COL4)) and COL4 &gt; 0">
                  <xsl:value-of select="COL4"/>
                </xsl:when>
                <xsl:when test="number(normalize-space(COL4)) and COL4 &lt; 0">
                  <xsl:value-of select="COL4 * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>


            <SideTagValue>
              <xsl:choose>
                <xsl:when test="COL3 != '' and COL3 = 'P'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="COL3 != '' and COL3 = 'S'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>

				  <xsl:when test="COL3 != '' and COL3 = 'SS'">
					  <xsl:value-of select="'5'"/>
				  </xsl:when>

                <xsl:when test="COL3 != '' and COL2 = 'ASG'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>

                <xsl:when test="COL3 != '' and COL2 = 'EXP'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>

                <!--<xsl:when test="COL2 = '' and COL16 != ''">
                  <xsl:value-of select="'A'"/>
                </xsl:when>-->

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>                
              </xsl:choose>
            </SideTagValue>

			  <!--<xsl:variable name ="varQty">
				  <xsl:choose>
					  <xsl:when test ="number(COL5) and COL5 &gt; 0">
						  <xsl:value-of select ="COL5"/>
					  </xsl:when>
					  <xsl:when test ="number(COL5) and COL5 &lt; 0">
						  <xsl:value-of select ="COL5 * -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name ="varNetNotional">
				  <xsl:choose>
					  <xsl:when test ="number(COL12) and COL12 &gt; 0">
						  <xsl:value-of select ="COL12"/>
					  </xsl:when>
					  <xsl:when test ="number(COL12) and COL12 &lt; 0">
						  <xsl:value-of select ="COL12 * -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>-->

			  <xsl:variable name ="varCommission">
				  <xsl:choose>
					  <xsl:when test ="number(COL15) ">
						  <xsl:value-of select ="COL15"/>
					  </xsl:when>
					 
					  
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name ="varFees">
				  <xsl:choose>
					  <xsl:when test ="number(COL12) ">
						  <xsl:value-of select ="COL12"/>
					  </xsl:when>


					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <Commission>
				  <xsl:choose>
					  <xsl:when test ="number($varCommission)">
						  <xsl:value-of select ="$varCommission"/>
					  </xsl:when>
					 
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>

				  <!--<xsl:choose>
					  <xsl:when test ="number($varCommission) and $varCommission &gt; 0">
						  <xsl:value-of select ="$varCommission"/>
					  </xsl:when>
					  <xsl:when test ="number($varCommission) and $varCommission &lt; 0">
						  <xsl:value-of select ="$varCommission * -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>-->
			  </Commission>

			  <Fees>
				  <xsl:choose>
					  <xsl:when test ="number($varFees) and $varFees &gt; 0">
						  <xsl:value-of select ="$varFees"/>
					  </xsl:when>
					  <xsl:when test ="number($varFees) and $varFees &lt; 0">
						  <xsl:value-of select ="$varFees * -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Fees>
			  <!--<CounterPartyID>
				  <xsl:value-of select="50"/>
				 
			  </CounterPartyID>-->


			  <!--<CounterPartyID>

				  <xsl:choose>
					  <xsl:when test ="COL2 = '31405346'">
						<xsl:value-of select ="69"/>
					  </xsl:when>

					  <xsl:when test ="COL2 = '59792710'">
						  <xsl:value-of select ="122"/>
					  </xsl:when>
					 <xsl:otherwise>
						  <xsl:value-of select="25"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CounterPartyID>-->

			  <OriginalPurchaseDate>
				  <xsl:value-of select="COL28"/>
			  </OriginalPurchaseDate>
			  
			  
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


