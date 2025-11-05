<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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

				<xsl:if test ="COL1 !='*' and COL1 !='CUSIP'">

					<PositionMaster>

						<!--<xsl:variable name = "varCurrencyID" >
							<xsl:call-template name="tempCurrencyCode">
								<xsl:with-param name="paramCurrencySymbol" select="COL10" />
							</xsl:call-template>
						</xsl:variable>-->

						<xsl:variable name = "varCouponFrequencyID" >
							<xsl:call-template name="tempCouponFrequencyID">
								<xsl:with-param name="paramCouponFrequencyID" select="COL9" />
							</xsl:call-template>
						</xsl:variable>

						<AUECID>
							<xsl:value-of select="80"/>
						</AUECID>

						<CouponFrequencyID>
							<xsl:value-of select="$varCouponFrequencyID"/>
						</CouponFrequencyID>

						<!--<CurrencyID>
							<xsl:value-of select ="$varCurrencyID"/>
						</CurrencyID>-->

						<CusipSymbol>
							<xsl:value-of select ="substring-before(COL1,' ')"/>
						</CusipSymbol>

						<!--<BloombergSymbol>
							<xsl:value-of select="normalize-space(COL28)"/>
						</BloombergSymbol>-->

						<UnderLyingSymbol>
							<xsl:value-of select ="normalize-space(substring-before(COL1,' '))"/>
						</UnderLyingSymbol>

						<TickerSymbol>
							<xsl:value-of select ="normalize-space(substring-before(COL1,' '))"/>
						</TickerSymbol>

						<FirstCouponDate>
							
							<xsl:choose>
								<xsl:when test="contains(COL4,'N/A')">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL4"/>
								</xsl:otherwise>
							</xsl:choose>
						</FirstCouponDate>

						<IssueDate>
							
							<xsl:choose>
								<xsl:when test="contains(COL3,'N/A')">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL3"/>
								</xsl:otherwise>
							</xsl:choose>
						</IssueDate>

						<ExpirationDate>
							<xsl:choose>
								<xsl:when test="contains(COL6,'N/A')">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL6"/>
								</xsl:otherwise>
							</xsl:choose>

						</ExpirationDate>

						<MaturityDate>
							<xsl:choose>
								<xsl:when test="contains(COL6,'N/A')">
									<xsl:value-of select="'1/1/1800'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL6"/>
								</xsl:otherwise>
							</xsl:choose>
						</MaturityDate>


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
							<xsl:value-of select="false()"/>
						</IsZero>

						<!--<FirstCouponDate>
							<xsl:value-of select ="COL4"/>
						</FirstCouponDate>-->

						<!--<AccrualBasisID>
							<xsl:choose>
								<xsl:when test="COL8='US MUNI: 30/360'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test="COL8='ACT/360'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test="COL8='ACT/ACT'">
									<xsl:value-of select ="'4'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccrualBasisID>-->

						<!--<Description>
							<xsl:value-of select="COL2"/>
						</Description>-->

						<!--<AccrualBasisID>
							<xsl:value-of select ="'2'"/>
						</AccrualBasisID>-->

						<!--<DaysToSettlement>
							<xsl:value-of select ="format-number(COL9,'#')"/>
						</DaysToSettlement>-->

						<!--<AssetID>
							<xsl:value-of select ="8"/>
						</AssetID>

						<UnderLyingID>
							<xsl:value-of select ="10"/>
						</UnderLyingID>

						<ExchangeID>
							<xsl:value-of select ="77"/>
						</ExchangeID>-->

						<LongName>
							<xsl:value-of select="COL2"/>
						</LongName>

						<!--<BloombergSymbol>
							<xsl:value-of select ="normalize-space(substring-before(COL1,' '))"/>
						</BloombergSymbol>-->

						<Multiplier>
							<xsl:value-of select="1"/>
						</Multiplier>

						<!--<BondTypeID>
							<xsl:value-of select="'2'"/>
						</BondTypeID>-->

						<!--<SecurityTypeID>
							<xsl:choose>
								<xsl:when test="COL12='MUNICIPAL BONDS'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL12='DISCOUNT'">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:when test="COL12='STREET CONVENTION'">
									<xsl:value-of select="4"/>
								</xsl:when>
							</xsl:choose>
						</SecurityTypeID>-->


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
