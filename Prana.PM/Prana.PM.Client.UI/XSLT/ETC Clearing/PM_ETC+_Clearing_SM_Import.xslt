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
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position) and COL4 ='Option'">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'ETC'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL5)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name ="varAsset">
							<xsl:choose>
								<xsl:when test="COL4='Option'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						

						<xsl:variable name="varUnderlyingSymbol">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL5),' '),' ')"/>
						</xsl:variable>
						<xsl:variable name="varExDay">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL5),'/'),'/')"/>
						</xsl:variable>
						<xsl:variable name="varMonth">
							<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),'/')"/>
						</xsl:variable>
						<xsl:variable name="varYear">
							<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL5),'/'),'/'),' ')"/>
						</xsl:variable>

						<xsl:variable name="varPutORCall">
							<xsl:value-of select="substring(substring-before(COL5,' '),1,1)"/>
						</xsl:variable>
						<xsl:variable name="varStrikePrice">
							<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),' '),'##.00')"/>
						</xsl:variable>

						<xsl:variable name ="varMonthCode">
							<xsl:call-template name ="MonthCode">
								<xsl:with-param name ="Month" select ="number($varMonth)"/>
								<xsl:with-param name ="PutOrCall" select="$varPutORCall"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varDays">
							<xsl:choose>
								<xsl:when test="substring($varExDay,1,1)='0'">
									<xsl:value-of select="substring($varExDay,2,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varExDay"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						
						<xsl:variable name="varOption">
							<xsl:variable name="varThirdFriday">
								<xsl:choose>
									<xsl:when test="number($varYear) and number($varMonth)">
										<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							
							<xsl:choose>
								<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
									<!--<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(number(concat($varStrikePrice,'.',$varDecimal)),'#.00'))"/>-->
									<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice,'D',$varDays)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<TickerSymbol>
							<!--<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:value-of select="$varOption"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:choose>
								
								<xsl:when test="COL4='Option'">
									<xsl:value-of select="$varOption"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</TickerSymbol>

						
						<PutOrCall>
							<xsl:choose>
								<xsl:when test="contains(substring(substring-before(normalize-space(COL5),' '),1,1),'P')">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:when test="contains(substring(substring-before(normalize-space(COL5),' '),1,1),'C')">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'-1'"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>
						
						
						
						<xsl:variable name="varStrickPrice">
							<xsl:choose>
								<xsl:when test="string-length(COL2)= '20'">
									<xsl:value-of select="substring(COL2,12)"/>
								</xsl:when>
								<xsl:when test="string-length(COL2)= '19'">
									<xsl:value-of select="substring(COL2,11)"/>
								</xsl:when>
								<xsl:when test="string-length(COL2)= '18'">
									<xsl:value-of select="substring(COL2,10)"/>
								</xsl:when>
								<xsl:when test="string-length(COL2)= '17'">
									<xsl:value-of select="substring(COL2,9)"/>
								</xsl:when>
								<xsl:when test="string-length(COL2)= '16'">
									<xsl:value-of select="substring(COL2,8)"/>
								</xsl:when>
								<xsl:when test="string-length(COL2)= '15'">
									<xsl:value-of select="substring(COL2,7)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--$varPutORCall-->

						<xsl:variable name="varUnderLyingSymbols">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name="varOsiOption">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(substring-after($varUnderLyingSymbols,' '),' '))= '4'">
									<xsl:value-of select="concat(substring-before(substring-after($varUnderLyingSymbols,' '),' '),(' '),(' '),$varYear,$varMonth,$varExDay,$varStrickPrice)"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(substring-after($varUnderLyingSymbols,' '),' '))='3'">
									<xsl:value-of select="concat(substring-before(substring-after($varUnderLyingSymbols,' '),' '),(' '),(' '),(' '),$varYear,$varMonth,$varExDay,$varStrickPrice)"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(substring-after($varUnderLyingSymbols,' '),' '))='2'">
									<xsl:value-of select="concat(substring-before(substring-after($varUnderLyingSymbols,' '),' '),(' '),(' '),(' '),(' '),$varYear,$varMonth,$varExDay,$varStrickPrice)"/>
								</xsl:when>
								<xsl:when test="string-length(substring-before(substring-after($varUnderLyingSymbols,' '),' '))">
									<xsl:value-of select="concat(substring-before(substring-after($varUnderLyingSymbols,' '),' '),(' '),(' '),(' '),(' '),(' '),$varYear,$varMonth,$varExDay,$varStrickPrice)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<OSIOptionSymbol>
							<xsl:choose>
								<xsl:when test="contains(substring(substring-before(COL5,' '),1,1),'P') or contains(substring(substring-before(COL5,' '),1,1),'C')">
									<xsl:value-of select="$varOsiOption"/>
								</xsl:when>
							</xsl:choose>
							
						</OSIOptionSymbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="contains(substring(substring-before(COL5,' '),1,1),'P') or contains(substring(substring-before(COL5,' '),1,1),'C')">
									<xsl:value-of select="concat($varOsiOption,'U')"/>
								</xsl:when>
							</xsl:choose>
							
						</IDCOOptionSymbol>

						<Multiplier>
							<xsl:value-of select="100"/>
						</Multiplier>

						<ExpirationDate>
							<xsl:value-of select="concat($varMonth,'/',$varExDay,'/',$varYear)"/>
						</ExpirationDate>

						<UnderLyingSymbol>
							<xsl:value-of select="$varUnderlyingSymbol"/>
						</UnderLyingSymbol>

						<StrikePrice>
							<xsl:choose>
								<xsl:when test="number($varStrikePrice)">
									<xsl:value-of select="$varStrikePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StrikePrice>

						<AUECID>
							<xsl:value-of select="12"/>
						</AUECID>
						<CurrencyID>
							<xsl:value-of select="1"/>
						</CurrencyID>
						<LongName>
							<xsl:value-of select="$varOption"/>
						</LongName>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
