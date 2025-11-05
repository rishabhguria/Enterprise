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

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
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
				<xsl:when test="$Month=07 ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08 ">
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

	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>
		<xsl:variable name="varUSymbol">
			<xsl:choose>
				<xsl:when test="substring($varBBCode,2,1) = '1'">
					<xsl:value-of select="substring-before($varBBCode,'1')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,2,1) = '2'">
					<xsl:value-of select="substring-before($varBBCode,'2')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,3,1) = '1'">
					<xsl:value-of select="substring-before($varBBCode,'1')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,3,1) = '2'">
					<xsl:value-of select="substring-before($varBBCode,'2')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,4,1) = '1'">
					<xsl:value-of select="substring-before($varBBCode,'1')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,4,1) = '2'">
					<xsl:value-of select="substring-before($varBBCode,'2')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,5,1) = '1'">
					<xsl:value-of select="substring-before($varBBCode,'1')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,5,1) = '2'">
					<xsl:value-of select="substring-before($varBBCode,'2')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,6,1) = '1'">
					<xsl:value-of select="substring-before($varBBCode,'1')"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,6,1) = '2'">
					<xsl:value-of select="substring-before($varBBCode,'2')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutORCall">
			<xsl:value-of select="substring(substring-after($varBBCode,($varUSymbol)),7,1)"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring(substring-after($varBBCode,($varUSymbol)),1,2)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">
			<xsl:value-of select="format-number(substring(substring-after($varBBCode,$varUSymbol),8) div 1000, '#.00')"/>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring(substring-after($varBBCode,($varUSymbol)),5,2)"/>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring(substring-after($varBBCode,($varUSymbol)),3,2)"/>
		</xsl:variable>


		<xsl:variable name="MonthCodeVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$varMonthCode"/>
				<xsl:with-param name="PutOrCall" select="$varPutORCall"/>
			</xsl:call-template>
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
		<xsl:value-of select="normalize-space(concat('O:',$varUSymbol,' ', $varExYear,$MonthCodeVar,$varStrike,'D',$varExpiryDay))"/>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="Asset" select="normalize-space(COL16)"/>

				<xsl:if test ="number($Position) and ($Asset!='Cash' or not(contains($Asset,'Payables')))">

					<PositionMaster>

						<xsl:variable name="PB_NAME" select="'MS'"/>

						<xsl:variable name="PB_SYMBOL_NAME" select="COL15"/>


						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="concat(COL1,COL18)"/>
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

						<xsl:variable name="PB_SUFFIX" select="COL12"/>


						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX]/@PranaSuffixCode"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="contains(COL16,'Options')">
									<xsl:call-template name="ConvertBBCodetoTicker">
										<xsl:with-param name="varBBCode" select="COL3"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:when test="COL16!='Options'">
									<xsl:choose>
										<xsl:when test="contains(COL3,'.')">
											<xsl:value-of select="concat(substring-before(COL3,'.'),$PRANA_SUFFIX_NAME)"/>		
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(normalize-space(COL3),$PRANA_SUFFIX_NAME)"/>
										</xsl:otherwise>
									</xsl:choose>
									
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>
						<!--<xsl:variable name="Underlying" select="substring-before(COL3,'1')"/>
						<xsl:variable name="undspaces">
							<xsl:call-template name="spaces">
								<xsl:with-param name="count" select="number(5 - string-length($Underlying))"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="IdcoLast" select="substring(COL3,string-length($Underlying)+1)"/>
						<xsl:variable name="Idco">
							<xsl:value-of select="concat($Underlying,$undspaces,normalize-space($IdcoLast),'U')"/>
						</xsl:variable>


						<IDCOOptionSymbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:when test="contains(COL16,'Options')">
									--><!--<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL3"/>
									</xsl:call-template>--><!--
									<xsl:value-of select="$Idco"/>
								</xsl:when>								

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</IDCOOptionSymbol>-->
						
						

						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>

								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>

								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
							<!--</xsl:otherwise>
							</xsl:choose>-->
						</MarkPrice>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($Position)">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="Side" select="normalize-space(COL4)"/>

						<Side>
							<xsl:choose>
								<xsl:when test="$Side='L' and contains($Asset,'Options')">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$Side='S' and contains($Asset,'Options')">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when test="$Side='L' and not(contains($Asset,'Options'))">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side='S' and not(contains($Asset,'Options'))">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<!--<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>-->

						<CurrencySymbol>
							<xsl:value-of select="normalize-space(COL13)"/>
						</CurrencySymbol>

						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="number($MarketValue)">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<xsl:variable name="MarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number($MarketValueBase)">
									<xsl:value-of select="$MarketValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

						<SMRequest>
							<xsl:value-of select="'true'"/>
						</SMRequest>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

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


