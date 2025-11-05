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
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
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
		<xsl:if test="substring-before(COL6,' ')='C' or substring-before(COL6,' ')='P'">
			<xsl:variable name="UnderlyingSymbol">			
					
						<xsl:value-of select="normalize-space(COL19)"/>			
							
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL17,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">

				<xsl:value-of select="substring-before(substring-after(substring-after(COL6,' '),' '),' ')"/>

			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(COL17,'/'),'/'),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring-before(COL6,' ')"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">

				<xsl:value-of select="format-number(COL18,'0.00')"/>
				<!--<xsl:value-of select="COL18"/>-->

				<!--<xsl:value-of select="format-number(COL18 div 1000 ,'#.00')"/>-->

			</xsl:variable>


			<xsl:variable name="MonthCodVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
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

				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>-->
			<!--

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>

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
  
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
		  
		  <xsl:variable name="Position">
			  <xsl:call-template name="Translate">
				  <xsl:with-param name="Number" select="COL4"/>
			  </xsl:call-template>
		  </xsl:variable>

		  <xsl:if test="number($Position) ">
          <PositionMaster>

            <PositionStartDate>
              <xsl:value-of select="COL10"/>
            </PositionStartDate>

            <PBSymbol>
              <xsl:value-of select="COL3"/>
            </PBSymbol>

          

			  <!--<xsl:variable name = "PB_COMPANY" >
				  <xsl:value-of select="COL3"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='WedbushS']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			  </xsl:variable>-->

			  <xsl:variable name="PB_NAME">
				  <xsl:value-of select="'Wedbush Securities'"/>
			  </xsl:variable>

			  <xsl:variable name = "PB_SYMBOL_NAME" >
				  <xsl:value-of select ="concat(COL6,' ',COL2)"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>


			  <xsl:variable name="PB_FUND_NAME" select="COL1"/>
			  <xsl:variable name ="PRANA_FUND_NAME">
				  <xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>
			  <FundName>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select ="$PRANA_FUND_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_FUND_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </FundName>

						<!--<FundName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>-->

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
					  <xsl:when test="COL3 != '' and COL3 != '*' and $varCallPut = 1">
						  <xsl:value-of select="substring-before(COL3,'1')"/>
					  </xsl:when>
					  <xsl:when test="COL3 = '' or COL3 = '*'">
						  <xsl:value-of select="normalize-space(COL2)"/>
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

			  <!--<xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="string-length(COL3 &gt; 7) and  (substring-before(COL6,' ')='C' or substring-before(COL6,' ')='P')">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>
					  
				  </xsl:choose>
			  </xsl:variable>-->

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="contains(COL16,'C') or contains(COL16,'P') ">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>
				  </xsl:choose>
			  </xsl:variable>


			  <xsl:variable name="Symbol">
				  <xsl:value-of select="normalize-space(COL3)"/>
			  </xsl:variable>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>

					  <xsl:when test="$Asset='EquityOption'">
						  <xsl:call-template name="Option">
							  <xsl:with-param name="Symbol" select="normalize-space(COL19)"/>
							  <xsl:with-param name="Suffix" select="''"/>
						  </xsl:call-template>
					  </xsl:when>

					  <xsl:when test="$Symbol!=''">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="normalize-space(COL2)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

			  <xsl:variable name="PB_Underlying" select="concat(COL6,' ',COL2)"/>

			  <xsl:variable name="PRANA_Multiplier">
				  <xsl:value-of select ="document('../../ReconMappingXML/MultiplierMapping.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_SYMBOL_NAME]/@PBMultiplier"/>
			  </xsl:variable>

			  <xsl:variable name="Price">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL5"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <xsl:variable name="CostBasis">
				  <xsl:choose>
					  <xsl:when test="$PRANA_Multiplier!=''">
						  <xsl:value-of select="$Price * $PRANA_Multiplier"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$Price"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <MarkPrice>

				  <xsl:choose>
					  <xsl:when test="$CostBasis &gt; 0">
						  <xsl:value-of select="$CostBasis"/>
					  </xsl:when>
					  <xsl:when test="$CostBasis &lt; 0">
						  <xsl:value-of select="($CostBasis * -1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>

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

			  <xsl:variable name="MarketValue">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL20"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <xsl:variable name="varMarketValue">
				  <xsl:choose>
					  <xsl:when test="$Asset='EquityOption' and number($MarketValue)">
						  <xsl:value-of select="$MarketValue * 100"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$PRANA_Multiplier!=''">
								  <xsl:value-of select="$MarketValue * $PRANA_Multiplier"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="$MarketValue"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>			  
				  
			  </xsl:variable>			  
			  <MarketValue>
				  <xsl:choose>
					  <xsl:when test="number($varMarketValue)">
						  <xsl:value-of select="$varMarketValue"/>
					  </xsl:when>
					  
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
				  
				  <!--<xsl:choose>
					  <xsl:when test="$Asset='EquityOption' and number($MarketValue)">
						  <xsl:value-of select="$MarketValue * 100"/>
					  </xsl:when>
					  <xsl:when test="number($MarketValue)">
						  <xsl:value-of select="$MarketValue"/>
					  </xsl:when>
					 
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>-->
			  </MarketValue>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>