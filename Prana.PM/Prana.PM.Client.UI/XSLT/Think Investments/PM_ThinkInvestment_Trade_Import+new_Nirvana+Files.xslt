<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 or $Month=6">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
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
		<xsl:if test="contains(COL19,OPTION)">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL4,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL4),'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:choose>
					<xsl:when test="not(contains(COL4,'US'))">
						<xsl:value-of select="substring-before(substring-after(normalize-space(COL4),' '),'/')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL4),' '),' '),'/')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' ')"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:choose>
					<xsl:when test="not(contains(COL4,'US'))">
						<xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' '),1,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' '),' '),1,1)"/>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:variable>
			<xsl:variable name="StrikePrice">

				<xsl:choose>
					<xsl:when test="not(contains(COL4,'US'))">
						<xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' '),2),'#.00')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' '),' '),2),'#.00')"/>
					</xsl:otherwise>
				</xsl:choose>

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

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL3"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BNPB'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:choose>
								<xsl:when test="contains(COL16,'SWAP')">
									<xsl:value-of select="concat(normalize-space(COL5),normalize-space(COL17))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="COL4"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL19 ='EQUITY' and COL16='SWAP'">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:when test="COL19 ='EQUITY' and COL16 ='CASH'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL19 ='FUTURE'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="COL19 ='FX FWD'">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:when test="COL19 ='FX SPOT'">
									<xsl:value-of select="'FXSpot'"/>
								</xsl:when>
								<xsl:when test="COL19 ='OPTION'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>							
							</xsl:choose>
						</xsl:variable>


						<PBAssetType>
							<xsl:value-of select="$Asset"/>
						</PBAssetType>

						<xsl:variable name ="varFXMonths">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL18,'-')) ='1'">
									<xsl:value-of select="concat('0',substring-before(COL18,'-'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL18,'-')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFXDay">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(substring-after(COL18,'-'),'-')) ='1'">
									<xsl:value-of select="concat('0',substring-before(substring-after(COL18,'-'),'-'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(substring-after(COL18,'-'),'-')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFXYear">
							<xsl:value-of select="substring(substring-after(substring-after(COL18,'-'),'-'),3,2)"/>
						</xsl:variable>

					

						<xsl:variable name="varLCurrency">
							<xsl:value-of select="substring-before(COL4,'(')"/>
						</xsl:variable>

						<xsl:variable name="varBCurrency">
							<xsl:value-of select="substring-before(substring-after(COL4,'('),')')"/>
						</xsl:variable>


						<xsl:variable name="varFXForward">
							<xsl:choose>
								<xsl:when test="COL12='EUR' or COL12='GBP' or COL12='NZD' or COL12='AUD'">
									<xsl:value-of select="concat(COL12,'/','USD',' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL12,' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varSEDOL">
							<xsl:value-of select="normalize-space(COL1)"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_NAME" >
							<xsl:value-of select ="substring-after(COL4,' ')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varFXSpot">
							<xsl:choose>
								<xsl:when test="COL12='EUR' or COL12='GBP' or COL12='NZD' or COL12='AUD'">
									<xsl:value-of select="concat(COL12,'/','USD')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL12)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL4,' ')">
									<xsl:value-of select="substring-before(COL4,' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL4"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Suffix" select="''"/>
										<xsl:with-param name="Symbol" select="COL4"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:when test="$Asset='FXSpot'">
									<xsl:value-of select="$varFXSpot"/>
								</xsl:when>

								<xsl:when test="$Asset='FXForward'">
									<xsl:value-of select="$varFXForward"/>
								</xsl:when>

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="''"/>
								</xsl:when>
							

								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="$Asset='FxSpot'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>

								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<xsl:variable name="PB_FUND_NAME" select="COL21"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</AccountName>


						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="substring-after(substring-after(substring-after(COL20,' '),'-'),' ')"/>
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
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>


						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varFXRate">
							<xsl:choose>
								<xsl:when test="COL11='*' or COL11=''">
									<xsl:value-of select="'1'"/>
								</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL11"/>
									</xsl:otherwise>
							</xsl:choose>
								</xsl:variable>

						<xsl:variable name="varCOL10">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCOL3">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL3"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCostBasis">
							<xsl:choose>

								<xsl:when test="COL12 != COL13">
									<xsl:value-of select="$CostBasis * $varFXRate"/>
								</xsl:when>
								
								<xsl:when test="COL19='EQUITY' and COL16='SWAP'">
									<xsl:value-of select="$CostBasis"/>
							     </xsl:when>								
									<xsl:when test="COL19='OTC OPTION' and COL16='SWAP'">
										<xsl:value-of select="($varCOL10 div $varCOL3) div 100"/>
									</xsl:when>

								<xsl:when test="COL19='FX SPOT'">
									<xsl:value-of select="$varCOL10 div $varCOL3"/>
								</xsl:when>

								<xsl:when test="COL19='FX FWD'">
									<xsl:value-of select="$varCOL10 div $varCOL3"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$CostBasis"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>		

				     <CostBasis>
							<xsl:choose>
								<xsl:when test="$varCostBasis &gt; 0">
									<xsl:value-of select="$varCostBasis"/>

								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0">
									<xsl:value-of select="$varCostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>					


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>

						

						<xsl:variable name="varCommission">
							<xsl:choose>
								<xsl:when test="COL12 != COL13">
									<xsl:value-of select="$Commission * $varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Commission"/>
								</xsl:otherwise>
							</xsl:choose>							
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>

								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name="varScefee">
							<xsl:choose>
								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="COL9"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varSecfeeFxRate">
							<xsl:choose>
								<xsl:when test="COL12 != COL13">
									<xsl:value-of select="$varScefee * $varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varScefee"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Secfee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varSecfeeFxRate"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test="$Secfee &gt; 0">
									<xsl:value-of select="$Secfee"/>

								</xsl:when>
								<xsl:when test="$Secfee &lt; 0">
									<xsl:value-of select="$Secfee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SecFee>

						<xsl:variable name="varOrfFee">
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="COL9"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varOrfFeeFxRate">
							<xsl:choose>
								<xsl:when test="COL12 != COL13">
									<xsl:value-of select="$varOrfFee * $varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varOrfFee"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="OrfFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varOrfFee"/>
							</xsl:call-template>
						</xsl:variable>
						<OrfFee>
							<xsl:choose>
								<xsl:when test="$OrfFee &gt; 0">
									<xsl:value-of select="$OrfFee"/>
								</xsl:when>
								<xsl:when test="$OrfFee &lt; 0">
									<xsl:value-of select="$OrfFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>



						<xsl:variable name ="Side" select="COL2"/>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side='BC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='BUY'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$Side='SELL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="$Side='SHRT'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='BC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="$Side='SHRT'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</SideTagValue>


						<xsl:variable name="Fees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>



						<xsl:variable name="varFees">
							<xsl:choose>
								<xsl:when test="COL12 != COL13">
									<xsl:value-of select="$Fees * $varFXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Fees"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Fees>
							<xsl:choose>
								<xsl:when test="$varFees &gt; 0">
									<xsl:value-of select="$varFees"/>

								</xsl:when>
								<xsl:when test="$varFees &lt; 0">
									<xsl:value-of select="$varFees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Fees>

						<xsl:variable name="varCOL12">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="FXRate">							
							<xsl:choose>
								<xsl:when test="COL12='EUR' or COL12='GBP' or COL12='NZD' or COL12='AUD'">
									<xsl:value-of select="varCOL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1 div $varCOL12"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</xsl:variable>
						<FXRate>
							<xsl:choose>
								<xsl:when test="number($FXRate)">
									<xsl:value-of select="$FXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</FXRate>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<SettlCurrencyName>
							<xsl:value-of select="COL13"/>
						</SettlCurrencyName>


						<!--<xsl:variable name="varOrigTransDate">
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="COL17"/>
							</xsl:call-template>
						</xsl:variable>-->

            <xsl:variable name="varTransDay">
              <xsl:value-of select="substring-after(substring-after(COL17,'-'),'-')"/>
            </xsl:variable>
            
            <xsl:variable name="varTransMonth">
              <xsl:value-of select="substring-before(substring-after(COL17,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varTransYear">
              <xsl:value-of select="substring-before(COL17,'-')"/>
            </xsl:variable>

            <xsl:variable name="varOrigTransDate">
              <xsl:value-of select="concat($varTransMonth,'/',$varTransDay,'/',$varTransYear)"/>
            </xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="concat($varTransMonth,'/',$varTransDay,'/',$varTransYear)"/>
						</PositionStartDate>



            <xsl:variable name="varTransSDay">
              <xsl:value-of select="substring-after(substring-after(COL18,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varTransSMonth">
              <xsl:value-of select="substring-before(substring-after(COL18,'-'),'-')"/>
            </xsl:variable>

            <xsl:variable name="varTransSYear">
              <xsl:value-of select="substring-before(COL18,'-')"/>
            </xsl:variable>

						<xsl:variable name="varOriginalPurchaseDate">
              <xsl:value-of select="concat($varTransSMonth,'/',$varTransSDay,'/',$varTransSYear)"/>
						</xsl:variable>
						<PositionSettlementDate>
							<xsl:value-of select="$varOriginalPurchaseDate"/>
						</PositionSettlementDate>




						<xsl:if test="$Asset='EquitySwap'">

							<IsSwapped>
								<xsl:value-of select ="'1'"/>
							</IsSwapped>

							<SwapDescription>
								<xsl:value-of select ="'SWAP'"/>
							</SwapDescription>

							<DayCount>
								<xsl:value-of select ="'365'"/>
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

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>