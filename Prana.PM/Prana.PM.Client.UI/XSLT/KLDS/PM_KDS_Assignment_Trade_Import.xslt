<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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
				<xsl:when test="$Month=06 ">
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
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
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

	<xsl:template name="Date">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='Jan'">
				<xsl:value-of select="01"/>
			</xsl:when>
			<xsl:when test="$Month='Feb'">
				<xsl:value-of select="02"/>
			</xsl:when>
			<xsl:when test="$Month='Mar'">
				<xsl:value-of select="03"/>
			</xsl:when>
			<xsl:when test="$Month='Apr'">
				<xsl:value-of select="04"/>
			</xsl:when>
			<xsl:when test="$Month='May'">
				<xsl:value-of select="05"/>
			</xsl:when>
			<xsl:when test="$Month='Jun'">
				<xsl:value-of select="06"/>
			</xsl:when>
			<xsl:when test="$Month='Jul'">
				<xsl:value-of select="07"/>
			</xsl:when>
			<xsl:when test="$Month='Aug'">
				<xsl:value-of select="08"/>
			</xsl:when>
			<xsl:when test="$Month='Sep'">
				<xsl:value-of select="09"/>
			</xsl:when>
			<xsl:when test="$Month='Oct'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$Month='Nov'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$Month='Dec'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL23,'Option') and contains(COL2,'/')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="COL26"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after($Symbol,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after($Symbol,' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after($Symbol,'/'),'/'),' ')"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(substring-after($Symbol,' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(substring-after($Symbol,' '),' '),2) ,'#.00')"/>
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
						<xsl:with-param name="Number" select="COL18"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="Trade1" >
					<xsl:choose>
						<!--<xsl:when test="contains(COL31,'KLDS') and contains(COL21,'MAN') and COL6!='*' and COL17!='*'">
				  <xsl:value-of select="'false'"/>
			  </xsl:when>-->
						<xsl:when test="contains(COL16,'KLDS') and contains(COL28,'E')">
							<xsl:value-of select="'false'"/>
						</xsl:when>


						<xsl:when test="COL16">
							<xsl:value-of select="'true'"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="'false'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test="number($NetPosition) and $Trade1 = 'true'  ">
					<!--and COL6!='*' and COL17!='*'-->

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Assignment'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="COL4"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:value-of select="COL26"/>
						</xsl:variable>
						<!--<xsl:variable name="PB_Code">
				  <xsl:value-of select="COL22"/>
			  </xsl:variable>-->

						<xsl:variable name="PB_YELLOW_NAME">
							<!--<xsl:value-of select="normalize-space()"/>-->
							<!--<xsl:choose>
								<xsl:when test ="contains(substring-after(normalize-space(COL20),' '),' ')">
									<xsl:value-of select="substring-after(substring-after(normalize-space(COL20),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(normalize-space(COL20),' ')"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select="''"/>
						</xsl:variable>

						<!--<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL91)"/>
						</xsl:variable>-->

						<xsl:variable name ="UnderlyingCounterParty">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@CounterParty"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name ="FUTURE_EXCHANGE_CODE">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_FLAG">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="MonthCode">
							<!--<xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="number(substring-before(COL26,'/'))"/>
              </xsl:call-template>-->
							<xsl:value-of select="substring(COL2,string-length(COL2)-1,1)"/>
						</xsl:variable>

						<xsl:variable name="Year" select="substring(COL2,string-length(COL2),1)"/>

						<xsl:variable name="MonthYearCode">
							<xsl:choose>
								<xsl:when test="$FUTURE_FLAG!=''">
									<xsl:value-of select="concat($Year,$MonthCode)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($MonthCode,$Year)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Underlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="translate($PRANA_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate($PB_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--<xsl:variable name="varPrana_Root">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCode = $varPBCode and @YellowFlag = $varYellowFlag]/@UnderlyingCode"/>
			  </xsl:variable>-->

						<AssetType>
							<xsl:choose>
								<xsl:when test="COL23='Option' and COL29='*'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<!--<xsl:when test="COL23='Option' and COL29='*'">
						  <xsl:value-of select="'FutureOption'"/>
					  </xsl:when>-->
								<xsl:when test="COL23='Future'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</AssetType>

						<xsl:variable name="TickerSymbol">
							<xsl:value-of select="concat(substring-before(COL2,' '),' ',substring-after(substring-after(COL2,' '),' '))"/>
						</xsl:variable>
						<xsl:variable name="OptionTicker">
							<xsl:call-template name="Option">
								<xsl:with-param name="Symbol" select="$TickerSymbol"/>
								<xsl:with-param name="Suffix" select="''"/>
							</xsl:call-template>
						</xsl:variable>


						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="translate($PRANA_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:when>
								<xsl:when test="contains(COL23,'Option') and contains(COL2,'/') and COL24='USD'">
									<xsl:value-of select="$OptionTicker"/>
								</xsl:when>
								<xsl:when test="contains(COL23,'Option') and contains(COL2,'/') and COL24='EUR'">
									<xsl:value-of select="concat('O:',substring-before(COL2,' '),' INDEX ',substring-after(COL2,' '),'-EEO')"/>
								</xsl:when>
								<xsl:when test="contains(COL23,'Option') and contains(COL2,'/') and COL24='GBP'">
									<xsl:value-of select="concat('O:',substring-before(COL2,' '),' INDEX ',substring-after(COL2,' '),'-EEO')"/>
								</xsl:when>
								<xsl:when test="COL23='Option'">
									<xsl:value-of select="concat($Underlying,' ',substring(substring-before(COL2,' '),string-length($Underlying)+1,3), normalize-space(substring-after(COL2,' ')))"/>

								</xsl:when>
								<xsl:when test="COL22!='*'">
									<xsl:choose>
										<xsl:when test="contains(COL2,' ')">
											<xsl:value-of select="normalize-space(concat($Underlying,' ',$MonthYearCode,$FUTURE_EXCHANGE_CODE))"/>
										</xsl:when>

										<xsl:when test="string-length(COL2)='5'">
											<xsl:value-of select="normalize-space(concat($Underlying,' ',$MonthYearCode,$FUTURE_EXCHANGE_CODE))"/>
										</xsl:when>
										<xsl:when test="string-length(COL2)='4'">
											<xsl:value-of select="normalize-space(concat($Underlying,' ',$MonthYearCode,$FUTURE_EXCHANGE_CODE))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="translate($PB_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:otherwise>

							</xsl:choose>



						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="translate($PRANA_SYMBOL_NAME,$lower_CONST,$upper_CONST)"/>
								</xsl:when>
								<xsl:when test="contains(COL23,'Option') and contains(COL2,'/')">
									<xsl:value-of select="concat(COL29,'U')"/>
								</xsl:when>

								<xsl:when test="COL22!='*'">


									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="normalize-space(COL64)='F'">

									<xsl:value-of select="normalize-space(concat($Underlying,' ',$MonthYearCode,translate($PB_YELLOW_NAME,$lower_CONST,$upper_CONST)))"/>


								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</IDCOOptionSymbol>

						<!--<xsl:variable name="Trade" >
              <xsl:choose>
                <xsl:when test="contains(COL31,'KLDS')">
                  <xsl:value-of select="'MAN'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="COL21"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->


						<xsl:variable name="PB_FUND_NAME" select="COL16"/>


						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:choose>
								<xsl:when test="COL23='Equity'">
									<xsl:value-of select="'BANK'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test="number(COL17)='7548600'">
									<xsl:value-of select="'SG'"/>
								</xsl:when>
								<xsl:when test="number(COL17)='7548601'">
									<xsl:value-of select="'SG Equities'"/>
								</xsl:when>
								<xsl:when test="number(COL17)='7548603'">
									<xsl:value-of select="'KSSGFUT'"/>
								</xsl:when>

								<xsl:when test="COL17='052BAF280'">
									<xsl:value-of select="'MS'"/>
								</xsl:when>

								<xsl:when test="COL17='0331AATY1'">
									<xsl:value-of select="'MS Options'"/>
								</xsl:when>
								<xsl:when test="COL17='001050992090'">
									<xsl:value-of select="'US Bank'"/>
								</xsl:when>



								<xsl:when test="number(COL17)='7548604'">
									<xsl:value-of select="'KSSGOPT'"/>
								</xsl:when>

								<xsl:when test="COL17='052BAHFZ2'">
									<xsl:value-of select="'KSMSFUT'"/>
								</xsl:when>

								<xsl:when test="COL17='0331AA1N5'">
									<xsl:value-of select="'KSMSOPT'"/>
								</xsl:when>

								<xsl:when test="COL17='001050993734'">
									<xsl:value-of select="'KSUSB'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$PRANA_FUND_NAME='BANK'">
											<xsl:value-of select ="''"/>
										</xsl:when>
										<xsl:when test ="$PRANA_FUND_NAME!=''">
											<xsl:value-of select ="$PRANA_FUND_NAME"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="$PB_FUND_NAME"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>


							</xsl:choose>
						</AccountName>

						<xsl:variable name="PB_CURRENCY" select="COL24"/>

						<xsl:variable name="PB_EXCHANGE" select="COL4"/>

						<xsl:variable name="CounterParty" select="COL16"/>

						<xsl:variable name="PRANA_COMMISSION">
							<xsl:value-of select ="document('../ReconMappingXML/FeesCommissionMapping.xml')/FeesCommissionMapping/PB[@Name=$PB_NAME]/MappingData[@Currency=$PB_CURRENCY and @Exchange=$PB_EXCHANGE  and @UnderlyingCode=$Underlying and @CounterParty = $CounterParty]/@Commission"/>
						</xsl:variable>



						<xsl:variable name="PRANA_FEES">
							<xsl:value-of select ="document('../ReconMappingXML/FeesCommissionMapping.xml')/FeesCommissionMapping/PB[@Name=$PB_NAME]/MappingData[@Currency=$PB_CURRENCY and @Exchange=$PB_EXCHANGE and @UnderlyingCode=$Underlying  and @CounterParty = $CounterParty]/@Fees"/>
						</xsl:variable>

						<xsl:variable name="PRANA_ClearingFee">
							<xsl:value-of select ="document('../ReconMappingXML/FeesCommissionMapping.xml')/FeesCommissionMapping/PB[@Name=$PB_NAME]/MappingData[@Currency=$PB_CURRENCY and @Exchange=$PB_EXCHANGE  and @UnderlyingCode=$Underlying and @CounterParty = $CounterParty]/@ClearingFee"/>
						</xsl:variable>
						<xsl:variable name="PB_STRATEGY" select="COL17"/>

						<xsl:variable name="PRANA_STRATEGY">
							<xsl:value-of select ="document('../ReconMappingXML/StrategyMapping.xml')/StrategyMapping/PB[@Name=$PB_NAME]/StrategyData[@PBStrategy=$PB_STRATEGY ]/@PranaStrategy"/>
						</xsl:variable>

						<xsl:variable name="STRATEGY_NAME">
							<xsl:choose>
								<xsl:when test ="normalize-space(COL6)='bmark'">
									<xsl:value-of select ="concat(normalize-space(COL6),',',normalize-space(COL7))"/>
									<!--<xsl:value-of select ="concat(normalize-space(COL6),',',normalize-space(substring-before(COL7,',')))"/>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="concat(normalize-space(COL6),',',normalize-space(COL7))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Strategy>
							<xsl:choose>

								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='blend (offshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='blend (onshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='hedge' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='msci_eafe (offshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='msci_eafe (onshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='sp500 (offshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='sp500 (onshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='tbill (offshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='tbill (onshore)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='msci_eafe (blend)' and $PRANA_STRATEGY != 'Spectrum'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='msci_eafe (offshore),mscieafe'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='msci_eafe (onshore),mscieafe'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='sp500 (offshore),sp500'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='sp500 (onshore),sp500'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								<xsl:when test ="normalize-space(COL6)='bmark' and normalize-space(COL7)='msci_emg (offshore)'">
									<xsl:value-of select ="$STRATEGY_NAME"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$PRANA_STRATEGY!=''">
											<xsl:value-of select ="$PRANA_STRATEGY"/>
										</xsl:when>
										<xsl:when test ="contains(COL16,'KLDS')">
											<xsl:value-of select ="'Prism'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="$PB_STRATEGY"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
							

						</Strategy>

						<!--<TradeAttribute2>
              <xsl:value-of select="COL7"/>
            </TradeAttribute2>

			  <TradeAttribute3>
				  <xsl:value-of select="COL10"/>
			  </TradeAttribute3>-->


						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition&gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition&lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<TradeAttribute1>
							<xsl:value-of select="COL28"/>
						</TradeAttribute1>

						<TradeAttribute2>
							<xsl:value-of select="COL21"/>
						</TradeAttribute2>

						<xsl:variable name="PB_COUNTER_PARTY" select="COL16"/>

						<xsl:variable name="varPrice_Mul">

							<xsl:choose>
								<xsl:when test="$UnderlyingCounterParty!=''">

									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @CounterParty = $PB_COUNTER_PARTY]/@PriceMul"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME]/@PriceMul"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="varStrike_Mul">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME ]/@StrikeMul"/>
						</xsl:variable>

						<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test ="number($varPrice_Mul)">
									<xsl:value-of select="COL19*$varPrice_Mul"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL19"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="Strike">
							<xsl:choose>
								<xsl:when test ="number($varStrike_Mul)">
									<xsl:value-of select="format-number(COL20*$varStrike_Mul,'#')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(COL20,'#')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>




						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>

								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>




						<xsl:variable name="Side" select="COL8"/>



						<SideTagValue>

							<xsl:choose>





								<!--<xsl:when test="$Side='B' and COL23='Option' and COL24='EUR'">
					  <xsl:value-of select="'A'"/>
				  </xsl:when>

				  <xsl:when test="$Side='SS' and COL23='Option' and COL24='EUR'">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>

				  <xsl:when test="$Side='B' and COL23='Option' and COL24='JPY'">
					  <xsl:value-of select="'A'"/>
				  </xsl:when>

				  <xsl:when test="$Side='SS' and COL23='Option' and COL24='JPY'">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>

				  <xsl:when test="$Side='B' and COL23='Option' and COL24='GBP'">
					  <xsl:value-of select="'A'"/>
				  </xsl:when>

				  <xsl:when test="$Side='SS' and COL23='Option' and COL24='GBP'">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>

				  <xsl:when test="$Side='B' and COL23='Option' and COL24='HKD'">
					  <xsl:value-of select="'A'"/>
				  </xsl:when>

				  <xsl:when test="$Side='SS' and COL23='Option' and COL24='HKD'">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>

				  <xsl:when test="$Side='B' and COL23='Option' and COL24='CHF'">
					  <xsl:value-of select="'A'"/>
				  </xsl:when>

				  <xsl:when test="$Side='SS' and COL23='Option' and COL24='CHF'">
					  <xsl:value-of select="'C'"/>
				  </xsl:when>-->

								<xsl:when test="$Side='B' and COL23='Option' and (COL17='7548600' or COL17='7548603' or COL17='052BAF280' or COL17='052BAHFZ2')">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="$Side='S' and COL23='Option' and (COL17='7548600' or COL17='7548603' or COL17='052BAF280' or COL17='052BAHFZ2')">
									<xsl:value-of select="'2'"/>
								</xsl:when>


								<xsl:when test="$Side='B' and COL23='Option'">
									<xsl:value-of select="'A'"/>
								</xsl:when>

								<xsl:when test="$Side='SS' and COL23='Option'">
									<xsl:value-of select="'C'"/>
								</xsl:when>





								<xsl:when test="$Side='S' and COL23='Option'">
									<xsl:value-of select="'D'"/>
								</xsl:when>

								<xsl:when test="$Side='S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:when test="$Side='B'">
									<xsl:value-of select="'1'"/>
								</xsl:when>






								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>

						</SideTagValue>

						<!--<SEDOL>
              <xsl:value-of select="COL9" />
            </SEDOL>-->

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<xsl:variable name="Date" select="substring-before(COL20,' ')"/>

						<PositionStartDate>
							<xsl:value-of select="$Date"/>

						</PositionStartDate>




						<xsl:variable name="PRANA_COUNTER_PARTY">
							<xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>



								<xsl:when test ="number($PRANA_COUNTER_PARTY)">
									<xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CounterPartyID>


						<xsl:variable name="COL35">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL35"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL39">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL39"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="OtherBrokerFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$COL35 + $COL39"/>
							</xsl:call-template>
						</xsl:variable>

						<Fees>

							<xsl:choose>
								<xsl:when test="contains(COL23,'Option') and contains(COL16,'MSET') and COL23='Option' and COL29!='*'">
									<xsl:value-of select="0.04 * $NetPosition"/>
								</xsl:when>

								<xsl:when test="number($PRANA_FEES)">
									<xsl:value-of select="$PRANA_FEES * $NetPosition"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Fees>



						<ClearingFee>

							<xsl:choose>
								<xsl:when test="contains(COL23,'Option') and contains(COL16,'MSET') and COL23='Option' and COL29!='*'">
									<xsl:value-of select="1.50 * $NetPosition"/>
								</xsl:when>

								<xsl:when test="number($PRANA_ClearingFee)">
									<xsl:value-of select="$PRANA_ClearingFee * $NetPosition"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</ClearingFee>




						<Commission>

							<xsl:choose>
								<xsl:when test="contains(COL23,'Option') and contains(COL16,'MSET') and COL23='Option' and COL29!='*'">
									<xsl:value-of select="1 * $NetPosition"/>
								</xsl:when>

								<xsl:when test="number($PRANA_COMMISSION)">
									<xsl:value-of select="$PRANA_COMMISSION * $NetPosition"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Commission>


						<CurrencySymbol>
							<xsl:value-of select="COL24"/>
						</CurrencySymbol>

						<Multiplier>
							<xsl:choose>
								<xsl:when test="COL34!='*'">
									<xsl:value-of select="COL34"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose>

						</Multiplier>



					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>