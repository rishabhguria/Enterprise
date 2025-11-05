<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>
			<xsl:when test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12 and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCodeVar">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position" select="COL5"/>

				<xsl:if test="number($Position) and (COL2='OPTION' or COL2='FUTFOP')">

					<PositionMaster>
						
						<xsl:variable name="PB_NAME" select="'GSEC'"/>


						<xsl:variable name="Week3">
							<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),9,1)"/>
						</xsl:variable>
						<xsl:variable name="FutureCode">
							<xsl:choose>
								<xsl:when test="COL34='*' or COL34='' and COL2='FUTFOP'">
									<xsl:value-of select="concat($Week3,'E')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL34)"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						<xsl:variable name="PB_ROOT_NAME">
							<xsl:choose>
								<xsl:when test="COL34='*' or COL34='' and COL2='FUTFOP'">
									<xsl:value-of select="$FutureCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring($FutureCode,1,2)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name ="FUTURE_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExchangeName"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_MULTIPLIER">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@StrikeMul"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_FLAG">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="Multiplier">
							<xsl:choose>
								<xsl:when test="number($FUTURE_MULTIPLIER)">
									<xsl:value-of select="$FUTURE_MULTIPLIER"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMonthCode" select="substring($FutureCode,3,1)"/>

						<xsl:variable name="varYearF" select="substring($FutureCode,4,1)"/>

						<xsl:variable name="MonthYearCode">
							<xsl:choose>
								<xsl:when test="$FUTURE_FLAG!=''">
									<xsl:value-of select="concat($varYearF,$varMonthCode)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($varMonthCode,$varYearF)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="$PRANA_ROOT_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_ROOT_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PutCall" select="normalize-space(COL39)"/>

						<xsl:variable name="StrikePrice">
							<xsl:choose>
								<xsl:when test="number(COL40)">
									<xsl:value-of select="COL40"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="Week">
							<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' '),9,1)"/>
						</xsl:variable>


						<xsl:variable name="varMonth1">
							<xsl:value-of select ="substring-before(substring-after(normalize-space(COL6),' '),' ')"/>

						</xsl:variable>

						<xsl:variable name="Month">
							<xsl:call-template name="MonthCodeVar">
								<xsl:with-param name="varMonth" select="$varMonth1"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Year">
							<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),2,1)"/>
						</xsl:variable>

						<xsl:variable name="PUTCALL">
							<xsl:value-of select="substring(substring-before(COL6,' '),1,1)"/>
						</xsl:variable>

						<xsl:variable name="StrikePrice1">
							<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL6),' '),' '),' '),' '),' ')"/>
						</xsl:variable>
						

						<xsl:variable name="Future">

							<xsl:choose>
								<xsl:when test="COL34='*' or COL34='' and COL2='FUTFOP'">
									<xsl:value-of select="concat($Week,'E',' ',$Month,$Year,$PUTCALL,$StrikePrice1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="normalize-space(concat($varUnderlying,' ',$MonthYearCode,$PutCall,$StrikePrice*$Multiplier,$FUTURE_SUFFIX_NAME))"/>
								</xsl:otherwise>
							</xsl:choose>
																
						</xsl:variable>

						<xsl:variable name="varMonth" select="number(substring(COL17,9,2))"/>

						<xsl:variable name="varYear" select="number(substring(COL17,7,2))"/>

						<xsl:variable name="varPutCall" select="normalize-space(COL39)"/>

						<xsl:variable name="MonthCode">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varMonth"/>
								<xsl:with-param name="varPutCall" select="$varPutCall"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="ThirdFriday">
							<xsl:choose>
								<xsl:when test="COL2='OPTION'">
									<xsl:value-of select="my:Now(concat(20,$varYear),$varMonth)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varDay" select="substring-before(substring-after(COL26,'/'),'/')"/>

						<AUECID>
							<xsl:choose>
								<xsl:when test="COL2='OPTION'">
									<xsl:value-of select="'12'"/>
								</xsl:when>
								<xsl:when test="COL2='FUTFOP'">
									<xsl:value-of select="'19'"/>
								</xsl:when>
							</xsl:choose>

						</AUECID>

						<TickerSymbol>
							<xsl:choose>
								<xsl:when test="COL2='OPTION'">
									<xsl:choose>
										<xsl:when test="substring-before(substring-after($ThirdFriday,'/'),'/')=$varDay - 1">
											<xsl:value-of select="concat('O:',substring-before(COL17,' '),' ',substring(COL17,7,2),$MonthCode,format-number(COL40,'#.00'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('O:',substring-before(COL17,' '),' ',substring(COL17,7,2),$MonthCode,format-number(COL40,'#.00'),'D',$varDay)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="COL2='FUTFOP'">
									<xsl:value-of select="$Future"/>
								</xsl:when>
							</xsl:choose>

						</TickerSymbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="COL2='OPTION'">
									<xsl:value-of select="concat(COL17,'U')"/>
								</xsl:when>
							</xsl:choose>
						</IDCOOptionSymbol>

						<BloombergSymbol>
							<xsl:choose>

								<xsl:when test="normalize-space(COL2)='FUTFOP' and (COL34='*' or COL34='')">
									<xsl:value-of select="concat($Week,'E',$Month,$Year,$PUTCALL,' ',$StrikePrice1,' ','Index')"/>
								</xsl:when>
								
								<xsl:when test="normalize-space(COL2)='FUTFOP' and (COL34!='*' or COL34!='')">
									<xsl:value-of select="COL34"/>
								</xsl:when>
		
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</BloombergSymbol>

						<Multiplier>
							<xsl:choose>
								<xsl:when test="number(COL42)">
									<xsl:value-of select="COL42"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Multiplier>

						<StrikePrice>
							<xsl:choose>
								<xsl:when test="number(COL40)">
									<xsl:value-of select="COL40"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StrikePrice>

						<ExpirationDate>
							<xsl:value-of select="COL26"/>
						</ExpirationDate>

						<PutOrCall>
							<xsl:choose>

								<xsl:when test="COL39='P' or COL39='p'">
									<xsl:value-of select="'0'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>

							</xsl:choose>
						</PutOrCall>

						<UnderLyingSymbol>
							<xsl:choose>
								<xsl:when test="COL2='OPTION'">
									<xsl:value-of select="substring-before(COL17,' ')"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL2)='FUTFOP' and (COL34='*' or COL34='')">
									<xsl:value-of select="concat($Week,'E',$Month,$Year)"/>
								</xsl:when>
								<xsl:when test="COL2='FUTFOP'">
									<xsl:value-of select="substring(substring-before(COL34,' '),1,4)"/>
								</xsl:when>
							</xsl:choose>
						</UnderLyingSymbol>

						<LongName>
							<xsl:value-of select="normalize-space(COL6)"/>
						</LongName>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>