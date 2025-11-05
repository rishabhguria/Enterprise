<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:template name="tempCouponFrequencyID">
		<xsl:param name="paramCouponFrequencyID"/>
		<xsl:choose>
			<xsl:when test="$paramCouponFrequencyID='12'">
				<xsl:value-of select="'0'"/>
			</xsl:when>
			<xsl:when test="$paramCouponFrequencyID='4'">
				<xsl:value-of select="'1'"/>
			</xsl:when>
			<xsl:when test="$paramCouponFrequencyID='2'">
				<xsl:value-of select="'2'"/>
			</xsl:when>
			<xsl:when test="$paramCouponFrequencyID='1'">
				<xsl:value-of select="'3'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'4'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL14) and contains(COL7,'%')">
					<PositionMaster>

						<!--<xsl:variable name="varCouponbeforePercent" select="substring-before(COL3,'%')"/>

						<xsl:variable name="numericValue" select="translate($varCouponbeforePercent, translate($varCouponbeforePercent, '0123456789.', ''), '')"/>

						<xsl:variable name="dotsCount" select="string-length(translate($numericValue, translate($numericValue, '.', ''), ''))" />

						<xsl:variable name="varCouponLength" select="string-length(translate($numericValue,'.',''))" />

						<xsl:variable name="numberWithDots">
							<xsl:choose>
								<xsl:when test="$dotsCount = 1">
									<xsl:value-of select="$numericValue" />
								</xsl:when>
								<xsl:when test="$dotsCount = 2 and $varCouponLength='1'">
									<xsl:value-of select="substring-after(substring-after($numericValue, '.'),'.')" />
								</xsl:when>
								<xsl:when test="$dotsCount = 2 ">
									<xsl:value-of select="substring-after(substring-after($numericValue, '.'),'.')" />
								</xsl:when>
								<xsl:when test="$dotsCount >= 3">
									<xsl:value-of select="substring-after(substring-after($numericValue, '.'), '.')" />
								</xsl:when>
								<xsl:when test="$dotsCount >= 4">
									<xsl:value-of select="substring-after(substring-after(substring-after($numericValue, '.'), '.'),'.')" />
								</xsl:when>
								<xsl:when test="$dotsCount >= 5">
									<xsl:value-of select="substring-after(substring-after(substring-after(substring-after($numericValue, '.'), '.'),'.'),'.')" />
								</xsl:when>
							</xsl:choose>
						</xsl:variable>-->

						<Coupon>
							<xsl:value-of select="'1'"/>
						</Coupon>

						<LongName>
							<xsl:value-of select="normalize-space(COL7)"/>
						</LongName>

						<CurrencyID>
							<xsl:value-of select="'1'"/>
						</CurrencyID>

						<xsl:variable name="varSymbol" select="COL10"/>

						<TickerSymbol>
							<xsl:value-of select="normalize-space($varSymbol)"/>
						</TickerSymbol>

						<UnderLyingSymbol>
							<xsl:value-of select="normalize-space($varSymbol)"/>
						</UnderLyingSymbol>

						<xsl:variable name="varSedol" select="COL9"/>

						<SedolSymbol>
							<xsl:value-of select="normalize-space($varSedol)"/>
						</SedolSymbol>

						<xsl:variable name="varISIN" select="COL11"/>

						<ISINSymbol>
							<xsl:value-of select="normalize-space($varISIN)"/>
						</ISINSymbol>

						<CusipSymbol>
							<xsl:value-of select="normalize-space($varSymbol)"/>
						</CusipSymbol>
						
						<xsl:variable name="varDate" select="substring-before(substring-after(normalize-space(COL7),'%'),'/')"/>
						<xsl:variable name="varMonth" select="substring-before(substring-after(substring-after(normalize-space(COL7),'%'),'/'),'/')"/>
						<xsl:variable name="varYear" select="concat('20',substring-before(substring-after(substring-after(substring-after(normalize-space(COL7),'%'),'/'),'/'),' '))"/>

						<MaturityDate>
							<xsl:value-of select="concat($varMonth,'/',$varDate,'/',$varYear)"/>
						</MaturityDate>

						<ExpirationDate>
							<xsl:value-of select="concat($varMonth,'/',$varDate,'/',$varYear)"/>
						</ExpirationDate>

						<IsZero>
							<xsl:value-of select="'FALSE'"/>
						</IsZero>

						<Multiplier>
							<xsl:value-of select="0.01"/>
						</Multiplier>

						<AssetCategory>
							<xsl:value-of select="'FixedIncome'"/>
						</AssetCategory>

						<UnderLyingID>
							<xsl:value-of select="'10'"/>
						</UnderLyingID>

						<ExchangeID>
							<xsl:value-of select="'77'"/>
						</ExchangeID>

						<AUECID>
							<xsl:value-of select="'80'"/>
						</AUECID>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>