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
			<xsl:when  test ="$varMonth='01' or $varMonth='1'and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02'  or $varMonth='2' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' or $varMonth='3' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' or $varMonth='4' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05'or $varMonth='5'  and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' or $varMonth='6' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' or $varMonth='7' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' or $varMonth='8' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' or $varMonth='9' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10'  and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12'  and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='01'  or $varMonth='1' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' or $varMonth='2'  and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03'or $varMonth='3' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' or $varMonth='4' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' or $varMonth='5' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' or $varMonth='6' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' or $varMonth='7' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' or $varMonth='8' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09'  or $varMonth='9' and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10' and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12' and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:if test ="number(COL18)">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Test'"/>
						</xsl:variable>

						<xsl:variable name ="varUnderlyingSymbol">
							<xsl:if test ="string-length(COL13) &gt; 9">
								<xsl:value-of select ="normalize-space(COL14)"/>
							</xsl:if>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test ="string-length(COL13) &gt; 9">
									<xsl:value-of select ="substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL24),' '),' '),'/'),'/'),' ')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:choose>
								<xsl:when test ="string-length(COL13) &gt; 9">
									<xsl:value-of select ="substring-before(substring-after(substring-after(normalize-space(COL24),' '),' '),'/')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varExDay">
							<xsl:choose>
								<xsl:when test ="string-length(COL13) &gt; 9">
									<xsl:value-of select ="substring-before(substring-after(substring-after(substring-after(normalize-space(COL24),' '),' '),'/'),'/')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varPutCall">
							<xsl:choose>
								<xsl:when test ="string-length(COL13) &gt; 9">
									<xsl:value-of select ="substring(substring-after(substring-after(substring-after(normalize-space(COL24),' '),' '),' '),1,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varStrikePrice">
							<xsl:choose>

								<xsl:when test ="string-length(COL13) &gt; 9">
									<xsl:value-of select ="number(COL22)"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varMonthCode">
							<xsl:call-template name ="MonthCode">
								<xsl:with-param name ="varMonth" select ="$varMonth"/>
								<xsl:with-param name ="varPutCall" select="$varPutCall"/>
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

						<!--<xsl:variable name="varThirdFriday">
							<xsl:choose>
								<xsl:when test ="string-length(COL13) &gt; 9 and number($varYear) and number($varMonth)">
									<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>-->

						<xsl:variable name="varOptionSymbol">

							<xsl:choose>
								<xsl:when test ="string-length(COL13) &gt; 9">
									<!--<xsl:choose>
										<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
										</xsl:when>
										<xsl:otherwise>-->
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
										<!--</xsl:otherwise>
									</xsl:choose>-->
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL24"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test ="string-length(COL13) &gt; 9">
											<xsl:value-of select ="$varOptionSymbol"/>
										</xsl:when>

										<xsl:otherwise>

											<xsl:choose>

												<xsl:when test ="COL13 !='*'">
													<xsl:value-of select ="normalize-space(COL13)"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
												</xsl:otherwise>

											</xsl:choose>

										</xsl:otherwise>

									</xsl:choose>

								</xsl:otherwise>

							</xsl:choose>

						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<PositionStartDate>
							<xsl:value-of select ="concat(substring-before(substring-after(COL5,'/'),'/'),'/',substring-before(COL5,'/'),'/',substring-after(substring-after(COL5,'/'),'/'))"/>
						</PositionStartDate>

						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL18)"/>
						</xsl:variable>

						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>

						<SideTagValue>

							<!--for NonOptions-->
							<xsl:choose>

								<xsl:when test ="string-length(COL13) &lt; 9">
									<xsl:choose>
										<xsl:when test ="normalize-space(COL11)='Buy'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>
										<xsl:when test ="normalize-space(COL11)='Sell'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
										<xsl:when test ="normalize-space(COL11)='SellShort'">
											<xsl:value-of select ="'5'"/>
										</xsl:when>
										<xsl:when test ="normalize-space(COL11)='CoverShort'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<!--for options-->

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="normalize-space(COL11)='Buy'">
											<xsl:value-of select ="'A'"/>
										</xsl:when>
										<xsl:when test ="normalize-space(COL11)='Sell'">
											<xsl:value-of select ="'C'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>

						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="COL20"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<xsl:variable name ="varCommission">
							<xsl:value-of select ="COL31"/>
						</xsl:variable>
						<Commission>
							<xsl:choose>

								<xsl:when test ="$varCommission &lt;0">
									<xsl:value-of select ="$varCommission*-1"/>
								</xsl:when>

								<xsl:when test ="$varCommission &gt;0">
									<xsl:value-of select ="$varCommission"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name ="varStampDuty">
							<xsl:choose>
								<xsl:when test ="($NetPosition &lt; 0) and COL1 = 'Equities'">
									<xsl:value-of select ="COL18*COL20*(0.0000174)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<StampDuty>
							<xsl:choose>

								<xsl:when test ="$varStampDuty &lt;0">
									<xsl:value-of select ="$varStampDuty*-1"/>
								</xsl:when>

								<xsl:when test ="$varStampDuty &gt;0">
									<xsl:value-of select ="$varStampDuty"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</StampDuty>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
