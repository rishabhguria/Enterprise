<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<!-- Put = 0,Call = 1 , Here First call/put code then 2 characters for month code -->
		<!-- Call month Codes e.g. 101 represents 1=Call, 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth=101">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=102">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=103">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=104">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=105">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=106">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=107">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=108">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=109">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=110">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=111">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=112">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<!-- Put month Codes e.g. 001 represents 0=Put, 01 = January-->
			<xsl:when test ="$varMonth=001">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=002">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=003">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=004">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=005">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=006">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=007">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=008">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=009">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=010">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=011">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=012">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="tempCurrencyCode">
		<xsl:param name="paramCurrencySymbol"/>
		<!-- 1 characters for metal code -->
		<!--  e.g. A represents A = aluminium-->
		<xsl:choose>
			<xsl:when test ="$paramCurrencySymbol='USD'">
				<xsl:value-of select ="'1'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='HKD'">
				<xsl:value-of select ="'2'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='JPY'">
				<xsl:value-of select ="'3'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GBP'">
				<xsl:value-of select ="'4'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='AED'">
				<xsl:value-of select ="'5'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BRL'">
				<xsl:value-of select ="'6'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CAD'">
				<xsl:value-of select ="'7'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='EUR'">
				<xsl:value-of select ="'8'"/>
			</xsl:when>

			<xsl:when test ="$paramCurrencySymbol='NOK'">
				<xsl:value-of select ="'9'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SGD'">
				<xsl:value-of select ="'10'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MUL'">
				<xsl:value-of select ="'11'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ZAR'">
				<xsl:value-of select ="'12'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SEK'">
				<xsl:value-of select ="'13'"/>
			</xsl:when>

			<xsl:when test ="$paramCurrencySymbol='AUD'">
				<xsl:value-of select ="'14'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CNY'">
				<xsl:value-of select ="'15'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='KRW'">
				<xsl:value-of select ="'16'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='BDT'">
				<xsl:value-of select ="'17'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='THB'">
				<xsl:value-of select ="'18'"/>
			</xsl:when>

			<xsl:when test ="$paramCurrencySymbol='dong'">
				<xsl:value-of select ="'19'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GBX'">
				<xsl:value-of select ="'20'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='INR'">
				<xsl:value-of select ="'21'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CHF'">
				<xsl:value-of select ="'23'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CLP'">
				<xsl:value-of select ="'24'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='COP'">
				<xsl:value-of select ="'25'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='CZK'">
				<xsl:value-of select ="'26'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='DKK'">
				<xsl:value-of select ="'27'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GHS'">
				<xsl:value-of select ="'28'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='HUF'">
				<xsl:value-of select ="'29'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='IDR'">
				<xsl:value-of select ="'30'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ILS'">
				<xsl:value-of select ="'31'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ISK'">
				<xsl:value-of select ="'32'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='KZT'">
				<xsl:value-of select ="'33'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='LVL'">
				<xsl:value-of select ="'34'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MXN'">
				<xsl:value-of select ="'35'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='NZD'">
				<xsl:value-of select ="'36'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PEN'">
				<xsl:value-of select ="'37'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PLN'">
				<xsl:value-of select ="'38'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='RON'">
				<xsl:value-of select ="'40'"/>
			</xsl:when>


			<xsl:when test ="$paramCurrencySymbol='RUB'">
				<xsl:value-of select ="'41'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SKK'">
				<xsl:value-of select ="'42'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='TRY'">
				<xsl:value-of select ="'43'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='ARS'">
				<xsl:value-of select ="'44'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='UYU'">
				<xsl:value-of select ="'45'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='TWD'">
				<xsl:value-of select ="'46'"/>
			</xsl:when>


			<xsl:when test ="$paramCurrencySymbol='BMD'">
				<xsl:value-of select ="'47'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='EEK'">
				<xsl:value-of select ="'48'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='GEL'">
				<xsl:value-of select ="'49'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='MYR'">
				<xsl:value-of select ="'51'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='SIT'">
				<xsl:value-of select ="'52'"/>
			</xsl:when>

			<xsl:when test ="$paramCurrencySymbol='XAF'">
				<xsl:value-of select ="'53'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='XOF'">
				<xsl:value-of select ="'54'"/>
			</xsl:when>

			<xsl:when test ="$paramCurrencySymbol='AZN'">
				<xsl:value-of select ="'55'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PKR'">
				<xsl:value-of select ="'56'"/>
			</xsl:when>
			<xsl:when test ="$paramCurrencySymbol='PHP'">
				<xsl:value-of select ="'57'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="tempCouponFrequencyID">
		<xsl:param name="paramCouponFrequencyID"/>
		<xsl:choose>
			<xsl:when test ="$paramCouponFrequencyID='12'">
				<xsl:value-of select ="'0'"/>
			</xsl:when>
			<xsl:when test ="$paramCouponFrequencyID='4'">
				<xsl:value-of select ="'1'"/>
			</xsl:when>
			<xsl:when test ="$paramCouponFrequencyID='2'">
				<xsl:value-of select ="'2'"/>
			</xsl:when>
			<xsl:when test ="$paramCouponFrequencyID='1'">
				<xsl:value-of select ="'3'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="'4'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

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

				<xsl:if test="number(COL13)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'FUCHS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>



						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL1,'Equity Option')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'Equity')">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'Future')">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'Future Option')">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FX')">
									<xsl:value-of select="'Fx'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FX Forward')">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'Private Equity')">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'Equity Swap')">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'Fixed Income')">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<TickerSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="COL5"/>
								</xsl:when>

								<xsl:when test="$Asset='Fx'">
									<xsl:value-of select="COL5"/>
								</xsl:when>

								<xsl:when test="$Asset='FXForward'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='PrivateEquity'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL5"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</TickerSymbol>

						
						<xsl:variable name="Exchange">
							<xsl:choose>
								<xsl:when test="COL3='HKG'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL3='OPTS'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL3='NYME'">
									<xsl:value-of select="78"/>
								</xsl:when>
								<xsl:when test="COL3='CMEE'">
									<xsl:value-of select="22"/>
								</xsl:when>
								<xsl:when test="COL3='FX'">
									<xsl:value-of select="30"/>
								</xsl:when>
								<xsl:when test="COL3='OTC'">
									<xsl:value-of select="77"/>
								</xsl:when>
								<xsl:when test="COL3='ASX'">
									<xsl:value-of select="61"/>
								</xsl:when>
								
							</xsl:choose>
						</xsl:variable>
						<ExchangeID>
							<xsl:value-of select="$Exchange"/>
						</ExchangeID>

						<xsl:variable name="UnderLying">
							<xsl:choose>
								<xsl:when test="COL2='AsiaXJapan'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL2='US'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL2='Multiple'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL2='Australia'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL2='EmergingDebt'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<UnderLyingID>
							<xsl:value-of select="$UnderLying"/>
						</UnderLyingID>

						<xsl:variable name="AsssetI">
							<xsl:choose>
								<xsl:when test="COL1='Equity'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL1='Equity Option'">
										<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL1='Future'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL1='Future Option'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL1='FX'">
                                     <xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL1='FX Forward'">
									<xsl:value-of select="11"/>
								</xsl:when>
								<xsl:when test="COL1='Private Equity'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL1='Equity Swap'">
									<xsl:value-of select="13"/>
								</xsl:when>
								<xsl:when test="COL1='Fixed Income'">
									<xsl:value-of select="8"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<AssetID>
							<xsl:value-of select="$AsssetI"/>
						</AssetID>

						<xsl:variable name="CurrencyI">
							<xsl:choose>
								<xsl:when test="COL4='HKD'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL4='USD'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL4='AUD'">
									<xsl:value-of select="14"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<CurrencyID>
							<xsl:value-of select="$CurrencyI"/>
						</CurrencyID>
						
						<UnderLyingSymbol>
							<xsl:value-of select ="COL14"/>
						</UnderLyingSymbol>
						<xsl:variable name="StrikePrice">
							<xsl:value-of select="COL16"/>
						</xsl:variable>
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
						<!--<CusipSymbol>
							<xsl:value-of select="COL8"/>
						</CusipSymbol>
						<SedolSymbol>
							<xsl:value-of select="COL10"/>
						</SedolSymbol>s

						<ISINSymbol>
							<xsl:value-of select="COL9"/>
						</ISINSymbol>-->
						<IssueDate>
							
							
							<xsl:value-of select="COL17"/>
						</IssueDate>

						<DaysToSettlement>
							<xsl:value-of select="365"/>
						</DaysToSettlement>
						<FirstCouponDate>
							<xsl:value-of select="COL18"/>
						</FirstCouponDate>
						<ExpirationDate>
							<xsl:value-of select="COL19"/>
						</ExpirationDate>
						<Multiplier>
							<xsl:value-of select="COL13"/>
						</Multiplier>

						<xsl:variable name="Lead">
							<xsl:choose>
								<xsl:when test="COL26='MXN'">
									<xsl:value-of select="35"/>
								</xsl:when>

								<xsl:when test="COL26='JPY'">
									<xsl:value-of select="3"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						
						
						<LeadCurrencyID>
							<!--<xsl:value-of select="$Lead"/>-->
							<xsl:choose>
								<xsl:when test="contains(COL26,'MXN')">
									<xsl:value-of select="35"/>
								</xsl:when>

								<xsl:when test="contains(COL26,'JPY')">
									<xsl:value-of select="3"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</LeadCurrencyID>
						
						<xsl:variable name="VS">
							<xsl:choose>
								<xsl:when test="COL27='USD'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<VsCurrencyID>
							<xsl:value-of select="$VS"/>
							
						</VsCurrencyID>

						<PutOrCall>
							<xsl:choose>

								<xsl:when test="contains(substring(substring-after(substring-after(substring-after(COL6,'/'),'/'),' '),1,1),'C') and contains(COL5,'O:')">
									<xsl:value-of select="'0'"/>
								</xsl:when>




								<xsl:when test="contains(substring(substring-after(substring-after(substring-after(COL6,'/'),'/'),' '),1,1),'P') and contains(COL5,'O:')">
									<xsl:value-of select="'1'"/>
								</xsl:when>


 
								<xsl:otherwise>
									<xsl:value-of select="'-1'"/>
								</xsl:otherwise>

							</xsl:choose>
						</PutOrCall>
					
						
						

						<LongName>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</LongName>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>