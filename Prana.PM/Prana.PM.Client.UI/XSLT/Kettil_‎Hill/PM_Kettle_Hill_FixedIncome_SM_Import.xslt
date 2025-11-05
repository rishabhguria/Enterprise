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

				<xsl:if test ="COL4 !='*'">

					<PositionMaster>

						<!--<xsl:variable name = "varCurrencyID" >
							<xsl:call-template name="tempCurrencyCode">
								<xsl:with-param name="paramCurrencySymbol" select="COL10" />
							</xsl:call-template>
						</xsl:variable>-->

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="COL1 = '*' or COL1 =''">
									<xsl:value-of select="COL3"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<AUECID>
							<xsl:value-of select="80"/>
						</AUECID>

						<CouponFrequencyID>
							<xsl:value-of select="0"/>
						</CouponFrequencyID>

						<CurrencyID>
							<xsl:value-of select ="1"/>
						</CurrencyID>

					

						<ISINSymbol>
							<xsl:value-of select="COL1"/>
						</ISINSymbol>

						<UnderLyingSymbol>
							<xsl:value-of select ="$varSymbol"/>
						</UnderLyingSymbol>

						<TickerSymbol>
							<xsl:value-of select ="$varSymbol"/>
						</TickerSymbol>

						<FirstCouponDate>
							<xsl:value-of select="'1/1/1800'"/>
						</FirstCouponDate>

						<IssueDate>
							<xsl:value-of select="'1/1/1800'"/>
						</IssueDate>

						<ExpirationDate>
							<xsl:value-of select="'1/1/1800'"/>
						</ExpirationDate>

						<MaturityDate>
							<xsl:value-of select="'1/1/1800'"/>
						</MaturityDate>


						<Coupon>
							<xsl:value-of select="0"/>
						</Coupon>


						<IsZero>
							<xsl:value-of select="true()"/>
						</IsZero>


						<LongName>
							<xsl:value-of select="normalize-space(COL4)"/>
						</LongName>

						<Multiplier>
							<xsl:value-of select="0.01"/>
						</Multiplier>

						<SecurityTypeID>
							<xsl:value-of select="0"/>
						</SecurityTypeID>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
