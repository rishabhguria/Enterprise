<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public static string NowSwap(int year, int month, int date)
		{
		DateTime weekEnd= new DateTime(year, month, date);
		weekEnd = weekEnd.AddDays(1);
		while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
		{
		weekEnd = weekEnd.AddDays(1);
		}
		return weekEnd.ToString();
		}

	</msxsl:script>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
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

		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space($Symbol),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),'/'),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space($Symbol),' '),' '),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),'/'),'/'),' ')"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),' '),1,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(normalize-space($Symbol),' '),' '),' '),' '),2),'##.00')"/>
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

		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>

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

	<xsl:template name="FormatDate">
		<xsl:param name="DateTime" />
		<!-- converts date time double number to 18/12/2009 -->

		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019" />
		</xsl:variable>

		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))" />
		</xsl:variable>

		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
		</xsl:variable>

		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
		</xsl:variable>

		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
		</xsl:variable>

		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))" />
		</xsl:variable>

		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
		</xsl:variable>

		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))" />
		</xsl:variable>

		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
		</xsl:variable>

		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
		</xsl:variable>

		<xsl:variable name ="varMonthUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nMonth) = 1">
					<xsl:value-of select ="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="nDayUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nDay) = 1">
					<xsl:value-of select ="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>

	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and COL9 !='Healthcare 0521' and COL9 !='Capital Markets' and COL9 !='Consumer 1218' and COL9 !='Consumer-1' and COL9 !='Convertibles 0920' and COL9 !='Events 0919' and COL9 !='Events 1' and COL9 !='Financials 0221' and COL9 !='Financials 1017' and COL9 !='Financials-1' and COL9 !='Financials-3' and COL9 !='Healthcare 1020' and COL9 !='Healthcare-1' and COL9 !='Healthcare-3' and COL9 !='IndexArb' and COL9 !='Industrials 0321' and COL9 !='Industrials 0420' and COL9 !='Insurance 0120' and COL9 !='Materials 1019' and COL9 !='Peak Main' and COL9 !='Quant 1' and COL9 !='Quant 2' and COL9 !='Renewables 0421' and COL9 !='Technology 0119' and COL9 !='Technology 0120' and COL9 !='Technology 0121' and COL9 !='Technology-2'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'HS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL10"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name = "varAsset" >
							<xsl:choose>
								<xsl:when test="COL55='Option'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL6='Bond'">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>
								<xsl:when test="COL56='CFD'">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL55)='FX'">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						    <xsl:variable name="varSwapSymbol">
              <xsl:value-of select="substring-before(COL8,' EQUITY')"/>
            </xsl:variable>

            <xsl:variable name="varSwapBloomberg">
              <xsl:value-of select="concat($varSwapSymbol,' EQUITY')"/>
            </xsl:variable>
			
			<xsl:variable name="varSwapBloomberg1">
              <xsl:value-of select="concat($varSwapSymbol,' EQUITY(US)')"/>
            </xsl:variable>
			
			 <xsl:variable name="varSwapSymbol2">
              <xsl:value-of select="substring-before(COL8,' INDEX')"/>
            </xsl:variable>
			
			<xsl:variable name="varSwapBloomberg3">
              <xsl:value-of select="concat($varSwapSymbol2,' INDEX')"/>
            </xsl:variable>
			
			<xsl:variable name="SwapSymbology">
			 <xsl:choose>
                
				 <xsl:when test="COL8!='' and contains(COL8,(' INDEX')) and COL14='USD'">
                  <xsl:value-of select="$varSwapBloomberg3"/>
                </xsl:when>
				 <xsl:when test="COL8!='' and not(contains(COL8,'US EQUITY')) and COL14='USD'">
                  <xsl:value-of select="$varSwapBloomberg1"/>
                </xsl:when>
				 <xsl:when test="COL8!='' and not(contains(COL8,'US EQUITY')) and COL14!='USD'">
                  <xsl:value-of select="$varSwapBloomberg"/>
                </xsl:when>
                <xsl:when test="COL8!='' and contains(COL8,'US EQUITY') and COL14='USD'">
                  <xsl:value-of select="$varSwapBloomberg"/>
                </xsl:when>
			
			

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
			  
			      </xsl:variable>


						<xsl:variable name = "PB_SUFFIX_NAME" >
							<xsl:value-of select ="substring-before(substring-after(COL8,'.'),'.')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="$varAsset='EquitySwap' and COL14=COL15">
									<xsl:value-of select ="COL49"/>
								</xsl:when>
								<xsl:when test="$varAsset='EquitySwap' and COL14!=COL15">
									<xsl:value-of select ="concat(COL49,'(US)')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL7"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						
			
						<xsl:variable name ="varFXDay">
							<xsl:value-of select="substring-after(substring-after(COL7,'-'),'-')"/>
						</xsl:variable>

						<xsl:variable name="varFXMonth">
							<xsl:value-of select="substring-before(substring-after(COL7,'-'),'-')"/>
						</xsl:variable>

						<xsl:variable name="varFXYear">
							<xsl:value-of select="substring-before(substring-after(substring-after(COL7,'/'),' '),'-')"/>
						</xsl:variable>


						<xsl:variable name="varFXForward">
							<xsl:value-of select="concat(substring-before(COL7,' '),' ',$varFXMonth,'/',$varFXDay,'/',$varFXYear)"/>
						</xsl:variable>

 <xsl:variable name="varOption">
					<xsl:call-template name="Option">
						<xsl:with-param name="Symbol" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$varAsset='FXForward'">
									<xsl:value-of select="$varFXForward"/>
								</xsl:when>
							<xsl:when test="$varAsset='EquityOption'">
						  <xsl:value-of select="$varOption"/>
					      </xsl:when>
								<xsl:when test="$varAsset='EquitySwap'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL7!=''">
									<xsl:value-of select ="''"/>
								</xsl:when>




								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<RowIndex>
							<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</RowIndex>

						<Bloomberg>
							<xsl:choose>

								<xsl:when test="$varAsset='FXForward'">
									<xsl:value-of select="''"/>
								</xsl:when>

						<xsl:when test="$varAsset='EquityOption'">
						  <xsl:value-of select="''"/>
					      </xsl:when>
								<xsl:when test="$varAsset='EquitySwap'">
									<xsl:value-of select="$SwapSymbology"/>
								</xsl:when>

								<xsl:when test="COL7!=''">
									<xsl:value-of select ="normalize-space(COL7)"/>
								</xsl:when>

								<xsl:when test="$varAsset='Option'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>


						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL38)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<xsl:variable name="PB_CURRENCY_NAME">
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<xsl:variable name="PRANA_CURRENCY_ID">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
						</xsl:variable>

						<CurrencyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_CURRENCY_ID)">
									<xsl:value-of select="$PRANA_CURRENCY_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CurrencyID>


						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY"/>
						</PBSymbol>


						<NetPosition>
							<xsl:choose>
								<xsl:when  test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when  test="$varCostBasis &gt; 0 and COL14='GBP'">
									<xsl:value-of select="$varCostBasis div 100"/>
								</xsl:when>
								<xsl:when  test="$varCostBasis &gt; 0 and COL14!='GBP'">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0 and COL14='GBP'">
									<xsl:value-of select="($varCostBasis div 100 )* (-1)"/>
								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0 and COL14!='GBP'">
									<xsl:value-of select="$varCostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL47"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
							
							<xsl:when  test="$varCommission &gt; 0 and COL55='Future'">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0 and COL55='Future'">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:when  test="$varCommission &gt; 0 and COL14='USD'">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0 and COL14='USD'">
									<xsl:value-of select="$varCommission * (-1)"/>
								</xsl:when>
								<xsl:when  test="$varCommission &gt; 0 and COL14!='USD'">
									<xsl:value-of select="COL18"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0 and COL14!='USD'">
									<xsl:value-of select="COL18 * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>



						<xsl:variable name="varSecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL46"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test="$varSecFee &gt; 0 and COL14='USD'">
									<xsl:value-of select="$varSecFee"/>
								</xsl:when>
								<xsl:when test="$varSecFee &lt; 0 and COL14='USD'">
									<xsl:value-of select="$varSecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="varORFFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL48"/>
							</xsl:call-template>
						</xsl:variable>
						<OrfFee>
							<xsl:choose>
								<xsl:when test="$varORFFee &gt; 0 and COL14='USD'">
									<xsl:value-of select="$varORFFee"/>
								</xsl:when>
								<xsl:when test="$varORFFee &lt; 0 and COL14='USD'">
									<xsl:value-of select="$varORFFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>




						<xsl:variable name="BuyFees2">
							<xsl:value-of select="-1 * (COL17 + ((COL12 * COL13) - COL18 + COL25))"/>
						</xsl:variable>

						<xsl:variable name="SellFees2">
							<xsl:value-of select=" (((COL12 * COL13) + (COL25 - COL18) ) +  COL17) * -1"/>
						</xsl:variable>

						<xsl:variable name="BuyFees2gbp">
							<xsl:value-of select="-1 * (COL17 + ((COL12 * COL13 * 0.01) - COL18 + COL25))"/>
						</xsl:variable>

						<xsl:variable name="SellFees2gbp">
							<xsl:value-of select=" (((COL12 * COL13 * 0.01) + (COL25 - COL18) ) +  COL17) * -1"/>
						</xsl:variable>

						<xsl:variable name="BuyOptionFees2">
							<xsl:value-of select="-1 * (COL17 + ((COL12 * COL13 * 100) - COL18 + COL25))"/>
						</xsl:variable>

						<xsl:variable name="SellOptionFees2">
							<xsl:value-of select="(((COL12 * COL13 * 100) + (COL25 - COL18) ) +  COL17) * -1"/>
						</xsl:variable>

						<xsl:variable name="BuyFIFees2">
							<xsl:value-of select="-1 * (COL24 + ((COL12 * COL13 * 0.01) - COL23 - COL18))"/>
						</xsl:variable>

						<xsl:variable name="SellFIFees2">
							<xsl:value-of select="(((COL12 * COL13 * 0.01) - COL23 - COL18) +  COL24) * -1"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="COL56='CFD'">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:when test="COL11='SellShort' and COL55='Equity' and COL14='GBP'">
									<xsl:value-of select="$SellFees2gbp"/>
								</xsl:when>
								<xsl:when test="COL11='Sell' and COL55='Equity' and COL14='GBP'">
									<xsl:value-of select="$SellFees2gbp"/>
								</xsl:when>
								<xsl:when test="COL11='Buy' and COL55='Equity' and COL14='GBP'">
									<xsl:value-of select="$BuyFees2gbp"/>
								</xsl:when>
								<xsl:when test="COL11='BuyCover' and COL55='Equity' and COL14='GBP'">
									<xsl:value-of select="$BuyFees2gbp"/>
								</xsl:when>

								<xsl:when test="COL11='SellShort' and COL55='Equity' ">
									<xsl:value-of select="$SellFees2"/>
								</xsl:when>
								<xsl:when test="COL11='Sell' and COL55='Equity'">
									<xsl:value-of select="$SellFees2"/>
								</xsl:when>
								<xsl:when test="COL11='Buy' and COL55='Equity'">
									<xsl:value-of select="$BuyFees2"/>
								</xsl:when>
								<xsl:when test="COL11='BuyCover' and COL55='Equity'">
									<xsl:value-of select="$BuyFees2"/>
								</xsl:when>

								<xsl:when test="COL11='SellShort' and COL55='Option' ">
									<xsl:value-of select="$SellOptionFees2"/>
								</xsl:when>
								<xsl:when test="COL11='Sell' and COL55='Option'">
									<xsl:value-of select="$SellOptionFees2"/>
								</xsl:when>
								<xsl:when test="COL11='Buy' and COL55='Option'">
									<xsl:value-of select="$BuyOptionFees2"/>
								</xsl:when>
								<xsl:when test="COL11='BuyCover' and COL55='Option'">
									<xsl:value-of select="$BuyOptionFees2"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>




						<xsl:variable name="varDateTime">
							<xsl:value-of select="COL2"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="$varDateTime"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="COL3"/>
						</PositionSettlementDate>

						<FXRate>
						<xsl:choose>
								<xsl:when  test="COL56='CFD'  and $varAsset= 'EquitySwap' and COL14= 'USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when  test="$varAsset != 'EquitySwap'">
									<xsl:value-of select="COL27"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>							
						</FXRate>

						<FXConversionMethodOperator>
							<xsl:choose>
								<xsl:when  test="COL15='EUR'">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:when  test="COL14='EUR'">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:when  test="COL15='GBP'">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:when  test="COL14='GBP'">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:when  test="COL15='USD'">
									<xsl:value-of select="'M'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'D'"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXConversionMethodOperator>



						<xsl:variable name="varSide">
							<xsl:value-of select="COL11"/>
						</xsl:variable>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL55='Option'">
									<xsl:choose>
										<xsl:when  test="$varSide='Buy'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when  test="$varSide='BuyCover'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SellShort'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Sell'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$varSide='Buy'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when  test="$varSide='BuyCover'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SellShort'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Sell'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="varAccruedInterest">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL23"/>
							</xsl:call-template>
						</xsl:variable>
						<AccruedInterest>
							<xsl:choose>
								<xsl:when test="$varAccruedInterest &gt; 0 and COL14='USD'">
									<xsl:value-of select="$varAccruedInterest"/>
								</xsl:when>
								<xsl:when test="$varAccruedInterest &lt; 0 and COL14='USD'">
									<xsl:value-of select="$varAccruedInterest * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccruedInterest>



						<!-- <xsl:variable name="varOTransDate"> -->
						<!-- <xsl:call-template name="FormatDate"> -->
						<!-- <xsl:with-param name="DateTime" select="$varDateTime"/> -->
						<!-- </xsl:call-template> -->
						<!-- </xsl:variable> -->

						<xsl:variable name="varOrigTransDate">
							<xsl:value-of select="$varDateTime"/>
						</xsl:variable>

						<xsl:if test="$varAsset='EquitySwap'">

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
								<xsl:value-of select ="$varOrigTransDate"/>
							</OrigTransDate>
							<xsl:variable name="varYear">
								<xsl:choose>
									<xsl:when test="contains($varOrigTransDate,'/')">
										<xsl:value-of select="substring-after(substring-after($varOrigTransDate,'/'),'/')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring-after(substring-after($varOrigTransDate,'-'),'-')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="Day">
								<xsl:choose>
									<xsl:when test="contains($varOrigTransDate,'/')">
										<xsl:choose>
											<xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'/'),'/'))) = 1">
												<xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'/'),'/'))"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring-before(substring-after($varOrigTransDate,'/'),'/')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'-'),'-'))) = 1">
												<xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'-'),'-'))"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring-before(substring-after($varOrigTransDate,'-'),'-')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="Month">
								<xsl:choose>
									<xsl:when test="contains($varOrigTransDate,'/')">
										<xsl:choose>
											<xsl:when test="string-length(number(substring-before($varOrigTransDate,'/'))) = 1">
												<xsl:value-of select="concat(0,substring-before($varOrigTransDate,'/'))"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring-before($varOrigTransDate,'/')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="string-length(number(substring-before($varOrigTransDate,'-'))) = 1">
												<xsl:value-of select="concat(0,substring-before($varOrigTransDate,'-'))"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="substring-before($varOrigTransDate,'-')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<xsl:variable name="SettleDate">
								<xsl:value-of select='my:NowSwap(number($varYear),number($Month),number($Day))'/>
							</xsl:variable>

							<FirstResetDate>
								<xsl:value-of select ="$SettleDate"/>
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


