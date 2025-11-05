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
			<xsl:when  test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12  and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<!--<xsl:param name="varSymbol">-->

		<xsl:variable name="varSymbol">
			<xsl:value-of select="normalize-space(COL18)"/>
		</xsl:variable>


		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15">
					<xsl:value-of select ="substring-before($varSymbol,' ')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),1,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),3,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),5,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),7,1)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceInt">
			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),8,5)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceDec">
			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,' ')),13,3)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test="number($varStrikePriceInt) or number($varStrikePriceDec)">
					<xsl:value-of select ="format-number(concat($varStrikePriceInt,'.',$varStrikePriceDec),'#.00')"/>
				</xsl:when>
			</xsl:choose>
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

		<!--<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="string-length(COL18) &gt; 15 and number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>-->


		<xsl:choose>
			<xsl:when test="string-length(COL18) &gt; 15">
				<!--<xsl:choose>
					<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
					</xsl:when>
					<xsl:otherwise>-->
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
					<!--</xsl:otherwise>
				</xsl:choose>-->
			</xsl:when>
		</xsl:choose>
		<!--</xsl:param>-->
	</xsl:template>


	<xsl:template name="GetSuffix">
		<xsl:param name="Suffix"/>
		<xsl:choose>
			<xsl:when test="$Suffix = 'JPY'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CHF'">
				<xsl:value-of select="'-SWX'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'EUR'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'CAD'">
				<xsl:value-of select="'-TC'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'ILS'">
				<xsl:value-of select="'-TAE'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:if test ="number(COL7)">
					<PositionMaster>


						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_CODE" >
							<xsl:value-of select ="COL9"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
						</xsl:variable>
					

						<xsl:variable name="varOptionSymbol">
							<xsl:call-template name="Option">
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<Symbol>

							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<!--<xsl:when test ="$PRANA_SUFFIX_NAME != '' and $PB_SUFFIX_CODE!='USD'">
									<xsl:value-of select ="concat(normalize-space(COL18),$PRANA_SUFFIX_NAME)"/>
								</xsl:when>-->

								<xsl:when test="string-length(COL18) &gt; 15">
									<xsl:value-of select="$varOptionSymbol"/>
								</xsl:when>


								<!--<xsl:when test="COL9!=USD">
									<xsl:call-template name ="GetSuffix">
										<xsl:with-param name ="Suffix" select ="COL9"/>
									</xsl:call-template>
								</xsl:when>-->
								<!--<xsl:when test="COL9=USD">
									<xsl:value-of select="COL5"/>
									
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select ="COL5"/>
								</xsl:otherwise>

							</xsl:choose>

						</Symbol>



						<xsl:variable name ="NetQuantity">
							<xsl:value-of select ="number(COL7)"/>
						</xsl:variable>


						<Quantity>

							<xsl:choose>

								<xsl:when test ="$NetQuantity ">
									<xsl:value-of select ="$NetQuantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>

						<Side>
							<xsl:choose>
								<xsl:when test="string-length(COL18) &gt; 15">
									<xsl:choose>
										<xsl:when test="COL6='L'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="COL6='S'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL6='L'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="COL6='S'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
								
								</xsl:choose>
							
						
						</Side>

						<CurrencySymbol>
							<xsl:value-of select="$PB_SUFFIX_CODE"/>
						</CurrencySymbol>

						<xsl:variable name ="MarkPrice">
							<xsl:value-of select ="number(COL10)"/>
						</xsl:variable>

						<MarkPrice>

							<xsl:choose>

								<xsl:when test ="$MarkPrice &lt;0">
									<xsl:value-of select ="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$MarkPrice &gt;0">
									<xsl:value-of select ="$MarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarkPrice>

						<xsl:variable name ="NetMarketValue">
							<xsl:value-of select ="number(COL13)"/>
						</xsl:variable>

						<MarketValue>

							<xsl:choose>

								<xsl:when test ="$NetMarketValue">
									<xsl:value-of select ="$NetMarketValue"/>
								</xsl:when>

								<!--<xsl:when test ="$NetMarketValue &gt;0">
									<xsl:value-of select ="$NetMarketValue"/>
								</xsl:when>-->

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValue>

						<xsl:variable name ="NetMarketValueBase">
							<xsl:value-of select ="number(COL14)"/>
						</xsl:variable>

						<MarketValueBase>

							<xsl:choose>

								<xsl:when test ="$NetMarketValueBase">
									<xsl:value-of select ="$NetMarketValueBase"/>
								</xsl:when>

								<!--<xsl:when test ="$NetMarketValueBase &gt;0">
									<xsl:value-of select ="$NetMarketValueBase"/>
								</xsl:when>-->

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValueBase>

						<xsl:variable name ="FXRate">
							<xsl:value-of select ="number(COL13) div number(COL14)"/>
						</xsl:variable>

						<FXRate>

							<xsl:choose>

								<xsl:when test ="$FXRate ">
									<xsl:value-of select ="$FXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</FXRate>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
