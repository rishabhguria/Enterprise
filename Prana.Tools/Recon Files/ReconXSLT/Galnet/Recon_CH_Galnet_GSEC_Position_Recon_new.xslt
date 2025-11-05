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

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>
			<xsl:when test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12 and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCodeVar">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="varSymbol"/>

		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:value-of select ="substring-before($varSymbol,' ')"/>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:value-of select ="substring-before(substring-after(substring-after($varSymbol,'/'),'/'),' ')"/>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:value-of select ="substring-before(substring-after(substring-after($varSymbol,' '),' '),'/')"/>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:value-of select ="substring-before(substring-after($varSymbol,'/'),'/')"/>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:value-of select ="substring(substring-after(substring-after(substring-after($varSymbol,'/'),'/'),' '),1,1)"/>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:value-of select ="substring-after(substring-after($varSymbol,'/'),$varPutCall)"/>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:call-template name ="MonthCode">
				<xsl:with-param name ="varMonth" select ="number($varMonth)"/>
				<xsl:with-param name ="varPutCall" select="$varPutCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varDays">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)='0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>

		<!--<xsl:value-of select="concat($varUnderlyingSymbol,number(number($varExDay)-1),substring(substring-after($varThirdFriday,'/'),1,2))"/>-->

		<xsl:choose>
			<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
				<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position" select="COL3"/>

				<xsl:variable name="Asset">
					<xsl:choose>
						<xsl:when test="contains(COL13,'/')='true'">
							<xsl:value-of select="'Option'"/>
						</xsl:when>
						<!--<xsl:when test="contains(substring-after(normalize-space(COL13),' '),' ')='true'">
							<xsl:value-of select="'FutureOption'"/>
						</xsl:when>-->
						<xsl:when test="COL3!='*' and COL12='*' and COL14='0'">
							<xsl:value-of select="'FutureOption'"/>
						</xsl:when>
						<xsl:when test="COL12='*'">
							<xsl:value-of select="'Future'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Equity'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>

					<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GSEC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME">
							<xsl:value-of select ="normalize-space(COL4)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select ="normalize-space(COL13)"/>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:choose>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="substring($Symbol,1,string-length(substring-before($Symbol,' '))-2)"/>
								</xsl:when>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="substring($Symbol,1,2)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="OptionSymbol">
							<xsl:choose>
								<xsl:when test="$Asset='Option'">
									<xsl:call-template name="Option">
										<xsl:with-param name="varSymbol" select="$Symbol"/>
									</xsl:call-template>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name ="FUTURE_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExchangeName"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_MULTIPLIER">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@StrikeMul"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_FLAG">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="Multiplier">
							<xsl:choose>
								<xsl:when test="number($FUTURE_MULTIPLIER)">
									<xsl:value-of select="$FUTURE_MULTIPLIER"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--<xsl:variable name="varMonth">
							<xsl:value-of select="number(substring-before(substring-after(substring-after($Symbol,' '),' '),'/'))"/>
						</xsl:variable>

						<xsl:variable name="varMonthCode">
							<xsl:choose>
								<xsl:when test="$Asset='Futures' or ($Asset='Option' and contains(COL3,'/')!='true')">
									<xsl:value-of select="substring($Symbol,3,1)"/>
								</xsl:when>
								<xsl:when test="$varMonth=1">
									<xsl:value-of select="'F'"/>
								</xsl:when>
								<xsl:when test="$varMonth=2">
									<xsl:value-of select="'G'"/>
								</xsl:when>
								<xsl:when test="$varMonth=3">
									<xsl:value-of select="'H'"/>
								</xsl:when>
								<xsl:when test="$varMonth=4">
									<xsl:value-of select="'J'"/>
								</xsl:when>
								<xsl:when test="$varMonth=5">
									<xsl:value-of select="'K'"/>
								</xsl:when>
								<xsl:when test="$varMonth=6">
									<xsl:value-of select="'M'"/>
								</xsl:when>
								<xsl:when test="$varMonth=7">
									<xsl:value-of select="'N'"/>
								</xsl:when>
								<xsl:when test="$varMonth=8">
									<xsl:value-of select="'Q'"/>
								</xsl:when>
								<xsl:when test="$varMonth=9">
									<xsl:value-of select="'U'"/>
								</xsl:when>
								<xsl:when test="$varMonth=10">
									<xsl:value-of select="'V'"/>
								</xsl:when>
								<xsl:when test="$varMonth=11">
									<xsl:value-of select="'X'"/>
								</xsl:when>
								<xsl:when test="$varMonth=12">
									<xsl:value-of select="'Z'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->

						<xsl:variable name="varMonthCode">
							<xsl:choose>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="substring($Symbol,string-length(substring-before($Symbol,' ')-1),1)"/>
								</xsl:when>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="substring($Symbol,3,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="substring($Symbol,string-length(substring-before($Symbol,' ')),1)"/>
								</xsl:when>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="substring($Symbol,4,1)"/>
								</xsl:when>									
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="MonthYearCode">
							<xsl:choose>
								<xsl:when test="$FUTURE_FLAG!=''">
									<xsl:value-of select="concat($varYear,$varMonthCode)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($varMonthCode,$varYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="$PRANA_ROOT_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_ROOT_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="PutCall">
							<xsl:value-of select="substring($Symbol,5,1)"/>
						</xsl:variable>

						<xsl:variable name="StrikePrice">
							<xsl:value-of select="substring-before(substring-after($Symbol,' '),' ')"/>
						</xsl:variable>

						<xsl:variable name="Week">
							<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),' '),' '),9,1)"/>
						</xsl:variable>


						<xsl:variable name="varMonth1">
							<xsl:value-of select ="substring-before(substring-after(normalize-space(COL4),' '),' ')"/>

						</xsl:variable>

						<xsl:variable name="Month">
							<xsl:call-template name="MonthCodeVar">
								<xsl:with-param name="varMonth" select="$varMonth1"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Year">
							<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),2,1)"/>
						</xsl:variable>

						<xsl:variable name="PUTCALL">
							<xsl:value-of select="substring(substring-before(COL4,' '),1,1)"/>
						</xsl:variable>

						<xsl:variable name="StrikePrice1">
							<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),' '),' ')"/>
						</xsl:variable>

						<xsl:variable name="Future">
							<xsl:choose>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="normalize-space(concat($varUnderlying,' ',$MonthYearCode,$FUTURE_SUFFIX_NAME))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(concat($varUnderlying,' ',$MonthYearCode,$PutCall,$StrikePrice*$Multiplier,$FUTURE_SUFFIX_NAME))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						
						
						<!--<xsl:variable name="FutureOption1">
							<xsl:choose>
								<xsl:when test="COL3!='0' and COL13='*' or COL13=''">
									<xsl:value-of select="concat($Week,'E',' ',$Month,$Year,$PUTCALL,$StrikePrice1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($varUnderlying,' ',$MonthYearCode,$PutCall,$StrikePrice*$Multiplier,$FUTURE_SUFFIX_NAME)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->

						<!--................................................................................................................-->

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when> 

								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="'jyoti'"/>
								</xsl:when>
								<!--<xsl:when test="$Asset='Future'">
									<xsl:value-of select="$Future"/>
								</xsl:when>-->

								<!--<xsl:when test="$Asset='Option'">
									<xsl:value-of select="$OptionSymbol"/>
								</xsl:when>-->

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="substring-before($Symbol,' ')"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<Quantity>
							<xsl:value-of select="$Position"/>
						</Quantity>

						<Side>
							<xsl:choose>

								<xsl:when test="$Asset='Option'">
									<xsl:choose>

										<xsl:when test="$Position &gt; 0">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>

										<xsl:when test="$Position &lt; 0">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test="$Position &gt; 0">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>

										<xsl:when test="$Position &lt; 0">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<xsl:variable name="MarkPrice" select="number(COL5)"/>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>

								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<TradeDate>
							<xsl:value-of select="COL2"/>
						</TradeDate>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="MarketValue" select="number(COL6)"/>

						<MarketValue>
							<xsl:choose>

								<xsl:when test="$MarketValue &gt; 0">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>

								<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>