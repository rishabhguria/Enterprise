<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

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


	<!--<xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="COL18='Option'">

      -->
	<!--</xsl:otherwise>-->
	<!--
      -->
	<!--

			</xsl:choose>-->
	<!--
    </xsl:if>-->
	<!--
  </xsl:template>-->

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL10"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="COL6='Equity Option'">

					<PositionMaster>


						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'KDS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME">
							<xsl:value-of select="COL7"/>
						</xsl:variable>
						
						<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>


						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_CURRENCY_ID">
							<xsl:value-of select="document('../ReconMappingXML/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_SUFFIX_NAME]/@PranaCurrencyID"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_ASSET_ID">
							<xsl:value-of select="document('../ReconMappingXML/AssetMapping.xml')/AssetMapping/PB[@Name=$PB_NAME]/AssetData[@AssetName=$PB_SUFFIX_NAME]/@PranaAssetID"/>
						</xsl:variable>
						
						
						<!--<xsl:variable name="PRANA_EXCHANGE_ID">
							<xsl:value-of select="document('../ReconMappingXML/ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@ExchangeName=$PB_EXCHANGE_NAME]/@PranaExchangeID"/>
						</xsl:variable>-->

						<xsl:variable name ="Asset">
							<xsl:choose>
								<xsl:when test="COL6='Equity Option'">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Symbol" select="normalize-space(COL1)"/>

						<xsl:variable name="UnderlyingSymbol">
				      		<xsl:value-of select="substring-before($Symbol,' ')"/>
						</xsl:variable>
						<xsl:variable name="ExpiryDay">
							<xsl:value-of select="substring-before(substring-after($Symbol,'/'),'/')"/>
						</xsl:variable>
						<xsl:variable name="ExpiryMonth">
						<xsl:choose>
						<xsl:when test="contains(substring-before(substring-after($Symbol,' '),' '),'US')">
						<xsl:value-of select="substring-after(substring-after(substring-before($Symbol,'/'),' '),' ')"/>
							<!--<xsl:value-of select="substring-before(substring-after(substring-after($Symbol,' '),' '),'/')"/>-->
						</xsl:when>
						<xsl:otherwise>
						<xsl:value-of select="substring-after(substring-before($Symbol,'/'),' ')"/>
						</xsl:otherwise>
						</xsl:choose>
							
						</xsl:variable>
						<xsl:variable name="ExpiryYear">
							<xsl:value-of select="substring-before(substring-after(substring-after($Symbol,'/'),'/'),' ')"/>
						</xsl:variable>

						<xsl:variable name="PutORCall">
							<xsl:value-of select="substring(substring-after(substring-after(substring-after($Symbol,'/'),'/'),' '),1,1)"/>
						</xsl:variable>
						<xsl:variable name="StrikePrice">
							<xsl:value-of select="format-number(substring-before(substring(substring-after(substring-after(substring-after($Symbol,'/'),'/'),' '),2),' '),'#.00')"/>
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
					

								<xsl:variable name="ExpiryDay2">
									<xsl:value-of select="substring-before(substring-after(COL1,'/'),'/')"/>
								</xsl:variable>
						
								<xsl:variable name="ExpiryMonth2">
									<xsl:choose>
										<xsl:when test="COL7='EUR'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='GBP'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='HKD'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='CAD'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='SEK'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='CHF'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='KRW'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='JPY'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='INR'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='AUD'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:when test="COL7='TWD'">
											<xsl:value-of select="substring-before(substring-after(COL1,' '),'/')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-after(substring-after(substring-before(COL1,'/'),' '),' ')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
						
								<xsl:variable name="ExpiryYear2">
									<xsl:value-of select="substring-before(substring-after(substring-after(COL1,'/'),'/'),' ')"/>
								</xsl:variable>
						<xsl:variable name="PutCall">
							<xsl:value-of select="substring(substring-after(substring-after(COL1,' '),' '),1,1)"/>
						</xsl:variable>

						<xsl:variable name="varOption">
							<xsl:choose>
								<!--<xsl:when test="contains(substring-before($Symbol,' '),'AEX')">-->
									<xsl:when test="COL7='EUR'">
									<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ','INDX',' ',$ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2,' ',$PutCall,$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="COL7='GBP'">
									<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ','INDX',' ',$ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2,' ',$PutCall,$StrikePrice)"/>
								</xsl:when>
								<xsl:when test="COL7='HKD'">
									<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ','INDX',' ',$ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2,' ',$PutCall,$StrikePrice)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>



						<TickerSymbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>




								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="concat($varOption,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>



								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</TickerSymbol>





						<PutOrCall>
							<xsl:choose>

								<xsl:when test="contains(substring(substring-after(substring-after(substring-after($Symbol,'/'),'/'),' '),1,1),'P')">
									<xsl:value-of select="'0'"/>
								</xsl:when>




								<xsl:when test="contains(substring(substring-after(substring-after(substring-after($Symbol,'/'),'/'),' '),1,1),'C')">
									<xsl:value-of select="'1'"/>
								</xsl:when>



								<xsl:otherwise>
									<xsl:value-of select="'-1'"/>
								</xsl:otherwise>

							</xsl:choose>
						</PutOrCall>


						<Multiplier>
							<xsl:value-of select="100"/>
						</Multiplier>

						<xsl:variable name="ExpiryDay1">
							<xsl:value-of select="substring-before(substring-after($Symbol,'/'),'/')"/>
						</xsl:variable>
						<xsl:variable name="ExpiryMonth1">
							<xsl:value-of select="substring-after(substring-after(substring-before($Symbol,'/'),' '),' ')"/>
						</xsl:variable>
						<xsl:variable name="ExpiryYear1">
							<xsl:value-of select="substring-before(substring-after(substring-after($Symbol,'/'),'/'),' ')"/>
						</xsl:variable>

						<ExpirationDate>
							<xsl:choose>
								<xsl:when test="COL7='EUR'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='GBP'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='HKD'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='AUD'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='INR'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='CAD'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='CHF'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='JPY'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='KRW'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='SEK'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='TWD'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:when test="COL7='USD'">
									<xsl:value-of select="concat($ExpiryMonth2,'/',$ExpiryDay2,'/',$ExpiryYear2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($ExpiryMonth1,'/',$ExpiryDay1,'/',$ExpiryYear1)"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select="COL3"/>-->

						</ExpirationDate>

						<UnderLyingSymbol>
							<xsl:value-of select="$UnderlyingSymbol"/>
						</UnderLyingSymbol>




						<StrikePrice>
							<xsl:choose>
								<xsl:when test="number($StrikePrice)">
									<xsl:value-of select="$StrikePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StrikePrice>

					
						


						<xsl:variable name="varCurrency">
							<xsl:choose>
								<xsl:when test ="COL7='USD'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="COL7='HKD'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="COL7='JPY'">
									<xsl:value-of select ="'3'"/>
								</xsl:when>
								<xsl:when test ="COL7='GBP'">
									<xsl:value-of select ="'4'"/>
								</xsl:when>
								<xsl:when test ="COL7='CAD'">
									<xsl:value-of select ="'7'"/>
								</xsl:when>
								<xsl:when test ="COL7='EUR'">
									<xsl:value-of select ="'8'"/>
								</xsl:when>
								<xsl:when test ="COL7='SEK'">
									<xsl:value-of select ="'13'"/>
								</xsl:when>

								<xsl:when test ="COL7='AUD'">
									<xsl:value-of select ="'14'"/>
								</xsl:when>
																
								<xsl:when test ="COL7='KRW'">
									<xsl:value-of select ="'16'"/>
								</xsl:when>
								
								<xsl:when test ="COL7='INR'">
									<xsl:value-of select ="'21'"/>
								</xsl:when>
								<xsl:when test ="COL7='CHF'">
									<xsl:value-of select ="'23'"/>
								</xsl:when>
								
								<xsl:when test ="COL7='TWD'">
									<xsl:value-of select ="'46'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExchange">
							<xsl:choose>
								<xsl:when test ="COL7='USD'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								<xsl:when test ="COL7='HKD'">
									<xsl:value-of select ="'137'"/>
								</xsl:when>
								<xsl:when test ="COL7='JPY'">
									<xsl:value-of select ="'139'"/>
								</xsl:when>
								<xsl:when test ="COL7='GBP'">
									<xsl:value-of select ="'136'"/>
								</xsl:when>
								<xsl:when test ="COL7='CAD'">
									<xsl:value-of select ="'134'"/>
								</xsl:when>
								<xsl:when test ="COL7='EUR'">
									<xsl:value-of select ="'94'"/>
								</xsl:when>
								<xsl:when test ="COL7='SEK'">
									<xsl:value-of select ="'141'"/>
								</xsl:when>

								<xsl:when test ="COL7='AUD'">
									<xsl:value-of select ="'133'"/>
								</xsl:when>

								<xsl:when test ="COL7='KRW'">
									<xsl:value-of select ="'140'"/>
								</xsl:when>

								<xsl:when test ="COL7='INR'">
									<xsl:value-of select ="'138'"/>
								</xsl:when>
								<xsl:when test ="COL7='CHF'">
									<xsl:value-of select ="'135'"/>
								</xsl:when>

								<xsl:when test ="COL7='TWD'">
									<xsl:value-of select ="'142'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="COL7='USD' or COL7='CAD'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL7='EUR' or COL7='GBP' or COL7='SEK' or COL7='CHF'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL7='KRW' or COL7='JPY' or COL7='INR' or COL7='HKD' or COL7='TWD'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL7='AUD'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<AssetID>
							<xsl:value-of select="2"/>
						</AssetID>
						
						<UnderLyingID>
							<xsl:value-of select="$varUnderlying"/>
						</UnderLyingID>


						<CurrencyID>
							<xsl:value-of select="$varCurrency"/>							
						</CurrencyID>

						<ExchangeID>
							
							<xsl:value-of select="$varExchange"/>
						</ExchangeID>
						

						<LongName>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</LongName>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>