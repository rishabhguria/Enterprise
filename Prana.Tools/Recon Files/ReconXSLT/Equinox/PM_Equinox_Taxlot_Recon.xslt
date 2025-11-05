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

	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>

		<xsl:if test="$PutOrCall='C'">

			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>

				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>

				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>

				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>

				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>

				<xsl:when test="$Month=6">
					<xsl:value-of select="'F'"/>
				</xsl:when>

				<xsl:when test="$Month=7 ">
					<xsl:value-of select="'G'"/>
				</xsl:when>

				<xsl:when test="$Month=8 ">
					<xsl:value-of select="'H'"/>
				</xsl:when>

				<xsl:when test="$Month=9 ">
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

		<xsl:if test="COL9='Options'">

			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL3,'1')"/>
			</xsl:variable>

			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(COL3,$UnderlyingSymbol),1,2)"/>
			</xsl:variable>

			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(COL3,$UnderlyingSymbol),3,2)"/>
			</xsl:variable>

			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(COL3,$UnderlyingSymbol),5,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(COL3,$UnderlyingSymbol),7,1)"/>
			</xsl:variable>

			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(COL3,$UnderlyingSymbol),8),'#.00')"/>
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

			<!--<xsl:variable name="ThirdFriday">
				<xsl:choose>

					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>

				</xsl:choose>
			</xsl:variable>-->

			<!--<xsl:choose>

				<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
					<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
				</xsl:when>

				<xsl:otherwise>-->
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
			<!--</xsl:otherwise>

			</xsl:choose>-->
		</xsl:if>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL3"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Pershing'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL6"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="PB_SUFFIX_NAME">
							<xsl:value-of select="substring-after(COL5,'_')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test="contains(COL1,'COMMON STOCK')">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						<AssetType>
							<xsl:value-of select="$varAssetType"/>
						</AssetType>

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test ="contains(COL5,'_')">
									<xsl:value-of select="substring-before(COL5,'_')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="translate($PRANA_SYMBOL_NAME,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="translate(concat($Symbol,$PRANA_SUFFIX_NAME),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="translate($PB_SYMBOL_NAME,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>



						<xsl:variable name="PB_FUND_NAME" select="''"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<Quantity>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>
							</xsl:choose>							
						</Quantity>

						<xsl:variable name="Side" select="COL3"/>
						<Side>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

					
						<TradeDate>

							<xsl:value-of select="COL2"/>
						</TradeDate>


						

						<xsl:variable name="varMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
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
						</MarketValue>


						<xsl:variable name="varMarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number($varMarketValueBase)">
									<xsl:value-of select="$varMarketValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>


						<xsl:variable name="varNetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValue &gt; 0">
									<xsl:value-of select="$varNetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValue &lt; 0">
									<xsl:value-of select="$varNetNotionalValue * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>


						<xsl:variable name="varNetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValueBase &gt; 0">
									<xsl:value-of select="$varNetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValueBase &lt; 0">
									<xsl:value-of select="$varNetNotionalValueBase * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>
						
						<!--<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>-->

						<xsl:variable name="CompanyName" select="COL6"/>

						<CompanyName>
							<xsl:value-of select="$CompanyName"/>
						</CompanyName>
						

					

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>
