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
			<xsl:when  test ="$varMonth='01' and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10' and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12' and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='01' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' and $varPutCall='C'">
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
				<xsl:if test ="number(COL9)">

					<PositionMaster>


						<xsl:variable name="varUnderlyingSymbol">
							<xsl:choose>
								<xsl:when test ="COL7='Option'">
									<xsl:value-of select ="substring-before(normalize-space(COL4),' ')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test ="COL7='Option'">
									<xsl:value-of select ="substring(substring-after(substring-after(normalize-space(COL4),' '),' '),1,2)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:choose>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'JAN'">
									<xsl:value-of select ="'01'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'FEB'">
									<xsl:value-of select ="'02'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'MAR'">
									<xsl:value-of select ="'03'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'APR'">
									<xsl:value-of select ="'04'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'MAY'">
									<xsl:value-of select ="'05'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'JUN'">
									<xsl:value-of select ="'06'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'JUL'">
									<xsl:value-of select ="'07'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'AUG'">
									<xsl:value-of select ="'08'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'SEP'">
									<xsl:value-of select ="'09'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'OCT'">
									<xsl:value-of select ="'10'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'NOV'">
									<xsl:value-of select ="'11'"/>
								</xsl:when>
								<xsl:when test ="COL7='Option' and substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/') = 'DEC'">
									<xsl:value-of select ="'12'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varPutCall">
							<xsl:choose>
								<xsl:when test ="COL7='Option'">
									<xsl:value-of select ="substring(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),1,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varMonthCode">
							<xsl:call-template name ="MonthCode">
								<xsl:with-param name ="varMonth" select ="$varMonth"/>
								<xsl:with-param name ="varPutCall" select="$varPutCall"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name ="varStrikePrice">
							<xsl:choose>

								<xsl:when test ="COL7='Option'">
									<xsl:value-of select ="format-number(substring(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),2),'#.00')"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varDateCode">
							<xsl:choose>
								<xsl:when test ="COL7='Option'">
									<xsl:value-of select ="substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),'/'),'/'),' ')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varDays">
							<xsl:choose>
								<xsl:when test="substring($varDateCode,1,1)='0'">
									<xsl:value-of select="substring($varDateCode,2,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varDateCode"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varThirdFriday">
							<xsl:choose>
								<xsl:when test ="COL7='Option'">
									<xsl:value-of select='my:Now(number(concat(20,$varYear)),number($varMonth))'/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varOptionSymbol">
							<xsl:choose>
								<xsl:when test ="COL7='Option'">
									<xsl:choose>
										<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varDateCode - 1)">
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice,'D',$varDays)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name = "PB_NAME" >
							<xsl:value-of select="'Test'"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@CompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<PBSymbol>
							<xsl:value-of select ="COL4"/>
						</PBSymbol>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_Symbol_NAME!=''">
									<xsl:value-of select="$PRANA_Symbol_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test ="COL8 !=''">

											<xsl:choose>

												<xsl:when test ="COL7='Option'">
													<xsl:value-of select ="$varOptionSymbol"/>
												</xsl:when>

												<xsl:when test ="COL7='Equity'">
													<xsl:value-of select ="COL8"/>
												</xsl:when>

											</xsl:choose>

										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="COL4"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<SideTagValue>

							<xsl:choose>
								<xsl:when test ="COL7='Equity' and number(COL9) &lt; 0">
									<xsl:value-of select ="'2'"/>
								</xsl:when>

								<xsl:when test ="COL7='Equity' and number(COL9) &gt; 0">
									<xsl:value-of select ="'1'"/>
								</xsl:when>

								<xsl:when test ="COL7='Option' and number(COL9) &gt; 0">
									<xsl:value-of select ="'A'"/>
								</xsl:when>

								<xsl:when test ="COL7='Option' and number(COL9) &lt; 0">
									<xsl:value-of select ="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<xsl:variable name ="varNetPosition">
							<xsl:value-of select ="number(COL9)"/>
						</xsl:variable>

						<NetPosition>

							<xsl:choose>
								<xsl:when test ="$varNetPosition &lt;0">
									<xsl:value-of select ="$varNetPosition*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$varNetPosition"/>
								</xsl:otherwise>
							</xsl:choose>

						</NetPosition>


						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL11)">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>					

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
