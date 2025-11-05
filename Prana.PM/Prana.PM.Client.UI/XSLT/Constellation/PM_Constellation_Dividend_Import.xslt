<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

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

	<xsl:template name="FormatDate">
		<xsl:param name="DateTime" />
		<!-- converts date time double number to 18/12/2009 -->

		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019" />
		</xsl:variable>

		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))" />
		</xsl:variable>

		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
		</xsl:variable>

		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
		</xsl:variable>

		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
		</xsl:variable>

		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))" />
		</xsl:variable>

		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
		</xsl:variable>

		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))" />
		</xsl:variable>

		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
		</xsl:variable>

		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
		</xsl:variable>

		<xsl:variable name ="varMonthUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nMonth) = 1">
					<xsl:value-of select ="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="nDayUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nDay) = 1">
					<xsl:value-of select ="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>

	</xsl:template>
	<xsl:template name="FormatDate2">
		<xsl:param name="varFullDate" />
		<xsl:variable name="varYear">
			<xsl:value-of select="substring($varFullDate, string-length($varFullDate) - 2 , 4)"/>
		</xsl:variable>
		<xsl:variable name="varWithoutYear">
			<xsl:value-of select="substring($varFullDate, 1, string-length($varFullDate) -3)"/>
		</xsl:variable>
		<xsl:variable name="varDay">
			<xsl:choose>
				<xsl:when test="$varWithoutYear &lt; 100">
					<xsl:value-of select="concat('0',substring($varWithoutYear, string-length($varWithoutYear) - 4, 1))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring($varWithoutYear, string-length($varWithoutYear) - 5, 2)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test="$varWithoutYear &lt; 999">
					<xsl:value-of select="concat('0',substring($varWithoutYear, 1, 1))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring($varWithoutYear, 5, 2)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL21"/>
					</xsl:call-template>
				</xsl:variable>		

				<xsl:variable name="varCurrency">
					<xsl:value-of select="COL13"/>
				</xsl:variable>

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_Name">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME" select="''"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
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

						<xsl:variable name="varSEDOL" select="normalize-space(COL11)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSEDOL !=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSEDOL !=''">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>
						
						<Amount>
							<xsl:choose>
								<xsl:when test="number($varAmount)">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>

						<xsl:variable name="varFXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL27"/>
							</xsl:call-template>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="$varFXRate &gt;0">
									<xsl:value-of select="$varFXRate" />
								</xsl:when>
								<xsl:when test="$varFXRate &lt; 0">
									<xsl:value-of select="$varFXRate * (-1)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0" />
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<xsl:variable name="varPayoutDate">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<PayoutDate>
							<xsl:value-of select="$varPayoutDate"/>
						</PayoutDate>

						<xsl:variable name="varExDate">
							<xsl:value-of select="COL18"/>
						</xsl:variable>

						<ExDate>
							<xsl:value-of select="$varExDate"/>
						</ExDate>

						<CurrencyName>
							<xsl:value-of select="$varCurrency"/>
						</CurrencyName>


						<Description>
							<xsl:choose>
								<xsl:when test="$varAmount &gt; 0">
									<xsl:value-of select="'Dividend Received'"/>
								</xsl:when>
								<xsl:when test ="$varAmount &lt; 0">
									<xsl:value-of select ="'Dividend Charged'"/>
								</xsl:when>								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>

						<ActivityType>
							<xsl:choose>
								<xsl:when test ="contains(COL5,'DIVIDEND RECEIVABLE') and contains(COL6,'DividendGross')">
									<xsl:value-of select ="'DividendExpense'"/>
								</xsl:when>
								<xsl:when test="contains(COL5,'DIVIDEND RECEIVABLE')">
									<xsl:value-of select="'DividendIncome'"/>
								</xsl:when>								
								<xsl:when test ="contains(COL5,'DIVIDEND PAYABLE') and contains(COL6,'DividendTax')">
									<xsl:value-of select ="'WithholdingTax'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ActivityType>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>