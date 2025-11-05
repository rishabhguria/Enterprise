<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime firstWednesday= new DateTime(year, month, 1);
		while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
		{
		firstWednesday = firstWednesday.AddDays(1);
		}
		return firstWednesday.ToString();
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



	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring(substring-after(substring-after(substring-after(COL4,'/'),'/'),' '),1,1),'P') or contains(substring(substring-after(substring-after(substring-after(COL4,'/'),'/'),' '),1,1),'C')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL4,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL4,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL4),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-before(substring-after(substring-after(COL4,'/'),'/'),' ')"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(substring-after(substring-after(COL4,'/'),'/'),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after(COL4,'/'),'/'),' '),2),'##.00')"/>
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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL10"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<xsl:variable name="varCurrency">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<!--<xsl:when test="COL12='ETF' or COL12='COMMON' or COL12='INTEREST_RATE_SWAP'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL12='FUTURES'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="COL12='OPTION' and COL34='FUTURES'">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>
								<xsl:when test="COL12='OPTION' and COL34='INDEX'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL12='GOVT BOND' and (COL34='' or COL43='*')">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>-->
								<xsl:when test="COL='INTEREST_RATE_SWAP' and COL='INTEREST'">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="Root_Code">
							<xsl:choose>
								<!--<xsl:when test="string-length(COL19)= 5 and $Asset='FutureOption'">
									<xsl:value-of select="substring(COL19,1,2)"/>
								</xsl:when>-->

								<xsl:when test="string-length(substring-before(normalize-space(COL19),' '))= 5">
									<xsl:value-of select="substring(substring-before(normalize-space(COL7),' '),1,3)"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(normalize-space(COL19),' '))= 4">
									<xsl:value-of select="substring(substring-before(normalize-space(COL7),' '),1,2)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="MONTH_CODE">
							<xsl:choose>
								<xsl:when test="string-length(COL19)= 5">
									<xsl:value-of select="substring(COL19,4,1)"/>
								</xsl:when>
								<xsl:when test="string-length(COL19)= 4">
									<xsl:value-of select="substring(COL19,3,1)"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),1,1)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>


						<xsl:variable name ="Year">
							<xsl:choose>
								<xsl:when test="string-length(normalize-space(COL19))= 6 and substring(COL19,6,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,6,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 6">
									<xsl:value-of select="substring(COL19,6,1)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 5 and substring(COL19,5,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,5,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 5">
									<xsl:value-of select="substring(COL19,5,1)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 4 and substring(COL19,4,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,4,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 4">
									<xsl:value-of select="substring(COL19,4,1)"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 3 and substring(COL19,3,1)='8'">
									<xsl:value-of select="concat('1',substring(COL19,3,1))"/>
								</xsl:when>
								<xsl:when test="string-length(normalize-space(COL19))= 3">
									<xsl:value-of select="substring(COL19,3,1)"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ') and substring(substring-after(COL19,' '),2,1)='8'">
									<xsl:value-of select="concat('1',substring(substring-after(COL19,' '),2,1))"/>
								</xsl:when>
								<xsl:when test="contains(COL19,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),2,1)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_UNDERLYING_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]/@UnderlyingCode"/>
						</xsl:variable>


						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_Multiplier">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]//@PriceMul"/>
						</xsl:variable>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
									<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Root_Code"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="varExchange">
							<xsl:choose>
								<xsl:when test="$PRANA_EXCHANGE_NAME!=''">
									<xsl:value-of select="$PRANA_EXCHANGE_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFutStrickPrice">
							<xsl:value-of select="substring-before(substring-after(COL14,' '),' ')"/>
						</xsl:variable>


						<xsl:variable name="PRANA_STRIKE_MULTIPLIER">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$Root_Code and @Currency=$varCurrency]/@StrikeMul"/>
						</xsl:variable>

						<xsl:variable name="varFutOptStrikePrice">
							<xsl:choose>
								<xsl:when test="$PRANA_STRIKE_MULTIPLIER!=''">
									<xsl:value-of select="format-number(($varFutStrickPrice )* ($PRANA_STRIKE_MULTIPLIER),'0.##')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varFutStrickPrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="varFUTUnderlyng">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(normalize-space(COL7),' '))= 5">
									<xsl:value-of select="substring(substring-before(normalize-space(COL7),' '),1,2)"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(normalize-space(COL7),' '))= 4">
									<xsl:value-of select="substring(substring-before(normalize-space(COL7),' '),1,2)"/>
								</xsl:when>
								<!--<xsl:when test="contains(COL29,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),2,1)"/>
								</xsl:when>-->
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFutCode">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL7,' '))= 5 and substring(substring-before(normalize-space(COL7),' '),5,1)='8'">
									<xsl:value-of select="concat(substring(substring-before(normalize-space(COL7),' '),4,1),'1',substring(substring-before(normalize-space(COL7),' '),5,1))"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(COL7,' '))= 5">
									<xsl:value-of select="substring(substring-before(normalize-space(COL7),' '),3)"/>
								</xsl:when>

								<xsl:when test="string-length(substring-before(COL7,' '))= 4 and substring(substring-before(normalize-space(COL7),' '),4,1)='8'">
									<xsl:value-of select="concat(substring(substring-before(normalize-space(COL7),' '),3,1),'1',substring(substring-before(normalize-space(COL7),' '),4,1))"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(COL7,' '))= 4">
									<xsl:value-of select="substring(substring-before(normalize-space(COL7),' '),3)"/>
								</xsl:when>
								<!--<xsl:when test="contains(COL19,' ')">
									<xsl:value-of select="substring(substring-after(COL19,' '),2,1)"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="substring(substring-before(normalize-space(COL7),' '),3)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>





						<xsl:variable name="Symbol">
							<xsl:value-of select="COL7"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<!--<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name ="Option">
										<xsl:with-param name="Symbol" select="COL4"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>-->


								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="concat($varFUTUnderlyng,' ',$varFutCode)"/>
								</xsl:when>



								<!--<xsl:when test="$Asset='FixdIncom'">
									<xsl:value-of select="COL6"/>
								</xsl:when>-->


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>



						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL3)"/>
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
						

						<xsl:variable name="varQuantity">
							<xsl:choose>
								<xsl:when test="$Asset='FixdIncom'">
									<xsl:value-of select="$Quantity div 1000"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Quantity"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						
						<Quantity>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>
						
						
						<xsl:variable name="varAvgPrice">
							<xsl:choose>

								<xsl:when test ="$AvgPrice &lt;0">
									<xsl:value-of select ="$AvgPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$AvgPrice &gt;0">
									<xsl:value-of select ="$AvgPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="number($PRANA_Multiplier)">
									<xsl:value-of select="$varAvgPrice * $PRANA_Multiplier"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>


						<xsl:variable name="Side" select="COL9"/>
						<Side>


							<xsl:choose>
								<xsl:when test="$Side ='B'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$Side ='S'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<TradeDate>
							<xsl:value-of select="COL2"/>
						</TradeDate>


						<xsl:variable name="COL18">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL18"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL16">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>



						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="COL23">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL23"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL21">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="TaxOnCommissions">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL19"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>

							<xsl:choose>
								<xsl:when test="$TaxOnCommissions &gt; 0">
									<xsl:value-of select="$TaxOnCommissions"/>
								</xsl:when>

								<xsl:when test="$TaxOnCommissions &lt; 0">
									<xsl:value-of select="$TaxOnCommissions * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</NetNotionalValue>

						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL18"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValueBase>

							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>

								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</NetNotionalValueBase>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>