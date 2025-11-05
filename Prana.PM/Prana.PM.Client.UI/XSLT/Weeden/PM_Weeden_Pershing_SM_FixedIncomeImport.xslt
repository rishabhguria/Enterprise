<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template name="DateFormate">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before($Date,'/'),'/',substring-before(substring-after($Date,'/'),'/'),'/','20',substring-after(substring-after($Date,'/'),'/'))"/>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:if test ="COL8='CORPORATE DEBT' or COL8='FEDERAL BOND' or COL8='MUNICIPAL DEBT'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<CouponFrequencyID>
							<xsl:value-of select="'0'"/>
						</CouponFrequencyID>

						<xsl:variable name="varCurrency">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>

						<CurrencyID>
							<xsl:choose>
								<xsl:when test="$varCurrency='USD'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='HKD'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='JPY'">
									<xsl:value-of select="'3'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='GBP'">
									<xsl:value-of select="'4'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='AED'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='BRL'">
									<xsl:value-of select="'6'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='CAD'">
									<xsl:value-of select="'7'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='EUR'">
									<xsl:value-of select="'8'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='NOK'">
									<xsl:value-of select="'9'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='SGD'">
									<xsl:value-of select="'10'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='MUL'">
									<xsl:value-of select="'11'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='ZAR'">
									<xsl:value-of select="'12'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='SEK'">
									<xsl:value-of select="'13'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='AUD'">
									<xsl:value-of select="'14'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='CNY'">
									<xsl:value-of select="'15'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='KRW'">
									<xsl:value-of select="'16'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='BDT'">
									<xsl:value-of select="'17'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='THB'">
									<xsl:value-of select="'18'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='VND'">
									<xsl:value-of select="'19'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='GBX'">
									<xsl:value-of select="'20'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='INR'">
									<xsl:value-of select="'21'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='CHF'">
									<xsl:value-of select="'23'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='CLP'">
									<xsl:value-of select="'24'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='COP'">
									<xsl:value-of select="'25'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='CZK'">
									<xsl:value-of select="'26'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='DKK'">
									<xsl:value-of select="'27'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='GHS'">
									<xsl:value-of select="'28'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='HUF'">
									<xsl:value-of select="'29'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='IDR'">
									<xsl:value-of select="'30'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='ILS'">
									<xsl:value-of select="'31'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='ISK'">
									<xsl:value-of select="'32'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='KZT'">
									<xsl:value-of select="'33'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='LVL'">
									<xsl:value-of select="'34'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='MXN'">
									<xsl:value-of select="'35'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='NZD'">
									<xsl:value-of select="'36'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='PEN'">
									<xsl:value-of select="'37'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='PLN'">
									<xsl:value-of select="'38'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='RON'">
									<xsl:value-of select="'40'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='RUB'">
									<xsl:value-of select="'41'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='SKK'">
									<xsl:value-of select="'42'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='TRY'">
									<xsl:value-of select="'43'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='ARS'">
									<xsl:value-of select="'44'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='UYU'">
									<xsl:value-of select="'45'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='TWD'">
									<xsl:value-of select="'46'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='BMD'">
									<xsl:value-of select="'47'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='EEK'">
									<xsl:value-of select="'48'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='GEL'">
									<xsl:value-of select="'49'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='MYR'">
									<xsl:value-of select="'51'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='SIT'">
									<xsl:value-of select="'52'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='XAF'">
									<xsl:value-of select="'53'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='XOF'">
									<xsl:value-of select="'54'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='AZN'">
									<xsl:value-of select="'55'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='PKR'">
									<xsl:value-of select="'56'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='MXP'">
									<xsl:value-of select="'57'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='PHP'">
									<xsl:value-of select="'58'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='BGN'">
									<xsl:value-of select="'59'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='HRK'">
									<xsl:value-of select="'60'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='MKD'">
									<xsl:value-of select="'61'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='RSD'">
									<xsl:value-of select="'62'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='VES'">
									<xsl:value-of select="'63'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='LKR'">
									<xsl:value-of select="'64'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='BHD'">
									<xsl:value-of select="'65'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='EGP'">
									<xsl:value-of select="'66'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='ILA'">
									<xsl:value-of select="'67'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='JOD'">
									<xsl:value-of select="'68'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='KES'">
									<xsl:value-of select="'69'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='KWF'">
									<xsl:value-of select="'70'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='MAD'">
									<xsl:value-of select="'71'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='NGN'">
									<xsl:value-of select="'72'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='OMR'">
									<xsl:value-of select="'73'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='QAR'">
									<xsl:value-of select="'74'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='TND'">
									<xsl:value-of select="'75'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='SAR'">
									<xsl:value-of select="'76'"/>
								</xsl:when>
								<xsl:when test="$varCurrency='KWD'">
									<xsl:value-of select="'77'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='CNH'">
									<xsl:value-of select="'78'"/>
								</xsl:when>

								<xsl:when test="$varCurrency='CNT'">
									<xsl:value-of select="'79'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CurrencyID>

						<UnderLyingID>
							<xsl:value-of select="'10'"/>
						</UnderLyingID>

						<AssetID>
							<xsl:value-of select="'8'"/>
						</AssetID>

						<ExchangeID>
							<xsl:value-of select="'77'"/>
						</ExchangeID>

						<xsl:variable name="varSymbol" select="COL5"/>

						<TickerSymbol>
							<xsl:value-of select="normalize-space($varSymbol)"/>
						</TickerSymbol>

						<LongName>
							<xsl:value-of select="normalize-space(COL9)"/>
						</LongName>

						<UnderLyingSymbol>
							<xsl:value-of select="normalize-space($varSymbol)"/>
						</UnderLyingSymbol>

						<xsl:variable name="varCUSIP" select="COL5"/>

						<CusipSymbol>
							<xsl:value-of select="normalize-space($varCUSIP)"/>
						</CusipSymbol>

						<xsl:variable name="varFirstCouponDate2" select="normalize-space(substring-after(COL9,'DTD'))"/>
						<xsl:variable name="varFirstCouponDate1">
							<xsl:choose>
								<xsl:when test="contains($varFirstCouponDate2,' ')">
									<xsl:value-of select="substring-before($varFirstCouponDate2,' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varFirstCouponDate2"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varFirstCouponDate">
							<xsl:call-template name="DateFormate">
								<xsl:with-param name="Date" select="$varFirstCouponDate1"/>
							</xsl:call-template>
						</xsl:variable>

						<FirstCouponDate>
							<xsl:choose>
								<xsl:when test="$varFirstCouponDate=''">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varFirstCouponDate"/>
								</xsl:otherwise>
							</xsl:choose>
						</FirstCouponDate>

						<IssueDate>
							<xsl:choose>
								<xsl:when test="$varFirstCouponDate=''">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varFirstCouponDate"/>
								</xsl:otherwise>
							</xsl:choose>
						</IssueDate>

						<xsl:variable name="varExpirationDate1" select="substring-before(substring-after(COL9,'% '),' ')"/>
						<xsl:variable name="varExpirationDate">
							<xsl:call-template name="DateFormate">
								<xsl:with-param name="Date" select="$varExpirationDate1"/>
							</xsl:call-template>
						</xsl:variable>
						<ExpirationDate>
							<xsl:choose>
								<xsl:when test="$varExpirationDate='//20'">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varExpirationDate"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExpirationDate>

						<MaturityDate>
							<xsl:choose>
								<xsl:when test="$varExpirationDate=''">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varExpirationDate"/>
								</xsl:otherwise>
							</xsl:choose>
						</MaturityDate>

						<xsl:variable name="varCouponLen" select="string-length(substring-before(COL9,'%'))-5"/>
						<xsl:variable name="varCoupon" select="substring-before(substring(COL9,$varCouponLen),'%')"/>
						<Coupon>
							<xsl:choose>
								<xsl:when test="$varCoupon=''">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varCoupon"/>
								</xsl:otherwise>
							</xsl:choose>
						</Coupon>

						<IsZero>
							<xsl:choose>
								<xsl:when test="number($varCoupon)=0">
									<xsl:value-of select="'TRUE'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'FALSE'"/>
								</xsl:otherwise>
							</xsl:choose>
						</IsZero>

						<AccrualBasisID>
							<xsl:value-of select="0"/>
						</AccrualBasisID>

						<DaysToSettlement>
							<xsl:value-of select ="1"/>
						</DaysToSettlement>

						<Multiplier>
							<xsl:value-of select="0.01"/>
						</Multiplier>

						<AssetCategory>
							<xsl:value-of select="'FixedIncome'"/>
						</AssetCategory>

						<xsl:variable name="BondType" select ="COL8"/>

						<SecurityTypeID>
							<xsl:choose>
								<xsl:when test="$BondType ='CORPORATE DEBT'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="$BondType ='MUNICIPAL DEBT'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="$BondType ='FEDERAL BOND'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecurityTypeID>

            <UDASector>
              <xsl:value-of select="'Undefined'"/>
            </UDASector>

            <UDASubSector>
              <xsl:value-of select="'Undefined'"/>
            </UDASubSector>

            <UDASecurityType>
              <xsl:value-of select="'Undefined'"/>
            </UDASecurityType>

            <UDAAssetClass>
              <xsl:value-of select="'Undefined'"/>
            </UDAAssetClass>

            <UDACountry>
              <xsl:value-of select="'Undefined'"/>
            </UDACountry>     

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

