<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

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


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:if test="contains(COL5,'/SWAP')">

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

								<xsl:when test="contains(COL1,'EQUITYOPTION') or contains(COL1,'Equity Option')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'EQUITY') or contains(COL1,'Equity')">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FUTURE') or contains(COL1,'Future')">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'EQUITYOPTION') or contains(COL1,'FutuerOption')">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FX') or contains(COL1,'Fx')">
									<xsl:value-of select="'Fx'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'CASH') or contains(COL1,'Cash')">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'INDICES') or contains(COL1,'Indices')">
									<xsl:value-of select="'Indices'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FXFORWARD') or contains(COL1,'FXForward')">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'PRIVATEEQUITY') or contains(COL1,'Private Equity')">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'EQUITYSWAP') or contains(COL1,'Equity Swap')">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FIXEDINCOME') or contains(COL1,'Fixed Income')">
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
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='Equity'">
									<xsl:choose>
										<xsl:when test="contains(COL5,'/SWAP')">
											<xsl:value-of select="substring-before(COL5,'/')"/>
										</xsl:when>
										<xsl:when test="contains(COL5,'/CFD')">
											<xsl:value-of select="substring-before(COL5,'/')"/>
										</xsl:when>
									</xsl:choose>

								</xsl:when>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="COL5"/>
								</xsl:when>

								<xsl:when test="$Asset='Fx'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='Cash'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='Indices'">
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


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</TickerSymbol>

						<xsl:variable name="Exchange">
							<xsl:choose>
								<xsl:when test="COL3='HKF'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL3='TASE'">
									<xsl:value-of select="154"/>
								</xsl:when>
								<xsl:when test="COL3='DBGI'">
									<xsl:value-of select="159"/>
								</xsl:when>
								<xsl:when test="COL3='MSE'">
									<xsl:value-of select="95"/>
								</xsl:when>
								<xsl:when test="COL3='DBGI'">
									<xsl:value-of select="96"/>
								</xsl:when>
								<xsl:when test="COL3='STX'">
									<xsl:value-of select="95"/>
								</xsl:when>
								<xsl:when test="COL3='MSE'">
									<xsl:value-of select="101"/>
								</xsl:when>
								<xsl:when test="COL3='PHS'">
									<xsl:value-of select="150"/>
								</xsl:when>
								<xsl:when test="COL3='TUR'">
									<xsl:value-of select="158"/>
								</xsl:when>
								<xsl:when test="COL3='WSE'">
									<xsl:value-of select="156"/>
								</xsl:when>
								<xsl:when test="COL3='GTS'">
									<xsl:value-of select="157"/>
								</xsl:when>
								<!--<xsl:when test="COL3='TUR'">
									<xsl:value-of select="98"/>
								</xsl:when>-->
								<xsl:when test="COL3='OPTS'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL3='NYME'">
									<xsl:value-of select="78"/>
								</xsl:when>
								<xsl:when test="COL3='CMEE'">
									<xsl:value-of select="22"/>
								</xsl:when>
								<xsl:when test="COL3='SES'">
									<xsl:value-of select="31"/>
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
								<xsl:when test="contains(COL3,'NASDAQ')">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL3='NYSE'">
									<xsl:value-of select="21"/>
								</xsl:when>
								<xsl:when test="COL3='CMEE'">
									<xsl:value-of select="22"/>
								</xsl:when>
								<xsl:when test="COL3='CMG'">
									<xsl:value-of select="23"/>
								</xsl:when>
								<xsl:when test="COL3='AMEX'">
									<xsl:value-of select="24"/>
								</xsl:when>
								<xsl:when test="COL3='TPX'">
									<xsl:value-of select="25"/>
								</xsl:when>
								<xsl:when test="COL3='OSM'">
									<xsl:value-of select="27"/>
								</xsl:when>
								<xsl:when test="COL3='JAQ'">
									<xsl:value-of select="28"/>
								</xsl:when>
								<xsl:when test="COL3='Bovespa'">
									<xsl:value-of select="29"/>
								</xsl:when>
								<xsl:when test="COL3='Oslo'">
									<xsl:value-of select="32"/>
								</xsl:when>
								<xsl:when test="COL3='FRA'">
									<xsl:value-of select="33"/>
								</xsl:when>
								<xsl:when test="COL3='BER'">
									<xsl:value-of select="34"/>
								</xsl:when>
								<xsl:when test="COL3='SEHK'">
									<xsl:value-of select="36"/>
								</xsl:when>
								<xsl:when test="COL3='GEM'">
									<xsl:value-of select="37"/>
								</xsl:when>
								<xsl:when test="COL3='SESDAQ'">
									<xsl:value-of select="38"/>
								</xsl:when>
								<xsl:when test="COL3='XETRA'">
									<xsl:value-of select="39"/>
								</xsl:when>
								<xsl:when test="COL3='HAN'">
									<xsl:value-of select="40"/>
								</xsl:when>
								<xsl:when test="COL3='Bremen Stock Exchange'">
									<xsl:value-of select="41"/>
								</xsl:when>
								<xsl:when test="COL3='Bavarian Stock Exchange'">
									<xsl:value-of select="42"/>
								</xsl:when>
								<xsl:when test="COL3='Euronext'">
									<xsl:value-of select="43"/>
								</xsl:when>
								<xsl:when test="COL3='VSE'">
									<xsl:value-of select="44"/>
								</xsl:when>
								<xsl:when test="COL3='BEL'">
									<xsl:value-of select="45"/>
								</xsl:when>
								<xsl:when test="COL3='CHP'">
									<xsl:value-of select="46"/>
								</xsl:when>
								<xsl:when test="COL3='HEX'">
									<xsl:value-of select="47"/>
								</xsl:when>
								<xsl:when test="COL3='ISE'">
									<xsl:value-of select="48"/>
								</xsl:when>
								<xsl:when test="COL3='ENAM'">
									<xsl:value-of select="49"/>
								</xsl:when>
								<xsl:when test="COL3='ENLI'">
									<xsl:value-of select="50"/>
								</xsl:when>
								<xsl:when test="COL3='SHSE'">
									<xsl:value-of select="62"/>
								</xsl:when>
								<xsl:when test="COL3='ASX'">
									<xsl:value-of select="61"/>
								</xsl:when>
								<xsl:when test="COL3='PS'">
									<xsl:value-of select="60"/>
								</xsl:when>
								<xsl:when test="COL3='BB'">
									<xsl:value-of select="59"/>
								</xsl:when>
								<xsl:when test="COL3='OMX'">
									<xsl:value-of select="58"/>
								</xsl:when>
								<xsl:when test="COL3='RTSX'">
									<xsl:value-of select="57"/>
								</xsl:when>
								<xsl:when test="COL3='SPBEX'">
									<xsl:value-of select="56"/>
								</xsl:when>
								<xsl:when test="COL3='MIL'">
									<xsl:value-of select="55"/>
								</xsl:when>
								<xsl:when test="COL3='ASE'">
									<xsl:value-of select="54"/>
								</xsl:when>
								<xsl:when test="COL3='SWX'">
									<xsl:value-of select="53"/>
								</xsl:when>
								<xsl:when test="COL3='CBOE'">
									<xsl:value-of select="52"/>
								</xsl:when>
								<xsl:when test="COL3='JSE'">
									<xsl:value-of select="51"/>
								</xsl:when>
								<xsl:when test="COL3='KSE'">
									<xsl:value-of select="63"/>
								</xsl:when>
								<xsl:when test="COL3='DSE'">
									<xsl:value-of select="64"/>
								</xsl:when>
								<xsl:when test="COL3='SET'">
									<xsl:value-of select="65"/>
								</xsl:when>
								<xsl:when test="COL3='STC'">
									<xsl:value-of select="66"/>
								</xsl:when>
								<xsl:when test="COL3='KOSDAQ'">
									<xsl:value-of select="67"/>
								</xsl:when>
								<xsl:when test="COL3='LSEP'">
									<xsl:value-of select="68"/>
								</xsl:when>
								<xsl:when test="COL3='NSE'">
									<xsl:value-of select="69"/>
								</xsl:when>
								<xsl:when test="COL3='BSE'">
									<xsl:value-of select="70"/>
								</xsl:when>
								<xsl:when test="COL3='TSX'">
									<xsl:value-of select="71"/>
								</xsl:when>
								<xsl:when test="COL3='MFE'">
									<xsl:value-of select="86"/>
								</xsl:when>
								<xsl:when test="COL3='SGX'">
									<xsl:value-of select="85"/>
								</xsl:when>
								<xsl:when test="COL3='SNFE'">
									<xsl:value-of select="84"/>
								</xsl:when>
								<xsl:when test="COL3='DTB'">
									<xsl:value-of select="83"/>
								</xsl:when>
								<xsl:when test="COL3='LIFFE'">
									<xsl:value-of select="82"/>
								</xsl:when>
								<xsl:when test="COL3='CBOT'">
									<xsl:value-of select="80"/>
								</xsl:when>
								<xsl:when test="COL3='eCBOT' or COL3='ECBOT'">
									<xsl:value-of select="81"/>
								</xsl:when>
								<xsl:when test="COL3='OTC'">
									<xsl:value-of select="77"/>
								</xsl:when>
								<xsl:when test="COL3='NYME'">
									<xsl:value-of select="78"/>
								</xsl:when>
								<xsl:when test="COL3='COMX'">
									<xsl:value-of select="79"/>
								</xsl:when>
								<xsl:when test="COL3='JKT'">
									<xsl:value-of select="145"/>
								</xsl:when>

								<xsl:when test="COL3='KLS'">
									<xsl:value-of select="147"/>
								</xsl:when>

								<xsl:when test="COL3='LSIN'">
									<xsl:value-of select="76"/>
								</xsl:when>
								<xsl:when test="COL3='TAI (OTC)'">
									<xsl:value-of select="75"/>
								</xsl:when>
								<xsl:when test="COL3='TAI'">
									<xsl:value-of select="74"/>
								</xsl:when>
								<xsl:when test="COL3='NAG'">
									<xsl:value-of select="73"/>
								</xsl:when>

								<xsl:when test="COL3='EURO Interest'">
									<xsl:value-of select="87"/>
								</xsl:when>
								<xsl:when test="COL3='EUREX'">
									<xsl:value-of select="88"/>
								</xsl:when>
								<xsl:when test="COL3='LIFFE US'">
									<xsl:value-of select="89"/>
								</xsl:when>
								<xsl:when test="COL3='US ICE'">
									<xsl:value-of select="90"/>
								</xsl:when>
								<xsl:when test="COL3='MEFF'">
									<xsl:value-of select="91"/>
								</xsl:when>
								<xsl:when test="COL3='BMV'">
									<xsl:value-of select="92"/>
								</xsl:when>
								<xsl:when test="COL3='MXO'">
									<xsl:value-of select="93"/>
								</xsl:when>
								<xsl:when test="COL3='EEO'">
									<xsl:value-of select="94"/>
								</xsl:when>
								<xsl:when test="COL3='TSE'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="contains(COL3,'LSE')">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL3='CDNX'">
									<xsl:value-of select="72"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="UnderLying">
							<xsl:choose>
								<xsl:when test="COL2='AsiaXJapan' or COL2='ASIAXJAPAN'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL2='US' or COL2='us'">
									<xsl:value-of select="1"/>
								</xsl:when>

								<xsl:when test="COL2='EU'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL2='Japan' or COL2='JAPAN'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL2='Brazil' or COL2='BRAZIL'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL2='Spot'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="COL2='Forwards'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="COL2='SA'">
									<xsl:value-of select="8"/>
								</xsl:when>

								<xsl:when test="COL2='OTCDerivatives'">
									<xsl:value-of select="11"/>
								</xsl:when>


								<xsl:when test="COL2='Multiple' or COL2='MULTIPLE'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL2='Australia' or COL2='AUSTRALIA'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL2='EMERGINGDEBT' or COL2='EmergingDebt'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="AsssetI">
							<xsl:choose>
								<xsl:when test="COL1='Equity' or COL1 = 'EQUITY'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL1='Equity Option' or COL1='EQUITYOPTION' ">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL1='Future' or COL1='FUTURE' ">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL1='Future Option' or COL1='FUTUREOPTION'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL1='FX' or COL1='Fx'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL1='Cash' or COL1='CASH'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="COL1='Indices' or COL1='INDICES'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="COL1='FXOption' or COL1='FXOPTION'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:when test="COL1='CreditDefaultSwap' or COL1='CREDITDEFAULTSWAP'">
									<xsl:value-of select="14"/>
								</xsl:when>
								<xsl:when test="COL1='Forex' or COL1='FOREX'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL1='FX Forward' or COL1='FXFORWARD'">
									<xsl:value-of select="11"/>
								</xsl:when>
								<xsl:when test="COL1='Private Equity' or COL1='PRIVATEEQUITY'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL1='Equity Swap'">
									<xsl:value-of select="13"/>
								</xsl:when>
								<xsl:when test="COL1='Fixed Income' or COL1='FIXEDINCOME' ">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>




						<xsl:variable name="CurrencyI">
							<xsl:choose>
								<xsl:when test="COL4='HKD'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL4='PLN'">
									<xsl:value-of select="38"/>
								</xsl:when>
								<xsl:when test="COL4='USD'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL4='AUD'">
									<xsl:value-of select="14"/>
								</xsl:when>
								<xsl:when test="COL4='CAD'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="COL4='JPY'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL4='GBP'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL4='AED'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL4='BRL'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="COL4='EUR'">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:when test="COL4='NOK'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL4='SGD'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:when test="COL4='MUL'">
									<xsl:value-of select="11"/>
								</xsl:when>
								<xsl:when test="COL4='ZAR'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL4='SEK'">
									<xsl:value-of select="13"/>
								</xsl:when>
								<xsl:when test="COL4='CNY'">
									<xsl:value-of select="15"/>
								</xsl:when>
								<xsl:when test="COL4='KRW'">
									<xsl:value-of select="16"/>
								</xsl:when>
								<xsl:when test="COL4='BDT'">
									<xsl:value-of select="17"/>
								</xsl:when>
								<xsl:when test="COL4='THB'">
									<xsl:value-of select="18"/>
								</xsl:when>
								<xsl:when test="COL4='dong'">
									<xsl:value-of select="19"/>
								</xsl:when>
								<xsl:when test="COL4='GBX'">
									<xsl:value-of select="20"/>
								</xsl:when>
								<xsl:when test="COL4='INR'">
									<xsl:value-of select="21"/>
								</xsl:when>
								<xsl:when test="COL4='CHF'">
									<xsl:value-of select="23"/>
								</xsl:when>
								<xsl:when test="COL4='CLP'">
									<xsl:value-of select="24"/>
								</xsl:when>
								<xsl:when test="COL4='COP'">
									<xsl:value-of select="25"/>
								</xsl:when>
								<xsl:when test="COL4='CZK'">
									<xsl:value-of select="26"/>
								</xsl:when>
								<xsl:when test="COL4='DKK'">
									<xsl:value-of select="27"/>
								</xsl:when>
								<xsl:when test="COL4='GHS'">
									<xsl:value-of select="28"/>
								</xsl:when>
								<xsl:when test="COL4='HUF'">
									<xsl:value-of select="29"/>
								</xsl:when>
								<xsl:when test="COL4='IDR'">
									<xsl:value-of select="30"/>
								</xsl:when>
								<xsl:when test="COL4='ILS'">
									<xsl:value-of select="31"/>
								</xsl:when>
								<xsl:when test="COL4='ISK'">
									<xsl:value-of select="32"/>
								</xsl:when>
								<xsl:when test="COL4='KZT'">
									<xsl:value-of select="33"/>
								</xsl:when>
								<xsl:when test="COL4='LVL'">
									<xsl:value-of select="34"/>
								</xsl:when>
								<xsl:when test="COL4='MXN'">
									<xsl:value-of select="35"/>
								</xsl:when>
								<xsl:when test="COL4='NZD'">
									<xsl:value-of select="37"/>
								</xsl:when>
								<xsl:when test="COL4='PEN'">
									<xsl:value-of select="38"/>
								</xsl:when>
								<xsl:when test="COL4='PLN'">
									<xsl:value-of select="39"/>
								</xsl:when>
								<xsl:when test="COL4='RON'">
									<xsl:value-of select="40"/>
								</xsl:when>
								<xsl:when test="COL4='RUB'">
									<xsl:value-of select="41"/>
								</xsl:when>
								<xsl:when test="COL4='SKK'">
									<xsl:value-of select="42"/>
								</xsl:when>
								<xsl:when test="COL4='TRY'">
									<xsl:value-of select="43"/>
								</xsl:when>
								<xsl:when test="COL4='ARS'">
									<xsl:value-of select="44"/>
								</xsl:when>
								<xsl:when test="COL4='UYU'">
									<xsl:value-of select="45"/>
								</xsl:when>
								<xsl:when test="COL4='TWD'">
									<xsl:value-of select="46"/>
								</xsl:when>

								<xsl:when test="COL4='BMD'">
									<xsl:value-of select="47"/>
								</xsl:when>
								<xsl:when test="COL4='EEK'">
									<xsl:value-of select="48"/>
								</xsl:when>
								<xsl:when test="COL4='GEL'">
									<xsl:value-of select="49"/>
								</xsl:when>
								<xsl:when test="COL4='MYR'">
									<xsl:value-of select="51"/>
								</xsl:when>
								<xsl:when test="COL4='SIT'">
									<xsl:value-of select="52"/>
								</xsl:when>
								<xsl:when test="COL4='XAF'">
									<xsl:value-of select="53"/>
								</xsl:when>
								<xsl:when test="COL4='XOF'">
									<xsl:value-of select="54"/>
								</xsl:when>
								<xsl:when test="COL4='AZN'">
									<xsl:value-of select="55"/>
								</xsl:when>
								<xsl:when test="COL4='PKR'">
									<xsl:value-of select="56"/>
								</xsl:when>
								<xsl:when test="COL4='PHP'">
									<xsl:value-of select="57"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<AUECID>
							<xsl:choose>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="19"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AUECID>
						<AssetID>
							<xsl:value-of select="$AsssetI"/>
						</AssetID>
						<ExchangeID>
							<xsl:value-of select="$Exchange"/>
						</ExchangeID>
						<UnderLyingID>
							<xsl:value-of select="$UnderLying"/>
						</UnderLyingID>

						<CurrencyID>
							<xsl:value-of select="$CurrencyI"/>
						</CurrencyID>


						<Coupon>
							<xsl:choose>
								<xsl:when test="COL1='FIXEDINCOME' or COL1='Fixed Income'">
									<xsl:value-of select="COL23"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Coupon>

						<SecurityTypeID>
							<xsl:choose>
								<xsl:when test="COL20='Corporate'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL20='Sovereign'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL20='BankDebt'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>


						</SecurityTypeID>
						<xsl:variable name="PB_Accrual_NAME">
							<xsl:value-of select="normalize-space(COL21)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_ACCRUAL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccrualCodeMapping.xml')/AccrualCodeMapping/PB[@Name=$PB_NAME]/AccrualData[@PBAccrualName=$PB_Accrual_NAME]/@PranaAccrulCode"/>
						</xsl:variable>
						<AccrualBasisID>
							<!--<xsl:choose>
								<xsl:when test="number($PRANA_ACCRUAL_NAME!='')">
									<xsl:value-of select="number($PRANA_ACCRUAL_NAME)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>-->

							<xsl:choose>
								<xsl:when test="COL21='Accrual_30_360'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL21='Accrual_30E_360'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL21='Actual_360'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL21='Actual_Actual'">
									<xsl:value-of select="4"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</AccrualBasisID>

						<CouponFrequencyID>
							<xsl:choose>
								<xsl:when test="COL22='Annually'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL22='Quaterly'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL22='SemiAnnually'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CouponFrequencyID>

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
						<CusipSymbol>
							<xsl:value-of select="COL8"/>
						</CusipSymbol>
						<SedolSymbol>
							<xsl:value-of select="COL10"/>
						</SedolSymbol>

						<ISINSymbol>
							<xsl:value-of select="COL9"/>
						</ISINSymbol>

						<BloombergSymbol>
							<xsl:value-of select="COL6"/>
						</BloombergSymbol>

						<OSIOptionSymbol>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>

						</OSIOptionSymbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>



						<IssueDate>

							<xsl:value-of select="COL17"/>
						</IssueDate>

						<MaturityDate>
							<xsl:value-of select="COL18"/>
						</MaturityDate>

						<DaysToSettlement>
							<xsl:choose>
								<xsl:when test="COL1='FIXEDINCOME' or COL1='Fixed Income'">
									<xsl:value-of select="COL25"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
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

							<!--<xsl:choose>
								<xsl:when test="contains(COL26,'MXN')">
									<xsl:value-of select="35"/>
								</xsl:when>

								<xsl:when test="contains(COL26,'JPY')">
									<xsl:value-of select="3"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:choose>
								<xsl:when test="number(COL26)">
									<xsl:value-of select="COL26"/>
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
							<!--<xsl:value-of select="$VS"/>-->

							<xsl:choose>
								<xsl:when test="number(COL27)">
									<xsl:value-of select="COL27"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
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

								<xsl:when test="contains(COL1,'EQUITYOPTION') or contains(COL1,'Equity Option')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'EQUITY') or contains(COL1,'Equity')">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FUTURE') or contains(COL1,'Future')">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'EQUITYOPTION') or contains(COL1,'FutuerOption')">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FX') or contains(COL1,'Fx')">
									<xsl:value-of select="'Fx'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'CASH') or contains(COL1,'Cash')">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'INDICES') or contains(COL1,'Indices')">
									<xsl:value-of select="'Indices'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FXFORWARD') or contains(COL1,'FXForward')">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'PRIVATEEQUITY') or contains(COL1,'Private Equity')">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'EQUITYSWAP') or contains(COL1,'Equity Swap')">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:when test="contains(COL1,'FIXEDINCOME') or contains(COL1,'Fixed Income')">
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
								<xsl:when test="$Asset='FixedIncome'">
									<xsl:value-of select="COL5"/>
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
								<xsl:when test="$Asset='Cash'">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:when test="$Asset='Indices'">
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


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</TickerSymbol>

						<xsl:variable name="Exchange">
							<xsl:choose>
								<xsl:when test="COL3='HKF'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL3='TASE'">
									<xsl:value-of select="154"/>
								</xsl:when>
								<xsl:when test="COL3='DBGI'">
									<xsl:value-of select="159"/>
								</xsl:when>
								<xsl:when test="COL3='MSE'">
									<xsl:value-of select="95"/>
								</xsl:when>
								<xsl:when test="COL3='DBGI'">
									<xsl:value-of select="96"/>
								</xsl:when>
								<xsl:when test="COL3='STX'">
									<xsl:value-of select="95"/>
								</xsl:when>
								<xsl:when test="COL3='MSE'">
									<xsl:value-of select="101"/>
								</xsl:when>
								<xsl:when test="COL3='PHS'">
									<xsl:value-of select="150"/>
								</xsl:when>
								<xsl:when test="COL3='TUR'">
									<xsl:value-of select="158"/>
								</xsl:when>
								<xsl:when test="COL3='WSE'">
									<xsl:value-of select="156"/>
								</xsl:when>
								<xsl:when test="COL3='GTS'">
									<xsl:value-of select="157"/>
								</xsl:when>
								<!--<xsl:when test="COL3='TUR'">
									<xsl:value-of select="98"/>
								</xsl:when>-->
								<xsl:when test="COL3='OPTS'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL3='NYME'">
									<xsl:value-of select="78"/>
								</xsl:when>
								<xsl:when test="COL3='CMEE'">
									<xsl:value-of select="22"/>
								</xsl:when>
								<xsl:when test="COL3='SES'">
									<xsl:value-of select="31"/>
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
								<xsl:when test="contains(COL3,'NASDAQ')">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL3='NYSE'">
									<xsl:value-of select="21"/>
								</xsl:when>
								<xsl:when test="COL3='CMEE'">
									<xsl:value-of select="22"/>
								</xsl:when>
								<xsl:when test="COL3='CMG'">
									<xsl:value-of select="23"/>
								</xsl:when>
								<xsl:when test="COL3='AMEX'">
									<xsl:value-of select="24"/>
								</xsl:when>
								<xsl:when test="COL3='TPX'">
									<xsl:value-of select="25"/>
								</xsl:when>
								<xsl:when test="COL3='OSM'">
									<xsl:value-of select="27"/>
								</xsl:when>
								<xsl:when test="COL3='JAQ'">
									<xsl:value-of select="28"/>
								</xsl:when>
								<xsl:when test="COL3='Bovespa'">
									<xsl:value-of select="29"/>
								</xsl:when>
								<xsl:when test="COL3='Oslo'">
									<xsl:value-of select="32"/>
								</xsl:when>
								<xsl:when test="COL3='FRA'">
									<xsl:value-of select="33"/>
								</xsl:when>
								<xsl:when test="COL3='BER'">
									<xsl:value-of select="34"/>
								</xsl:when>
								<xsl:when test="COL3='SEHK'">
									<xsl:value-of select="36"/>
								</xsl:when>
								<xsl:when test="COL3='GEM'">
									<xsl:value-of select="37"/>
								</xsl:when>
								<xsl:when test="COL3='SESDAQ'">
									<xsl:value-of select="38"/>
								</xsl:when>
								<xsl:when test="COL3='XETRA'">
									<xsl:value-of select="39"/>
								</xsl:when>
								<xsl:when test="COL3='HAN'">
									<xsl:value-of select="40"/>
								</xsl:when>
								<xsl:when test="COL3='Bremen Stock Exchange'">
									<xsl:value-of select="41"/>
								</xsl:when>
								<xsl:when test="COL3='Bavarian Stock Exchange'">
									<xsl:value-of select="42"/>
								</xsl:when>
								<xsl:when test="COL3='Euronext'">
									<xsl:value-of select="43"/>
								</xsl:when>
								<xsl:when test="COL3='VSE'">
									<xsl:value-of select="44"/>
								</xsl:when>
								<xsl:when test="COL3='BEL'">
									<xsl:value-of select="45"/>
								</xsl:when>
								<xsl:when test="COL3='CHP'">
									<xsl:value-of select="46"/>
								</xsl:when>
								<xsl:when test="COL3='HEX'">
									<xsl:value-of select="47"/>
								</xsl:when>
								<xsl:when test="COL3='ISE'">
									<xsl:value-of select="48"/>
								</xsl:when>
								<xsl:when test="COL3='ENAM'">
									<xsl:value-of select="49"/>
								</xsl:when>
								<xsl:when test="COL3='ENLI'">
									<xsl:value-of select="50"/>
								</xsl:when>
								<xsl:when test="COL3='SHSE'">
									<xsl:value-of select="62"/>
								</xsl:when>
								<xsl:when test="COL3='ASX'">
									<xsl:value-of select="61"/>
								</xsl:when>
								<xsl:when test="COL3='PS'">
									<xsl:value-of select="60"/>
								</xsl:when>
								<xsl:when test="COL3='BB'">
									<xsl:value-of select="59"/>
								</xsl:when>
								<xsl:when test="COL3='OMX'">
									<xsl:value-of select="58"/>
								</xsl:when>
								<xsl:when test="COL3='RTSX'">
									<xsl:value-of select="57"/>
								</xsl:when>
								<xsl:when test="COL3='SPBEX'">
									<xsl:value-of select="56"/>
								</xsl:when>
								<xsl:when test="COL3='MIL'">
									<xsl:value-of select="55"/>
								</xsl:when>
								<xsl:when test="COL3='ASE'">
									<xsl:value-of select="54"/>
								</xsl:when>
								<xsl:when test="COL3='SWX'">
									<xsl:value-of select="53"/>
								</xsl:when>
								<xsl:when test="COL3='CBOE'">
									<xsl:value-of select="52"/>
								</xsl:when>
								<xsl:when test="COL3='JSE'">
									<xsl:value-of select="51"/>
								</xsl:when>
								<xsl:when test="COL3='KSE'">
									<xsl:value-of select="63"/>
								</xsl:when>
								<xsl:when test="COL3='DSE'">
									<xsl:value-of select="64"/>
								</xsl:when>
								<xsl:when test="COL3='SET'">
									<xsl:value-of select="65"/>
								</xsl:when>
								<xsl:when test="COL3='STC'">
									<xsl:value-of select="66"/>
								</xsl:when>
								<xsl:when test="COL3='KOSDAQ'">
									<xsl:value-of select="67"/>
								</xsl:when>
								<xsl:when test="COL3='LSEP'">
									<xsl:value-of select="68"/>
								</xsl:when>
								<xsl:when test="COL3='NSE'">
									<xsl:value-of select="69"/>
								</xsl:when>
								<xsl:when test="COL3='BSE'">
									<xsl:value-of select="70"/>
								</xsl:when>
								<xsl:when test="COL3='TSX'">
									<xsl:value-of select="71"/>
								</xsl:when>
								<xsl:when test="COL3='MX'">
									<xsl:value-of select="86"/>
								</xsl:when>
								<xsl:when test="COL3='SGX'">
									<xsl:value-of select="85"/>
								</xsl:when>
								<xsl:when test="COL3='SNFE'">
									<xsl:value-of select="84"/>
								</xsl:when>
								<xsl:when test="COL3='DTB'">
									<xsl:value-of select="83"/>
								</xsl:when>
								<xsl:when test="COL3='LIFFE'">
									<xsl:value-of select="82"/>
								</xsl:when>
								<xsl:when test="COL3='CBOT'">
									<xsl:value-of select="80"/>
								</xsl:when>
								<xsl:when test="COL3='eCBOT' or COL3='ECBOT'">
									<xsl:value-of select="81"/>
								</xsl:when>
								<xsl:when test="COL3='OTC'">
									<xsl:value-of select="77"/>
								</xsl:when>
								<xsl:when test="COL3='NYME'">
									<xsl:value-of select="78"/>
								</xsl:when>
								<xsl:when test="COL3='COMX'">
									<xsl:value-of select="79"/>
								</xsl:when>
								<xsl:when test="COL3='JKT'">
									<xsl:value-of select="145"/>
								</xsl:when>

								<xsl:when test="COL3='KLS'">
									<xsl:value-of select="147"/>
								</xsl:when>

								<xsl:when test="COL3='LSIN'">
									<xsl:value-of select="76"/>
								</xsl:when>
								<xsl:when test="COL3='TAI (OTC)'">
									<xsl:value-of select="75"/>
								</xsl:when>
								<xsl:when test="COL3='TAI'">
									<xsl:value-of select="74"/>
								</xsl:when>
								<xsl:when test="COL3='NAG'">
									<xsl:value-of select="73"/>
								</xsl:when>
								<xsl:when test="COL3='ICU'">
									<xsl:value-of select="102"/>
								</xsl:when>

								<xsl:when test="COL3='EURO Interest'">
									<xsl:value-of select="87"/>
								</xsl:when>
								<xsl:when test="COL3='EUREX'">
									<xsl:value-of select="88"/>
								</xsl:when>
								<xsl:when test="COL3='LIFFE US'">
									<xsl:value-of select="89"/>
								</xsl:when>
								<xsl:when test="COL3='US ICE'">
									<xsl:value-of select="90"/>
								</xsl:when>
								<xsl:when test="COL3='MEFF'">
									<xsl:value-of select="91"/>
								</xsl:when>
								<xsl:when test="COL3='BMV'">
									<xsl:value-of select="92"/>
								</xsl:when>
								<xsl:when test="COL3='MXO'">
									<xsl:value-of select="93"/>
								</xsl:when>
								<xsl:when test="COL3='EEO'">
									<xsl:value-of select="94"/>
								</xsl:when>
								<xsl:when test="COL3='TSE'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="contains(COL3,'LSE')">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL3='CDNX'">
									<xsl:value-of select="72"/>
								</xsl:when>
								<xsl:when test="COL3='CME'">
									<xsl:value-of select="96"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="UnderLying">
							<xsl:choose>
								<xsl:when test="COL2='AsiaXJapan' or COL2='ASIAXJAPAN'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL2='US' or COL2='us'">
									<xsl:value-of select="1"/>
								</xsl:when>

								<xsl:when test="COL2='EU'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL2='Japan' or COL2='JAPAN'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL2='Brazil' or COL2='BRAZIL'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL2='Spot'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="COL2='Forwards'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="COL2='SA'">
									<xsl:value-of select="8"/>
								</xsl:when>

								<xsl:when test="COL2='OTCDerivatives'">
									<xsl:value-of select="11"/>
								</xsl:when>


								<xsl:when test="COL2='Multiple' or COL2='MULTIPLE'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL2='Australia' or COL2='AUSTRALIA'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL2='EMERGINGDEBT' or COL2='EmergingDebt'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="AsssetI">
							<xsl:choose>
								<xsl:when test="COL1='Equity' or COL1 = 'EQUITY'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL1='Equity Option' or COL1='EQUITYOPTION' ">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL1='Future' or COL1='FUTURE' ">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL1='Future Option' or COL1='FUTUREOPTION'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL1='FX' or COL1='Fx'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL1='Cash' or COL1='CASH'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="COL1='Indices' or COL1='INDICES'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="COL1='FXOption' or COL1='FXOPTION'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:when test="COL1='CreditDefaultSwap' or COL1='CREDITDEFAULTSWAP'">
									<xsl:value-of select="14"/>
								</xsl:when>
								<xsl:when test="COL1='Forex' or COL1='FOREX'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL1='FX Forward' or COL1='FXFORWARD'">
									<xsl:value-of select="11"/>
								</xsl:when>
								<xsl:when test="COL1='Private Equity' or COL1='PRIVATEEQUITY'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL1='Equity Swap'">
									<xsl:value-of select="13"/>
								</xsl:when>
								<xsl:when test="COL1='Fixed Income' or COL1='FIXEDINCOME' ">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>




						<xsl:variable name="CurrencyI">
							<xsl:choose>
								<xsl:when test="COL4='HKD'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL4='PLN'">
									<xsl:value-of select="38"/>
								</xsl:when>
								<xsl:when test="COL4='USD'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL4='AUD'">
									<xsl:value-of select="14"/>
								</xsl:when>
								<xsl:when test="COL4='CAD'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="COL4='JPY'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL4='GBP'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL4='AED'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL4='BRL'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="COL4='EUR'">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:when test="COL4='NOK'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL4='SGD'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:when test="COL4='MUL'">
									<xsl:value-of select="11"/>
								</xsl:when>
								<xsl:when test="COL4='ZAR'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL4='SEK'">
									<xsl:value-of select="13"/>
								</xsl:when>
								<xsl:when test="COL4='CNY'">
									<xsl:value-of select="15"/>
								</xsl:when>
								<xsl:when test="COL4='KRW'">
									<xsl:value-of select="16"/>
								</xsl:when>
								<xsl:when test="COL4='BDT'">
									<xsl:value-of select="17"/>
								</xsl:when>
								<xsl:when test="COL4='THB'">
									<xsl:value-of select="18"/>
								</xsl:when>
								<xsl:when test="COL4='dong'">
									<xsl:value-of select="19"/>
								</xsl:when>
								<xsl:when test="COL4='GBX'">
									<xsl:value-of select="20"/>
								</xsl:when>
								<xsl:when test="COL4='INR'">
									<xsl:value-of select="21"/>
								</xsl:when>
								<xsl:when test="COL4='CHF'">
									<xsl:value-of select="23"/>
								</xsl:when>
								<xsl:when test="COL4='CLP'">
									<xsl:value-of select="24"/>
								</xsl:when>
								<xsl:when test="COL4='COP'">
									<xsl:value-of select="25"/>
								</xsl:when>
								<xsl:when test="COL4='CZK'">
									<xsl:value-of select="26"/>
								</xsl:when>
								<xsl:when test="COL4='DKK'">
									<xsl:value-of select="27"/>
								</xsl:when>
								<xsl:when test="COL4='GHS'">
									<xsl:value-of select="28"/>
								</xsl:when>
								<xsl:when test="COL4='HUF'">
									<xsl:value-of select="29"/>
								</xsl:when>
								<xsl:when test="COL4='IDR'">
									<xsl:value-of select="30"/>
								</xsl:when>
								<xsl:when test="COL4='ILS'">
									<xsl:value-of select="31"/>
								</xsl:when>
								<xsl:when test="COL4='ISK'">
									<xsl:value-of select="32"/>
								</xsl:when>
								<xsl:when test="COL4='KZT'">
									<xsl:value-of select="33"/>
								</xsl:when>
								<xsl:when test="COL4='LVL'">
									<xsl:value-of select="34"/>
								</xsl:when>
								<xsl:when test="COL4='MXN'">
									<xsl:value-of select="35"/>
								</xsl:when>
								<xsl:when test="COL4='NZD'">
									<xsl:value-of select="37"/>
								</xsl:when>
								<xsl:when test="COL4='PEN'">
									<xsl:value-of select="38"/>
								</xsl:when>
								<xsl:when test="COL4='PLN'">
									<xsl:value-of select="39"/>
								</xsl:when>
								<xsl:when test="COL4='RON'">
									<xsl:value-of select="40"/>
								</xsl:when>
								<xsl:when test="COL4='RUB'">
									<xsl:value-of select="41"/>
								</xsl:when>
								<xsl:when test="COL4='SKK'">
									<xsl:value-of select="42"/>
								</xsl:when>
								<xsl:when test="COL4='TRY'">
									<xsl:value-of select="43"/>
								</xsl:when>
								<xsl:when test="COL4='ARS'">
									<xsl:value-of select="44"/>
								</xsl:when>
								<xsl:when test="COL4='UYU'">
									<xsl:value-of select="45"/>
								</xsl:when>
								<xsl:when test="COL4='TWD'">
									<xsl:value-of select="46"/>
								</xsl:when>

								<xsl:when test="COL4='BMD'">
									<xsl:value-of select="47"/>
								</xsl:when>
								<xsl:when test="COL4='EEK'">
									<xsl:value-of select="48"/>
								</xsl:when>
								<xsl:when test="COL4='GEL'">
									<xsl:value-of select="49"/>
								</xsl:when>
								<xsl:when test="COL4='MYR'">
									<xsl:value-of select="51"/>
								</xsl:when>
								<xsl:when test="COL4='SIT'">
									<xsl:value-of select="52"/>
								</xsl:when>
								<xsl:when test="COL4='XAF'">
									<xsl:value-of select="53"/>
								</xsl:when>
								<xsl:when test="COL4='XOF'">
									<xsl:value-of select="54"/>
								</xsl:when>
								<xsl:when test="COL4='AZN'">
									<xsl:value-of select="55"/>
								</xsl:when>
								<xsl:when test="COL4='PKR'">
									<xsl:value-of select="56"/>
								</xsl:when>
								<xsl:when test="COL4='PHP'">
									<xsl:value-of select="57"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<AUECID>
							<xsl:choose>
								<xsl:when test="$Asset='FutureOption'">
									<xsl:value-of select="19"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AUECID>
						<AssetID>
							<xsl:value-of select="$AsssetI"/>
						</AssetID>
						<ExchangeID>
							<xsl:value-of select="$Exchange"/>
						</ExchangeID>
						<UnderLyingID>
							<xsl:value-of select="$UnderLying"/>
						</UnderLyingID>

						<CurrencyID>
							<xsl:value-of select="$CurrencyI"/>
						</CurrencyID>


						<Coupon>
							<xsl:choose>
								<xsl:when test="COL1='FIXEDINCOME' or COL1='Fixed Income'">
									<xsl:value-of select="COL23"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Coupon>

						<SecurityTypeID>
							<xsl:choose>
								<xsl:when test="COL20='Corporate'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL20='Sovereign'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL20='BankDebt'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>


						</SecurityTypeID>
						<xsl:variable name="PB_Accrual_NAME">
							<xsl:value-of select="normalize-space(COL21)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_ACCRUAL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccrualCodeMapping.xml')/AccrualCodeMapping/PB[@Name=$PB_NAME]/AccrualData[@PBAccrualName=$PB_Accrual_NAME]/@PranaAccrulCode"/>
						</xsl:variable>
						<AccrualBasisID>
							<!--<xsl:choose>
								<xsl:when test="number($PRANA_ACCRUAL_NAME!='')">
									<xsl:value-of select="number($PRANA_ACCRUAL_NAME)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>-->

							<xsl:choose>
								<xsl:when test="COL21='Accrual_30_360'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL21='Accrual_30E_360'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL21='Actual_360'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL21='Actual_Actual'">
									<xsl:value-of select="4"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</AccrualBasisID>

						<CouponFrequencyID>
							<xsl:choose>
								<xsl:when test="COL22='Annually'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL22='Quaterly'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL22='SemiAnnually'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CouponFrequencyID>

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
						<CusipSymbol>
							<xsl:value-of select="COL8"/>
						</CusipSymbol>
						<SedolSymbol>
							<xsl:value-of select="COL10"/>
						</SedolSymbol>

						<ISINSymbol>
							<xsl:value-of select="COL9"/>
						</ISINSymbol>

						<BloombergSymbol>
							<xsl:value-of select="COL6"/>
						</BloombergSymbol>

						<OSIOptionSymbol>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>

						</OSIOptionSymbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>



						<IssueDate>

							<xsl:value-of select="COL17"/>
						</IssueDate>

						<MaturityDate>
							<xsl:value-of select="COL18"/>
						</MaturityDate>

						<DaysToSettlement>
							<xsl:choose>
								<xsl:when test="COL1='FIXEDINCOME' or COL1='Fixed Income'">
									<xsl:value-of select="COL25"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
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

							<!--<xsl:choose>
								<xsl:when test="contains(COL26,'MXN')">
									<xsl:value-of select="35"/>
								</xsl:when>

								<xsl:when test="contains(COL26,'JPY')">
									<xsl:value-of select="3"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:choose>
								<xsl:when test="number(COL26)">
									<xsl:value-of select="COL26"/>
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
							<!--<xsl:value-of select="$VS"/>-->

							<xsl:choose>
								<xsl:when test="number(COL27)">
									<xsl:value-of select="COL27"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</VsCurrencyID>

						<PutOrCall>
							<xsl:choose>

								<xsl:when test="contains(substring(substring-after(substring-after(substring-after(COL6,'/'),'/'),' '),1,1),'C') and contains(COL5,'O:')">
									<xsl:value-of select="'1'"/>
								</xsl:when>




								<xsl:when test="contains(substring(substring-after(substring-after(substring-after(COL6,'/'),'/'),' '),1,1),'P') and contains(COL5,'O:')">
									<xsl:value-of select="'0'"/>
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