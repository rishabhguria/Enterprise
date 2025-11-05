<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs xsi xsl">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
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
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL8)and COL2!='Description'">
					<PositionMaster>
						<xsl:variable name = "varCurrencyID" >
							<xsl:call-template name="tempCurrencyCode">
								<xsl:with-param name="paramCurrencySymbol" select="COL25" />
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name = "varCouponFrequencyID" >
							<xsl:call-template name="tempCouponFrequencyID">
								<xsl:with-param name="paramCouponFrequencyID" select="COL9" />
							</xsl:call-template>
						</xsl:variable>
						<CouponFrequencyID>
							<xsl:value-of select="$varCouponFrequencyID"/>
						</CouponFrequencyID>
						<CurrencyID>
							<xsl:value-of select ="$varCurrencyID"/>
				     	</CurrencyID>
						<UnderLyingSymbol>
							<xsl:value-of select ="COL24"/>
						</UnderLyingSymbol>
						<TickerSymbol>
							<xsl:value-of select ="COL24"/>
						</TickerSymbol>
						<FirstCouponDate>
							<xsl:choose>
								<xsl:when test="COL12='Y'">
									<xsl:value-of select ="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL4"/>
								</xsl:otherwise>
							</xsl:choose>
						</FirstCouponDate>
						<IssueDate>
							<xsl:value-of select ="COL3"/>
						</IssueDate>
						<MaturityDate>
							<xsl:value-of select="COL6"/>
						</MaturityDate>

						<ExpirationDate>
							<xsl:value-of select ="COL6"/>
						</ExpirationDate>
						<xsl:choose>
							<xsl:when test="number(COL8)">
								<Coupon>
									<xsl:value-of select="COL8"/>
								</Coupon>
							</xsl:when>
							<xsl:otherwise>
								<Coupon>
									<xsl:value-of select="0"/>
								</Coupon>
							</xsl:otherwise>
						</xsl:choose>
						<IsZero>
							<xsl:choose>
								<xsl:when test="COL12='Y'">
									<xsl:value-of select="true()"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="false()"/>
								</xsl:otherwise>
							</xsl:choose>
						</IsZero>
						<Multiplier>
							<xsl:value-of select="'1'"/>
						</Multiplier>
						<AccrualBasisID>
							<xsl:choose>
								<xsl:when test ="contains(COL10,'30/360')">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="contains(COL10,'360')">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="4"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccrualBasisID>
						<AssetID>
							<xsl:value-of select ="8"/>
						</AssetID>
						<UnderLyingID>
							<xsl:value-of select ="10"/>
						</UnderLyingID>
						<ExchangeID>
							<xsl:value-of select ="77"/>
						</ExchangeID>
						<BloombergSymbol>
							<xsl:value-of select="COL29"/>
						</BloombergSymbol>
						<SedolSymbol>
							<xsl:value-of select="COL15"/>
						</SedolSymbol>
						<LongName>
							<xsl:value-of select="COL2"/>
						</LongName>
						<DaysToSettlement>
							<xsl:value-of select ="3"/>
						</DaysToSettlement>
						<SecurityTypeID>
							<xsl:value-of select="3"/>
						</SecurityTypeID>
			     	</PositionMaster>
		    	</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
