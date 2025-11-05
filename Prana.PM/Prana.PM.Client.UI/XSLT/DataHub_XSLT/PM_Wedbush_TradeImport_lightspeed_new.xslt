<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<!--Third Friday check-->
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


	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>



	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="normalize-space(COL36)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL37,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">

				<xsl:value-of select="substring-before(substring-after(substring-after(COL18,' '),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(COL37,'/'),'/'),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring-before(COL18,' ')"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">

				<xsl:value-of select="format-number(COL38,'##.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
		</xsl:if>
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
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="number(COL4) and COL23='0'">

					<PositionMaster>
						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL18)"/>
						</xsl:variable>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BAML'"/>
						</xsl:variable>
						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>


						<xsl:variable name ="varCallPut">
							<xsl:choose>
								<xsl:when test="COL19 = '*' and COL5 != ''">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="COL5 != '' and COL5 != '*' and $varCallPut = 1">
									<xsl:value-of select="substring-before(COL5,'1')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL5)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varUnderlyingLength">
							<xsl:value-of select="string-length($varUnderlying)"/>
						</xsl:variable>

						<xsl:variable name="varMonthCode">
							<xsl:if test ="$varCallPut = 1">
								<xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:value-of select="COL5"/>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_Symbol_NAME != ''">
									<xsl:value-of select ="$PRANA_Symbol_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL36"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<PBSymbol>
							<xsl:if test="COL5 != '' and COL5 != '*'">
								<xsl:value-of select="COL5"/>
							</xsl:if>
						</PBSymbol>


						<CostBasis>
							<xsl:choose>
								<xsl:when  test="number(COL6)">
									<xsl:value-of select="COL6"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose >
						</CostBasis>



						<PositionStartDate>
							<xsl:value-of select="COL26"/>
						</PositionStartDate>

						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number(normalize-space(COL4)) and COL4 &gt; 0">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:when test="number(normalize-space(COL4)) and COL4 &lt; 0">
									<xsl:value-of select="COL4 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="COL3 = 'P' and  COL34='2'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="COL3 = 'P' and COL34='3'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										

										<xsl:when test="COL3 = 'S' and COL34='2'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="COL3 = 'S' and COL34='3'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL3 = 'P' and  COL34='2'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="COL3 = 'P' and COL34='3'">
											<xsl:value-of select="'B'"/>
										</xsl:when>


										<xsl:when test="COL3 = 'S' and COL34='2'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="COL3 = 'S' and COL34='3'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>


						</SideTagValue>

						<xsl:variable name ="varCommission">
							<xsl:choose>
								<xsl:when test ="number(COL15) ">
									<xsl:value-of select ="COL15"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varStampDuty">
							<xsl:choose>
								<xsl:when test ="number(COL12) ">
									<xsl:value-of select ="COL12"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Commission>
							<xsl:choose>
								<xsl:when test ="number($varCommission)">
									<xsl:value-of select ="$varCommission"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Commission>

						<xsl:variable name="varFees" select="number(COL10) + number(COL22)"/>

						<Fees>
							<xsl:choose>
								<xsl:when test ="number($varFees)">
									<xsl:value-of select ="$varFees"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</Fees>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ="number($varStampDuty) and $varStampDuty &gt; 0">
									<xsl:value-of select ="$varStampDuty"/>
								</xsl:when>
								<xsl:when test ="number($varStampDuty) and $varStampDuty &lt; 0">
									<xsl:value-of select ="$varStampDuty * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>


						<CounterPartyID>								
									<xsl:value-of select="47"/>								
						</CounterPartyID>

						<OriginalPurchaseDate>
							<xsl:value-of select="COL28"/>
						</OriginalPurchaseDate>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


